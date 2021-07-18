using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MovementRules
{


    public static List<TileData> DetermineValidTiles(PieceData data, BoardModel state) {
        return DetermineTiles(data, state, true);
    }

    public static List<TileData> DetermineAccessableTiles(PieceData data, BoardModel state) {
        return DetermineTiles(data, state, false);
    }

    private static List<TileData> DetermineTiles(PieceData data, BoardModel state, bool filterCheck)
    {
        List<TileData> tiles;
        switch (data.pieceType)
        {
            case PieceType.Pawn:
                tiles = Pawn(data, state);
                break;
            case PieceType.Rook:
                tiles = Rook(data, state);
                break;
            case PieceType.Knight:
                tiles = Knight(data, state);
                break;
            case PieceType.Bishop:
                tiles = Bishop(data, state);
                break;
            case PieceType.Queen:
                tiles = Queen(data, state);
                break;
            case PieceType.King:
                tiles = King(data, state);
                break;
            default:
                return new List<TileData>(); // you somehow fucked up, but here's an empty list. 
        }
        return filterCheck ? FilterMovesCausingCheck(data, state, tiles) : tiles;
    }

    private static List<TileData> Pawn(PieceData data, BoardModel state) {

        List<TileData> valid = new List<TileData>();
        int direction = data.isWhite ? 1 : -1;

        Vector2Int tile = data.currentTile.tile;
        //straight 
        if (state.ValidTileIndex(tile.x, tile.y + direction) &&
            !state.TileOccupied(tile.x, tile.y + direction))
        {
            valid.Add(state.GetTileData(tile.x, tile.y + direction));
        }
        //take left
        if (state.ValidTileIndex(tile.x - 1, tile.y + direction) &&
            state.TileOccupiedByOppositeTeam(tile.x - 1, tile.y + direction, data.isWhite))
        {
            valid.Add(state.GetTileData(tile.x - 1, tile.y + direction));
        }
        //take right
        if (state.ValidTileIndex(tile.x + 1, tile.y + direction) &&
            state.TileOccupiedByOppositeTeam(tile.x + 1, tile.y + direction, data.isWhite))
        {
            valid.Add(state.GetTileData(tile.x + 1, tile.y + direction));
        }
        //First move, you can move two spaces. 
        if (state.ValidTileIndex(tile.x, tile.y + 2 * direction) && data.moveCounter == 0 &&
            !state.TileOccupied(tile.x, tile.y + 2 * direction) &&
            !state.TileOccupied(tile.x, tile.y + direction))
        {
            valid.Add(state.GetTileData(tile.x, tile.y + 2 * direction));
        }
        return valid;
    }

    private static List<TileData> Rook(PieceData data, BoardModel state)
    {
        List<TileData> valid = new List<TileData>();
        TileData tile = data.currentTile;
        valid.AddRange(ValidInDirection(tile, 1, 0, state, data.isWhite));
        valid.AddRange(ValidInDirection(tile, -1, 0, state, data.isWhite));
        valid.AddRange(ValidInDirection(tile, 0, 1, state, data.isWhite));
        valid.AddRange(ValidInDirection(tile, 0, -1, state, data.isWhite));
        return valid;
    }

    private static List<TileData> Knight(PieceData data, BoardModel state)
    {
        List<TileData> valid = new List<TileData>();
        Vector2Int tile = data.currentTile.tile;
        // this isn't the best but I ceebs to figure out anything better rn.
        if (isValid(tile.x - 1, tile.y - 2, state, data.isWhite)) valid.Add(state.GetTileData(tile.x - 1, tile.y - 2));
        if (isValid(tile.x + 1, tile.y - 2, state, data.isWhite)) valid.Add(state.GetTileData(tile.x + 1, tile.y - 2));
        if (isValid(tile.x - 1, tile.y + 2, state, data.isWhite)) valid.Add(state.GetTileData(tile.x - 1, tile.y + 2));
        if (isValid(tile.x + 1, tile.y + 2, state, data.isWhite)) valid.Add(state.GetTileData(tile.x + 1, tile.y + 2));
        if (isValid(tile.x - 2, tile.y - 1, state, data.isWhite)) valid.Add(state.GetTileData(tile.x - 2, tile.y - 1));
        if (isValid(tile.x + 2, tile.y - 1, state, data.isWhite)) valid.Add(state.GetTileData(tile.x + 2, tile.y - 1));
        if (isValid(tile.x - 2, tile.y + 1, state, data.isWhite)) valid.Add(state.GetTileData(tile.x - 2, tile.y + 1));
        if (isValid(tile.x + 2, tile.y + 1, state, data.isWhite)) valid.Add(state.GetTileData(tile.x + 2, tile.y + 1));
        return valid;
    }

    private static List<TileData> Bishop(PieceData data, BoardModel state)
    {
        List<TileData> valid = new List<TileData>();
        TileData tile = data.currentTile;
        valid.AddRange(ValidInDirection(tile, 1, 1, state, data.isWhite));
        valid.AddRange(ValidInDirection(tile, 1, -1, state, data.isWhite));
        valid.AddRange(ValidInDirection(tile, -1, 1, state, data.isWhite));
        valid.AddRange(ValidInDirection(tile, -1, -1, state, data.isWhite));
        return valid;
    }

    private static List<TileData> King(PieceData data, BoardModel state)
    {
        List<TileData> valid = new List<TileData>();
        Vector2Int tile = data.currentTile.tile;
        for (int x_offset = -1; x_offset <= 1; x_offset++) {
            for (int y_offset = -1; y_offset <= 1; y_offset++) {
                //skip the current square. skip invalid squares. 
                int i = tile.x + x_offset;
                int j = tile.y + y_offset;
                if (x_offset == 0 && y_offset == 0) continue;
                if (state.ValidTileIndex(i, j) &&
                    (!state.TileOccupied(i, j) ||
                    state.TileOccupiedByOppositeTeam(i, j, data.isWhite)))
                {
                    valid.Add(state.GetTileData(i, j));
                }
            }
        }
        string colour = data.isWhite ? "white" : "black";
       // Debug.Log(colour + " king has: " + valid.Count + " Possible moves");
        return valid;
    }

    public static List<TileData> KingNonRecursive(PieceData data, BoardModel state)
    {
        List<TileData> valid = new List<TileData>();
        Vector2Int tile = data.currentTile.tile;
        for (int x_offset = -1; x_offset <= 1; x_offset++)
        {
            for (int y_offset = -1; y_offset <= 1; y_offset++)
            {
                //skip the current square. skip invalid squares. 
                int i = tile.x + x_offset;
                int j = tile.y + y_offset;
                if (x_offset == 0 && y_offset == 0) continue;
                if (state.ValidTileIndex(i, j) &&
                    (!state.TileOccupied(i, j) ||
                    state.TileOccupiedByOppositeTeam(i, j, data.isWhite)))
                {
                    valid.Add(state.GetTileData(i, j));
                }
            }
        }
        return valid;
    }

    private static List<TileData> Queen(PieceData data, BoardModel state)
    {
        List<TileData> tiles = Bishop(data, state);
        tiles.AddRange(Rook(data, state));
        return tiles;
    }

    private static List<TileData> FilterMovesCausingCheck(PieceData data, BoardModel state, List<TileData> tiles) {
        //Debug.Log("Prefilter: " + tiles.Count);
        List<TileData> tilesToRemove = new List<TileData>();
        foreach (TileData tileTo in tiles) {
            // alter model to test move. 
            TileData tileFrom = data.currentTile;
            PieceData takenPiece = tileTo.occupyingObject;
            tileFrom.empty();
            tileTo.fill(data);
            data.currentTile = tileTo;
            data.moveCounter++;
            if (takenPiece != null) {
                takenPiece.isRemoved = true;
            }
            if (BoardModel.TestForCheck(state, data.isWhite)) tilesToRemove.Add(tileTo);

            if (takenPiece != null)
            {
                takenPiece.isRemoved = false;
            }
            //undo move.
            tileTo.empty();
            tileFrom.fill(data);
            data.currentTile = tileFrom;
            data.moveCounter--;
            if (takenPiece != null) {
                tileTo.fill(takenPiece);
            }
        }
        foreach (TileData remove in tilesToRemove) {
            tiles.Remove(remove);
        }
       // Debug.Log("Postfilter: " + tiles.Count );
        return tiles;
    }

    //utility function for determining valid tiles 
    public static List<TileData> ValidInDirection(TileData currentTile, int x_dir, int y_dir, BoardModel state, bool isWhite)
    {
        List<TileData> valid = new List<TileData>();
        Vector2Int tile = currentTile.tile;
        for (int offset = 1; offset < 8; offset++)
        {
            int i = tile.x + offset * x_dir;
            int j = tile.y + offset * y_dir;
            if (!state.ValidTileIndex(i, j)) break;
            if (!state.TileOccupied(i, j))
            {
                valid.Add(state.GetTileData(i, j));
            }
            else if (state.TileOccupiedByOppositeTeam(i, j, isWhite))
            {
                // can only take the first instance of an enemy.
                valid.Add(state.GetTileData(i, j));
                break;
            }
            else break;
        }
        return valid;
    }

    private static bool isValid(int i, int j, BoardModel state, bool isWhite)
    {
        return (state.ValidTileIndex(i, j) && (!state.TileOccupied(i, j) || state.TileOccupiedByOppositeTeam(i, j, isWhite)));
    }
}
