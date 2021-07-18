using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : MonoBehaviour
{
    public bool isWhite;
    public bool currentTurn = false;
    public bool isChecked;
    public GameManager gm;
    public PieceView selectedPiece;
    public List<TileData> validMovements = new List<TileData>();
    public PlayerAgent enemy;

    public void Start() {
        gm = FindObjectOfType<GameManager>();
    }

    public void SelectPiece(PieceView p) {
        //If it's the same piece, yeet out. I think the piece might be getting hit multiple times with mouse bounce.
        if (selectedPiece == p) return;
       // Debug.Log("Selected: " + p.name);
        ClearSelection();
        p.SelectPiece();
        selectedPiece = p;
        validMovements = MovementRules.DetermineValidTiles(p.data, gm.GetBoardState());
    }

    public virtual void ClearSelection() {
        if (selectedPiece != null) {
            selectedPiece.DeselectPiece();
            selectedPiece = null;
        }
    }

    public Movement BuildMovement(PieceView selectedPiece, TileView ts) {
        return new Movement(selectedPiece, selectedPiece.currentTile, ts,
            ts.data.isOccupied, ts.occupyingPieceView, false, isWhite);
    }

    public void SubmitMovement(Movement m ) {
        gm.SubmitMovement(m);
    }

    //Currently start/end turn only change the turn state. In the future, they might be used for other things, so I want them seperate.
    public virtual void StartTurn() {
        currentTurn = true; 
    }

    public virtual void EndTurn() {
        currentTurn = false;
        if (selectedPiece != null) {
            selectedPiece.DeselectPiece();
            selectedPiece = null;
        }
    }

    public GenericPiece GetSelectedPiece() {
        return null;
    }


    public List<PieceView> GetTeamPieces() {
        List<PieceView> pieces = new List<PieceView>(FindObjectsOfType<PieceView>()) ;
        string colour = isWhite ? "white" : "black";
        pieces.RemoveAll((PieceView p ) => p.data.isWhite != isWhite || p.data.isRemoved); // I feel at home =w=
        //Debug.Log(colour + "player has " + pieces.Count + " pieces");
        return pieces;
    }

    public List<Movement> EnumerateValidMovements() {
        List<Movement> moves = new List<Movement>();
        foreach (PieceView piece in GetTeamPieces()) {
            List<TileData> tiles = MovementRules.DetermineValidTiles(piece.data, gm.board);
            foreach (TileData tile in tiles) {
                moves.Add(BuildMovement(piece, TileView.GetTileViewFromData(tile)));
            }
        }
        return moves;   
    }

   
}
