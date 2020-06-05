using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targetable : MonoBehaviour
{
    public delegate void AttackCallbackDel(bool KilledEnemy);
    public AttackCallbackDel AttackCallback;
    public UnitAllegiance Allegiance;
    public abstract void TakeDamage(float Damage, AttackCallbackDel Callback=null);
}
