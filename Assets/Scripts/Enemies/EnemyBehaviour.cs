using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
   
    [SerializeField]
    AudioSource enmSound;

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer, whatIsProjectile;

    public int maxHealth = 100;

    public float health;

    public Transform shootingPoint;

    public Animator animator;

    //Viata
    private bool _healthBarHidden;
    [SerializeField]
    private KeyCode damageKey;

    // Miscare
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Atac
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    // Stare actuala
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, playerVisible;

    private void Start()
    {
        health = maxHealth;
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }


    public GameObject HealthPickup;
    public GameObject AmmoPickup;
    public GameObject InvincibilityPickup;

    private void Update()
    {
        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy),0f);
        }

        // Verificam daca este in range pentru atac sau pentru urmarire
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        

        if (!playerInSightRange && !playerInAttackRange) Patroling();

        if (playerInSightRange && !playerInAttackRange) ChasePlayer();

        if (playerInAttackRange && playerInSightRange){
            AttackPlayer();
            enmSound.Play();
        } 
       
    }

    public void LateUpdate()
    {
        /// Daca inamicul poate vedea playerul
        if (playerInSightRange)
        {
            /// Verificam daca inamicul poate vedea playerul, ignoram corpul playerului si proiectilele
            LayerMask playerVisibleIgnore = ~(whatIsPlayer | whatIsProjectile);
            playerVisible = !Physics.Linecast(transform.position, player.position, out RaycastHit hit, playerVisibleIgnore);

            /// Daca inamicul vede playerul si bara de viata este asunsa o aratam
            if (playerVisible && _healthBarHidden)
            {
                ShowHealthBar();
            }
            /// Daca inamicul nu vede playerul si bara de viata se vede o ascundem
            else if (!playerVisible && !_healthBarHidden)
            {
                HideHealthBar();
            }
        }
        /// Daca inamicul nu poate vedea playerul si bara de viata se vede o ascundem
        else if (!_healthBarHidden)
        {
            HideHealthBar();
            _healthBarHidden = true;
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Am ajuns la walkpoint
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {

        // Alegem un punct random unde sa se deplaseze inamicul
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {

        // Oprim inamicul
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            /// Atac
            Rigidbody rb = Instantiate(projectile, shootingPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);
            /// Sfarsit atac

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void HideHealthBar()
    {
        _healthBarHidden = true;
    }

    private void ShowHealthBar()
    {
        _healthBarHidden = false;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void DestroyEnemy()
    {
        // The enemy has a small chance of spawning a Power Up upon it's death
        float chance = Random.Range(0, 1f);
        // Debug.Log(chance);
        WeaponSystem weaponSystem = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<WeaponSystem>();
        if (weaponSystem.bulletsLeft < (2 / 10 * weaponSystem.magazineSize) && chance >= 0.6f) // If the player is low on ammo, we help him by giving him a bigger chance to replenish it
        {
            Instantiate(AmmoPickup, transform.position, Quaternion.identity);
        }
        else if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().health < 25 && chance >= 0.6f) // Same story as above, but for health
        {
            Instantiate(HealthPickup, transform.position, Quaternion.identity);
        }
        else if (chance >= 0.8f) // 20% chance 
        {
            if (chance < 0.85f) // ~5% chance of Invincibility Pickup
            {
                Instantiate(InvincibilityPickup,transform.position, Quaternion.identity);
            }
            else if (chance < 0.92f) // ~7% chance of Health
            {
                Instantiate(HealthPickup, transform.position, Quaternion.identity);
            }
            else // ~8% chance of Ammo
            {
                Instantiate(AmmoPickup, transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);   
    }

    /// <summary>
    /// Pentru a putea vizualiza in editor rangeul de atac si vedere al inamicului si pentru a vedea linia linecast-ului
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawLine(transform.position, player.position);
    }
}
