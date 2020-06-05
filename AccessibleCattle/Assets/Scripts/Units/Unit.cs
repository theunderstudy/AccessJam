using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using DG.Tweening;


public enum UnitStates
{
    nil,
    sleep,
    move,
    attack
}

public enum UnitAllegiance
{
    nil,
    friendly,
    enemy
}
[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Targetable
{
    protected Targetable CurrentTarget;
    public NavMeshAgent Agent;
    public UnitStates CurrentState;   
    protected Vector3 FinalDestination;

    public Ability[] Abilities;
    private Ability CurrentAbility;
    public Ability BasicAttack;


    [Header("Stats")]

    protected float HP = 10;
    public float MaxHP = 10;
    public float AgroRange = 5;

    public float CooldownMultipier = 1;
    public float DamageMultiplier = 1;


    public MeshRenderer[] PrimaryMeshRenderers;
    public MeshRenderer[] SecondaryMeshRenderers;


 
    public virtual void Init(UnitAllegiance NewAllegiance)
    {
        Allegiance = NewAllegiance;
        CurrentState = UnitStates.sleep;
        GetComponent<MeshRenderer>().material = NewAllegiance == UnitAllegiance.friendly ?  UnitManager.Instance.FriendlyMat : UnitManager.Instance.EnemyMat;
        SetFinalDestination(NewAllegiance == UnitAllegiance.friendly ? GameManager.Instance.EnemyBase.transform.position : GameManager.Instance.FriendlyBase.transform.position);
        SetState(UnitStates.move);
        ColorManager.Instance.SetUnitColors(this);
        HP = MaxHP;
    }
    protected virtual void SetState(UnitStates NewState)
    {
        CurrentState = NewState;
    }
    public virtual void SetFinalDestination(Vector3 Destination)
    {
        FinalDestination = Destination;
        SetMoveTarget(Destination);
    }
    public virtual void SetMoveTarget(Vector3 Destination)
    {
        Agent.isStopped = false;
        Agent.SetDestination(Destination);        
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case UnitStates.nil:
                // move towards enemy base
                break;
            case UnitStates.sleep:
                // Wait for interuption
                break;
            case UnitStates.move:
                Targetable NewTargetInRange;
                // Check for abilities which have valid targets
                foreach (Ability ability in Abilities)
                {
                    if (!ability.CanUse(this))
                    {
                        continue;
                    }
                    NewTargetInRange = ability.GetValidTargetInRange(this);
                    if (NewTargetInRange)
                    {
                        CurrentAbility = ability;
                        CurrentTarget = NewTargetInRange;
                        Agent.isStopped = true;
                        SetState(UnitStates.attack);
                        return;
                    }
                }

                // Check if target in basic attack range
                NewTargetInRange = BasicAttack.GetValidTargetInRange(this);
                if (NewTargetInRange!=null)
                {
                    CurrentAbility = BasicAttack;
                    CurrentTarget = NewTargetInRange;
                    Agent.isStopped = true;
                    SetState(UnitStates.attack);
                    return;
                }

                // Check if target in agro range
                NewTargetInRange = UnitManager.Instance.GetNearestUnitOfOtherAllegianceInRange(transform.position,Allegiance,AgroRange);
                if (NewTargetInRange != null)
                {
                    SetMoveTarget(NewTargetInRange.transform.position);
                    return;
                }
                // Else keep moving towards final destination
                break;
            case UnitStates.attack:
                // Damage enemy on timer,
                // Break if out of range
                if (CurrentTarget == null)
                {
                    SetFinalDestination(FinalDestination);
                    SetState(UnitStates.move);
                    return;
                }
                transform.LookAt(CurrentTarget.transform);
                if (CurrentAbility.CanUse(this))
                {
                    CurrentAbility.Use(this,CurrentTarget);
                    transform.DOPunchPosition(Vector3.forward /8, 0.5f);
                }
                else
                {
                    // Check for other abilities which can be used
                }

                break;
            default:
                break;
        }
    }


    protected void Die()
    {
        UnitManager.Instance.RemoveUnit(this);
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        
    }

    public override void TakeDamage(float Damage, AttackCallbackDel Callback)
    {
        HP -= Damage;
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, HP / MaxHP);
        if (HP < 0)
        {
            Callback(true);
            Die();
            return;
        }
        if (Callback!=null)
        {
            Callback(false);
        }
    }
}

