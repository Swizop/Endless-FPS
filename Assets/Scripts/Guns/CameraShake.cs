using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake (float duration, float strength)
    {
        Vector3 originalPosition = transform.localPosition;
        float timeElapsed = 0.0f;
        while(timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;
            transform.localPosition = new Vector3(x, y, originalPosition.z);
            timeElapsed += Time.deltaTime;
            yield return null; // inainte sa se treaca la urmatorul pas in while, asteapta sa 
        }
        transform.localPosition = originalPosition;
    }
}
