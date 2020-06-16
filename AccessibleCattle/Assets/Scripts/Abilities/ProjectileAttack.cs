using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : Ability
{
    public Projectile ProjectilePrefab;
    public override void Use(Unit Owner, Targetable Target)
    {
        Projectile NewProjectile = Instantiate(ProjectilePrefab);
        DamagePacket NewPacket = new DamagePacket(Damage * Owner.DamageMultiplier , DamageType);
        NewProjectile.Init(Owner , NewPacket);
        NewProjectile.Launch(Owner.transform.position , Target.transform.position);
        bReady = false;
    }
}
