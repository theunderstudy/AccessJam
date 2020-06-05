using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    public GameObject UnitScreen;
    public BuildingUI BuildingScreen;

    public void OpenUnitScreen(Unit InUnit) { }
    public void OpenBuildingScreen(UnitSpawner Building)
    {
        BuildingScreen.gameObject.SetActive(true);
        BuildingScreen.Init(Building);
    }

    public void CloseOpenUI()
    {
        BuildingScreen.gameObject.SetActive(false);
    }
}
