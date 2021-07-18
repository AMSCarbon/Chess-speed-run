using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : GenericPiece
{
    public Knight()
    {
        pieceValue = 3;
        pieceType = PieceType.Knight;
    }


    public override List<TileData> DetermineValidTiles(BoardModel state)
    {
        Debug.Log("Knight rules");
        List<TileData> valid = new List<TileData>();
        /*
        Vector2Int tile = currentTile.tile;
        // this isn't the best but I cbf to figure out anything better.
        if (isValid(tile.x - 1, tile.y - 2, state)) valid.Add(state.GetTile(tile.x - 1, tile.y - 2));
        if (isValid(tile.x + 1, tile.y - 2, state)) valid.Add(state.GetTile(tile.x + 1, tile.y - 2));
        if (isValid(tile.x - 1, tile.y + 2, state)) valid.Add(state.GetTile(tile.x - 1, tile.y + 2));
        if (isValid(tile.x + 1, tile.y + 2, state)) valid.Add(state.GetTile(tile.x + 1, tile.y + 2));
        if (isValid(tile.x - 2, tile.y - 1, state)) valid.Add(state.GetTile(tile.x - 2, tile.y - 1));
        if (isValid(tile.x + 2, tile.y - 1, state)) valid.Add(state.GetTile(tile.x + 2, tile.y - 1));
        if (isValid(tile.x - 2, tile.y + 1, state)) valid.Add(state.GetTile(tile.x - 2, tile.y + 1));
        if (isValid(tile.x + 2, tile.y + 1, state)) valid.Add(state.GetTile(tile.x + 2, tile.y + 1));
        */
        return valid;
    }

    private bool isValid(int i, int j, BoardModel state) {
        return (state.ValidTileIndex(i, j) && (!state.TileOccupied(i, j) || state.TileOccupiedByOppositeTeam(i, j, isWhite)));
    }

}
