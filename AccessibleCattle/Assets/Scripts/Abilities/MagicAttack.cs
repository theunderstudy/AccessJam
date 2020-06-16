using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : Ability
{
    public Projectile MagicProjectile;
    public override void Use(Unit Owner, Targetable Target)
    {
        Projectile NewProjectile = Instantiate(MagicProjectile);
        DamagePacket NewPacket = new DamagePacket(Damage * Owner.DamageMultiplier, DamageType);
        NewProjectile.Init(Owner, NewPacket);
        NewProjectile.Launch(Owner.transform.position, Target.transform.position);
        bReady = false;
    }
}
