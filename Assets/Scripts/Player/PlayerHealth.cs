using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

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
            // Function to add ammo
        }

        if (other.CompareTag("StarPickUp"))
        {
            Destroy(other.transform.parent.parent.gameObject);
            // Function to make player unable to lose health for a set amount of time
        }

        // Debug.Log(other.tag);
    }


}
