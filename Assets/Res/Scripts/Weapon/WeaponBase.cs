using UnityEngine;
using static stg_models;

public class WeaponBase : MonoBehaviour
{
    #region Variables

    #region Public Variables

    [Header("Settings")]
    public WeaponSettingsModel settings;

    [Header("References")]
    public Animator animator;
    [field: SerializeField]
    private WeaponBase WeaponData;

    [Header("Weapon Breathing")]
    public Transform WeaponSwayObject;

    public float SwayAmountA = 1;
    public float SwayAmountB = 2;
    public float SwayScale = 600;
    public float SwayLerpSpeed = 14;

    public float SwayTime;
    public Vector3 SwayPosition;

    [Header("Sights")]
    public Transform sightTarget;
    public float sightOffset;
    public float aimingInTime;

    #endregion

    #region Protected Variables

    protected InputHandler characterContorller;

    #endregion

    #region Private Variables

    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;

    bool isInitialised;

    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    Vector3 TargetWeaponRotation;
    Vector3 TargetWeaponRotationVelocity;

    [HideInInspector]
    public bool isAimingIn;

    #endregion

    #endregion

    #region Methods

    #region Unity Callbacks

    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
    }
    private void Update()
    {
        if (!isInitialised)
        {
            return;
        }

        CalculateWeaponRotation();
        SetWeaponAmimations();
        CalculateWeaponSway();
        CalculateAimingIn();
    }

    #endregion

    #region Custom Methods
    public void Initialize(InputHandler characterContorller)
    {
        this.characterContorller = characterContorller;
        isInitialised = true;
    }
    private void CalculateWeaponRotation()
    {
        animator.speed = characterContorller.weaponAnimationSpeed;

        TargetWeaponRotation.y += (isAimingIn ? settings.SwayAmount / 5 : settings.SwayAmount) * (settings.SwayXInverted ? -characterContorller.input_View.x : characterContorller.input_View.x) * Time.deltaTime;

        TargetWeaponRotation.x += (isAimingIn ? settings.SwayAmount / 5 : settings.SwayAmount) * (settings.SwayYInverted ? characterContorller.input_View.y : -characterContorller.input_View.y) * Time.deltaTime;

        TargetWeaponRotation.x = Mathf.Clamp(TargetWeaponRotation.x, -settings.SwayClampX, settings.SwayClampX);
        TargetWeaponRotation.y = Mathf.Clamp(TargetWeaponRotation.x, -settings.SwayClampY, settings.SwayClampY);
        TargetWeaponRotation.z = isAimingIn ? 0 : TargetWeaponRotation.y;

        TargetWeaponRotation = Vector3.SmoothDamp(TargetWeaponRotation, Vector3.zero, ref TargetWeaponRotationVelocity, settings.SwayResetSmoothing);

        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, TargetWeaponRotation, ref newWeaponRotationVelocity, settings.SwaySmooting);

        transform.localRotation = Quaternion.Euler(newWeaponRotation);
    }
    private void CalculateWeaponSway()
    {
        var targetPosition = LissaJousCurve(SwayTime, SwayAmountA, SwayAmountB) / (isAimingIn ? SwayScale * 4 : SwayScale);

        if (!isAimingIn)
            SwayPosition = Vector3.Lerp(SwayPosition, targetPosition, Time.smoothDeltaTime * SwayLerpSpeed);
        else
            SwayPosition = Vector3.zero;

        SwayTime += Time.deltaTime;

        if (SwayTime > 6.3f)
            SwayTime = 0;
        
    }
    private Vector3 LissaJousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B - Time + Mathf.PI));
    }
    protected virtual void SetWeaponAmimations()
    {
        animator.SetBool("isSprinting", characterContorller.isSprinting);
    }
    protected virtual void CalculateAimingIn()
    {
        var targetPosition = transform.position;

        if (isAimingIn)
            targetPosition = characterContorller.camereHolder.transform.position + (WeaponSwayObject.transform.position - sightTarget.position) + (characterContorller.camereHolder.transform.forward * sightOffset);
        
        weaponSwayPosition = WeaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, aimingInTime);
        WeaponSwayObject.transform.position = weaponSwayPosition + SwayPosition;
    }


    #endregion

    #endregion
}
