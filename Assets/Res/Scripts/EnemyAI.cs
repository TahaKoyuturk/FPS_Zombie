using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    #region Variables

    #region Public Variables

    [SerializeField] LayerMask groundLayer, playerLayer;

    [SerializeField] float range;
    [SerializeField] float sightRange;
    [SerializeField] float attackRange;
    [SerializeField] float hitDamage = 20;

    #endregion

    #region Private Variables

    Vector3 destPoint;

    GameObject player;

    NavMeshAgent agent;

    Animator animator;

    BoxCollider boxCollider;

    Rigidbody rb;

    private int health = 100;
    private bool once = true;
    private bool isSoundPlaying = false;

    bool walkPoint;
    bool playerInSight, playerInAttackRange;
    bool isHit;

    #endregion

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator= GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        
    }

    private void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttackRange)
            Patrol();
        if (playerInSight && !playerInAttackRange)
            Chase();
        if (playerInSight && playerInAttackRange)
            Attack();
    }
    private void OnTriggerEnter(Collider other)
    {

        var player = other.GetComponent<InputHandler>();

        if (player != null && !this.isHit)
        {
            this.isHit = true;
            EventManager.Broadcast(GameEvent.OnPlayerHit, hitDamage);
            Invoke(nameof(Wait1Sec), 1.5f);
        }

    }
    #endregion

    #region Custom Methods

    public void TakeDamage()
    {
        health -= 40;

        if (health <= 0 && once)
        {
            once = false;
            AudioManager.Instance.PlaySound("creature_dead");

            animator.SetBool("isAttacking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);

            EventManager.Broadcast(GameEvent.OnKillCounterText, -1);
            EventManager.Broadcast(GameEvent.OnEnemyKill, -1);

            PlayerPrefs.SetInt("Kills", PlayerPrefs.GetInt("Kills") + 1);

            if(PlayerPrefs.GetInt("Kills") >= PlayerPrefs.GetInt("Highscore"))
            {
                PlayerPrefs.SetInt("Highscore", PlayerPrefs.GetInt("Highscore") + 1);
                EventManager.Broadcast(GameEvent.OnHighScoreUpdate,-1);
            }

            animator.SetBool("isDead", true);

            rb.isKinematic = true;
            agent.speed = 0;

            Destroy(this.gameObject, 3.5f);
        }
    }
    private void Attack()
    {
        if (!isSoundPlaying)
            StartCoroutine(PlaySound(.684f, "creature_attack"));

        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.freezeRotation= true;

        animator.SetBool("isAttacking", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
    }

    private void Chase()
    {
        if(!isSoundPlaying)
            StartCoroutine(PlaySound(1.903f, "creature_idle"));

        rb.freezeRotation = false;
        rb.isKinematic = false;

        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", false);
        agent.SetDestination(player.transform.position);
    }
    private void Patrol()
    {
        if (!isSoundPlaying)
            StartCoroutine(PlaySound(1.903f, "creature_idle"));

        rb.freezeRotation = false;
        rb.isKinematic = false;

        animator.SetBool("isAttacking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", true);

        if (!walkPoint) 
            SearchForDest();

        if(walkPoint)
            agent.SetDestination(destPoint);
        
        if(Vector3.Distance(transform.position, destPoint) < 2) 
            walkPoint= false;
    }

    private void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x,transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkPoint = true;
        }
    }
    private void EnableAttack()
    {
        boxCollider.enabled = true;
    }
    private void DisableAttack()
    {
        boxCollider.enabled = false;
    }
    private void Wait1Sec()
    {
        this.isHit = false;
    }
    IEnumerator PlaySound(float time, string name)
    {
        isSoundPlaying = true;
        AudioManager.Instance.PlaySound(name);
        yield return new WaitForSeconds(time);
        isSoundPlaying = false;
    }

    #endregion
}
