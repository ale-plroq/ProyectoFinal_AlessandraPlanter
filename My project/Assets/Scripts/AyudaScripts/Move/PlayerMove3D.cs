using UnityEditor.EditorTools;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(CharacterController))]
public class PlayerMove3D : MonoBehaviour
{
    [Range(1f, 10f), Tooltip("How fast will the player move")]
    public float speed = 1f;
    private float trueSpeed => speed / 10f;

    public Vector2 moveDirection { get; set; } = Vector2.zero;

    private CharacterController controller;
    public PlayerActions actions { get; private set; }

    [Tooltip("Is the player a sprite (2D) character?")]
    public bool isSprite;
    public bool isFlipped {get; private set;}
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        actions = new PlayerActions();
        actions.Enable();
        actions.Game.Enable();

        controller = GetComponent<CharacterController>();

        if (isSprite)
        {
            if (!TryGetComponent(out spriteRenderer))
            {
                throw new System.Exception("PlayerMove3D: isSprite is true but no SpriteRenderer found on the GameObject.");
            }
        }
    }

    private void Update()
    {
        moveDirection = actions.Game.Move.ReadValue<Vector2>();

        if (isSprite)
        {
            if (!isFlipped && moveDirection.x < 0)
            {
                spriteRenderer.flipX = true;
                isFlipped = true;
            }
            else if (isFlipped && moveDirection.x > 0)
            {
                spriteRenderer.flipX = false;
                isFlipped = false;
            }
        }
    }

    private void FixedUpdate()
    {
        controller.Move(new Vector3(moveDirection.x, 0, moveDirection.y) *  trueSpeed);
    }
}
