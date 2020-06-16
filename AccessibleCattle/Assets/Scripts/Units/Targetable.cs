using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targetable : MonoBehaviour
{
    public delegate void AttackCallbackDel(bool KilledEnemy);
    public AttackCallbackDel AttackCallback;
    public UnitAllegiance Allegiance;

    public ArmourTypes ArmourType;
    public float DamageReduction = 0.5f;
    public abstract void TakeDamage(DamagePacket IncomingDamagePacket, AttackCallbackDel Callback=null);

    public virtual void ApplyDefenceModifiers(ref DamagePacket IncomingDamagePacket)
    {        
        switch (IncomingDamagePacket.DamageType)
        {
            case DamageTypes.normal:
                break;
            case DamageTypes.melee:
                // Magic armour would reduce melee damage
                if (ArmourType == ArmourTypes.magic)
                {
                    IncomingDamagePacket.Damage *= 1 - DamageReduction;
                }
                break;
            case DamageTypes.ranged:
                // Melee armour would reduce ranged damage
                if (ArmourType == ArmourTypes.melee)
                {
                    IncomingDamagePacket.Damage *= 1 - DamageReduction;
                }
                break;
            case DamageTypes.magic:
                // Ranged armour would reduce magic damage 
                if (ArmourType == ArmourTypes.ranged)
                {
                    IncomingDamagePacket.Damage *= 1 - DamageReduction;
                }

                break;
            default:
                break;
        }
       
    }
}
