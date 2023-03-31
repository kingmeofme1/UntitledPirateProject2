using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverlapObj : MonoBehaviour
{
    private bool isInRange;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //DETECTS PLAYER IF IN RANGE
            isInRange = true;
            //Debug.Log("Player is in range!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // DETECTS IF THE PLAYER HAS LEFT THE RANGE/ IS OUT OF THE RANGE
            isInRange = false;
            //Debug.Log("Player is no longer in range!");
        }
    }

    public bool IsInRange()
    {
        return isInRange;
    }
}
