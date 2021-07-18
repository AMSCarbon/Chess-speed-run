using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Agent used to try minimise the number of moves the enemy can make in the next turn.
//Now just uses a heuristic which works kinda well. Beats me about half the time >.>
public class MinimiseMovesAgent : PlayerAgent
{

    public void Start()
    {
        foreach (PlayerAgent agent in FindObjectsOfType<PlayerAgent>()) {
            if (agent.name != this.name) enemy = agent; break;
        }
        base.Start();
    }

    private void Update()
    {
        if (!currentTurn || gm.gameOver) return;
        List<Movement> possibleMoves = EnumerateValidMovements();

        if (possibleMoves.Count == 0)
        {
            Debug.Log("I got no moves");
            Debug.Break();
            return;
        }
        //Debug.Log("Moving " + possibleMoves[index].movedPiece.name + " to " + possibleMoves[index].tileTo.name);
        SubmitMovement(PickMove(possibleMoves));
    }

    private Movement PickMove(List<Movement> available) {
        int maxScore = int.MinValue;
        List<Movement> shuffled = available.OrderBy(x => Guid.NewGuid()).ToList();
        Movement selectedMove = available[0];
        //one in a hundred chance of making a random move. 
        if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.01) return selectedMove;
        foreach (Movement move in shuffled)
        {
            PieceData data = move.movedPiece.data;
            TileData tileTo = move.tileTo.data;
            // alter model to test move. 
            TileData tileFrom = data.currentTile;
            PieceData takenPiece = tileTo.occupyingObject;
            tileFrom.empty();
            tileTo.fill(data);
            data.currentTile = tileTo;
            data.moveCounter++;

            //movement building uses the views
            TileView.GetTileViewFromData(tileFrom).occupyingPieceView = null;
            TileView.GetTileViewFromData(tileTo).occupyingPieceView = move.movedPiece;

            if (takenPiece != null)
            {
                takenPiece.isRemoved = true;
            }
            //carry out opperation here
            int score = EvaluateBoard(move, isWhite );
           // Debug.Log(score);
            if (score > maxScore) {
                selectedMove = move;
                maxScore = score;
            }

            //undo move.
            TileView.GetTileViewFromData(tileFrom).occupyingPieceView = move.movedPiece;
            TileView.GetTileViewFromData(tileTo).occupyingPieceView = move.takenPiece; //may be null but it's fine. 

            if (takenPiece != null)
            {
                takenPiece.isRemoved = false;
            }
            tileTo.empty();
            tileFrom.fill(data);
            data.currentTile = tileFrom;
            data.moveCounter--;
            if (takenPiece != null)
            {
                tileTo.fill(takenPiece);
            }
        }
        
        return selectedMove;
    }

    //Evaluate the state of the board having just made a movement.
    private int EvaluateBoard(Movement move, bool evaluateForWhite) {
        int score = 0;
        score += move.pieceTaken ? move.takenPiece.data.pieceValue^2 : 0;
        if (BoardModel.TestForCheck(gm.board, !isWhite)) {
            score += 2; //encourage the player to check.
            if (BoardModel.TestForCheckMate(gm.board, !isWhite)) {
                score += 10000000; //if you can checkmate take it.
            }
        }

        score += EnumerateValidMovements().Count();
        //score -= enemy.EnumerateValidMovements().Count();
        
        //for each move the oponent can make, minus the points for each piece.
        // this way the player will be more conservative rather than just yeeting pieces in.
        foreach (Movement m in enemy.EnumerateValidMovements()) {
            if (m.pieceTaken && m.takenPiece == null) {
                Debug.Log("broken move" );
            }
            //Avoid having pieces taken, especially for more valuable pieces. 
            score -= m.pieceTaken ? m.takenPiece.data.pieceValue^2: 0;
        };
        return score;
    }

}

    
