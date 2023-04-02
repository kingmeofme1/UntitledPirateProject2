using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text score;

    private static int sessionHighScore = 0;
    private int scoreIndex = 0;

    private void Start()
    {
        score.text = scoreIndex.ToString();
    }
    public void UpdateScore(int scoreMod)
    {
        //INCREASES THE SCORE BY ONE ON FIXING A HOLE
        scoreIndex += scoreMod;
        score.text = scoreIndex.ToString();
    }

    public bool IsHighScore()
    {
        if(scoreIndex > sessionHighScore)
        {
            sessionHighScore = scoreIndex;
            return true;
        }
        else
        {
            return false;
        }
    }

}
