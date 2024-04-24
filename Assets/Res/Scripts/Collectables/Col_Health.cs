using System.Collections;
using UnityEngine;
public class Col_Health : MonoBehaviour,ICollectible
{
    #region Variables

    #region Public Variables

    public PlayerData playerdata;

    [SerializeField] private float healAmount;

    #endregion

    #region Private Variables

    private float _heal_amount = 30;
    #endregion

    #endregion

    #region Methods

    #region Unity Callbacks

    private void Start()
    {
        playerdata.CurrentHealth = playerdata.MaxHealth;
        _heal_amount = healAmount;
    }

    #endregion

    #region Custom Methods
    public void Collect()
    {
        AudioManager.Instance.PlaySound("medicine");

        playerdata.CurrentHealth += healAmount;

        if(playerdata.CurrentHealth >= playerdata.MaxHealth)
        {
            playerdata.CurrentHealth = playerdata.MaxHealth;
        }

        EventManager.Broadcast(GameEvent.OnHealthUpdate, playerdata.CurrentHealth);

        StartCoroutine(WaitTillRespawn(12));
    }
    IEnumerator WaitTillRespawn(float time)
    {
        transform.gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        healAmount=_heal_amount;
        transform.gameObject.SetActive(true);
    }
    #endregion

    #endregion
}
