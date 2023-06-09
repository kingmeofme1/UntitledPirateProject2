using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HoleManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Slider waterSlider;

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

    public AudioSource newHoleSound;
    public AudioSource holeTapedSound;
    public AudioSource victorySound;
    public AudioSource defeatSound;


    [SerializeField] private ParticleSystem holeFixedParticles;

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
        {
            waterMeterPercentage += waterPercentagePerSecond * Time.deltaTime;
            waterSlider.value = waterMeterPercentage;

            if (waterMeterPercentage >= 100)
            {
                ruleManager.KillAllAudio();
                PlayFanfare();
                Time.timeScale = 0;
                gameOverMenu.SetActive(true);
                isHoleLeaking = false;
            }
        }
    }


    public void IsFixed(int holeID) //fixes hole, gets a new hole!
    {
        if(holeID == activeHole && holeList[holeID].TryGetComponent(out SpriteRenderer spriteRenderer)) //check we fixed an actually broken hole
        {
            spriteRenderer.sprite = holeFixed;
            holeTapedSound.Play();
            if (holeFixedParticles != null)
            {
                holeFixedParticles.transform.position = spriteRenderer.transform.position;
                holeFixedParticles.Play();
            }
            isHoleLeaking = false;

            scoreManager.UpdateScore(1);

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
        newHoleSound.Play();
        isHoleLeaking = true;
    }

    public void PlayFanfare()
    {
        if (scoreManager.IsHighScore())
        {
            victorySound.Play();
        }
        else
        {
            defeatSound.Play();
        }
    }    
}
