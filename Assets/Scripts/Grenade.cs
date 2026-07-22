using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float damage = 20f;
    public float lifetime = 5f;
    public LayerMask hittableLayers;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Return if the collided object's layer is not in the LayerMask
        if ((hittableLayers.value & (1 << collision.gameObject.layer)) == 0)
            return;

        Target target = collision.collider.GetComponent<Target>();

        if (target != null)
            target.TakeDamage(damage);

        Destroy(gameObject);
    }
}