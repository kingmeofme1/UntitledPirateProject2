using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateGoon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
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
            RotateSprite(direction);
            //chase player
        }
        else
        {
            waypointScript.enabled = true; //this puts them back on patrol
        }
        
    }

    private void RotateSprite(Vector2 direction)
    {
        float unitAngle = CalculateAngle(direction);
        if (unitAngle < 0)
        {
            unitAngle += 360;
        }
        if (unitAngle < 45 || unitAngle >= 315)
        {
            spriteRenderer.sprite = sprites[1];
            spriteRenderer.flipX = false;
        }
        else if (unitAngle >= 225)
        {
            spriteRenderer.sprite = sprites[0];
        }
        else if (unitAngle >= 135)
        {
            spriteRenderer.sprite = sprites[1];
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.sprite = sprites[2];
        }
    }

    public float CalculateAngle(Vector2 vector)
    {
        float aimAngle;
        if (vector.x == 0)
        {
            aimAngle = 90;
        }
        else
        {
            aimAngle = Mathf.Rad2Deg * Mathf.Atan(vector.y / vector.x);
        }

        if (vector.x < 0 || (vector.x == 0 && vector.y < 0))
        {
            aimAngle += 180;
        }
        return aimAngle;
    }
}
