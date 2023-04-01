using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExpandOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool expand = false;
    public GameObject theObject;
    public float scaleMod = 0f;
    public float maxScaleMod = 0.2f;
    public Vector3 initialScaling;
    // Start is called before the first frame update
    void Start()
    {
        theObject = gameObject;
        initialScaling = theObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (expand)
        {
            if(scaleMod < maxScaleMod)
            {
                scaleMod += 0.01f;
            }
            theObject.transform.localScale = new Vector3(initialScaling.x + scaleMod, initialScaling.y + scaleMod, initialScaling.z + scaleMod);
        }
        else
        {
            if (scaleMod > 0)
            {
                scaleMod -= 0.01f;
            }
            theObject.transform.localScale = new Vector3(initialScaling.x + scaleMod, initialScaling.y + scaleMod, initialScaling.z + scaleMod);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        expand = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        expand = false;
    }
}
