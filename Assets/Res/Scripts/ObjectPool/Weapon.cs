using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Variables

    #region Public Variables

    public GameObject bulletPrefab;
    public Transform muzzlePos;
    public ParticleSystem muzzlePS;
    public WeaponData weaponData;

    #endregion

    #region Private Variables

    private float fireRate = 3f;
    private int NumberOfBullet = 30;
    private float timeSinceLastShot = 0;
    private bool isInUI=false;
    ObjectPool bulletPool;

    #endregion

    #endregion

    #region Methods

    #region Unity Callbacks
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnUIOpening, DisableFiring);
        EventManager.AddHandler(GameEvent.OnUIClosing, EnableFiring);
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnUIOpening, DisableFiring);
        EventManager.RemoveHandler(GameEvent.OnUIClosing, EnableFiring);
    }
    private void DisableFiring(object obj)
    {
        isInUI = true;
    }
    private void EnableFiring(object obj)
    {
        isInUI = false;
    }
    private void Start()
    {
        weaponData.LeftoverAmmo = weaponData.AmmoCapacity;

        fireRate = weaponData.FireRate;

        NumberOfBullet = weaponData.AmmoCapacity;

        bulletPool = new ObjectPool(bulletPrefab, NumberOfBullet);
    }
    private void Update()
    {
        timeSinceLastShot += Time.time;

        if (weaponData.LeftoverAmmo <= 0)
        {
            return;
        }

        if (Input.GetMouseButton(0) && timeSinceLastShot > 1f / fireRate && !isInUI)
        {
            bulletPool.ActivateNext(muzzlePos.position, transform.rotation);
            
            muzzlePS.Play();

            AudioManager.Instance.PlaySound("rifle_shoot");

            weaponData.LeftoverAmmo--;

            EventManager.Broadcast(GameEvent.OnAmmoTextUpdate, weaponData.LeftoverAmmo);

            timeSinceLastShot = 0;
        }
    }

    #endregion

    #endregion
}
