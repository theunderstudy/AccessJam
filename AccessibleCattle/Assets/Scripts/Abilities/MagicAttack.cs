using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : Ability
{
    public Projectile MagicProjectile;
    public override void Use(Unit Owner, Targetable Target)
    {
        Projectile NewProjectile = Instantiate(MagicProjectile);
        NewProjectile.Init(Owner, Damage * Owner.DamageMultiplier);
        NewProjectile.Launch(Owner.transform.position, Target.transform.position);
        bReady = false;
    }
}
