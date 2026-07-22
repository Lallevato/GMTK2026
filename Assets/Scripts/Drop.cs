using UnityEngine;

abstract public class Drop : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRadius = 10f;
    public float hitDistance = 1f;

    [Header("Homing Movement")]
    public float minSpeed = 0.5f;
    public float maxSpeed = 12f;
    public float acceleration = 2f;
    public float turnSmoothness = 6f;

    [Header("Idle Motion")]
    public float idleRotationSpeed = 30f;
    public float bobHeight = 0.3f;
    public float bobSpeed = 2f;

    private Transform player;
    private bool homing;
    private float currentSpeed;
    private Vector3 startPosition;
    private float bobOffset;
    public AudioClip sfx;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        bobOffset = Random.value * Mathf.PI * 2f;
        currentSpeed = minSpeed;
    }

    void Update()
    {
        if (player == null) return;

        Collider playerCol = player.GetComponent<Collider>();
        Vector3 targetPoint = playerCol.bounds.center;
        float distance = Vector3.Distance(transform.position, targetPoint);

        if (!homing)
        {
            IdleMotion();

            if (distance <= detectionRadius)
            {
                homing = true;
            }
        }
        else
        {
            HomeTowardsPlayer(targetPoint);
        }
    }

    void IdleMotion()
    {
        // Rotate in place
        transform.Rotate(Vector3.up, idleRotationSpeed * Time.deltaTime, Space.World);

        // Vertical bob
        float bob = Mathf.Sin(Time.time * bobSpeed + bobOffset) * bobHeight;
        transform.position = new Vector3(
            transform.position.x,
            startPosition.y + bob,
            transform.position.z
        );
    }

    void HomeTowardsPlayer(Vector3 targetPoint)
    {
        // Check first — no overshoot
        float distance = Vector3.Distance(transform.position, targetPoint);
        if (distance <= hitDistance)
        {
            if (sfx != null)
            {
                AudioSource.PlayClipAtPoint(sfx, gameObject.transform.position);
            }
            Effect();
            return;
        }

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            maxSpeed,
            acceleration * Time.deltaTime
        );

        Vector3 direction = (targetPoint - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSmoothness * Time.deltaTime
        );

        transform.position += direction * currentSpeed * Time.deltaTime;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    protected abstract void Effect();
}
