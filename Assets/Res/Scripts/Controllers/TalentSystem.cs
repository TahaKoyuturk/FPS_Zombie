using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalentSystem : MonoBehaviour
{
    #region Variables

    #region Public Variables

    [SerializeField] private List<TalentData> datas;

    [SerializeField] private List<TalentUI> weaponUI;
    [SerializeField] private List<TalentUI> characterUI;

    [Space(5)]
    [Header("Texts")]
    [SerializeField] private TMP_Text TalentPointText;

    [Header("Data Refs")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private WeaponData weaponData;

    #endregion

    #region Private Varibles

    private int i = 0, j = 0;

    #endregion

    #endregion

    #region Methods

    #region Unity Callbacks

    void Start()
    {
        TalentPointText.text = playerData.TalentPoint.ToString();

        SetUIs();
    }

    #endregion

    #region Custom Methods

    private void SetUIs()
    {
        i = 0;
        j = 0;

        foreach (TalentData data in datas)
        {
            if (data.talentChoice == TalentChoice.Character)
            {
                SetCharacterUI(data, i);
                i++;
            }
            if (data.talentChoice == TalentChoice.Weapon)
            {
                SetWeaponUI(data, j);
                j++;
            }
        }
    }

    private void SetCharacterUI(TalentData data, int index)
    {
        if(playerData.TalentPoint >= data.price)
        {
            characterUI[index].blurImage.gameObject.SetActive(false);
        }
        else
        {
            characterUI[index].blurImage.gameObject.SetActive(true);
        }

        characterUI[index].title.text = data.title.ToString();
        characterUI[index].image.sprite = data.image;
        characterUI[index].price.text = data.price.ToString();
        characterUI[index].button.onClick.AddListener(() => CharacterButtonClicked(data,index));
    }

    private void SetWeaponUI(TalentData data, int index)
    {
        if (playerData.TalentPoint >= data.price)
        {
            weaponUI[index].blurImage.gameObject.SetActive(false);
        }
        else
        {
            weaponUI[index].blurImage.gameObject.SetActive(true);
        }

        weaponUI[index].blurImage.gameObject.SetActive(false);
        weaponUI[index].title.text = data.title.ToString();
        weaponUI[index].image.sprite = data.image;
        weaponUI[index].price.text = data.price.ToString();
        weaponUI[index].button.onClick.AddListener(() => WeaponButtonClicked(data,index));
    }

    private void CharacterButtonClicked(TalentData data,int index)
    {
        
        if (playerData.TalentPoint >= data.price)
        {
            characterUI[index].button.interactable = true;
            characterUI[index].blurImage.gameObject.SetActive(false);
            playerData.TalentPoint -= data.price;
        }
        else
        {
            characterUI[index].button.interactable = false;
            characterUI[index].blurImage.gameObject.SetActive(true);
            return;
        }

        TalentPointText.text = playerData.TalentPoint.ToString();

        switch (data.id)
        {
            case 0:
                CTU_Inc_MoveSpeed();
                break;
            case 1:
                CTU_Inc_JumpPower();
                break;
            case 2:
                CTU_Inc_MaxHp();
                break;
            default:
                break;
        }
    }

    private void WeaponButtonClicked(TalentData data,int index)
    {

        if (playerData.TalentPoint >= data.price)
        {
            weaponUI[index].button.interactable = true;
            weaponUI[index].blurImage.gameObject.SetActive(false);
            playerData.TalentPoint -= data.price;
        }
        else
        {
            weaponUI[index].button.interactable = false;
            weaponUI[index].blurImage.gameObject.SetActive(true);
            return;
        }

        TalentPointText.text = playerData.TalentPoint.ToString();

        switch (data.id)
        {
            case 0:
                WTU_Inc_DamageAmount();
                break;
            case 1:
                WTU_Inc_AmmoCapacity();
                break;
            case 2:
                WTU_PierceShot();
                break;
            default:
                break;
        }
    }
    private void CTU_Inc_MoveSpeed()
    {
        playerData.MoveSpeed += 1.25f;
        SetUIs();
    }
    private void CTU_Inc_JumpPower()
    {
        playerData.JumpPower += 1.25f;
        SetUIs();
    }
    private void CTU_Inc_MaxHp()
    {
        playerData.MaxHealth += 100;
        SetUIs();
    }
    private void WTU_Inc_DamageAmount()
    {
        weaponData.Damage += 15;
        SetUIs();
    }
    private void WTU_Inc_AmmoCapacity()
    {
        weaponData.AmmoCapacity += 10;
        SetUIs();
    }
    private void WTU_PierceShot()
    {
        weaponData.isPierceShot = true;
        SetUIs();
    }


    #endregion

    #endregion
}
[Serializable]
public class TalentUI
{
    public UnityEngine.UI.Button button;

    public UnityEngine.UI.Image image;

    public TMP_Text title;

    public TMP_Text price;

    public UnityEngine.UI.Image blurImage;
}
