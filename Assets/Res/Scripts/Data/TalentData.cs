using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TalentData", menuName = "Data/Talent", order = 1)]
public class TalentData : ScriptableObject
{
    public int id;

    public Sprite image;

    public string title;

    public int price;

    public TalentChoice talentChoice;
}
public enum TalentChoice
{
    Weapon,
    Character
}
