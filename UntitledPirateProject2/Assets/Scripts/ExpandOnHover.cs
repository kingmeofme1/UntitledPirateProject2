using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExpandOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool expand = false;
    public GameObject button;
    public float scaleMod = 0f;
    public float maxScaleMod = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
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
            button.transform.localScale = new Vector3(1 + scaleMod, 1 + scaleMod, 1 + scaleMod);
        }
        else
        {
            if (scaleMod > 0)
            {
                scaleMod -= 0.01f;
            }
            button.transform.localScale = new Vector3(1 + scaleMod, 1 + scaleMod, 1 + scaleMod);
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
