using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DamageTypes
{
    normal,
    melee,
    ranged,
    magic
}


public enum ArmourTypes
{
    normal,
    melee,
    ranged,
    magic
}
public struct DamagePacket
{
    public DamagePacket(float InDamage, DamageTypes InDamageType)
    {
        Damage = InDamage;
        DamageType = InDamageType;
    }
    public float Damage;
    public DamageTypes DamageType;
}
public abstract class Ability : MonoBehaviour
{
    public DamageTypes DamageType;
    public float CoolDown = 1;
    public float Damage = 1;
    public float AbilityRange = 1.5f;
    protected float CurrentTime = 0;
    protected bool bReady=true;

   
    // Update is called once per frame  

    public virtual bool CanUse(Unit Owner)
    {
        if (!bReady)
        {
            CurrentTime +=( Time.deltaTime * Owner.CooldownMultipier);
            if (CurrentTime > CoolDown)
            {
                CurrentTime = 0;
                bReady = true;
                return true;
            }
            return false;
        }

        return true;
    }
    public virtual Targetable GetValidTargetInRange(Unit Owner)
    {
        return UnitManager.Instance.GetNearestUnitOfOtherAllegianceInRange(Owner.transform.position, Owner.Allegiance, AbilityRange);
        
    }

    // Build interface for this
    public abstract void Use(Unit Owner,Targetable Target);

    
}
