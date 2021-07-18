using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAgent : PlayerAgent
{

    private void Update()
    {
        if (!currentTurn || gm.gameOver) return;

        List<Movement> possibleMoves = EnumerateValidMovements();
        int index = (int)Mathf.Floor(Random.Range(0.0f, (float)possibleMoves.Count));

        if (possibleMoves.Count == 0)
        {
            Debug.Log("I got no moves");
            Debug.Break();
            return;
        }
        //Debug.Log("Moving " + possibleMoves[index].movedPiece.name + " to " + possibleMoves[index].tileTo.name);
        SubmitMovement(possibleMoves[index]);
    }

}

    
