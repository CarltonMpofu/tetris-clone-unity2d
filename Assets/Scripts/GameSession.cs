using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    bool startGame;

    bool soundOn;

    bool ghostPieceOn;

    bool startCountDown;

    bool isPaused;
    private void Awake() {
        
        int gameSessionCount = FindObjectsOfType<GameSession>().Length;

        if(gameSessionCount > 1){
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startGame = false;
        soundOn = true;
        ghostPieceOn = true;
        startCountDown = false;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(startGame)
        {
            if(isPaused==false && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F1)))
            { // Pause game
                isPaused = true;
                FindObjectOfType<SpawnShape>().ShowPauseCanvas();
            }

            if(isPaused == true && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.F12) || Input.GetKeyDown(KeyCode.Return)))
            { // Unpause game
                FindObjectOfType<SpawnShape>().HidePauseCanvas();
            }
        }
        
    }

    public void UnPauseGame(){
        isPaused = false;
    } // UnPauseGame

    public bool GetIsPaused(){
        return isPaused;
    } // GetIsPaused

    public void GameOver(){
        startGame = false;
        FindObjectOfType<SpawnShape>().ShowGameOverCanvas();
    } // GameOver

    public void SetStartGame(bool val){
        startGame = val;
    } // SetStartGame

    public bool GetStartGame(){
        return startGame;
    } // GetStartGame

    public void SetSoundValue(bool isOn){
        soundOn = isOn;
    } // SetSoundValue

    public void SetGhostPieceValue(bool isOn){
        ghostPieceOn = isOn;
    } // SetGhostPieceValue

    public bool GetSoundValue(){
        return soundOn;
    } // GetSoundValue

    public bool GetGhostPieceValue(){
        return ghostPieceOn;
    } // GetGhostPieceValue
}
