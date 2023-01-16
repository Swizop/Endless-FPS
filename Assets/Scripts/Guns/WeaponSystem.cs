using UnityEngine;
using EZCameraShake;

public class WeaponSystem : MonoBehaviour
{

     // --- Audio ---
        public AudioClip GunShotClip;
        public AudioSource source;
        public Vector2 audioPitch = new Vector2(.9f, 1.1f);
        //public bool scopeActive = true;

   // private AudioSource mAudioSource;

    public int damage, difficultyAffectedDamage;

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

    public Score score;

     private void Start()
        {
            if(source != null) source.clip = GunShotClip;
           // timeLastFired = 0;
            //lastScopeState = scopeActive;
        }

    private void Awake()
    {
        difficultyAffectedDamage = damage;
        bulletsLeft = magazineSize;
        readyToShoot = true;
        //mAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //mAudioSource.Play();
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
        difficultyAffectedDamage = damage;
        readyToShoot = false;
        // Spread is influenced by difficulty and by the user's state (running or not)
        float influencedSpread = spread + ((float)pauseSettingsController.difficultyLevel / 100);
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
        if(Random.Range(0, 100) < ((pauseSettingsController.difficultyLevel+1) * 40))
        {
            difficultyAffectedDamage = newDamage;
        }

        // args: pozitia de start, directia, locul de stocare al ray-ului, range-ul, ce layer e afectat
        if (Physics.Raycast(fpsCamera.transform.position, shotDirection, out raycastHit, range, whatIsEnemy))
        {
            // Debug.Log(raycastHit.collider.name);

            if (raycastHit.collider.CompareTag("Enemy"))
            {
                score.EventPerformed(difficultyAffectedDamage == damage ? Score.UserEvent.perfectShot : Score.UserEvent.normalShot);
                raycastHit.transform.gameObject.GetComponent<EnemyBehaviour>().health =
                    Mathf.Max(0, raycastHit.transform.gameObject.GetComponent<EnemyBehaviour>().health - difficultyAffectedDamage);
                raycastHit.transform.gameObject.GetComponent<EnemyBehaviour>().SlowDown();

                Instantiate(damagePopup, raycastHit.transform.position + new Vector3(0,1,0), GameObject.FindGameObjectWithTag("Player").transform.rotation);
            }
            else
            {
                // Punem bullet hole acolo unde am lovit
                GameObject decalObject = Instantiate(bulletHoleGraphic, raycastHit.point + (raycastHit.normal * 0.025f), Quaternion.identity) as GameObject;
                // Rotim Decalul aplicat pe textura
                decalObject.transform.rotation = Quaternion.FromToRotation(Vector3.forward, raycastHit.normal);
            }
             // --- Handle Audio ---
            if (source != null)
            {
                // --- Sometimes the source is not attached to the weapon for easy instantiation on quick firing weapons like machineguns, 
                // so that each shot gets its own audio source, but sometimes it's fine to use just 1 source. We don't want to instantiate 
                // the parent gameobject or the program will get stuck in a loop, so we check to see if the source is a child object ---
                if(source.transform.IsChildOf(transform))
                {
                    source.Play();
                }
                else
                {
                    // --- Instantiate prefab for audio, delete after a few seconds ---
                    AudioSource newAS = Instantiate(source);
                    if ((newAS = Instantiate(source)) != null && newAS.outputAudioMixerGroup != null && newAS.outputAudioMixerGroup.audioMixer != null)
                    {
                        // --- Change pitch to give variation to repeated shots ---
                        newAS.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", Random.Range(audioPitch.x, audioPitch.y));
                        newAS.pitch = Random.Range(audioPitch.x, audioPitch.y);

                        // --- Play the gunshot sound ---
                        newAS.PlayOneShot(GunShotClip);

                        // --- Remove after a few seconds. Test script only. When using in project I recommend using an object pool ---
                        Destroy(newAS.gameObject, 4);
                    }
                }
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
