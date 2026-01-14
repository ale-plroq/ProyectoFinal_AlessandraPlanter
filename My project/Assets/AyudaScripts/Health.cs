using UnityEngine;

[RequireComponent (typeof(Collider))]
public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;

        if (!gameObject.TryGetComponent<Collider>(out _))
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        gameObject.SetActive(false);
    }
}