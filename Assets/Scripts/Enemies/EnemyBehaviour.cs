using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer, whatIsProjectile;

    public int maxHealth = 100;

    public float health;

    public Transform shootingPoint;


    //Viata
    public EnemyHealthBar npcHealthBar;
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
        npcHealthBar.SetMaxHealth(maxHealth);
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        npcHealthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    private void Update()
    {
    
        /// Pentru a putea testa bara de viata si raspunsul la damage al inamicului
        if (Input.GetKeyDown(damageKey))
        {
            TakeDamage(Random.Range(maxHealth / 20, (int)(maxHealth / 3.3f) ));
        }

        // Verificam daca este in range pentru atac sau pentru urmarire
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        

        if (!playerInSightRange && !playerInAttackRange) Patroling();

        if (playerInSightRange && !playerInAttackRange) ChasePlayer();

        if (playerInAttackRange && playerInSightRange) AttackPlayer();

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
        npcHealthBar.gameObject.SetActive(false);
        _healthBarHidden = true;
    }

    private void ShowHealthBar()
    {
        npcHealthBar.gameObject.SetActive(true);
        _healthBarHidden = false;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        npcHealthBar.SetHealth(health);

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        npcHealthBar.DestroyHealthBar();
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
