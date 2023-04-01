using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CutScenePlayer : MonoBehaviour
{
    public TMP_Text dialogueBox;
    public List<string> dialogueStringList;
    public List<AudioSource> dialogueAudioList;
    public int currentLine = 0;
    public int currentChar = 0;
    public int framesBetweenUpdates = 5;
    public int curFrames = 0;
    public bool readyForNextLine = false;
    public string endOfLineText = "<color=green> (space to continue)</color>";

    public List<GameObject> events;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(readyForNextLine == false) //if we are still displaying the last line, keep going
        {
            curFrames++;
            if (curFrames > framesBetweenUpdates)
            {
                curFrames = 0;
                DisplayCurrentLine();
            }
        }
        else //if we are ready for the next line, look for player input, the continue
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckCutsceneEvents();
                readyForNextLine = false;
            }
        }

        
    }

    public void DisplayCurrentLine() //displays current line 1 character at a time. once the last line is reached, prepares to start on the next line, but waits for input from player
    {
        if(currentLine >= dialogueStringList.Count) 
        {
            Debug.Log("end of dialogue");
            gameObject.SetActive(false);
            return;
        }
        string textToDisplay = dialogueStringList[currentLine].Substring(0, currentChar);
        dialogueBox.text = textToDisplay;
        if (currentChar < dialogueStringList[currentLine].Length) {
            currentChar++;
        }
        else
        {
            dialogueBox.text = textToDisplay + endOfLineText;
            currentChar = 0;
            currentLine++;
            readyForNextLine = true;
        }
    }

    public void CheckCutsceneEvents()
    {
        if(currentLine >= events.Count)
        {
            Debug.Log("endOfCutscene");
            SceneManager.LoadScene("KingScene");
            return;
        }
        if (events[currentLine] != null)
        {
            events[currentLine].SetActive(true);
        }
    }


}
