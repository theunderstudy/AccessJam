using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : Ability
{
    public Projectile ProjectilePrefab;
    public override void Use(Unit Owner, Targetable Target)
    {
        Projectile NewProjectile = Instantiate(ProjectilePrefab);
        NewProjectile.Init(Owner , Damage * Owner.DamageMultiplier);
        NewProjectile.Launch(Owner.transform.position , Target.transform.position);
        bReady = false;
    }
}
