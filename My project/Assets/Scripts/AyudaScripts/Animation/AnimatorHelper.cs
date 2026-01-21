using UnityEngine;

[RequireComponent (typeof(Animator))]
public class AnimatorHelper : MonoBehaviour
{
    [Header("Player Move")]
    public bool hasPlayerMove3D;
    private PlayerMove3D _playerMove3D;
    private Vector2 _playerMoveInput => _playerMove3D.moveDirection;
    
    [Header("Player Jump")]
    public bool hasPlayerJump;
    private PlayerJump _playerJump;
    private bool _playerIsJumping => !_playerJump.isGrounded;

    [Header("Attack")]
    public bool hasPlayerAttack;
    private PlayerAttack3D _playerAttack;
    private bool _playerIsAttacking => _playerAttack.actions.Game.Attack.triggered;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _playerMove3D = hasPlayerMove3D ? gameObject.TryGetComponent(out _playerMove3D) ? _playerMove3D : gameObject.AddComponent<PlayerMove3D>() : null;

        _playerJump = hasPlayerJump ? gameObject.TryGetComponent(out _playerJump) ? _playerJump : gameObject.AddComponent<PlayerJump>() : null;

        _playerAttack = hasPlayerAttack ? gameObject.TryGetComponent(out _playerAttack) ? _playerAttack : gameObject.AddComponent<PlayerAttack3D>() : null;
    }

    private void LateUpdate()
    {
        if (hasPlayerAttack && _playerIsAttacking)
        {
            _animator.SetTrigger("Attack");
            return;
        }
        if (hasPlayerJump)
        {
            _animator.SetBool("Jumping", _playerIsJumping);
            if (_playerIsJumping)
            {
                return;
            }
        }
        if (hasPlayerMove3D)
        {
            _animator.SetFloat("MoveHor", _playerMoveInput.x);
            _animator.SetFloat("MoveVer", _playerMoveInput.y);
            return;
        }
        
    }
}
