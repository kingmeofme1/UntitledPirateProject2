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
    public void UpdateScore()
    {
        scoreIndex++;
        score.text = scoreIndex.ToString();
    }
}
