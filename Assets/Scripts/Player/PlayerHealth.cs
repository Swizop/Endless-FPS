using System;
using Assets.Pixelation.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ISaveable
{
    public int health = 100;
    public bool invincible = false;

    void DisableInvincibility()
    {
        invincible = false;
        GetComponentInChildren<Pixelation>().enabled = false;
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
            GetComponentInChildren<Pixelation>().enabled = true;
            Invoke("DisableInvincibility", 5f);
        }

        // Debug.Log(other.tag);
    }

    public object CaptureState()
    {
        return new SaveData()
        {
            health = this.health
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;
        health = saveData.health;
    }


    [Serializable]
    private struct SaveData
    {
        public int health;
    }
}
