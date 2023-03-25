using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using TMPro;
using Cinemachine;
using Google.Protobuf;

public class PlayerController : MonoBehaviour
{
    [Header("Photon")]
    [SerializeField] private PhotonView _photonView;
    
    #region Player Components
    
    [Header("Player Components")]
    public Transform MainCamera;
    public Transform PlayerTransform;
    public CharacterController Controller;
    public EntityAttributeHolder Attributes;
    
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _sizeText;
    
    private CinemachineFreeLook _freeLook;
    
    private Animator _animator;
    private bool _hasAnimator;
    
    #endregion
    
    #region Player Settings
    
    [Header("Player Settings")]
    public float JumpHeight = 1.2f;
    private readonly float _gravity = Physics.gravity.y;
    public float RotationSpeed = 10f;
    public float MoveSpeed = 5.0f;
    public float DashDistance = 30.0f;
    public float DashCooldown = 2.0f;

    #endregion
    
    #region Input Actions
    
    private InputAction _movement, _jump, _dash, _attack;
    
    #endregion
    
    #region ANIMATION
    
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    
    #endregion
    
    #region PROPERTIES
    
    private Quaternion _targetRotation = Quaternion.identity;
    private Vector3 _velocity = Vector3.zero;
    private Vector2 _moveInput;
    private bool _isMoving, _canMove = true;
    private bool _canDash = true;
    private float _rotationTime = 0.0f;
    
    #endregion
    
    #region PLAYER DATA
    
    private PlayerData _playerData;
    
    #endregion
    
    public float Size
    {
        get => (float) Attributes.Size.GetValue();
        set => Attributes.Size.SetValue(value);
    }
    
    private void Awake()
    {
        if (MainCamera == null)
            MainCamera = GameObject.FindWithTag("MainCamera").transform;
        
        _freeLook = MainCamera!.GetComponent<CinemachineFreeLook>();
    }
    
    private void Start()
    {
        // Set up player animator.
        _hasAnimator = TryGetComponent(out _animator);
        
        // Set up input action references.
        _movement = InputManager.Move;
        _jump = InputManager.Jump;
        _dash = InputManager.Dash;
        _attack = InputManager.Attack;
        
        // Listen for input actions.
        _jump.started += Jump;
        _dash.started += Dash;
        _attack.started += Attack;
        
        AssignAnimationIDs();
        
        if (_photonView.IsMine)
        {
            _freeLook.Follow = PlayerTransform;
            _freeLook.LookAt = PlayerTransform;
            _photonView.OwnershipTransfer = OwnershipOption.Takeover;
        }
        
        // Update player name attribute.
        _nameText.SetText(_photonView.Owner.NickName);
    }

    private void Update()
    {
        if (!_photonView.IsMine)
            return;
        
        _hasAnimator = TryGetComponent(out _animator);
            
        // Player movement.
        UpdatePosition();
        HandleVelocity();
        HandleRotation();
        HandleDashing();
    }
    
    /// <summary>
    /// Updates the player's growth attribute display.
    /// </summary>
    /// <param name="amount">The amount to increase.</param>
    public void UpdateGrowth(float amount)
    {
        // Increase player size.
        var currentSize = Size;
        Size = currentSize + amount;
        
        // Update player size text.
        _sizeText.SetText($"Size: {Math.Round(Size, 2)}");
    }
    
    private void AssignAnimationIDs()
    {
        // ToDo:
        // 1. Add our own animation IDs.
        
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    #region METHODS
    
    /// <summary>
    /// Invoked when the jump action is performed.
    /// </summary>
    /// <param name="context">The input context.</param>
    private void Jump(InputAction.CallbackContext context)
    {
        if (Controller.isGrounded)
            _velocity.y += Mathf.Sqrt(JumpHeight * -2f * _gravity);
        
        // ToDo:
        // Add jump animations.
    }
    
    /// <summary>
    /// Invoked when the dash action is performed.
    /// </summary>
    /// <param name="context">The input context.</param>
    private void Dash(InputAction.CallbackContext context)
    {
        if (!Controller.isGrounded || !_canDash)
            return;

        _velocity += transform.forward * DashDistance;
        _canDash = false;
        
        // ToDo:
        // 1. Add dash animations.
    }
    
    /// <summary>
    /// Invoked when the player attacks.
    /// </summary>
    /// <param name="context">The input context.</param>
    private void Attack(InputAction.CallbackContext context)
    {
        // ToDo:
        // 1. Attack implementation.
        // 2. Attack animations.
    }
    
    /// <summary>
    /// Handles the player's position.
    /// </summary>
    private void UpdatePosition()
    {
        // Check if the player can move.
        if (!_canMove)
            return;
        
        // Get the input values.
        _moveInput = _movement.ReadValue<Vector2>();
        // Update the player's state.
        _isMoving = _moveInput != Vector2.zero;
        if (Controller.isGrounded && _velocity.y < 0)
            _velocity.y = 0f;
        
        // Calculate a target position & direction.
        var position = new Vector3(_moveInput.x, 0, _moveInput.y);
        // Adjust for camera rotation.
        position = MainCamera.forward * position.z + MainCamera.right * position.x;
        position.y = 0;
        
        // Move the character controller.
        Controller.Move(position * (Time.deltaTime * MoveSpeed));
        
        // ToDo:
        // 1. Move animations.
    }
    
    /// <summary>
    /// Handles the player's velocity.
    /// </summary>
    private void HandleVelocity()
    {
        // Apply gravity to the velocity.
        _velocity.y += _gravity * Time.deltaTime;
        // Apply drag/friction to the velocity.
        _velocity.x *= 0.9f;
        _velocity.z *= 0.9f;
        // Move the controller in the direction of velocity.
        Controller.Move(_velocity * Time.deltaTime);
    }
    
    /// <summary>
    /// Handles the player's rotation.
    /// </summary>
    private void HandleRotation()
    {
        // Check if the player has a target rotation.
        if (_targetRotation != Quaternion.identity)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                _targetRotation, Time.deltaTime * RotationSpeed);
            _rotationTime += 0.3f + RotationSpeed * Time.deltaTime;
            
            // Check if the player has reached the target rotation.
            if (_rotationTime >= RotationSpeed)
            {
                _canMove = true;
                _rotationTime = 0.0f;
                _targetRotation = Quaternion.identity;
            }
            
            return;
        }
        
        // Check if the player is moving.
        if (!_isMoving)
            return;
        
        // Calculate rotation.
        var angle = Mathf.Atan2(_moveInput.x, _moveInput.y) *
            Mathf.Rad2Deg + MainCamera.eulerAngles.y;
        var rotation = Quaternion.Euler(0f, angle, 0f);
        // Rotate the player transform.
        PlayerTransform.rotation = Quaternion.Lerp(PlayerTransform.rotation,
            rotation, Time.deltaTime * RotationSpeed);
    }
    
    /// <summary>
    /// Handles the player's dashing.
    /// </summary>
    private void HandleDashing()
    {
        if (_canDash)
            return;
        
        DashCooldown -= Time.deltaTime;
        if (!(DashCooldown <= 0))
            return;
        
        DashCooldown = 2.0f;
        _canDash = true;
    }
    
    #endregion
}
