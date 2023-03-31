using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractObj : MonoBehaviour
{
    private bool isInRange;
    [SerializeField] private KeyCode interactkey;
    [SerializeField] private UnityEvent interactAction;

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(interactkey))
            {
                interactAction.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //DETECTS PLAYER IF IN RANGE
            isInRange = true;
            Debug.Log("Player is in range!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // DETECTS IF THE PLAYER HAS LEFT THE RANGE/ IS OUT OF THE RANGE
            isInRange = false;
            Debug.Log("Player is no longer in range!");
        }
    }
}
