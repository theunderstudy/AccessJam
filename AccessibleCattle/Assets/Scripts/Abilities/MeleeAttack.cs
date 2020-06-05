using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Ability
{
    public override void Use(Unit Owner, Targetable Target)
    {
        Target.TakeDamage(Damage * Owner.DamageMultiplier, (bool bKilled) =>
        {
            if (bKilled)
            {
                Debug.Log("Killed enemy");
            }
        });

        bReady = false;
    }
}
