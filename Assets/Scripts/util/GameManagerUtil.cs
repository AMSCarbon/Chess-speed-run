using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GameManager))]
public class GameManagerUtil : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager manager = (GameManager)target;

        if (GUILayout.Button("Switch turns"))
        {
            manager.SwitchTurns();
        }

        if (GUILayout.Button("Test check"))
        {
            manager.testCheck();
        }

        if (GUILayout.Button("Test check mate"))
        {
            manager.testCheckmate();
        }

        if (GUILayout.Button("Test stale mate"))
        {
            manager.testStaleMate();
        }

        if (GUILayout.Button("Print board state"))
        {
            manager.printBoardState();
        }

        if (GUILayout.Button("Undo"))
        {
            manager.UndoMove();
        }

    }
}
