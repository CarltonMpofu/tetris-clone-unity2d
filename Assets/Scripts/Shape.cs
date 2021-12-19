using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] Vector3 rotationPoint;
    [SerializeField] Color color;


    GamePlay gamePlay;
    GameData gameData;
    Shape ghostPiece;
    GameSession gameSession;

    bool moveShape = false;
    public float shapeSpeed;
    int currentState;
    int nextState;
    bool isSoftDrop = false;
    bool isHardDrop = false;
    int lineDiff;

    private void Awake() {
       gamePlay = FindObjectOfType<GamePlay>();
        // if (!gamePlay)
        // {
        //     Debug.LogError("NO GAMEPLAY!!!");
        // } 
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = 0;
        nextState = 0;

        gameData = FindObjectOfType<GameData>();
        if(!gameData){
            Debug.LogError("NO GAME DISPLAY FOUND!!");
        }

        shapeSpeed = gameData.GetSpeed();

        color = transform.GetComponentInChildren<SpriteRenderer>().color;

        gameSession = FindObjectOfType<GameSession>();
        if(!gameSession){
            Debug.LogError("NO GAME SESSION FOUND!!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession.GetStartGame() && gameSession.GetIsPaused() == false)
        {
            // Move right
            if (Input.GetKeyDown(KeyCode.RightArrow) && moveShape)
            {
                transform.position += Vector3.right;

                if (!gamePlay.IsWithinBorders(transform) || !gamePlay.GridHasSpace(transform))
                { 
                    transform.position += Vector3.left;
                }

                MoveGhostPieceToBottom();
            }

            // Move left
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && moveShape)
            {
                transform.position += Vector3.left;

                if (!gamePlay.IsWithinBorders(transform) || !gamePlay.GridHasSpace(transform))
                {
                    transform.position += Vector3.right;
                }

                MoveGhostPieceToBottom();
            }

            // Rotate right
            else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.X)) && moveShape)
            {
                Transform currentTransform = transform;
                currentTransform.RotateAround(currentTransform.TransformPoint(rotationPoint), Vector3.forward, -90);
                nextState = (currentState + 1) % 4;

                if (!gamePlay.IsWithinBorders(transform) || gamePlay.HasReachedBottom(transform) || !gamePlay.GridHasSpace(transform))
                {
                    bool canRorate = gamePlay.CanRotate(transform, currentState, nextState);
                    if (!canRorate)
                    {
                        currentTransform.RotateAround(currentTransform.TransformPoint(rotationPoint), Vector3.forward, 90);
                        nextState -= 1;
                        nextState = nextState < 0 ? 3 : nextState;
                    }
                    else
                        currentState = nextState;
                }
                else
                {
                    currentState = nextState;
                    gamePlay.playRotateAudio();
                }
                RotateGhostPiece(true);
                MoveGhostPieceToBottom();

            }
            else if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.Z)) && moveShape)
            {
                Transform currentTransform = transform;
                currentTransform.RotateAround(currentTransform.TransformPoint(rotationPoint), Vector3.forward, 90);
                nextState = (currentState - 1) < 0 ? 3 : (currentState - 1);

                if (!gamePlay.IsWithinBorders(transform) || gamePlay.HasReachedBottom(transform) || !gamePlay.GridHasSpace(transform))
                {

                    bool canRorate = gamePlay.CanRotate(transform, currentState, nextState);
                    if (!canRorate)
                    {
                        currentTransform.RotateAround(currentTransform.TransformPoint(rotationPoint), Vector3.forward, -90);
                        nextState = (nextState + 1) % 4;
                    }
                    else
                        currentState = nextState;

                }
                else
                {
                    currentState = nextState;
                    gamePlay.playRotateAudio();
                }
                RotateGhostPiece(false);
                MoveGhostPieceToBottom();

            }

            // Soft drop
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                shapeSpeed = gameData.GetSoftSpeed();
                isSoftDrop = true;
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                isSoftDrop = false;
                shapeSpeed = gameData.GetSpeed();
            }

            // Hard drop
            if (Input.GetKeyDown(KeyCode.Space) && moveShape)
            {
                lineDiff = (int)((transform.position.y + 9) - (ghostPiece.transform.position.y + 9));
                transform.position = ghostPiece.transform.position;
                isHardDrop = true;
            }

        }
        
        // if shape(this gameobject) has no child blocks then destroy the shape
        if(transform.childCount == 0){
            Destroy(gameObject);
        }
    }

    private void RotateGhostPiece(bool clockwise)
    {
        Transform currentTransform = ghostPiece.transform;
        if (clockwise)
            currentTransform.RotateAround(currentTransform.TransformPoint(rotationPoint), Vector3.forward, -90);
        else
        {
            currentTransform.RotateAround(currentTransform.TransformPoint(rotationPoint), Vector3.forward, 90);
        }
    } // RotateGhostPiece

    private void MoveGhostPieceToBottom()
    {
        ghostPiece.transform.position = transform.position;
        while (moveShape)
        {
            ghostPiece.transform.position += Vector3.down;

            if (gamePlay.HasReachedBottom(ghostPiece.transform) || gamePlay.GridHasSpace(ghostPiece.transform) == false)
            {
                ghostPiece.transform.position += Vector3.up;
                break;
            }
        }
    } // MoveGhostPieceToBottom

    public void pauseShape(){
        moveShape = false;
        StopCoroutine(MoveTheShape());
    } // pauseShape

    public void UnPauseShape(){
        moveShape = true;
        StartCoroutine(MoveTheShape());
    } // UnPauseShape

    /// <summary>
    /// Make the shapes ghost piece and then start moving the shape
    /// </summary>
    public void MoveShape(){
        moveShape = true;
        gameSession = FindObjectOfType<GameSession>();
        ghostPiece = Instantiate(this, transform.position, Quaternion.identity);
        foreach(Transform ghostTransform in ghostPiece.transform)
        {
            if (!gameSession.GetGhostPieceValue())
            { // GhostPiece not enabled
                ghostTransform.gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0f);
            }
            else // GhostPiece enabled
                ghostTransform.gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.3f);
        }
        MoveGhostPieceToBottom();
        StartCoroutine(MoveTheShape());
        
    } // MoveShape

    /// <summary>
    /// Move the shape down after the specified number of seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveTheShape()
    {
        while (moveShape)
        {
            if(isHardDrop)
            {
                gameData.AddDropScore(false, lineDiff);
                isHardDrop = false;
            }

            transform.position += Vector3.down;

            if (gamePlay.HasReachedBottom(transform) || gamePlay.GridHasSpace(transform)==false)
            {
                gamePlay.playPlaceShapeAudio();
                Destroy(ghostPiece.gameObject);
                transform.position += Vector3.up;

                gamePlay.AddBlocksToGrid(transform);
                gamePlay.RemoveFullRows(transform);

                if(gamePlay.HasReachedTop(transform)){
                    gameSession.GameOver();
                }

                moveShape = false;
                yield break;
            }
            else{
                if(isSoftDrop)
                    gameData.AddDropScore(true, lineDiff);
            }
            yield return new WaitForSeconds(shapeSpeed);
        }
        
    } // MoveTheShape
   

    public bool IsShapeMoving()
    {
        return moveShape;
    } // IsShapeMoving
}
