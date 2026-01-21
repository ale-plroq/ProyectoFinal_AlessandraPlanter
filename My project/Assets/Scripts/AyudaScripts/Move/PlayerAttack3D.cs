using UnityEngine;

public class PlayerAttack3D : MonoBehaviour
{
    [Tooltip("Damage dealt by the player's attack")]
    public int attackDamage = 10;
    [Tooltip("Size of the attack area (if using Sphere)")]
    public float attackSize = 1f;
    [Tooltip("Size of the attack area (if using Cube)")]
    public Vector3 attackSize3D = Vector3.one;
    [Tooltip("Offset of the attack area from the player")]
    public float attackOffset = 1f;
    [Tooltip("Layer(s) that can be attacked")]
    public LayerMask attackLayer;
    [Tooltip("Shape of the attack area")]
    public AttackShape attackShape = AttackShape.Sphere;
    public enum AttackShape
    {
        Sphere,
        Cube
    }

    public PlayerActions actions {get; private set;}

    [Tooltip("Direction of the attack - Object's forward, or world right")]
    public AttackDirection attackDirection = AttackDirection.Forward;
    public enum AttackDirection
    {
        Forward,
        Right
    }
    [Tooltip("Is the player a sprite (2D) character?")]
    public bool isSprite = false;
    private PlayerMove3D playerMove;
    [Tooltip("Show attack gizmos in editor")]
    public bool showGizmos = false;

    void Start()
    {
        if (TryGetComponent(out playerMove))
        {
            actions = playerMove.actions;
        } else
        {
            actions = new PlayerActions();
            actions.Enable();
            actions.Game.Enable();
        }

        if (isSprite)
        {
            attackDirection = AttackDirection.Right;
        }
    }

    void Update()
    {
        if (actions.Game.Attack.triggered)
        {
            Vector3 direction = attackDirection == AttackDirection.Forward ? Vector3.forward : playerMove.isFlipped ? Vector3.left : Vector3.right;
            switch (attackShape)
            {
                case AttackShape.Sphere:
                    Collider[] colliders = Physics.OverlapSphere(transform.position + direction * attackOffset, attackSize, attackLayer);
                    foreach (var collider in colliders)
                    {
                        if (collider.TryGetComponent(out Health health))
                        {
                            health.TakeDamage(attackDamage);
                        }
                    }
                    break;
                case AttackShape.Cube:
                    Collider[] colliders2 = Physics.OverlapBox(transform.position + direction * attackOffset, attackSize3D, Quaternion.identity, attackLayer);
                    foreach (var collider in colliders2)
                    {
                        if (collider.TryGetComponent(out Health health))
                        {
                            health.TakeDamage(attackDamage);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            var dir = attackDirection == AttackDirection.Forward ? Vector3.forward : playerMove.isFlipped ? Vector3.left : Vector3.right;
            Vector3 position = transform.position + dir * attackOffset;
            Gizmos.color = Color.red;
            if (attackShape == AttackShape.Sphere)
            {
                Gizmos.DrawWireSphere(position, attackSize);
            }
            else if (attackShape == AttackShape.Cube)
            {
                Gizmos.DrawWireCube(position, attackSize3D);
            }
        }
    }
}
