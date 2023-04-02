using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class RuleManager : MonoBehaviour
{
    public float shoutDelay = 5.0f; //in seconds
    private float timeOfLastShout = -10f; //just means we immediately get a shout
    private bool isAnnouncingRule;
    [SerializeField] private Color shoutTextColor;
    [SerializeField] private float delayBeforeRuleIsApplied;

    private int currentRule = 0;
    public float timeToEscapeDuctTape;
    public List<string> stringsRules;
    public List<string> stringsBarks;
    public List<string> stringsTaped;
    public List<AudioSource> audiosRules; //unimplemented
    public List<AudioSource> audiosBarks;
    public List<AudioSource> audiosTaped; //unimplemented
    public bool wasAudioPlayed = false;
    public AudioSource captainTaped;


    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerMovement playerMovement;
    private Vector2 movementInput;

    public GameObject captain;
    public GameObject shoutObject;
    public TMP_Text shoutObjectText;
    public Camera theCamera;

    private bool playerBreakingRules;
    private bool isCaptainTaped;
    private float ruleStartTime;

    [Header("Rule-specific")]
    private bool moneyTriggered = false;
    public BoxCollider2D leftSideCollider;
    public BoxCollider2D rightSideCollider;
    public List<OverlapObj> listMoney;
    public float anthemDuration;
    public GameObject musicNotes;

    [SerializeField] float textUpMod = 50f;
    [SerializeField] float textWaveSpeed = 5f;

    private void Start()
    {
        ResetRule();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timeOfLastShout > shoutDelay)
        {
            Shout();
        }

        if (!isCaptainTaped && !isAnnouncingRule)
            CheckRuleBreaks();
    }

    public void TapeCaptain()
    {
        print("TAPED!");
        captainTaped.Play();
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
        wasAudioPlayed = false; //reset audio
        int newRule = Random.Range(0, stringsRules.Count);
        while(newRule == currentRule)
        {
            newRule = Random.Range(0, stringsRules.Count);
        }
        currentRule = newRule;

        InitializeRule();
        Shout();
        StartCoroutine(nameof(AnnounceRule));
    }

    private IEnumerator AnnounceRule()
    {
        isAnnouncingRule = true;
        shoutObjectText.color = Color.white;

        yield return new WaitForSeconds(delayBeforeRuleIsApplied);

        ruleStartTime = Time.time;
        shoutObjectText.color = shoutTextColor;
        isAnnouncingRule = false;
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
                foreach(OverlapObj money in listMoney)
                {
                    money.gameObject.SetActive(true);
                }
                break;
            case (int)rule.NO_RIGHT:
                rightSideCollider.gameObject.SetActive(true);
                break;
            case (int)rule.ANTHEM:
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
                foreach (OverlapObj money in listMoney)
                {
                    money.gameObject.SetActive(false);
                }
                moneyTriggered = false;
                break;
            case (int)rule.NO_RIGHT:
                rightSideCollider.gameObject.SetActive(false);
                break;
            case (int)rule.ANTHEM:
                print("Deactivating.");
                musicNotes.SetActive(false);
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
                    CheckRuleWalking();
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

    private void CheckRuleWalking()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && playerMovement.isMoving)
        {
            playerBreakingRules = true;
        }
    }

    private void CheckRuleAnthem()
    {
        if(Time.time >= ruleStartTime + anthemDuration)
        {
            ResetRule();
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            musicNotes.SetActive(true);
        }
        else
        {
            playerBreakingRules = true;
        }
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
        else if (isAnnouncingRule)
        {
            ShoutAnnouncement();
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
    
    private void ShoutAnnouncement() //shouts next rule rule
    {
        DisplayShoutText(stringsRules[currentRule]);
    }

    private void ShoutBark() //shouts a random bark/quip when player breaks rule
    {
        DisplayShoutText(stringsBarks[Random.Range(0, stringsBarks.Count)]);
        PlayShoutAudio(audiosBarks[Random.Range(0, audiosBarks.Count)]);
    }

    private void ShoutTaped()
    {
        DisplayShoutText(stringsTaped[Random.Range(0, stringsTaped.Count)]);
    }
    

    private void DisplayShoutText(string shoutText)
    {
        shoutObjectText.text = shoutText;
    }

    private void PlayShoutAudio(AudioSource shoutAudio)
    {
        if (!wasAudioPlayed)
        {
            wasAudioPlayed = true;
            shoutAudio.Play();
        }
        
    }
    #endregion

    public bool IsRulesBroken()
    {
        return playerBreakingRules;
    }

    public enum rule { NO_RUN = 0, NO_LEFT = 1, NO_MONEY = 2, NO_FISH = 3, NO_RIGHT = 4, ANTHEM = 5}
    
}
