using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUpdate : MonoBehaviour
{
    void Update()
    {
         GetComponent<Image>().fillAmount = GetComponentInParent<EnemyBehaviour>().health / (GetComponentInParent<EnemyBehaviour>().maxHealth * 1.0f);
    }
}
