using UnityEngine;
using EZCameraShake;

public class WeaponSystem : MonoBehaviour
{
    public int damage;
    public GameObject damagePopup;
    public float reloadTime, spread, range, fireRate, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowHoldToShoot;
    public int bulletsShot, bulletsLeft;

    bool isShooting, readyToShoot, isReloading;

    public Camera fpsCamera;
    public CharacterController characterController;
    public Transform attackPoint;
    public RaycastHit raycastHit;
    public LayerMask whatIsEnemy;
    public PauseSettingsController pauseSettingsController;

    // Elemente de grafica
    public float cameraShakeStrength, cameraShakeDuration;

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
        // Spread is influenced by difficulty and by the user's state (running or not)
        float influencedSpread = spread + ((float)pauseSettingsController.difficultyLevel / 45);
        if (characterController.velocity.magnitude > 0)
        {
            influencedSpread *= 1.5f;
        }

        // Random seed
        Random.InitState(System.DateTime.Now.Millisecond);
        float spreadX = Random.Range(-influencedSpread, influencedSpread);
        float spreadY = Random.Range(-influencedSpread, influencedSpread);
        float spreadZ = Random.Range(-influencedSpread, influencedSpread);

        Vector3 shotDirection = fpsCamera.transform.forward + new Vector3(spreadX, spreadY, spreadZ);

        Random.InitState(System.DateTime.Now.Millisecond);
        // Generam un damage nou pentru hit, mai mic decat damage-ul normal al armei.
        int newDamage = Random.Range(1, damage);
        // Damage-ul hitului va fi schimbat in cel nou (mai mic), cu o probabilitate dependenta de dificultatea jocului
        if(Random.Range(0, 100) < (pauseSettingsController.difficultyLevel * 10))
        {
            damage = newDamage;
        }

        // args: pozitia de start, directia, locul de stocare al ray-ului, range-ul, ce layer e afectat
        if (Physics.Raycast(fpsCamera.transform.position, shotDirection, out raycastHit, range, whatIsEnemy))
        {
            // Debug.Log(raycastHit.collider.name);

            if (raycastHit.collider.CompareTag("Enemy"))
            {
                raycastHit.transform.gameObject.GetComponent<EnemyBehaviour>().health =
                    Mathf.Max(0, raycastHit.transform.gameObject.GetComponent<EnemyBehaviour>().health - damage);

                Instantiate(damagePopup, raycastHit.transform.position + new Vector3(0,1,0), GameObject.FindGameObjectWithTag("Player").transform.rotation);
            }
            else
            {
                // Punem bullet hole acolo unde am lovit
                GameObject decalObject = Instantiate(bulletHoleGraphic, raycastHit.point + (raycastHit.normal * 0.025f), Quaternion.identity) as GameObject;
                // Rotim Decalul aplicat pe textura
                decalObject.transform.rotation = Quaternion.FromToRotation(Vector3.forward, raycastHit.normal);
            }
        }

        CameraShaker.Instance.ShakeOnce(cameraShakeStrength, 1f, .1f, 1f);

        bulletsLeft--;
        bulletsShot--;

        // Punem flash la arma, il distrugem dupa 2 secunde
        Destroy(Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity),0.5f);
        

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
