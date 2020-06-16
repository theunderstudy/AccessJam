using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{

    public GameObject UnitScreen;
    public BuildingUI BuildingScreen;
    public FadeText FadeTextPrefab;
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


    public void DisplayTextOnScreen(Vector3 WorldPosition, string Text , Color Color)
    {
        FadeText NewText = Instantiate(FadeTextPrefab,transform);
        NewText.Init(Text,Color);
        NewText.transform.position = Camera.main.WorldToScreenPoint(WorldPosition);

        NewText.transform.DOLocalMoveY(NewText.transform.localPosition.y+ 50, 1.0f).SetEase(Ease.OutQuart);
        NewText.MainText.DOFade(0, 1.5f).SetEase(Ease.InQuint).OnComplete(() => Destroy(NewText)); 
    }
}
