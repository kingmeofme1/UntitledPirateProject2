using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text score;

    private static int sessionHighScore = 0;
    private int scoreIndex = 0;

    public TMP_Text highScoreText;

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
            highScoreText.text = "New high score! " + sessionHighScore.ToString() + "  points!";
            return true;
        }
        else
        {
            highScoreText.text = "Better luck next time, " + scoreIndex.ToString() + "/" + (sessionHighScore + 1).ToString() + " points for a new high score";
            return false;
        }
    }

}
