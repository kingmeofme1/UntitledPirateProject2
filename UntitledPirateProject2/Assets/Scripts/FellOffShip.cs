using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellOffShip : MonoBehaviour
{
    public GameObject player;
    public RuleManager ruleManager;
    public ScoreManager scoreManager;
    public int pointsLost = -5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("player has fallen off the ship!");
            player.transform.SetPositionAndRotation(new Vector3(0, 0, player.transform.position.z), Quaternion.identity);
            ruleManager.ResetRule();
            scoreManager.UpdateScore(pointsLost);
        }
    }

}
