using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    public GameObject gameLoseUI;
    public GameObject gameWinUI;
    bool gameIsOver;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        Guard.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        Guard2.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        Player.NotFinish += ShowGameLoseUI;

        FindObjectOfType<Player>().OnReachedEndOfLevel += ShowGameWinUI;
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("stealth");
                Time.timeScale = 1;
                DontShowGameLoseUI();
            }
        }
	}

    void ShowGameWinUI()
    {
        OnGameOver(gameWinUI);
    }

    void ShowGameLoseUI()
    {
        OnGameOver(gameLoseUI);
    }
    void DontShowGameLoseUI()
    {
        OnGameOver2(gameLoseUI);
        OnGameOver2(gameWinUI);
    }

    void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        gameIsOver = true;
        Guard.OnGuardHasSpottedPlayer -= ShowGameLoseUI;
        FindObjectOfType<Player>().OnReachedEndOfLevel -= ShowGameWinUI;
    }
    void OnGameOver2(GameObject gameOverUI)
    {
        gameOverUI.SetActive(false);
        gameIsOver = false;
    }
}
