using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public bool invincible = false;

    void DisableInvincibility()
    {
        invincible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPickUp") && health < 100)
        {
            Destroy(other.transform.parent.parent.gameObject);
            health = Mathf.Min(100, health + 25);
        }

        if (other.CompareTag("AmmoPickUp"))
        {
            Destroy(other.transform.parent.parent.gameObject);
            GetComponentInChildren<WeaponSystem>().bulletsLeft = Mathf.Min(GetComponentInChildren< WeaponSystem >().magazineSize, GetComponentInChildren<WeaponSystem>().bulletsLeft + 50);
        }

        if (other.CompareTag("StarPickUp"))
        {
            Destroy(other.transform.parent.parent.gameObject);
            invincible = true;
            Invoke("DisableInvincibility", 5f);
        }

        // Debug.Log(other.tag);
    }


}
