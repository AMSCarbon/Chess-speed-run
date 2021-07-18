using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : GenericPiece
{
    public Bishop() {
        pieceValue = 3;
        pieceType = PieceType.Bishop;
    }
    public override List<TileData> DetermineValidTiles(BoardModel state)
    {
        Debug.Log("Bishop rules");
        List<TileData> valid = new List<TileData>();
        /*
        Vector2Int tile = currentTile.tile;
        valid.AddRange(ValidInDirection(1, 1, state));
        valid.AddRange(ValidInDirection(1, -1, state));
        valid.AddRange(ValidInDirection(-1, 1, state));
        valid.AddRange(ValidInDirection(-1, -1, state));
        */
        return valid;
    }

}
