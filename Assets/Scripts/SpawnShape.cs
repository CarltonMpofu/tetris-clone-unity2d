using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnShape : MonoBehaviour
{
    [SerializeField] GameObject[] myShapes;
    [SerializeField] Transform parentTransform;
    [SerializeField] Transform spawnPositions1;
    [SerializeField] Transform spawnPositions2;
    [SerializeField] Transform spawnPositions3;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject pauseBackground;
    [SerializeField] GameObject gameoverCanvas;

    // [SerializeField] int shapeIdx = 0;

    GameObject[] nextShapes;

    GameObject currentShape;

    GameSession gameSession;

    bool startTimer;
    float countDownTimer = 4f;

    // Start is called before the first frame update
    void Start()
    {
        startTimer = true;

        gameSession = FindObjectOfType<GameSession>();

        if(!gameSession){
            Debug.LogError("NO GAME SESSION FOUND");
        }

        pauseBackground.SetActive(false);
        pauseCanvas.SetActive(false);
        gameoverCanvas.SetActive(false);
        countdownText.gameObject.SetActive(true);
    }

   void InstantiateShapes()
    {
        for(int i = 0; i < nextShapes.Length; i++)
        {
             int shapeIndex = Random.Range(0, myShapes.Length);
            //  shapeIndex = shapeIdx;
             GameObject shape = Instantiate(myShapes[shapeIndex], transform.position, Quaternion.identity);
             shape.transform.SetParent(parentTransform);
             nextShapes[i] = shape;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(startTimer){
            countDownTimer -= Time.deltaTime;
            float seconds = Mathf.FloorToInt(countDownTimer % 60);
            if(seconds > 0)
            {
                countdownText.text = seconds.ToString();
            }
            else
            {
                startTimer = false;
                countDownTimer = 4f;
                countdownText.gameObject.SetActive(false);
                if (gameSession.GetStartGame() == false)
                { // Start the game
                    nextShapes = new GameObject[4];
                    InstantiateShapes();
                    currentShape = nextShapes[0];
                    SetSpawnPositions();
                    currentShape.transform.position = transform.position + new Vector3(0, 1, 0);
                    currentShape.GetComponent<Shape>().MoveShape();
                    gameSession.SetStartGame(true);
                }
                else
                { // Unpause the game
                    currentShape.GetComponent<Shape>().UnPauseShape();
                    gameSession.UnPauseGame();
                }
            }
        }

        
        if (gameSession.GetStartGame() && gameSession.GetIsPaused()==false && !currentShape.GetComponent<Shape>().IsShapeMoving())
        { // move the next shape
            SetShapeOrder();
            currentShape = nextShapes[0];
            currentShape.GetComponent<Shape>().MoveShape();
        }
    }

    public void ShowGameOverCanvas(){
        pauseCanvas.SetActive(false);
        gameoverCanvas.SetActive(true);
        pauseBackground.SetActive(true);
    } // ShowGameOverCanvas

    public void HidePauseCanvas(){
        pauseCanvas.SetActive(false);
        pauseBackground.SetActive(false);
        startTimer = true;
        countdownText.gameObject.SetActive(true);
    } // HidePauseCanvas

    public void ShowPauseCanvas(){
        currentShape.GetComponent<Shape>().pauseShape();
        pauseCanvas.SetActive(true);
        pauseBackground.SetActive(true);
    } // ShowPauseCanvas


    void SetSpawnPositions(){
        nextShapes[0].transform.position = transform.position;
        nextShapes[1].transform.position = spawnPositions1.position;
        nextShapes[2].transform.position = spawnPositions2.position;
        nextShapes[3].transform.position = spawnPositions3.position;
    } // SetSpawnPositions

    void SetShapeOrder()
    {
        int shapeIndex = Random.Range(0, myShapes.Length);
        // shapeIndex = shapeIdx;
        GameObject shape = Instantiate(myShapes[shapeIndex], transform.position, Quaternion.identity);
        shape.transform.SetParent(parentTransform);
        nextShapes[0] = nextShapes[1];
        nextShapes[1] = nextShapes[2];
        nextShapes[2] = nextShapes[3];
        nextShapes[3] = shape;

        
        SetSpawnPositions();

    } // SetShapeOrder
}
