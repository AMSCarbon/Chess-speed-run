using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : GenericPiece
{
    public Queen()
    {
        pieceValue = 9;
        pieceType = PieceType.Queen;
    }


    public override List<TileData> DetermineValidTiles(BoardModel state)
    {
        Debug.Log("Queen rules");
        List<TileData> valid = new List<TileData>();
        /*
       Vector2Int tile = currentTile.tile;
       valid.AddRange(ValidInDirection(1, 1, state));
       valid.AddRange(ValidInDirection(1, -1, state));
       valid.AddRange(ValidInDirection(-1, 1, state));
       valid.AddRange(ValidInDirection(-1, -1, state));
       valid.AddRange(ValidInDirection(1, 0, state));
       valid.AddRange(ValidInDirection(-1, 0, state));
       valid.AddRange(ValidInDirection(0, 1, state));
       valid.AddRange(ValidInDirection(0, -1, state));
       */
        return valid;
    }
}
