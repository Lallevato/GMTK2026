using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log($"Taking {damage}. New hp: {health}");
        if (health <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}