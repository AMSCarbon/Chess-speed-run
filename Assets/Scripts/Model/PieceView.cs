using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains methods and data for actually being instantiated. 
public class PieceView : MonoBehaviour
{
    public PieceData data;
    public Material normalMaterial;
    public Material selectedMaterial;
    public TileView currentTile;
    
    //the concept of being selected is unrelated to the data model.
    private bool isSelected = false;

    //Let the view assign colour so we can build things in the inspector ottay.
    public bool isWhite; 

    public void Start()
    {
        data = Instantiate(data);
        data.isWhite = isWhite;
        FindObjectOfType<GameManager>().board.RegisterPiece(data);
        updateCurrentTile();
    }

    public void SelectPiece()
    {
        Material[] mat = new Material[1];
        mat[0] = selectedMaterial;
        GetComponent<Renderer>().materials = mat;
    }

    public void DeselectPiece()
    {
        Material[] mat = new Material[1];
        mat[0] = normalMaterial;
        GetComponent<Renderer>().materials = mat;
    }

    //Piece determines which tile it's on by shooting a ray downwards. 
    public void updateCurrentTile()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * 3, Vector3.down);
        //Cast ray from above to be outside of the 
       
        int layer_mask = LayerMask.GetMask("Tile");
        if (Physics.Raycast(ray, out hit, 5, layer_mask))
        {
            TileView tile = hit.transform.gameObject.GetComponent<TileView>();
            if (tile != null)
            {
                currentTile = tile;
                data.currentTile = tile.data;
                tile.fill(this);
            }
        }
    }

    public void RemovePiece()
    {
        // for now, just yeet them away. 
        data.isRemoved = true;
        transform.position = new Vector3(-1000, -1000, -1000);
    }

}
