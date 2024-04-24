using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    #region Variables

    #region Public Variables

    public PlayerData PlayerData;

    #endregion

    #region Private Variables

    private int _XP_Incremental = 20;
    #endregion

    #endregion

    #region Methods

    #region Unity Callbacks

    private void Start()
    {
        PlayerData.CurrentXP = 0;
        _XP_Incremental = (PlayerPrefs.GetInt("Level") + 1) * 20;
        EventManager.Broadcast(GameEvent.OnXPSet, PlayerData.CurrentXP);
    }
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnEnemyKill, EarnXP);
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnEnemyKill, EarnXP);
    }

    #endregion

    #region Custom Methods

    public void EarnXP(object value)
    {
        PlayerData.CurrentXP += _XP_Incremental;

        EventManager.Broadcast(GameEvent.OnXPUpdate, _XP_Incremental);

        if (PlayerData.CurrentXP >= PlayerData.MaxXP)
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            PlayerData.MaxXP += 100;
            EventManager.Broadcast(GameEvent.OnLevelUp, PlayerPrefs.GetInt("Level"));
            
        }
    }

    #endregion

    #endregion
}
