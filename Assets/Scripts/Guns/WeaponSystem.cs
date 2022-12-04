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
    public Rigidbody rigidBody;
    public Transform attackPoint;
    public RaycastHit raycastHit;
    public LayerMask whatIsEnemy;

    // Elemente de grafica
    public CameraShake cameraShake;
    public float cameraShakeStrength, cameraShakeDuration;

    // TODO - Display bullet amount

    public GameObject muzzleFlash, bulletHoleGraphic;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        ParseInput();
    }

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
            bulletsShot = bulletsPerTap;
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

        var spreadAffectedByRunning = spread;
        if(rigidBody.velocity.magnitude > 0)
        {
            spreadAffectedByRunning *= 1.5f;
        }
        float spreadX = Random.Range(-spreadAffectedByRunning, spreadAffectedByRunning);
        float spreadY = Random.Range(-spreadAffectedByRunning, spreadAffectedByRunning);
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

        cameraShake.Shake(cameraShakeDuration, cameraShakeStrength);

        bulletsLeft--;
        bulletsShot--;

        // Punem bullet hole acolo unde am lovit
        Instantiate(bulletHoleGraphic, raycastHit.point, Quaternion.Euler(0, 180, 0));
        // Punem flash la arma
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        Invoke("ResetShootingState", fireRate); // se va apela functia dupa ce trece timpul dat ca al 2-lea argument

        if(bulletsShot > 0 && bulletsLeft > 0) // daca mai sunt gloante de tras
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShootingState()
    {
        readyToShoot = true;
    }
}
