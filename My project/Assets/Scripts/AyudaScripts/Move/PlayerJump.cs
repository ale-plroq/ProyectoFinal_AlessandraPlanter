using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(CharacterController))]
public class PlayerJump : MonoBehaviour
{
    [Range(0.5f, 10f), Tooltip("How fast will the player jump upwards")]
    public float jumpSpeed = 1f;
    [Range(0.5f, 5f), Tooltip("How high will the player jump")]
    public float jumpZenith = 2f;
    [Range(0.5f, 10f), Tooltip("How fast will the player fall")]
    public float fallSpeed = 1.9f;
    [Header("Ground Check")]
    [Tooltip("Check if the player is on a floor while jumping - (If you can jump to higher places)")]
    public bool checkFloor = true;
    [Tooltip("Distance to check for the floor below the player")]
    public float floorCheckDistance = 0.3f;
    [Tooltip("Layer mask to consider as floor when checking for ground")]
    public LayerMask floorMask;

    private CharacterController controller;
    private PlayerActions playerActions;

    public bool isGrounded { get; set; } = true;

    private Coroutine jumpCoroutine;

    private void Start()
    {
        if (TryGetComponent(out PlayerMove3D pM3D))
        {
            playerActions = pM3D.actions;
        }
        else
        {
            playerActions = new PlayerActions();
            playerActions.Enable();
            playerActions.Game.Enable();
        }

        controller = GetComponent<CharacterController> ();
    }

    private void Update()
    {
        if (isGrounded && playerActions.Game.Jump.triggered && jumpCoroutine == null)
        {
            jumpCoroutine =  StartCoroutine(nameof(Jump));
        }
    }

    private IEnumerator Jump()
    {
        isGrounded = false;

        float y = transform.position.y;
        bool up = true;
        float t = 0f;
        float zenith = y + jumpZenith;

        while(up)
        {
            var jumpPos = Mathf.Lerp(y, y + zenith, t);
            t += Time.deltaTime * jumpSpeed;
            controller.Move(new Vector3(0, jumpPos - transform.position.y, 0));

            if (t >= 1f)
            {
                up = false;
                t = 0f;
                break;
            }
            yield return null;
        }
        while (!up)
        {
            var fallPos = Mathf.Lerp(y + zenith, y, t);
            t += Time.deltaTime * fallSpeed;

            controller.Move(new Vector3(0, fallPos - transform.position.y, 0));
            if (t >= 1f)
            {
                if (!checkFloor)
                {
                    isGrounded = true;
                    break;
                } else
                {
                    if (Physics.Raycast(transform.position, Vector3.down, floorCheckDistance, floorMask))
                    {
                        isGrounded = true;
                        break;
                    }
                    zenith = transform.position.y;
                    y = transform.position.y - jumpZenith;
                    t = 0f;
                }
            }
            yield return null;
        }
        jumpCoroutine = null;
    }
}
