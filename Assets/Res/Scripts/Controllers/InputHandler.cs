using System;
using UnityEngine;
using static stg_models;

public class InputHandler : MonoBehaviour, IDamageable
{
    #region Variables

    #region Public Variables

    [Header("References")]
    public Transform camereHolder;
    [SerializeField] private CharacterController CharacterController;
    [SerializeField] private PlayerData playerData;

    [Header("Settings")]
    [SerializeField] private PlayerSettingsModel playerSettings;
    [SerializeField] private float viewClampYMin = -70;
    [SerializeField] private float viewClampYMax = 80;

    [Header("Gravity")]
    [SerializeField] private float GravityAmount;
    [SerializeField] private float GravityMin;
    
    [SerializeField] private Vector3 jumpingForce;
    
    [Header("Weapon")]
    [SerializeField] private WeaponBase currentWeapon;

    public float weaponAnimationSpeed;

    [Header("Aiming In")]
    public bool isAimingIn;

    [HideInInspector]
    public bool isSprinting;

    [HideInInspector]
    public Vector2 input_Movement;
    [HideInInspector]
    public Vector2 input_View;

    #endregion

    #region Private Variables

    private MyInputActions inputActions;

    private Vector3 jumpingForceVelocity;
    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

    private Vector3 newMovementSpeed;
    private Vector3 newMovementVelocity;
    private float PlayerGravity;

    #endregion

    #endregion

    #region Methods

    #region Unity Callbacks

    private void Awake()
    {
        inputActions = new MyInputActions();

        // Movement Controller
        inputActions.Character.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
        inputActions.Character.View.performed += e => input_View = e.ReadValue<Vector2>();
        inputActions.Character.Jump.performed += e => Jump();
        inputActions.Character.Sprint.performed += e => ToggleSprint();
        inputActions.Character.SprintReleased.performed += e => StopSprint();
        newCharacterRotation = transform.localRotation.eulerAngles;

        // Fire Controller
        inputActions.Weapon.Fire2Pressed.performed += e => AimingPressed();
        inputActions.Weapon.Fire2Released.performed += e => AimingReleased();
        if (currentWeapon)
            currentWeapon.Initialize(this);

        inputActions.Enable();

        // Camera Controller 
        newCameraRotation = camereHolder.localRotation.eulerAngles;

        if (this.TryGetComponent(out CharacterController characterController))
        {
            CharacterController = characterController;
        }

        playerSettings.WalkingForwardSpeed = playerData.MoveSpeed;
        playerSettings.JumpingHeight = playerData.JumpPower;

        if (playerData.JumpPower >= 10)
            playerSettings.JumpingFalloff = 0.25f;
        
        else 
            playerSettings.JumpingFalloff = 0.12f;
        
       
    }

    void Update()
    {
        CalculateMovement();
        CalculateView();
        CalculateJump();
        CalculateAimingIn();
    }

    #endregion

    #region Custom Methods

    private void CalculateMovement()
    {
        if (input_Movement.y <= 0.2f)
        {
            isSprinting = false;
        }
        weaponAnimationSpeed = (CharacterController.velocity.magnitude / playerSettings.WalkingForwardSpeed);

        if (weaponAnimationSpeed > 1)
            weaponAnimationSpeed = 1;

        var verticalSpeed = playerSettings.WalkingForwardSpeed;
        var horizontalSpeed = playerSettings.WalkingStrafeSpeed;

        if (isSprinting)
        {
            verticalSpeed = playerSettings.RunningForwardSpeed;
            horizontalSpeed = playerSettings.RunningStrafeSpeed;
        }

        newMovementSpeed = Vector3.SmoothDamp(
            newMovementSpeed, 
            new Vector3(
                horizontalSpeed * input_Movement.x * Time.deltaTime, 
                0, 
                verticalSpeed * input_Movement.y * Time.deltaTime),
                    ref newMovementVelocity,
                    playerSettings.MovementSmoothing);


        var movementSpeed = transform.TransformDirection(newMovementSpeed);


        if (PlayerGravity > GravityMin && jumpingForce.y < 0.1f)
            PlayerGravity -= GravityAmount - Time.deltaTime;

        if (PlayerGravity < -1 && CharacterController.isGrounded)
            PlayerGravity = -1;

        if (jumpingForce.y > 0.1f)
            PlayerGravity = 0;


        movementSpeed.y += PlayerGravity;

        movementSpeed += jumpingForce * Time.deltaTime;

        CharacterController.Move(movementSpeed);

    }
    private void CalculateView()
    {
        newCharacterRotation.y += playerSettings.ViewXSensitivity * (playerSettings.ViewXInverted ? (-input_View.x) : input_View.x) * Time.deltaTime;

        transform.rotation = Quaternion.Euler(newCharacterRotation); 

        newCameraRotation.x += playerSettings.ViewYSensitivity * (playerSettings.ViewYInverted ? input_View.y : -input_View.y) * Time.deltaTime;

        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        camereHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }
    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.JumpingFalloff);
    }
    private void CalculateAimingIn()
    {
        if (!currentWeapon)
        {
            return;
        }
        currentWeapon.isAimingIn = isAimingIn;
    }
    private void Jump()
    {
        if(!CharacterController.isGrounded)
        {
            return;
        }
        jumpingForce = Vector3.up * playerSettings.JumpingHeight;
    }
    private void ToggleSprint()
    {
        if (input_Movement.y <= 0.2f)
        {
            isSprinting = false;
            return;
        }

        isSprinting = !isSprinting;

    }
    private void StopSprint()
    {
        if(playerSettings.SprintingHold)
            isSprinting = false;
    }
    private void AimingPressed()
    {
        isAimingIn = true;

        if(isSprinting)
            isSprinting= false;
    }
    private void AimingReleased()
    {
        isAimingIn = false;
    }

    void IDamageable.TakeDamage(int damage)
    {
        throw new NotImplementedException();
    }

    #endregion

    #endregion
}
