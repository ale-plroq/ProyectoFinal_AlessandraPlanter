using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyTouchAttack : MonoBehaviour
{
    [Tooltip("Damage dealt to the player on touch")]
    public int damage = 10;
    [Tooltip("Cooldown time between attacks in seconds")]
    public float attackCoolDown = 1f;
    private bool canAttack = true;
    private float t = 0f;
    [Tooltip("Layer of the player object")]
    public LayerMask playerLayer = 3;
    [Tooltip("Should the enemy push the player when it damages them?")]
    public bool pushOnHit = false;
    [Tooltip("How far topush the player when hit"), Range(0.5f, 5f)]
    public float pushDistance = 1f;

    [Header("Debug")]
    [Tooltip("Radius of the attack range")]
    public float attackSize = 1f;
    [Tooltip("Offset of the attack range from the enemy's position")]
    public Vector3 attackOffset = Vector3.zero;
    [Tooltip("Should the attack range be shown in the editor?")]
    public bool showGizmos = true;

    void Start()
    {
        t = attackCoolDown;
    }
    void Update()
    {
        if (!canAttack)
        {
            t -= Time.deltaTime;
            if (t <= 0)
            {
                canAttack = true;
                t = attackCoolDown;
            }
        }
        else
        {
            var colliders = Physics.OverlapSphere(transform.position + attackOffset, attackSize, playerLayer);
            foreach (var collider in colliders)
            {
                if (collider.gameObject.TryGetComponent(out Health health))
                {
                    canAttack = false;
                    health.TakeDamage(damage);
                    if (pushOnHit)
                    {
                        collider.gameObject.GetComponent<CharacterController>().Move((collider.transform.position - transform.position).normalized * pushDistance);
                    }
                    break;
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + attackOffset, attackSize);
        }
    }
}
