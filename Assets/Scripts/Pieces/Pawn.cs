using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : GenericPiece
{
    public Pawn() {
        pieceValue = 1;
        pieceType = PieceType.Pawn;
    }

    public override List<TileData> DetermineValidTiles(BoardModel state) {
        List<TileData> valid = new List<TileData>();
        int direction = isWhite ? 1 : -1;
        /*
        Vector2Int tile = currentTile.tile;
        //straight 
        if (state.ValidTileIndex(tile.x, tile.y + direction) && !state.TileOccupied(tile.x, tile.y + direction)) {
            valid.Add(state.GetTile(tile.x, tile.y + direction));
        }
        //take left
        if (state.ValidTileIndex(tile.x - 1, tile.y + direction) && state.TileOccupiedByOppositeTeam(tile.x-1, tile.y + direction, isWhite))
        {
            valid.Add(state.GetTile(tile.x-1, tile.y + direction));
        }
        //take right
        if (state.ValidTileIndex(tile.x + 1, tile.y + direction) && state.TileOccupiedByOppositeTeam(tile.x+1, tile.y + direction, isWhite))
        {
            valid.Add(state.GetTile(tile.x+1, tile.y + direction));
        }
        //First move, you can move two spaces. 
        if (state.ValidTileIndex(tile.x, tile.y + 2 * direction) && moveCounter == 0 && !state.TileOccupied(tile.x, tile.y + 2*direction)) {
            valid.Add(state.GetTile(tile.x, tile.y + 2*direction));
        }
        */
        return valid;
    }
}
