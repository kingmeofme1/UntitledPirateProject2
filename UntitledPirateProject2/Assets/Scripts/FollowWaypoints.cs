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
    private float delayTimer = 0f;

    private bool isStopped;

    void Update()
    {
        Vector2 direction = waypoints[currentWaypointIndex] - (Vector2)transform.position;
        direction.Normalize();

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
}
