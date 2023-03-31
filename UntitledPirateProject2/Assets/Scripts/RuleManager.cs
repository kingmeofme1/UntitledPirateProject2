using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RuleManager : MonoBehaviour
{
    public float shoutDelay = 5.0f; //in seconds
    private float timeOfLastShout = -10f; //just means we immediately get a shout

    public int currentRule = 0;
    public List<string> stringsRules;
    public List<string> stringsBarks;
    public List<string> stringsTaped;

    public GameObject captain;
    public GameObject shoutObject;
    public TMP_Text shoutObjectText;
    public Camera theCamera;

    public bool playerBreakingRules;
    public bool moneyTriggered = false;
    public BoxCollider2D leftSideCollider;
    public List<OverlapObj> listMoney;

    [SerializeField] float textUpMod = 50f;
    [SerializeField] float textWaveSpeed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckRuleBreaks();
        if(Time.time - timeOfLastShout > shoutDelay)
        {
            Shout();
        }
        Vector3 textPos = theCamera.WorldToScreenPoint(captain.transform.position);
        //shoutObject.transform.SetPositionAndRotation(textPos, Quaternion.identity);
        shoutObject.transform.SetPositionAndRotation(new Vector3(textPos.x + 3 * Mathf.Sin(textWaveSpeed * Time.time), textPos.y + textUpMod, textPos.z), Quaternion.identity);
    }

    public void ResetRule() //called when a hole is plugged
    {
        playerBreakingRules = false;
        moneyTriggered = false;
        int newRule = Random.Range(0, stringsRules.Count - 1);
        if(newRule == currentRule)
        {
            currentRule++;
        }
        else
        {
            currentRule = newRule;
        }
        Shout();
    }

    private void Shout()
    {
        timeOfLastShout = Time.time;
        if (playerBreakingRules)
        {
            ShoutBark();
        } else if (false)
        {
            ShoutTaped();
        }
        else
        {
            ShoutRule();
        }

    }

    private void CheckRuleBreaks()
    {
        if (!playerBreakingRules) //if they have broken the rules, no need to keep checking til this is reset
        {
            switch (currentRule)
            {
                case (int)rule.NO_RUN:
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        Debug.Log("player is runing");
                        playerBreakingRules = true;
                    }
                    break;
                case (int)rule.NO_LEFT:
                    CheckRuleLeftSide();
                    break;
                case (int)rule.NO_MONEY:
                    CheckRuleMoney();
                    break;
            }
        }
    }

    private void CheckRuleLeftSide()
    {
        Collider2D[] results = new Collider2D[15]; //15 is arbitrary
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        Physics2D.OverlapCollider(leftSideCollider, filter, results); //uses the actual polygon collider
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i] != null && results[i].TryGetComponent(out PlayerMovement player))
            {
                Debug.Log("player on left side");
                playerBreakingRules = true;
            }
        }
    }

    private void CheckRuleMoney()
    {
        for(int i = 0; i < listMoney.Count; i++)
        {
            if (listMoney[i].IsInRange())
            {
                Debug.Log("Player is stealing me gold");
                moneyTriggered = true;
            }
        }
        if (moneyTriggered)
        {
            playerBreakingRules = true;
        }
    }

    private void ShoutRule() //shouts current rule
    {
        DisplayShoutText(stringsRules[currentRule]);
    }

    private void ShoutBark() //shouts a random bark/quip when player breaks rule
    {
        DisplayShoutText(stringsBarks[Random.Range(0, stringsBarks.Count)]);
    }

    private void ShoutTaped()
    {
        DisplayShoutText(stringsTaped[Random.Range(0, stringsTaped.Count)]);
    }
    

    private void DisplayShoutText(string shoutText)
    {
        shoutObjectText.text = shoutText;
    }

    public enum rule { NO_RUN = 0, NO_LEFT = 1, NO_MONEY = 2, NO_FISH = 3}

}
