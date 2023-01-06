using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUpdate : MonoBehaviour
{
    void Update()
    {
         GetComponent<Image>().fillAmount = GetComponentInParent<EnemyHealthBar>().health / 100.0f;
    }
}
