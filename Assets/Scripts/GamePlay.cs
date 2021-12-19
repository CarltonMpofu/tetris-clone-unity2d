using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    [SerializeField] Transform borderLeft;
    [SerializeField] Transform borderRight;
    [SerializeField] int xOffset = 4;
    [SerializeField] int yOffset = 9;
    [SerializeField] AudioClip rotateAudioClip;
    [SerializeField] AudioClip placeShapeAudioClip;
    [SerializeField] AudioClip removeRowsAudioClip;

    GameData gameData;
    AudioSource audioSource;

    Transform[,] grid;
    int width = 10;
    int height = 21;

    private void Awake() {
        grid = new Transform[width, height];
        for (int row = 0; row < width; row++)
        {
            for (int column = 0; column < height; column++)
            {
                grid[row, column] = null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        if(!gameData){
            Debug.LogError("NO GAME SESSION FOUND!!");
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void playRemoveRowsShapeAudio(){
        if (FindObjectOfType<GameSession>().GetSoundValue())
        {
            audioSource.clip = removeRowsAudioClip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    } // playRemoveRowsShapeAudio

    public void playPlaceShapeAudio(){
        if (FindObjectOfType<GameSession>().GetSoundValue())
        {
            audioSource.clip = placeShapeAudioClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
    } // playPlaceShapeAudio

    public void playRotateAudio(){
        if (FindObjectOfType<GameSession>().GetSoundValue())
        {
            audioSource.clip = rotateAudioClip;
            audioSource.volume = 0.5f;
            audioSource.Play();
        }
    } // playRotateAudio

    public bool HasReachedTop(Transform shapeTransform){
        foreach (Transform blockTransform in shapeTransform)
        {
            Vector2 blockPosition = RoundVector(blockTransform.position);
            int blockXindex = (int)blockPosition.x + xOffset;
            int blockYindex = (int)blockPosition.y + yOffset;
            if(blockYindex >= height-2){
                return true;
            }
        }
        return false;
    } // HasReachedTop

    public bool CanRotate(Transform shapeTransform, int currentState, int nextState)
    { 
        foreach (string test in WallKick.Test(shapeTransform.gameObject.tag, currentState, nextState))
        {
            string[] testValues = test.Split(',');
            int x = int.Parse(testValues[0]);
            int y = int.Parse(testValues[1]);

            Vector2 currentPosition = RoundVector(shapeTransform.position);
            Vector2 newPosition = new Vector2(currentPosition.x + x, currentPosition.y + y);
            shapeTransform.position = newPosition;

            if(!HasReachedBottom(shapeTransform) && IsWithinBorders(shapeTransform) && GridHasSpace(shapeTransform))
            {
                return true;
            }
            else
            {
                currentPosition = RoundVector(shapeTransform.position);
                newPosition = new Vector2(currentPosition.x - x, currentPosition.y - y);
                shapeTransform.position = newPosition;
            }
       }
        return false;
    } // CanRotate

    /// <summary>
    /// Move blocks down to fill in missing gap
    /// </summary>
    /// <param name="removedRows"></param>
    private void MoveBlocksDown(List<int> removedRows)
    {
        if (removedRows.Count == 0) // No rows to remove
            return;
        
        removedRows.Sort();
        playRemoveRowsShapeAudio();

        for(int i = removedRows.Count-1; i >= 0; i--)
        { // Start with rows at the top
            for(int row = removedRows[i]+1; row < height; row++)
            { // Move all blocks above current row to the bottom
                for(int blockIndex = 0; blockIndex < width; blockIndex++)
                { // Move the block
                    Transform blockTransform = grid[blockIndex, row];
                    if (blockTransform != null)
                    {
                        Vector2 blockPosition = blockTransform.position;

                        blockTransform.position = new Vector2(blockPosition.x, blockPosition.y - 1);

                        grid[blockIndex, row] = null;
                        grid[blockIndex, row - 1] = blockTransform;
                    }
                }
            }
        }
    } // MoveBlocksDown

    /// <summary>
    /// Destroys all blocks in a full row
    /// </summary>
    /// <param name="row"></param>
    private void RemoveBlocks(int row)
    {
        for (int blockIndex = 0; blockIndex < width; blockIndex++)
        {
            Destroy(grid[blockIndex, row].gameObject);
        }
    } // RemoveBlocks

    /// <summary>
    /// Checks whether the row is full(for each row covered by shape)
    /// If the row is full then it removes all the blocks at the row.
    /// </summary>
    /// <param name="shapeTransform"></param>
    public void RemoveFullRows(Transform shapeTransform)
    {
        List<int> rows = GetRows(shapeTransform);
        List<int> removedRows = new List<int>();

        int lineCount = 0;
        foreach(int row in rows)
        {
            int counter = 0;
            for (int blockIndex = 0; blockIndex < width; blockIndex++)
            {
                if (grid[blockIndex, row] != null)
                    counter++;
            }
            
            if( counter == 10)
            { // Row is full
                RemoveBlocks(row);
                removedRows.Add(row);
                lineCount += 1;
            }
        }
        gameData.AddRowScore(lineCount);
        MoveBlocksDown(removedRows);

    } // RemoveFullRows

    /// <summary>
    /// Returns the row indexes the shape covers 
    /// </summary>
    /// <param name="shapeTransform"></param>
    /// <returns></returns>
    private List<int> GetRows(Transform shapeTransform)
    {
        List<int> rows = new List<int>();
        foreach (Transform blockTransform in shapeTransform)
        {
            Vector2 blockPosition = RoundVector(blockTransform.position);
            if (!rows.Contains((int)blockPosition.y + yOffset))
            {
                rows.Add((int)blockPosition.y + yOffset);
            }
        }
        return rows;
    }



    private Vector2 RoundVector(Vector2 blockPosition)
    {
        return new Vector2(Mathf.Round(blockPosition.x), Mathf.Round(blockPosition.y));
    } // RoundVector

    /// <summary>
    /// Check if the current shape is still within the left and right border
    /// </summary>
    /// <returns></returns>
    public bool IsWithinBorders(Transform shapeTransform)
    {
        foreach (Transform blockTransform in shapeTransform)
        {
            int blockPosition = (int)RoundVector(blockTransform.position).x;
            int rightBorderPos = (int)(borderRight.position.x - 0.5);
            int leftBorderPos = (int)(borderLeft.position.x + 0.5);
            if (blockPosition > rightBorderPos || blockPosition < leftBorderPos)
            {
                return false;
            }
        }
        return true;
    } // IsWithinBorders

    public void AddBlocksToGrid(Transform shapeTransform)
    {
        foreach (Transform blockTransform in shapeTransform)
        {
            Vector2 blockPosition = RoundVector(blockTransform.position);
            int blockXindex = (int)blockPosition.x + xOffset;
            int blockYindex = (int)blockPosition.y + yOffset;
            grid[blockXindex, blockYindex] = blockTransform;
        }
    } // AddBlocksToGrid

    public bool GridHasSpace(Transform shapeTransform)
    {
        foreach (Transform blockTransform in shapeTransform)
        {
            Vector2 blockPosition = RoundVector(blockTransform.position);
            int blockXindex = (int)blockPosition.x + xOffset;
            int blockYindex = (int)blockPosition.y + yOffset;
            if (grid[blockXindex, blockYindex] != null)
            {
                return false;
            }
        }
        return true; 
    } // GridHasSpace

    /// <summary>
    ///Check if the current shape has reached the bottom of the grid
    /// </summary>
    /// <returns></returns>
    public bool HasReachedBottom(Transform shapeTransform)
    {
        foreach (Transform blockTransform in shapeTransform)
        {
            int blockPosition = (int)blockTransform.position.y + yOffset;
            if (blockPosition < 0)
            {
                return true;
            }
        }

        return false;
    } // HasReachedBottom
}
