using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{

    public TileData data;
    public PieceView occupyingPieceView;

    public void Awake()
    {
        data = ScriptableObject.CreateInstance("TileData") as TileData;
        int column = name[0] - 'A';
        int row = int.Parse(name[2].ToString()) - 1;
        data.tile = new Vector2Int(column, row);
    }

    public void Start()
    {
        Register();
    }

    public void Register() {
        FindObjectOfType<GameManager>().board.RegisterTile(data.tile.x, data.tile.y, data);
    }

    public void empty() {
        data.isOccupied = false;
        data.occupyingObject = null;
        occupyingPieceView = null;
    }

    public void fill(PieceData p)
    {
        data.isOccupied = true;
        data.occupyingObject = p;
    }

    public void fill(PieceView p) {
        data.isOccupied = true;
        data.occupyingObject = p.data;
        occupyingPieceView = p;
    }

    //Interface for getting the views from data. 
    public static List<TileView> GetTileViewsFromData(List<TileData> data) {
        List<TileView> views = new List<TileView>();
        foreach (TileView view in FindObjectsOfType<TileView>()) {
            if (data.Contains(view.data)) views.Add(view);
        }
        return views;
    }

    public static TileView GetTileViewFromData(TileData data)
    {
        foreach (TileView view in FindObjectsOfType<TileView>())
        {
            if (data  == view.data) return view;
        }
        return null;
    }

}
