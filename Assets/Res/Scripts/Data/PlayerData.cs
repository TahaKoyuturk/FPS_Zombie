using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player", order = 1)]
public class PlayerData : ScriptableObject
{
    public float MaxHealth;
    public float MaxXP;

    public float MoveSpeed;
    public float JumpPower;

    public int TalentPoint;

    [HideInInspector]
    public float CurrentHealth;
    [HideInInspector]
    public float CurrentXP;
    [HideInInspector]
    public int NumOfKills = 0;
}
