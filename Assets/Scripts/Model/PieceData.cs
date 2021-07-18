using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Piece Data")]
public class PieceData : ScriptableObject
{
    public int moveCounter = 0;
    public bool isWhite;
    public TileData currentTile;
    public bool isRemoved = false;
    public PieceType pieceType;
    public int pieceValue; 

    public void printData() {
        string data = "moveCounter:" + moveCounter + " isWhite:" + isWhite +
                        " currentTile:" + currentTile.tile + " isRemoved:" + isRemoved;
        Debug.Log(data);
    }
}
