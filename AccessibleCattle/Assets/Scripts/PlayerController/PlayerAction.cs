using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public Targetable CurrentTarget;
    public virtual void InputDown()
    {
        // Check for raycast target. Unit or building
        CurrentTarget = PlayerController.Instance.GetTargetableAtMousePosition();

    }
    public virtual void InputHeld()
    {
        // move the screen
    }
    public virtual void InputUp()
    {
        if (CurrentTarget!=null)
        {
            Targetable Temp = PlayerController.Instance.GetTargetableAtMousePosition();
            if (Temp != CurrentTarget)
            {
                CurrentTarget = null;
                return;
            }


            if (CurrentTarget is Unit)
            {
                UIManager.Instance.OpenUnitScreen(CurrentTarget as Unit);
                Debug.Log("Unit selected");
            }
            if (CurrentTarget is UnitSpawner)
            {
                UIManager.Instance.OpenBuildingScreen(CurrentTarget as UnitSpawner);
                Debug.Log("building selected");
            }
        }

        CurrentTarget = null;
    }
}
