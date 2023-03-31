using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer pSprite;
    public Rigidbody2D pBody;
    public CircleCollider2D pCollider;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            pBody.velocity = (new Vector2(-speed, 0));
        }
        else if (Input.GetKey(KeyCode.W))
        {
            pBody.velocity = (new Vector2(0, speed));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            pBody.velocity = (new Vector2(speed, 0));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            pBody.velocity = (new Vector2(0, -speed));
        }
        else
        {
            pBody.velocity = Vector2.zero;
        }
    }
}
