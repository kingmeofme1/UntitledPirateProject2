using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWaypoints : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float waypointPause = 2f;

    [SerializeField]
    private Vector2[] waypoints;

    private int currentWaypointIndex = 0;

    private bool isStopped;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    void Update()
    {
        Vector2 direction = waypoints[currentWaypointIndex] - (Vector2)transform.position;
        direction.Normalize();
        RotateSprite(direction);
        // move towards the current waypoint
        if(!isStopped)
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

        // next waypoint reached
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex]) < 0.1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }

            StartCoroutine(nameof(StopAtWaypoint));
        }

    }

    private IEnumerator StopAtWaypoint()
    {
        isStopped = true;
        yield return new WaitForSeconds(waypointPause);
        isStopped = false ;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i], 1f);
            if (i < waypoints.Length - 1)
                Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
            else
                Gizmos.DrawLine(waypoints[i], waypoints[0]);
        }
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    private void RotateSprite(Vector2 direction)
    {
        if (sprites.Length <= 0) return;

        float unitAngle = CalculateAngle(direction);
        if(unitAngle < 0)
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
