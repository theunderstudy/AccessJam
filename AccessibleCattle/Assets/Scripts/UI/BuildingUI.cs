using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingUI : MonoBehaviour
{
    public Text BuildingNameText;
    public Image BuildingImage;
    // Progress bars
    public Image AttackDamageFillBar;
    public Image AttackSpeedFillBar;
    public Image TrainingSpeedFillBar;

    public UnitSpawner CurrentBuilding;

    public void Init(UnitSpawner Building)
    {
        CurrentBuilding = Building;
        UpdateBars();
    }
    public void UpgradeDamage()
    {
        CurrentBuilding.UnitDamageMultiplier.Upgrade();
        UpdateBars();
    }
    public void UpgradeSpeed()
    {
        CurrentBuilding.UnitAttackSpeedMultiplier.Upgrade();
        UpdateBars();
    }
    public void UpgradeTraining()
    {
        CurrentBuilding.TrainingSpeed.Upgrade();
        UpdateBars();
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
    private void UpdateBars()
    {
        DOTween.Kill(AttackDamageFillBar.GetInstanceID());
        DOTween.Kill(AttackSpeedFillBar.GetInstanceID());
        DOTween.Kill(TrainingSpeedFillBar.GetInstanceID());
        AttackDamageFillBar.DOFillAmount(CurrentBuilding.UnitDamageMultiplier.CurrentValue / CurrentBuilding.UnitDamageMultiplier.MaxValue, 0.5f);
        AttackSpeedFillBar.DOFillAmount(CurrentBuilding.UnitAttackSpeedMultiplier.CurrentValue / CurrentBuilding.UnitAttackSpeedMultiplier.MaxValue, 0.5f);
        TrainingSpeedFillBar.DOFillAmount(CurrentBuilding.TrainingSpeed.CurrentValue / CurrentBuilding.TrainingSpeed.MaxValue, 0.5f);
    }
}
