using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    [HideInInspector]
    public List<Unit> AllUnits =  new List<Unit>();
    private List<Unit> FriendlyUnits = new List<Unit>();
    private List<Unit> EnemyUnits = new List<Unit>();


    public Material FriendlyMat, EnemyMat;
    public void RegisterUnit(Unit NewUnit)
    {
        AllUnits.Add(NewUnit);
        switch (NewUnit.Allegiance)
        {
            case UnitAllegiance.friendly:
                FriendlyUnits.Add(NewUnit);
                break;
            case UnitAllegiance.enemy:
                EnemyUnits.Add(NewUnit);
                break;
            default:
                break;
        }
    }

    public void RemoveUnit(Unit NewUnit)
    {
        AllUnits.Remove(NewUnit);
        switch (NewUnit.Allegiance)
        {
            case UnitAllegiance.friendly:
                FriendlyUnits.Remove(NewUnit);
                break;
            case UnitAllegiance.enemy:
                EnemyUnits.Remove(NewUnit);
                break;
            default:
                break;
        }
    }

    public Unit GetNearestUnitOfOtherAllegianceInRange(Vector3 Position, UnitAllegiance Allegiance , float Range)
    {
        Unit ClosestUnit=null;
        Unit TestUnit = null;
        float CurrentDistance = Mathf.Infinity;
        float TestDistance;

        switch (Allegiance)
        {
            case UnitAllegiance.nil:
                break;
            case UnitAllegiance.friendly:
                for (int i = 0; i < EnemyUnits.Count; i++)
                {
                    TestUnit = EnemyUnits[i];
                    TestDistance = Mathf.Abs(Vector3.Distance(Position, TestUnit.transform.position));
                    if (TestDistance < Range)
                    {
                        if (TestDistance < CurrentDistance)
                        {
                            CurrentDistance = TestDistance;
                            ClosestUnit = TestUnit;
                        }
                    }
                }
                break;
            case UnitAllegiance.enemy:
                for (int i = 0; i < FriendlyUnits.Count; i++)
                {
                    TestUnit = FriendlyUnits[i];
                    TestDistance = Mathf.Abs(Vector3.Distance(Position, TestUnit.transform.position));
                    if (TestDistance < Range)
                    {
                        if (TestDistance < CurrentDistance)
                        {
                            CurrentDistance = TestDistance;
                            ClosestUnit = TestUnit;
                        }
                    }
                }
                break;

           
            default:
                break;
        }

        return ClosestUnit;
    }
}
