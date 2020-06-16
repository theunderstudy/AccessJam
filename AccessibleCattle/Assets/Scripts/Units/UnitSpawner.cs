using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuildingStat
{
    public float CurrentValue;
    public float MaxValue;
    public float UpgradeAmount;
    public void Upgrade()
    {
        if (CurrentValue < MaxValue)
        {
            CurrentValue += UpgradeAmount;
        }      
    }
}
public class UnitSpawner : Targetable
{
    public Unit UnitToSpawn;

    public BuildingStat TrainingSpeed;
    public BuildingStat UnitDamageMultiplier;
    public BuildingStat UnitAttackSpeedMultiplier;

    private float CurrentTime = 1;

    public MeshRenderer[] PrimaryMeshRenderers;
    public MeshRenderer[] SecondaryMeshRenderers;


    private void Awake()
    {
        ColorManager.Instance.SetSpawnerColors(this);
    }
    public virtual void SpawnUnit()
    {
        if (!bSpawn)
        {
            return;
        }
        Unit NewUnit = Instantiate(UnitToSpawn , transform.position,transform.rotation , transform);
        NewUnit.Init(Allegiance);
        NewUnit.CooldownMultipier *= UnitAttackSpeedMultiplier.CurrentValue;
        NewUnit.DamageMultiplier *= UnitDamageMultiplier.CurrentValue;

        int Bounty = NewUnit.GoldBounty;
        Bounty += (int)Mathf.Lerp(1,10 , UnitDamageMultiplier.CurrentValue / UnitDamageMultiplier.MaxValue);
        Bounty += (int)Mathf.Lerp(1,10 , UnitAttackSpeedMultiplier.CurrentValue / UnitAttackSpeedMultiplier.MaxValue);

        NewUnit.GoldBounty = Bounty;

        //NewUnit.SetDestination(GameManager.Instance.EnemyBase.transform.position);
        UnitManager.Instance.RegisterUnit(NewUnit);
    }

    public bool bSpawn = false;

    private void Update()
    {
        CurrentTime -= Time.deltaTime;
        if (CurrentTime <= TrainingSpeed.CurrentValue)
        {
            SpawnUnit();
            CurrentTime = 0;
        }
    }

    public override void TakeDamage(DamagePacket InDamagePacket, AttackCallbackDel Callback = null)
    {
        Debug.Log("Building damaged");
    }
}
