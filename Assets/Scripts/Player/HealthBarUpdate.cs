using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpdate : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Converting from a 0-100 health system to the 0-1f fill amount
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>())
            GetComponent<Image>().fillAmount = (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().health / 100.0f);
    }
}
