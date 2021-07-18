using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : GenericPiece
{
    public King()
    {
        pieceValue = 0;
        pieceType = PieceType.King;
    }

    public override List<TileData> DetermineValidTiles(BoardModel state)
    {
        Debug.Log("King rules");
        List<TileData> valid = new List<TileData>();
        /*
        Vector2Int tile = currentTile.tile;
        List<TileData> reachAbleByEnemies = state.TilesReachableByEnemy(isWhite);
        Debug.Log(reachAbleByEnemies.Count);
        for (int x_offset = -1; x_offset <= 1; x_offset++) {
            for (int y_offset = -1; y_offset <= 1; y_offset++) {
                //skip the current square. skip invalid squares. 
                int i = tile.x + x_offset;
                int j = tile.y + y_offset;
                if (x_offset == 0 && y_offset == 0) continue;
                if (state.ValidTileIndex(i, j) &&
                    !reachAbleByEnemies.Contains(state.GetTile(i, j)) &&
                    (!state.TileOccupied(i, j) || 
                    state.TileOccupiedByOppositeTeam(i, j, isWhite)))
                {
                    valid.Add(state.GetTile(i, j));
                }
            }
        }*/
        return valid;
    }

    // The King's DetermineValidTiles function tests for check, which triggers the other kings test for check. 
    //This function is the same without the test for check.
    public List<TileData> ReachableTiles(BoardModel state)
    {
        Debug.Log("King rules");
        List<TileData> valid = new List<TileData>();

        /*
        Vector2Int tile = currentTile.tile;
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
                    state.TileOccupiedByOppositeTeam(i, j, isWhite))) 
                {
                    valid.Add(state.GetTile(i, j));
                }
            }
        }
        */
        return valid;
    }
}
