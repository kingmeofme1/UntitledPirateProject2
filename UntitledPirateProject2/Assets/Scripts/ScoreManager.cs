using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text score;

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
}
