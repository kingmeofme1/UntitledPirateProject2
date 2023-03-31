using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoleManager : MonoBehaviour
{
    [SerializeField] private Sprite holeLeaking;
    [SerializeField] private Sprite holeFixed;

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

    public void IsFixed(int holeID) //fixes hole, gets a new hole!
    {
        if(holeID == activeHole && holeList[holeID].TryGetComponent(out SpriteRenderer spriteRenderer)) //check we fixed an actually broken hole
        {
            spriteRenderer.sprite = holeFixed;
            scoreManager.UpdateScore();
            int nextHole = Random.Range(0, holeList.Count - 1); //picks a random hole, -1 for the case of being a dupe.
            if(nextHole == activeHole) //if it is a dupe, since we offset by 1 above, we can increment to get a distinct hole.
            {
                activeHole++;
            }
            else //if the hole was already different, we can just use it then!
            {
                activeHole = nextHole;
            }
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
            spriteRenderer.sprite = holeLeaking;
        }
    }

    
}
