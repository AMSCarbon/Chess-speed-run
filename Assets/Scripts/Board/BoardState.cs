using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardModel
{
    //This is a scriptable object of an unoccupied tile. 
    public TileData emptyTile;
    TileData[,] board = new TileData[8, 8];
    private bool isWhiteTurn = true;
    private List<PieceData> pieces = new List<PieceData>();

    public TileData GetTileData(int col, int row) { return board[col, row]; }
    public List<TileData> validMovesOutOfStalemate;
    //Is the given tile reachable by any of the pieces from the opposing team.
    public List<TileData> TilesReachableByEnemy(bool testForWhite) {
        List<TileData> reachable = new List<TileData>();
        foreach (PieceData piece in GetTeamPieces(!testForWhite)) {
            reachable.AddRange(MovementRules.DetermineAccessableTiles(piece, this));
        }
        return reachable;
    }

    public List<PieceData> GetTeamPieces(bool white) {
        List<PieceData> team = new List<PieceData>(pieces);
        team.RemoveAll((PieceData p) => p.isWhite != white || p.isRemoved); 
        return team;
    }

    //utility functions for determining valid moves
    public bool TileOccupied(int col, int row)
    {
        return board[col, row].isOccupied;
    }

    public bool TileOccupiedByOppositeTeam(int col, int row, bool white)
    {
        if (board[col, row].isOccupied)
        {
            //occupied by yours or the other person. 
            return board[col, row].occupyingObject.isWhite != white;
        }
        //unoccupied
        return false;
    }

    public bool ValidTileIndex(int col, int row) { return col >= 0 && col <= 7 && row >= 0 && row <= 7; }

    //View API
    //Interface for TileView to register its data. When the game begins a tile view instantiates its own data which will overwrite this one.
    public void RegisterTile(int row, int col, TileData td)
    {
        if (td != null)
        {
            board[row, col] = td;
        }
    }

    public void RegisterPiece(PieceData piece) {
        pieces.Add(piece);
    }

    public void UpdateBoardModel(PieceData piece, TileData from, TileData to) {
        from.empty();
        to.fill(piece); // Automatically overwrites the tiles data, but taken piece data will remember where it was, if we needa backtrack. 
        piece.currentTile = to;
        piece.moveCounter++;
        QueenPawn(piece, to);
    }

    private void QueenPawn(PieceData piece, TileData to) {
        //pawns cant go backwards so don't need to really consider the colour
        if (piece.pieceType == PieceType.Pawn && (to.tile.y == 0 || to.tile.y == 7  )) {
            piece.pieceType = PieceType.Queen;
        }
    }

    // If true, testing if the white king is in check.
    public static bool TestForCheck(BoardModel state, bool testForWhite) {
        //Debug.Log("Testing for check");
        PieceData king = state.FindKing(testForWhite);
        //Debug.Log("Getting enemy piece movements");
        List<TileData> reachable = state.TilesReachableByEnemy(testForWhite);
        return reachable.Contains(king.currentTile);
    }

    // If true, testing if the white king is in check.
    public static bool TestForCheckMate(BoardModel state, bool testForWhite)
    {
        //If the king can move, then return. 
        PieceData king = state.FindKing(testForWhite);
        if (MovementRules.DetermineValidTiles(king, state).Count > 0)
        {
          //  Debug.Log("King can move out of check");
            return false;
        }

        //Optimisation. Determine which pieces are checking the king, then determine if we can move to block.
        //conjecture. if there's more than one piece checking then it's impossible to move another piece to resolve check.
        List<PieceData> checkingPieces = state.GetCheckingPieces(testForWhite);
        if (checkingPieces.Count == 0) return false; // There's no check, you're just stupid.
        if (checkingPieces.Count > 1)
        {
           // Debug.Log("Multiple pieces checking the king. King cant move. Checkmate");
            return true;
        }
        // get the tiles that the piece checking the king can get to.
        // remove the king because we can't override him 
        // add in the checking piece cause we can just take the piece.
        List<TileData> blockTiles = state.TilesRequiredForCheck(king, checkingPieces[0]);
        MovementRules.DetermineValidTiles(checkingPieces[0], state);
        blockTiles.Remove(king.currentTile);
        blockTiles.Add(checkingPieces[0].currentTile);

        //For every piece we have, and every tile they can get to, if any one of them is within the checking piece's tile
        // then there is at least one movement that can be made to escape checkmate.
        foreach (PieceData piece in state.GetTeamPieces(testForWhite)) {
            if (piece.pieceType == PieceType.King) continue; // skip king.
            foreach (TileData tile in MovementRules.DetermineValidTiles(piece, state)) {
                if (blockTiles.Contains(tile))
                {
                  //  Debug.Log("Piece can be moved to block the check.");
                    //Debug.Log(piece.pieceType + " " + piece.currentTile.tile);
                    return false;
                }
            }
        }
        Debug.Log("Checkmate");
        return true;
    }


    // If true, testing if the white king is in check.
    public static bool TestForStaleMate(BoardModel state, bool testForWhite)
    {
        List<PieceData> teamPieces = state.GetTeamPieces(testForWhite);
        List<PieceData> opponentPieces = state.GetTeamPieces(!testForWhite);
        string colour = testForWhite ? "white" : "black";
        //Debug.Log("Testing stalemate for " + colour);
        //Only the two kings left, cant really anything. Valid moves but they can't check eachother.
        if (teamPieces.Count == 1 && opponentPieces.Count == 1) return true;
        foreach (PieceData piece in state.GetTeamPieces(testForWhite))
        {
            //Debug.Log(colour + " " + piece.pieceType + " This piece is : " + (piece.isRemoved ? "removed" : "on board"));
            if (MovementRules.DetermineValidTiles(piece, state).Count > 0)
            {
               // Debug.Log("Not in stalemate");
               foreach(TileData tile in MovementRules.DetermineValidTiles(piece, state)) {
                    //Debug.Log(colour + " " + piece.pieceType + " from: " + piece.currentTile.tile + " to: " + tile.tile);
                }
                return false;
            }
        }

        Debug.Log("Stalemate");
        return true;
    }

    private List<PieceData> GetCheckingPieces( bool testForWhite) {

        List<PieceData> checkingPieces= new List<PieceData>();
        TileData kingTile= FindKing(testForWhite).currentTile;
        List<TileData> reachable = TilesReachableByEnemy(testForWhite);

        foreach (PieceData piece in GetTeamPieces(!testForWhite))
        {
            if (piece.pieceType == PieceType.King) continue; // cant be mutually checking each other? 
            //If the valid tiles the piece can go through contains the king, then this piece is checking it. 
            if (MovementRules.DetermineValidTiles(piece, this).Contains(kingTile)) {
                checkingPieces.Add(piece);
            }
        }
        return checkingPieces;
    }

    private List<TileData> TilesRequiredForCheck(PieceData king, PieceData checkingPiece) {
        List<TileData> tiles = new List<TileData>();
        TileData kingTile = king.currentTile;
        TileData checkingTile = checkingPiece.currentTile;
        tiles.Add(checkingTile);
        
        //pawns must be right next to the piece, knights can jump.
        if (checkingPiece.pieceType == PieceType.Pawn || 
            checkingPiece.pieceType == PieceType.Knight) return tiles;

        Vector2Int direction = checkingTile.tile - kingTile.tile;
        direction = new Vector2Int(Math.Sign(direction.x), Math.Sign(direction.y));

        for(int i = 0; i < 8; i++){
            Vector2Int indicies = kingTile.tile + i * direction;
            tiles.Add(GetTileData(indicies.x, indicies.y));
            if (indicies == checkingTile.tile) break;
        }
        return tiles;
    }

    private PieceData FindKing(bool white) {
        foreach (PieceData piece in pieces) {
            if (piece.isWhite == white && piece.pieceType == PieceType.King) return piece;
        }
        return null; // If there's no king then somethings derped lmao.
    }

    public void printState() {
        string boardDisplay = "+++++++++++++++++\n";
        for (int i = 7; i >= 0; i--) {
            boardDisplay += "++";
            for (int j = 0; j < 8; j++) {
                if (board[j, i].isOccupied)
                {
                    switch (board[j, i].occupyingObject.pieceType)
                    {
                        case PieceType.Pawn:
                            boardDisplay += "p|";
                            break;
                        case PieceType.Rook:
                            boardDisplay += "R|";
                            break;
                        case PieceType.Knight:
                            boardDisplay += "N|";
                            break;
                        case PieceType.Bishop:
                            boardDisplay += "B|";
                            break;
                        case PieceType.Queen:
                            boardDisplay += "Q|";
                            break;
                        case PieceType.King:
                            boardDisplay += "K|";
                            break;
                        default:
                            boardDisplay += "X|";// you somehow screwed up
                            break;
                    }

                }
                else {
                    boardDisplay += "-|";
                }
                
            }
            boardDisplay += "++\n";
        }
        boardDisplay += "+++++++++++++++++\n";

        Debug.Log(boardDisplay);
    }
}
