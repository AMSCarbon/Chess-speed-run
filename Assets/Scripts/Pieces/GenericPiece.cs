using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    King,
    Queen
}

[ExecuteInEditMode]
public class GenericPiece : MonoBehaviour
{
    public int moveCounter = 0;
    public bool isWhite;
    public TileView currentTile;
    public bool isRemoved = false;
    private bool isSelected = false;

    public Material normalMaterial;
    public Material selectedMaterial;
    public void Start()
    {
        updateCurrentTile();
    }
    public void SelectPiece() {
        Material[] mat = new Material[1];
        mat[0] = selectedMaterial;
        GetComponent<Renderer>().materials = mat;
    }

    public void DeselectPiece() {
        Material[] mat = new Material[1];
        mat[0] = normalMaterial;
        GetComponent<Renderer>().materials = mat;
    }

    //Piece determines which tile it's on by shooting a ray downwards. 
    public void updateCurrentTile()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position+Vector3.up*3, Vector3.down);
        //Cast ray from above to be outside of the 
        int layer_mask = LayerMask.GetMask("Tile");
        if (Physics.Raycast(ray, out hit,5,layer_mask))
        {
            TileView ts = hit.transform.gameObject.GetComponent<TileView>();
            if (ts != null) {
                currentTile = ts;
                ts.data.isOccupied = true;
            }
        }
    }

    //piece specific functions and values

    public virtual PieceType pieceType { get; protected set; }
    public PieceType GetPieceType() { return pieceType; }

    public virtual int pieceValue { get; protected set; }
    public int getPoints() { return pieceValue; }

    public virtual List<TileData> DetermineValidTiles(BoardModel state) { Debug.Log("generic rules"); return new List<TileData>(); }

    public void RemovePiece() {
        // for now, just yeet them away. 
        isRemoved = true;
        transform.position = new Vector3(-100, -100, -100);
    }

    //utility function for determining valid tiles 
    protected List<TileData> ValidInDirection(int x_dir, int y_dir, BoardModel state)
    {
        List<TileData> valid = new List<TileData>();
       /* Vector2Int tile = currentTile.tile;
        for (int offset = 1; offset < 8; offset++)
        {
            int i = tile.x + offset * x_dir;
            int j = tile.y + offset * y_dir;
            if (!state.ValidTileIndex(i, j)) break;
            if (!state.TileOccupied(i, j))
            {
                valid.Add(state.GetTile(i, j));
            }
            else if (state.TileOccupiedByOppositeTeam(i, j, isWhite))
            {
                // can only take the first instance of an enemy.
                valid.Add(state.GetTile(i, j));
                break;
            }
            else break;
        }*/
        return valid;
    }

}
