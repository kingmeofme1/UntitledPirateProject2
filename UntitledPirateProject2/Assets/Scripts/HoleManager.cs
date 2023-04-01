using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoleManager : MonoBehaviour
{
    [SerializeField] private Sprite holeLeaking;
    [SerializeField] private Sprite holeWarning;
    [SerializeField] private Sprite holeFixed;
    private SpriteRenderer activeHoleSprite;

    [Header("Water Leak")]
    [Range(0, 100)]
    [SerializeField] private float waterPercentagePerSecond;

    [SerializeField] private float timeBeforeLeak;
    private bool isHoleLeaking;
    private float waterMeterPercentage;


    public List<GameObject> holeList;
    public int activeHole = 0;
    public ScoreManager scoreManager;
    public RuleManager ruleManager;

    private void Start()
    {
        if(holeList != null && holeList.Count > 0)
        {
            activeHole = Random.Range(0, holeList.Count);
            ActivateHole();
        }
        else
        {
            Debug.Log("Initialization error, need more holes");
        }
    }

    private void Update()
    {
        if (isHoleLeaking)
            waterMeterPercentage += waterPercentagePerSecond * Time.deltaTime;

        print(Mathf.Round(waterMeterPercentage));
    }


    public void IsFixed(int holeID) //fixes hole, gets a new hole!
    {
        if(holeID == activeHole && holeList[holeID].TryGetComponent(out SpriteRenderer spriteRenderer)) //check we fixed an actually broken hole
        {
            spriteRenderer.sprite = holeFixed;
            isHoleLeaking = false;

            scoreManager.UpdateScore();

            // pick a new hole
            int nextHole = Random.Range(0, holeList.Count);
            while(nextHole == activeHole)
            {
                nextHole = Random.Range(0, holeList.Count);
            }
            activeHole = nextHole;

            ActivateHole();
            ruleManager.ResetRule(); //update's the rules manager to have a new rule
        }
        else
        {
            Debug.Log("wrong hole!");
        }
    }


    public void ActivateHole()
    {
        if(holeList[activeHole].TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            activeHoleSprite = spriteRenderer;
            spriteRenderer.sprite = holeWarning;
            StartCoroutine(nameof(DelayBeforeLeak));
        }
    }

    private IEnumerator DelayBeforeLeak()
    {
        yield return new WaitForSeconds(timeBeforeLeak);
        activeHoleSprite.sprite = holeLeaking;
        isHoleLeaking = true;
    }

    
}
