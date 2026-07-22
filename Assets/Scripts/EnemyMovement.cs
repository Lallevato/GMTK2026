using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class EnemyMovement : MonoBehaviour
{
    [Header("Wandering")]
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float minWaitTime = 3f;
    [SerializeField] private float maxWaitTime = 5f;

    [Header("Player")]
    [SerializeField] protected Transform player;
    [SerializeField] private float detectionRange = 12f;
    [SerializeField] private float attackRange = 2f;

    public int damage = 100;

    protected NavMeshAgent agent;
    protected Animator animator;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        StartCoroutine(WanderRoutine());
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            // Chase
            agent.isStopped = false;
            agent.SetDestination(player.position);

            animator.SetBool("Walking", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            // Wander
            agent.isStopped = false;

            bool isMoving = agent.pathPending ||
                            (agent.remainingDistance > agent.stoppingDistance &&
                             agent.velocity.sqrMagnitude > 0.01f);

            animator.SetBool("Walking", isMoving);
            animator.SetBool("Idle", !isMoving);
        }
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, player.position) <= detectionRange)
            {
                yield return null;
                continue;
            }
            // Wait 3-5 seconds before choosing a new destination.
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            Vector3 destination;
            if (TryGetRandomPoint(transform.position, wanderRadius, out destination))
            {
                agent.SetDestination(destination);
            }
        }
    }

    private bool TryGetRandomPoint(Vector3 center, float radius, out Vector3 result)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * radius;
            randomPoint.y = center.y;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = center;
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            try
            {
                collision.gameObject.GetComponent<Target>().TakeDamage(damage);
            }
            catch
            {
                Debug.Log("Could not get target");
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }
}