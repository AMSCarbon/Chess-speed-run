using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement 
{
    public Movement(PieceView movedPiece, TileView from, TileView to, bool taken, PieceView takenPiece ,bool isCastling,  bool whiteMove) {
        this.movedPiece = movedPiece;
        this.tileFrom = from;
        this.tileTo = to;
        this.pieceTaken = taken;
        this.takenPiece = takenPiece;
        this.isCastling = isCastling;
        this.whiteMove = whiteMove;
    }


    public PieceView movedPiece;
    public TileView tileFrom;
    public TileView tileTo;
    public bool pieceTaken;
    public PieceView takenPiece;
    public bool isCastling;
    public bool whiteMove;
}
