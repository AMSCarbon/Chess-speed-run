using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    public PlayerAgent whitePlayer;
    public PlayerAgent blackPlayer;
    public BoardModel board;
    public float maxMovement = 0.05f;
    public int games = 0;
    public int staleMates = 0;
    public int whiteWins = 0;
    public int blackWins = 0;
    public GameObject whiteTeamPrefab;
    public GameObject blackTeamPrefab;
    public bool executingMovement = false;
    public bool whiteTurn = true;

    public Mesh queenMesh;
    private bool needToQueen = false; // flag to show pawn should be queened once it finishes moving. 
    public int seed;
    public bool gameOver = false;
    private int maxStartDelay = 300;
    private int currentDelay = 0;
    public Movement latestMovement;
    private List<Movement> moveHistory = new List<Movement>();
    public void Awake()
    {
        seed = (int)DateTime.Now.ToBinary();
        UnityEngine.Random.InitState(seed);
        NewGame();
    }

    public void NewGame() {
        gameOver = false;
        board = new BoardModel();
        //scrub the board clean.
        foreach (TileView tile in FindObjectsOfType<TileView>()) {
            tile.empty();
            tile.Register();
        }
        //destroy the players, if they exist
        if (whitePlayer != null) {
            Destroy(whitePlayer.gameObject);
        }
        if (blackPlayer != null) {
            Destroy(blackPlayer.gameObject);
        }
        //recreate the players.
        GameObject whiteTeam = Instantiate(whiteTeamPrefab);
        GameObject blackTeam = Instantiate(blackTeamPrefab);
        whitePlayer = whiteTeam.GetComponent<PlayerAgent>();
        blackPlayer = blackTeam.GetComponent<PlayerAgent>();
        whitePlayer.enemy = blackPlayer;
        blackPlayer.enemy = whitePlayer;
        whiteTurn = true;
        currentDelay = maxStartDelay;
        games++;
        Debug.Log("New game started");

    }

    public bool QueryMovement(Movement movement)
    {
        return true;
    }

    public BoardModel GetBoardState() {
        return board;
    }

    public void SubmitMovement(Movement movement) {
        executingMovement = true;
        currentAgent().EndTurn();
        moveHistory.Add(movement);
        latestMovement = movement;
        PieceType typeBefore = movement.movedPiece.data.pieceType;
        board.UpdateBoardModel(movement.movedPiece.data, movement.tileFrom.data, movement.tileTo.data);
        PieceType typeAfter = movement.movedPiece.data.pieceType;
        if (typeBefore == PieceType.Pawn && typeAfter == PieceType.Queen)
        {
            needToQueen = true;
        }
        if (movement.pieceTaken) {
            if (movement.takenPiece.data.pieceType == PieceType.King)
            {
                Debug.Log("Trying to take the king?");
                Debug.Break();
            }
        }  
      
    }

    public void Update()
    {

        if (gameOver) NewGame();
        if (currentDelay > 0) {
            currentDelay--;
            if(currentDelay == 0) whitePlayer.StartTurn();
            return;
        }
        if (!executingMovement) return;
        // Will pass when the view has finished updating. 
        if (!UpdateView()) return; 
        executingMovement = false;
        if (BoardModel.TestForCheck(board, !whiteTurn))
        {

            if (BoardModel.TestForCheckMate(board, !whiteTurn))
            {
                Debug.Log("Checkmate");
                gameOver = true;
                if (whiteTurn) whiteWins++;
                else blackWins++;
            }
        }
        else if (BoardModel.TestForStaleMate(board, !whiteTurn))
        {
            Debug.Log("Stalemate");
            gameOver = true;
            staleMates++;
        }
        nextAgent().StartTurn();
        whiteTurn = !whiteTurn;
       
    }

    private bool UpdateView() {
        Vector3 from = latestMovement.movedPiece.transform.position;
        Vector3 t = latestMovement.tileTo.transform.position;
        Vector3 target = new Vector3(t.x, from.y, t.z);
        Vector3 movement = Vector3.MoveTowards(latestMovement.movedPiece.transform.position, target, maxMovement);
        float distance = Vector3.Distance(from, movement);

        //Changed to make the AI run faster.
        latestMovement.movedPiece.transform.position = movement;
        //finish moving the PieceView
        if (distance < 0.0001f)
        {
            if (latestMovement.pieceTaken)
            {
                latestMovement.takenPiece.RemovePiece();
            }
            latestMovement.tileTo.occupyingPieceView = latestMovement.movedPiece;
            latestMovement.movedPiece.currentTile = latestMovement.tileTo;
            if (needToQueen) {
                UpdatePawnToQueen(latestMovement.movedPiece);
                needToQueen = false;
            }
            return true;
        }
        return false;
    }

    //Pawn made it to otherside of the board and becomes a queen. Model updates itself, we needa update the view here.
    private void UpdatePawnToQueen(PieceView piece) {
        Debug.Log("Updating mesh for new queen");
        piece.GetComponent<MeshFilter>().mesh = queenMesh;
    }

    private PlayerAgent currentAgent() {
        return whiteTurn ? whitePlayer:blackPlayer;
    }

    private PlayerAgent nextAgent()
    {
        return whiteTurn ? blackPlayer : whitePlayer;
    }

    public void SwitchTurns() {
        currentAgent().EndTurn();
        nextAgent().StartTurn();
        whiteTurn = !whiteTurn;
    }

    public void testCheck() {
        Debug.Log("Player is " + (BoardModel.TestForCheck(board, whiteTurn) ? "" : " not ") + "checked");
    }

    public void testCheckmate()
    {
        if (BoardModel.TestForCheck(board, whiteTurn))
        {
            Debug.Log("Player is " + (BoardModel.TestForCheckMate(board, whiteTurn) ? "" : " not ") + "in checkmate");
        }
        else {
            Debug.Log("player is not in checkmate");
        }
    }

    public void testStaleMate() {
        if (BoardModel.TestForCheck(board, whiteTurn)) Debug.Log("Player is in check, therefor they can't be in stalemate");
        else Debug.Log("Player is " + (BoardModel.TestForStaleMate(board, whiteTurn) ? "" : " not ") + "in stalemate");
    }


    public void printBoardState() {
        board.printState();
    }

    public void UndoMove() {
        if (moveHistory.Count == 0) {
            Debug.Log("No history to undo");
            return;
        }

        latestMovement = moveHistory[moveHistory.Count - 1];
        moveHistory.RemoveAt(moveHistory.Count - 1);
        executingMovement = false;
        currentAgent().EndTurn();
        Debug.Log("Undoing move");
        //move piece back to the previous spot.
        PieceView moved = latestMovement.movedPiece;
        Debug.Log("moved piece: " +  moved.name);
        board.UpdateBoardModel(moved.data, latestMovement.tileTo.data, latestMovement.tileFrom.data);
        moved.transform.position = latestMovement.tileFrom.transform.position;

        if (latestMovement.pieceTaken) {
            PieceView restore = latestMovement.takenPiece;
            restore.transform.position = restore.currentTile.transform.position;
            restore.currentTile.data.fill(restore.data);
            restore.data.isRemoved = false;
        }
        latestMovement.movedPiece.data.moveCounter -= 2; 

        if(moveHistory.Count > 0) latestMovement = moveHistory[moveHistory.Count - 1];
        nextAgent().StartTurn();
        whiteTurn = !whiteTurn;
    }
}
