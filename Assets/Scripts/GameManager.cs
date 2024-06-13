using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private bool cursorCaptured = false;

    public bool gameWon = false;

    [SerializeField] public GameObject gameOverCanvas;
    [SerializeField] public TMP_Text winloseText;

    [SerializeField] private SceneScript sceneScript;
    [SerializeField] private TMP_Text collectedtext;

    // Total number of items(will be fixed, but due to some glitches setting it manually)
    [SerializeField] int itemsToCollect;
    private int itemsCollected = 0;

    void Start()
    {
        gameOverCanvas.SetActive(false);
    }

    public void GameEnd()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        if (gameWon) {
            winloseText.text = "YOU WIN !!";
        }
        else
        {
            winloseText.text = "YOU LOSE !!";
        }
    }

    public void CollectItem()
    {
        itemsCollected++;
        collectedtext.text = "Collected: " + itemsCollected.ToString();
        if(itemsCollected >= itemsToCollect)
        {
            gameWon = true;
            GameEnd();
        }
    }

    void CursorCapture(bool val)
    {
        Cursor.visible = val;
        Cursor.lockState = val ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Leave"))
        {
            cursorCaptured = !cursorCaptured;
            CursorCapture(cursorCaptured);
        }

        // Test message
        if (Input.GetButtonDown("Jump")) {
            sceneScript.ButtonSendMessage("I'm Jumping");
        }
    }
}
