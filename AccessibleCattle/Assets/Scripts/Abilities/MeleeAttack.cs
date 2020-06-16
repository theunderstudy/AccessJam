using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Ability
{
    public override void Use(Unit Owner, Targetable Target)
    {
        DamagePacket MeleeDamagePacked = new DamagePacket(Damage * Owner.DamageMultiplier , DamageType);

        Target.TakeDamage( MeleeDamagePacked, (bool bKilled) =>
        {
            if (bKilled)
            {
                Debug.Log("Killed enemy");
            }
        });

        bReady = false;
    }
}
