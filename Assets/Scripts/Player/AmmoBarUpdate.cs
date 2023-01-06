using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBarUpdate : MonoBehaviour
{
    void Update()
    {
        var weaponComponent = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<WeaponSystem>();
        if (weaponComponent)
            GetComponent<Image>().fillAmount = (weaponComponent.bulletsLeft * 1.0f / (weaponComponent.magazineSize));
    }
}
