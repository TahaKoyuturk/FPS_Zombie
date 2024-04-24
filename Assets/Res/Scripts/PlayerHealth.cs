using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region Variables

    #region Public Variables

    public PlayerData PlayerData;
    #endregion

    #endregion

    #region Methods

    #region Unity Callbacks

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerHit, TakeDamage);
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerHit, TakeDamage);
    }
    private void Start()
    {
        EventManager.Broadcast(GameEvent.OnHealthUpdate, PlayerData.MaxHealth);
        EventManager.Broadcast(GameEvent.OnMaxHealthSet, PlayerData.MaxHealth);

        if(!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level",0);

        if (!PlayerPrefs.HasKey("Kills"))
            PlayerPrefs.SetInt("Kills", 0);

        if (!PlayerPrefs.HasKey("Highscore"))
            PlayerPrefs.SetInt("Highscore", 0);

        EventManager.Broadcast(GameEvent.OnHighScoreUpdate, -1);

    }

    #endregion

    #region Custom Methods

    public void TakeDamage(object damage)
    {
        float _damage = 20f;

        if (float.TryParse(damage.ToString(), out _damage))
        {
            PlayerData.CurrentHealth -= _damage; 
            EventManager.Broadcast(GameEvent.OnHealthUpdate, PlayerData.CurrentHealth);

            if (PlayerData.CurrentHealth <= 0)
            {
                EventManager.Broadcast(GameEvent.OnPlayerDead,-1);
            }
        }
        else
        {
            Debug.LogError("Damage parameter is not convertible to a float!");
        }
    }

    #endregion

    #endregion
}
