using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameData : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI linesText;

    int score, level, lines;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        level = 1;
        lines = 0;

        UpdateText();
    }

    public float GetSpeed(){
        return Mathf.Pow((0.8f - ((level - 1) * 0.007f)), level - 1);
    } // GetSpeed

    public float GetSoftSpeed(){
        return Mathf.Pow((0.8f - ((level - 1 + 8) * 0.007f)), level - 1 + 8);
    } // GetSoftSpeed

    void UpdateText(){
        levelText.text = level.ToString();
        scoreText.text = score.ToString();
        linesText.text = lines.ToString();
    } // UpdateText

    public void AddDropScore(bool isSoft, int lineCount)
    {
        if(isSoft) // Soft drop
            score += 1;
        else
        { // Hard drop
            score += (lineCount * 2);
        }

        UpdateText();
    } // AddDropScore

    public void AddSoftDropScore(){
        score += 1;

        UpdateText();
    } // AddSoftDropScore

    public void AddRowScore(int lineCount){
        if(lineCount == 1){
            score += (100 * level);
            UpdateLines(lineCount);
        }
        else if(lineCount == 2){
            score += 300 * level;
            UpdateLines(lineCount);
        }
        else if(lineCount == 3){
            score += 500 * level;
            UpdateLines(lineCount);
        }else if(lineCount == 4)
        {
            score += 800 * level;
            UpdateLines(lineCount);
        }
        else{}

        UpdateText();
    } // AddRowScore

    private void UpdateLines(int lineCount){
        for (int i = 0; i < lineCount; i++){
            lines += 1;
            if ((float)lines % 10.0f == 0)
            {
                level += 1;
            }
        }
            
    } // UpdateLines
}


