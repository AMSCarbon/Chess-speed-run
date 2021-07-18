using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : GenericPiece
{
    public Rook()
    {
        pieceValue = 5;
        pieceType = PieceType.Rook;
    }


    public override List<TileData> DetermineValidTiles(BoardModel state)
    {
        Debug.Log("Rook rules");
        List<TileData> valid = new List<TileData>();
        /*
        Vector2Int tile = currentTile.tile;
        valid.AddRange(ValidInDirection(1, 0, state));
        valid.AddRange(ValidInDirection(-1, 0, state));
        valid.AddRange(ValidInDirection(0, 1, state));
        valid.AddRange(ValidInDirection(0, -1, state));
        */
        return valid;
    }
}
