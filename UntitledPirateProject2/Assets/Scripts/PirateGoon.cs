using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateGoon : MonoBehaviour
{
    public RuleManager ruleManager;
    public PlayerMovement player;

    public FollowWaypoints waypointScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ruleManager.IsRulesBroken())
        {
            waypointScript.enabled = false;
            Vector2 direction = player.transform.position - transform.position;
            transform.position += (Vector3)(direction.normalized * waypointScript.GetMoveSpeed() * Time.deltaTime);
            //chase player
        }
        else
        {
            waypointScript.enabled = true; //this puts them back on patrol
        }

    }
}
