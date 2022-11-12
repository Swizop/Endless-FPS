using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script de pus pe toate armele
public class PickupController : MonoBehaviour
{
    public WeaponSystem weaponSystem;
    public Rigidbody rigidBody;
    public BoxCollider boxCollider;

    public Transform player, gunContainer, fpsCamera;
    public float pickUpRange, dropForwardForce, dropUpwardForce;

    public bool isWeaponEquipped;
    public static bool isAnyWeaponEquipped;

    private void Start()
    {
        if(!isWeaponEquipped)
        {
            weaponSystem.enabled = false;
            rigidBody.isKinematic = false;
            boxCollider.isTrigger = false;
        }
        else
        {
            weaponSystem.enabled = true;
            rigidBody.isKinematic = true;
            boxCollider.isTrigger = true;
            isAnyWeaponEquipped = true;
        }
    }

    private void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if(!(isWeaponEquipped || isAnyWeaponEquipped) && Input.GetKeyDown(KeyCode.E) && distanceToPlayer.magnitude <= pickUpRange)
        {
            Pickup();
        }

        if(isWeaponEquipped && Input.GetKeyDown(KeyCode.E))
        {
            Drop();
        }
    }

    private void Pickup()
    {
        isWeaponEquipped = true;
        isAnyWeaponEquipped = true;
        weaponSystem.enabled = true;

        // Arma nu mai interactioneaza cu mediul din jur
        rigidBody.isKinematic = true;
        boxCollider.isTrigger = true;

        // Arma e facuta un child al gunContainer si ii pastreaza propritetatile
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
    }

    private void Drop()
    {
        isWeaponEquipped = false;
        isAnyWeaponEquipped = false;
        weaponSystem.enabled = false;

        // Arma interactioneaza cu mediul din jur
        rigidBody.isKinematic = false;
        boxCollider.isTrigger = false;

        transform.SetParent(null);

        // daca jucatorul alearga / se misca, se transfera si la arma
        rigidBody.velocity = player.GetComponent<Rigidbody>().velocity;
        rigidBody.AddForce(fpsCamera.forward * dropForwardForce, ForceMode.Impulse);
        rigidBody.AddForce(fpsCamera.up * dropUpwardForce, ForceMode.Impulse);
        // se roteste random
        float randomRotation = Random.Range(-1f, 1f);
        rigidBody.AddTorque(new Vector3(randomRotation, randomRotation, randomRotation) * 10);
    }
}
