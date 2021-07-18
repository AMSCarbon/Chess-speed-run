using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAgent : PlayerAgent {

    Camera cam;

    public float mouseScroll = 3.0f;
    public GameObject legalMoveHighlighter;
    private List<GameObject> legalMoveHighlights = new List<GameObject>();

    public void Start()
    {
        cam = Camera.main;
        base.Start();
    }

    public void Update()
    {
        if (currentTurn && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {

                Transform objectHit = hit.transform;
                Debug.DrawLine(ray.origin, hit.point, Color.red, 10);
                PieceView PieceView = objectHit.GetComponent<PieceView>();
                if (PieceView != null)
                {
                    // Is the piece from my team uwu
                    if (PieceView.data.isWhite == isWhite)
                    {
                        SelectPiece(PieceView);
                        CreateHighlight();
                    }
                    else if (PieceView.data.isWhite != isWhite && selectedPiece != null && validMovements.Contains(PieceView.data.currentTile))
                    {
                        SubmitMovement(BuildMovement(selectedPiece, PieceView.currentTile));
                    }
                }

                TileView ts = objectHit.GetComponent<TileView>();
                //move to empty space
                if (ts != null)
                {
                    if (selectedPiece != null && validMovements.Contains(ts.data))
                    {
                        SubmitMovement(BuildMovement(selectedPiece, ts));
                    }
                }
            }
        }
       

      
    }

    private void CreateHighlight() {
        DestroyHighlights();
        legalMoveHighlights = new List<GameObject>();
        foreach (TileView ts in TileView.GetTileViewsFromData(validMovements)) {
            legalMoveHighlights.Add( Instantiate(legalMoveHighlighter, ts.transform));
        }
    }
    
    private void DestroyHighlights() {
        foreach (GameObject go in legalMoveHighlights)
        {
            Destroy(go);
        }
    }

    public override void EndTurn()
    {
        DestroyHighlights();
        base.EndTurn();
    }

    public override void ClearSelection()
    {
        base.ClearSelection();
        DestroyHighlights();
    }

}
