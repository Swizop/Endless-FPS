using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public int damage;
    public float reloadTime, spread, range, fireRate, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowHoldToShoot;
    int bulletsShot, bulletsLeft;

    bool isShooting, readyToShoot, isReloading;

    public Camera fpsCamera;
    public Transform attackPoint;
    public RaycastHit raycastHit;
    public LayerMask whatIsEnemy;

    private void ParseInput()
    {
        if(allowHoldToShoot)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        } else
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading)
        {
            Reload();
        }

        if (bulletsLeft > 0 && readyToShoot && !isReloading && isShooting) {
            Shoot();
        }
    }

    private void Reload()
    {
        isReloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        isReloading = false;
        bulletsLeft = magazineSize;
    }

    private void Shoot()
    {
        readyToShoot = false;

        // TODO - assess daca si cum punem spread mai mare cand alergi. probabil ceva gen if(rigidbody.velocity.magnitude > 0) spread *= 1.5f;
        //          sau daca userul tine apasat wsad
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);
        Vector3 shotDirection = fpsCamera.transform.forward;
        shotDirection.x += spreadX;
        shotDirection.y += spreadY;


        // args: pozitia de start, directia, locul de stocare al ray-ului, range-ul, ce layer e afectat
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out raycastHit, range, whatIsEnemy))
        {
            Debug.Log(raycastHit.collider.name);

            // TODO - pune tagul "Enemy" la adversari
            if (raycastHit.collider.CompareTag("Enemy"))
            {
                // TODO - implementeaza un AI care trage. Redu-i din hp, ceva gen
                // raycastHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
                Debug.Log("lovit");
            }
        }
        bulletsLeft--;
        Invoke("ResetShootingState", timeBetweenShots); // se va apela functia dupa ce trece timpul dat ca al 2-lea argument
    }

    private void ResetShootingState()
    {
        readyToShoot = true;
    }
}
