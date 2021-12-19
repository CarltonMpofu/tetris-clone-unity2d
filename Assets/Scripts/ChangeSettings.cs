using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSettings : MonoBehaviour
{

    Toggle toggle;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(onToggleValueChanged);

        gameSession = FindObjectOfType<GameSession>();
        if(!gameSession)
            Debug.LogError("NO GAME SESSION FOUND!!");

        if(gameObject.CompareTag("Sound")){
            toggle.isOn =  gameSession.GetSoundValue();
        }
        else{
            toggle.isOn = gameSession.GetGhostPieceValue();
        }
    }

    private void onToggleValueChanged(bool isOn){
        if(gameObject.CompareTag("Sound")){
            gameSession.SetSoundValue(isOn);
        }
        else{
            gameSession.SetGhostPieceValue(isOn);
        }
    } // onToggleValueChanged
}
