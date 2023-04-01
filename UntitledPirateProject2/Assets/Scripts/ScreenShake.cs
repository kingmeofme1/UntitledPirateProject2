using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private float elapsed = 0f;

    private float x, y;

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector2 originalPos = transform.localPosition; 

        elapsed = 0f;

        while (elapsed < duration)
        {
            x = Random.Range(-1f, 1f) * magnitude;
            y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector2(x, y);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
