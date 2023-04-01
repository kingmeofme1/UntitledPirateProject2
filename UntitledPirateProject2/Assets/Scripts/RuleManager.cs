using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RuleManager : MonoBehaviour
{
    public float shoutDelay = 5.0f; //in seconds
    private float timeOfLastShout = -10f; //just means we immediately get a shout

    public int currentRule = 0;
    public float timeToEscapeDuctTape;
    public List<string> stringsRules;
    public List<string> stringsBarks;
    public List<string> stringsTaped;

    public Transform playerTransform;

    public GameObject captain;
    public GameObject shoutObject;
    public TMP_Text shoutObjectText;
    public Camera theCamera;

    private bool playerBreakingRules;
    private bool isCaptainTaped;

    [Header("Rule-specific")]
    private bool moneyTriggered = false;
    public BoxCollider2D leftSideCollider;
    public BoxCollider2D rightSideCollider;
    public List<OverlapObj> listMoney;

    private Vector3 lastPlayerPosition;

    [SerializeField] float textUpMod = 50f;
    [SerializeField] float textWaveSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timeOfLastShout > shoutDelay)
        {
            Shout();
        }
        Vector3 textPos = theCamera.WorldToScreenPoint(captain.transform.position);
        shoutObject.transform.SetPositionAndRotation(new Vector3(textPos.x + 3 * Mathf.Sin(textWaveSpeed * Time.time), textPos.y + textUpMod, textPos.z), Quaternion.identity);

        if (!isCaptainTaped)
            CheckRuleBreaks();
    }

    public void TapeCaptain()
    {
        print("TAPED!");
        isCaptainTaped = true;
        StartCoroutine(nameof(EscapeDuctTape));
    }

    private IEnumerator EscapeDuctTape()
    {
        yield return new WaitForSeconds(timeToEscapeDuctTape);
        isCaptainTaped = false;
    }

    #region Rules

    public void ResetRule() //called when a hole is plugged
    {
        ExitRule();

        int newRule = Random.Range(0, stringsRules.Count);
        while(newRule == currentRule)
        {
            newRule = Random.Range(0, stringsRules.Count);
        }
        currentRule = newRule;

        InitializeRule();
        Shout();
    }

    // -- Things that only need to be done once per rule are here.
    private void InitializeRule()
    {
        switch (currentRule)
        {
            case (int)rule.NO_RUN:
                break;
            case (int)rule.NO_LEFT:
                 leftSideCollider.gameObject.SetActive(true);
                break;
            case (int)rule.NO_MONEY:
                break;
            case (int)rule.NO_RIGHT:
                rightSideCollider.gameObject.SetActive(true);
                break;
            case (int)rule.ANTHEM:
                lastPlayerPosition = playerTransform.position;
                break;
        }
    }

    private void ExitRule()
    {
        playerBreakingRules = false;
        switch (currentRule)
        {
            case (int)rule.NO_RUN:
                break;
            case (int)rule.NO_LEFT:
                leftSideCollider.gameObject.SetActive(false);
                break;
            case (int)rule.NO_MONEY:
                moneyTriggered = false;
                break;
            case (int)rule.NO_RIGHT:
                rightSideCollider.gameObject.SetActive(false);
                break;
            case (int)rule.ANTHEM:
                break;
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
                case (int)rule.NO_RIGHT:
                    CheckRuleRightSide();
                    break;
                case (int)rule.ANTHEM:
                    CheckRuleAnthem();
                    break;
            }
        }
    }

    private void CheckRuleAnthem()
    {
        if (lastPlayerPosition != playerTransform.position)
        {
            Debug.Log("Player did not stay still for the anthem");
            playerBreakingRules = true;
        }
        
        lastPlayerPosition = playerTransform.position;
    }

    private void CheckRuleLeftSide()
    {
        CheckRuleSide(leftSideCollider);
    }
    
    private void CheckRuleRightSide()
    {
        CheckRuleSide(rightSideCollider);
    }

    private void CheckRuleSide(BoxCollider2D sideCollider)
    {
        Collider2D[] results = new Collider2D[15]; //15 is arbitrary
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        Physics2D.OverlapCollider(sideCollider, filter, results); //uses the actual polygon collider
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i] != null && results[i].TryGetComponent(out PlayerMovement player))
            {
                Debug.Log($"player on {sideCollider.name}.");
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

    #endregion

    #region Shouts
    private void Shout()
    {
        timeOfLastShout = Time.time;
        if (playerBreakingRules)
        {
            ShoutBark();
        }
        else if (isCaptainTaped)
        {
            ShoutTaped();
        }
        else
        {
            ShoutRule();
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
    #endregion

    public bool IsRulesBroken()
    {
        return playerBreakingRules;
    }

    public enum rule { NO_RUN = 0, NO_LEFT = 1, NO_MONEY = 2, NO_FISH = 3, NO_RIGHT = 4, ANTHEM = 5}
    
}
