using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoleManager : MonoBehaviour
{
    public void IsFixed() //Just disables the hole gameobject
    {
        gameObject.SetActive(false);
    }

    // TODO : make it so that holes re enable randomly after a hole is fixed
}
