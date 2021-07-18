using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "TileData", menuName = "Tile Data")]
public class TileData : ScriptableObject
{
    public Vector2Int tile;
    public bool isOccupied;
    public PieceData occupyingObject;

   
    public void empty()
    {
        isOccupied = false;
        occupyingObject = null;
    }

    public void fill(PieceData p)
    {
        isOccupied = true;
        occupyingObject = p;
    }

    public TileView DeepCopy()
    {
        TileView ts = new TileView();
        
/*
        ts.name = name;
        ts.tile = tile;
        ts.isOccupied = isOccupied;
        if (isOccupied) ts.occupyingObject = null;
        */
        return ts;
    }
}
