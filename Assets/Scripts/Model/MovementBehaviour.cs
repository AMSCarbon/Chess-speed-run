using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Movement", menuName = "Movement Type" )]
public class MovementBehaviour : ScriptableObject
{
    public virtual List<TileData> DetermineValidTiles(PieceData data, BoardModel state) { return null; }

    //utility function for determining valid tiles 
    protected List<TileData> ValidInDirection(TileData currentTile, int x_dir, int y_dir, BoardModel state, bool isWhite)
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


    private bool isValid(int i, int j, BoardModel state, bool isWhite)
    {
        return (state.ValidTileIndex(i, j) && (!state.TileOccupied(i, j) || state.TileOccupiedByOppositeTeam(i, j, isWhite)));
    }

}
