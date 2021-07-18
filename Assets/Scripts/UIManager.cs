using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class UIManager : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject pauseMenu;


    public bool paused = false;

    public void Awake()
    {
        pauseMenu.GetComponent<Canvas>().enabled = false;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) paused = !paused;
        pauseMenu.GetComponent<Canvas>().enabled = paused;
        mainUI.GetComponentInChildren<Text>().text = FindObjectOfType<GameManager>().whiteTurn ? "white" : "black";
    }


    public void onRestartClick() {
        FindObjectOfType<GameManager>().NewGame();
        paused = false;
    }
}
