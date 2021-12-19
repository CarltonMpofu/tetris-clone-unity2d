using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSettingsScene(){
        SceneManager.LoadScene("Settings Scene");
    }

    public void LoadStartScene(){
        GameSession gameSession = FindObjectOfType<GameSession>();
        gameSession.SetStartGame(false);
        gameSession.UnPauseGame();
        SceneManager.LoadScene("Start Scene");
    }

    public void LoadGameScene(){
        
        SceneManager.LoadScene("Game Scene");
    }

    public void LoadHelpScene(){
        SceneManager.LoadScene("Help Scene");
    }
}
