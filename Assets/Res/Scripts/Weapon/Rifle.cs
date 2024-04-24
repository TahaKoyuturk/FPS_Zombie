using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : WeaponBase
{
    protected override void SetWeaponAmimations()
    {
        animator.SetBool("isSprinting", characterContorller.isSprinting);
    }
}
