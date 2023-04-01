using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject credits;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleCredits()
    {
        credits.SetActive(!credits.activeSelf);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("KingScene");
    }
}
