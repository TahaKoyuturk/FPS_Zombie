using System.Collections;
using UnityEngine;

public class Col_Magazine : MonoBehaviour,ICollectible
{
    #region Variables

    #region Public Variables

    [SerializeField] private int MagazineAmount = 30;

    public WeaponData weaponData;

    #endregion

    #region Private Variables
    private int _mag_amount = 30;
    #endregion
    #endregion

    #region Methods

    #region Unity Callbacks

    private void Start()
    {
        _mag_amount= MagazineAmount;
        EventManager.Broadcast(GameEvent.OnAmmoTextUpdate, weaponData.LeftoverAmmo);
    }

    #endregion

    #region Custom Methods

    public void Collect()
    {
        if (MagazineAmount <= 0)
        {
            StartCoroutine(WaitTillRespawn(15));
        } 

        weaponData.LeftoverAmmo += MagazineAmount;

        if(weaponData.LeftoverAmmo >= weaponData.AmmoCapacity)
        {
            this.MagazineAmount = weaponData.LeftoverAmmo - weaponData.AmmoCapacity;
            weaponData.LeftoverAmmo = weaponData.AmmoCapacity;
        }

        EventManager.Broadcast(GameEvent.OnAmmoTextUpdate, weaponData.LeftoverAmmo);
        AudioManager.Instance.PlaySound("item_equip");
    }

    IEnumerator WaitTillRespawn(float time)
    {
        transform.gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        MagazineAmount = _mag_amount;
        transform.gameObject.SetActive(true);
    }
    #endregion

    #endregion
}
