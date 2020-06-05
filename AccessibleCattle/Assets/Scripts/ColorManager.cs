using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ColorManager))]
public class ColorManagerEditor : Editor
{
    private ColorManager m_CM;

    private float IntensitySliderLast = 1;
    private ColorSet StoredColors;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        m_CM = (ColorManager)target;
        if (GUILayout.Button("Generate color sets"))
        {
            m_CM.GenerateColorSets();
        }

     //Color PrimaryFriendlyColor;
     //Color SecondaryFriendlyColor;
     //Color PrimaryEnemyColor;
     //Color SecondaryEnemyColor;
     //Color BackgroundColor;
     //Color GroundColor;
      
        // check if color normal color set has changed
        if (StoredColors.PrimaryFriendlyColor != m_CM.NormalSet.PrimaryFriendlyColor)
        {
            StoredColors.PrimaryFriendlyColor = m_CM.NormalSet.PrimaryFriendlyColor;
            m_CM.GenerateColorSets();
        }
        if (StoredColors.SecondaryFriendlyColor != m_CM.NormalSet.SecondaryFriendlyColor)
        {
            StoredColors.SecondaryFriendlyColor = m_CM.NormalSet.SecondaryFriendlyColor;
            m_CM.GenerateColorSets();
        }
        if (StoredColors.PrimaryEnemyColor != m_CM.NormalSet.PrimaryEnemyColor)
        {
            StoredColors.PrimaryEnemyColor = m_CM.NormalSet.PrimaryEnemyColor;
            m_CM.GenerateColorSets();
        }
        if (StoredColors.SecondaryEnemyColor != m_CM.NormalSet.SecondaryEnemyColor)
        {
            StoredColors.SecondaryEnemyColor = m_CM.NormalSet.SecondaryEnemyColor;
            m_CM.GenerateColorSets();
        }
        if (StoredColors.GroundColor != m_CM.NormalSet.GroundColor)
        {
            StoredColors.GroundColor = m_CM.NormalSet.GroundColor;
            m_CM.GenerateColorSets();
        }
        if (StoredColors.BackgroundColor != m_CM.NormalSet.BackgroundColor)
        {
            StoredColors.BackgroundColor = m_CM.NormalSet.BackgroundColor;
            m_CM.GenerateColorSets();
        }
    }
}
#endif


[System.Serializable]
public struct ColorSet
{
    public Color PrimaryFriendlyColor;
    public Color SecondaryFriendlyColor;
    public Color PrimaryEnemyColor;
    public Color SecondaryEnemyColor;
    public Color BackgroundColor;
    public Color GroundColor;
}
[System.Serializable]
public enum ColorSettings
{
    Normal,
    Protanopia,
    Deuteranopia,
    Tritanopia
}


public class ColorManager : Singleton<ColorManager>
{  
    public MeshRenderer[] GroundMat;
    public ColorSet NormalSet;
    public ColorSet ProtanopiaSet;
    public ColorSet DeuteranopiaSet;
    public ColorSet TritanopiaSet;
    private ColorSet CurrentSet;
    private float ColorTweenTime = 1;
    private ColorSettings SelectedColor = ColorSettings.Normal;

    protected override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.HasKey("ColorSetting"))
        {
            SelectedColor = (ColorSettings)PlayerPrefs.GetInt("ColorSetting");
        }
        switch (SelectedColor)
        {
            case ColorSettings.Normal:
                CurrentSet = NormalSet;
                break;
            case ColorSettings.Protanopia:
                CurrentSet = ProtanopiaSet;
                break;
            case ColorSettings.Deuteranopia:
                CurrentSet = DeuteranopiaSet;
                break;
            case ColorSettings.Tritanopia:
                CurrentSet = TritanopiaSet;
                break;

            default:
                break;
        }
        Camera.main.backgroundColor = CurrentSet.BackgroundColor;
        for (int i = 0; i < GroundMat.Length; i++)
        {
            GroundMat[i].material.color = CurrentSet.GroundColor;
        }

    }

    public void GenerateColorSets()
    {
        ///  Protanopia = reduced Red sensitivity
        //Friendly
        Color TempColor = NormalSet.PrimaryFriendlyColor;
        ModifyColor_Protanopia(ref TempColor);//TempColor = TempColor - (ProtanopiaReduction * ((ColorLossIntensity) / 100));
        ProtanopiaSet.PrimaryFriendlyColor = TempColor;

        TempColor = NormalSet.SecondaryFriendlyColor;
        ModifyColor_Protanopia(ref TempColor); //TempColor = TempColor - (ProtanopiaReduction * ((ColorLossIntensity) / 100));
        ProtanopiaSet.SecondaryFriendlyColor = TempColor;

        //Enemies
        TempColor = NormalSet.PrimaryEnemyColor;
        ModifyColor_Protanopia(ref TempColor);// TempColor = TempColor - (ProtanopiaReduction * ((ColorLossIntensity) / 100));
        ProtanopiaSet.PrimaryEnemyColor = TempColor;

        TempColor = NormalSet.SecondaryEnemyColor;
        ModifyColor_Protanopia(ref TempColor); //TempColor = TempColor - (ProtanopiaReduction * ((ColorLossIntensity) / 100));
        ProtanopiaSet.SecondaryEnemyColor = TempColor;

        //Enviro
        TempColor = NormalSet.GroundColor;
        ModifyColor_Protanopia(ref TempColor);// TempColor = TempColor - (ProtanopiaReduction * ((ColorLossIntensity) / 100));
        ProtanopiaSet.GroundColor = TempColor;

        TempColor = NormalSet.BackgroundColor;
        ModifyColor_Protanopia(ref TempColor); //TempColor = TempColor - (ProtanopiaReduction * ((ColorLossIntensity) / 100)); ;
        ProtanopiaSet.BackgroundColor = TempColor;

        ///  Deuteranopia = reduced Green sensitivity
        // Friendly
        TempColor = NormalSet.PrimaryFriendlyColor;
        ModifyColor_Deuteranopia(ref TempColor);//TempColor = TempColor - (DeuteranopiaReduction * ((ColorLossIntensity) / 100));
        DeuteranopiaSet.PrimaryFriendlyColor = TempColor;

        TempColor = NormalSet.SecondaryFriendlyColor;
        ModifyColor_Deuteranopia(ref TempColor); //TempColor = TempColor - (DeuteranopiaReduction * ((ColorLossIntensity) / 100));
        DeuteranopiaSet.SecondaryFriendlyColor = TempColor;

        //Enemies
        TempColor = NormalSet.PrimaryEnemyColor;
        ModifyColor_Deuteranopia(ref TempColor);// TempColor = TempColor - (DeuteranopiaReduction * ((ColorLossIntensity) / 100));
        DeuteranopiaSet.PrimaryEnemyColor = TempColor;

        TempColor = NormalSet.SecondaryEnemyColor;
        ModifyColor_Deuteranopia(ref TempColor);// TempColor = TempColor - (DeuteranopiaReduction * ((ColorLossIntensity) / 100));
        DeuteranopiaSet.SecondaryEnemyColor = TempColor;

        //Enviro
        TempColor = NormalSet.GroundColor;
        ModifyColor_Deuteranopia(ref TempColor); //TempColor = TempColor - (DeuteranopiaReduction * ((ColorLossIntensity) / 100));
        DeuteranopiaSet.GroundColor = TempColor;

        TempColor = NormalSet.BackgroundColor;
        ModifyColor_Deuteranopia(ref TempColor);// TempColor = TempColor - (DeuteranopiaReduction * ((ColorLossIntensity) / 100));
        DeuteranopiaSet.BackgroundColor = TempColor;

        /// Tritanopia = reduced Blue sensitivity
        // Friendly
        TempColor = NormalSet.PrimaryFriendlyColor;
        ModifyColor_Tritanopia(ref TempColor); //TempColor = TempColor - (TritanopiaReduction * ((ColorLossIntensity) / 100));
        TritanopiaSet.PrimaryFriendlyColor = TempColor;

        TempColor = NormalSet.SecondaryFriendlyColor;
        ModifyColor_Tritanopia(ref TempColor); //TempColor = TempColor - (TritanopiaReduction * ((ColorLossIntensity) / 100));
        TritanopiaSet.SecondaryFriendlyColor = TempColor;

        //Enemies
        TempColor = NormalSet.PrimaryEnemyColor;
        ModifyColor_Tritanopia(ref TempColor);// TempColor = TempColor - (TritanopiaReduction * ((ColorLossIntensity) / 100));
        TritanopiaSet.PrimaryEnemyColor = TempColor;

        TempColor = NormalSet.SecondaryEnemyColor;
        ModifyColor_Tritanopia(ref TempColor);// TempColor = TempColor - (TritanopiaReduction * ((ColorLossIntensity) / 100));
        TritanopiaSet.SecondaryEnemyColor = TempColor;

        //Enviro
        TempColor = NormalSet.GroundColor;
        ModifyColor_Tritanopia(ref TempColor); //TempColor = TempColor - (TritanopiaReduction * ((ColorLossIntensity) / 100));
        TritanopiaSet.GroundColor = TempColor;

        TempColor = NormalSet.BackgroundColor;
        ModifyColor_Tritanopia(ref TempColor);// TempColor = TempColor - (TritanopiaReduction * ((ColorLossIntensity) / 100));
        TritanopiaSet.BackgroundColor = TempColor;

        if (Application.isPlaying)
        {
            SetAndUpdateColors((int)SelectedColor);
        }
    }


    float[] ProtanopiaR = { 56.667f, 43.333f, 0 };
    float[] ProtanopiaG = { 55.833f, 44.167f, 0 };
    float[] ProtanopiaB = { 0, 24.167f, 75.833f };

    public void ModifyColor_Protanopia(ref Color color)
    {
        float R = color.r;
        float G = color.g;
        float B = color.b;
        color.r = R * ProtanopiaR[0] / 100.0f + G * ProtanopiaR[1] / 100.0f + B * ProtanopiaR[2] / 100.0f;
        color.g = R * ProtanopiaG[0] / 100.0f + G * ProtanopiaG[1] / 100.0f + B * ProtanopiaG[2] / 100.0f;
        color.b = R * ProtanopiaB[0] / 100.0f + G * ProtanopiaB[1] / 100.0f + B * ProtanopiaB[2] / 100.0f;
    }

    float[] DeuteranopiaR = { 62.5f, 37.5f, 0 };
    float[] DeuteranopiaG = { 70, 30, 0 };
    float[] DeuteranopiaB = { 0, 30, 70 };

    public void ModifyColor_Deuteranopia(ref Color color)
    {
        float R = color.r;
        float G = color.g;
        float B = color.b;
        color.r = R * DeuteranopiaR[0] / 100.0f + G * DeuteranopiaR[1] / 100.0f + B * DeuteranopiaR[2] / 100.0f;
        color.g = R * DeuteranopiaG[0] / 100.0f + G * DeuteranopiaG[1] / 100.0f + B * DeuteranopiaG[2] / 100.0f;
        color.b = R * DeuteranopiaB[0] / 100.0f + G * DeuteranopiaB[1] / 100.0f + B * DeuteranopiaB[2] / 100.0f;
    }
    float[] TritanopiaR = { 95, 5, 0 };
    float[] TritanopiaG = { 0, 43.333f, 56.667f };
    float[] TritanopiaB = { 0, 47.5f, 52.5f };

    public void ModifyColor_Tritanopia(ref Color color)
    {
        float R = color.r;
        float G = color.g;
        float B = color.b;
        color.r = R * TritanopiaR[0] / 100.0f + G * TritanopiaR[1] / 100.0f + B * TritanopiaR[2] / 100.0f;
        color.g = R * TritanopiaG[0] / 100.0f + G * TritanopiaG[1] / 100.0f + B * TritanopiaG[2] / 100.0f;
        color.b = R * TritanopiaB[0] / 100.0f + G * TritanopiaB[1] / 100.0f + B * TritanopiaB[2] / 100.0f;
    }

    public void SetAndUpdateColors(int ColorInt)
    {

        SelectedColor = (ColorSettings)ColorInt;
        PlayerPrefs.SetInt("ColorSetting", (int)SelectedColor);
        switch (SelectedColor)
        {
            case ColorSettings.Normal:
                CurrentSet = NormalSet;
                break;
            case ColorSettings.Protanopia:
                CurrentSet = ProtanopiaSet;
                break;
            case ColorSettings.Deuteranopia:
                CurrentSet = DeuteranopiaSet;
                break;
            case ColorSettings.Tritanopia:
                CurrentSet = TritanopiaSet;
                break;
            default:
                break;
        }

        DOTween.Kill(Camera.main.GetInstanceID());
        Camera.main.DOColor(CurrentSet.BackgroundColor, ColorTweenTime);

        for (int i = 0; i < GroundMat.Length; i++)
        {
            DOTween.Kill(GroundMat[i].GetInstanceID());
            GroundMat[i].material.DOColor(CurrentSet.GroundColor, ColorTweenTime);
        }

        foreach (Unit unit in UnitManager.Instance.AllUnits)
        {
            SetUnitColors(unit, ColorTweenTime);


        }
        UnitSpawner[] Spawners = FindObjectsOfType<UnitSpawner>();

        foreach (UnitSpawner spawner in Spawners)
        {
            SetSpawnerColors(spawner, ColorTweenTime);
        }

    }

    public void SetSpawnerColors(UnitSpawner Spawner, float Time = 0)
    {
        if (Time == 0)
        {
            for (int i = 0; i < Spawner.PrimaryMeshRenderers.Length; i++)
            {
                Spawner.PrimaryMeshRenderers[i].material.color = Spawner.Allegiance == UnitAllegiance.friendly ? CurrentSet.PrimaryFriendlyColor : CurrentSet.PrimaryEnemyColor;
            }
            for (int i = 0; i < Spawner.SecondaryMeshRenderers.Length; i++)
            {
                Spawner.SecondaryMeshRenderers[i].material.color = Spawner.Allegiance == UnitAllegiance.friendly ? CurrentSet.SecondaryFriendlyColor : CurrentSet.SecondaryEnemyColor;
            }
        }
        else
        {
            for (int i = 0; i < Spawner.PrimaryMeshRenderers.Length; i++)
            {
                DOTween.Kill(Spawner.PrimaryMeshRenderers[i].material.GetInstanceID());
                Spawner.PrimaryMeshRenderers[i].material.DOColor(Spawner.Allegiance == UnitAllegiance.friendly ? CurrentSet.PrimaryFriendlyColor : CurrentSet.PrimaryEnemyColor, Time);
            }
            for (int i = 0; i < Spawner.SecondaryMeshRenderers.Length; i++)
            {
                DOTween.Kill(Spawner.SecondaryMeshRenderers[i].material.GetInstanceID());

                Spawner.SecondaryMeshRenderers[i].material.DOColor(Spawner.Allegiance == UnitAllegiance.friendly ? CurrentSet.SecondaryFriendlyColor : CurrentSet.SecondaryEnemyColor, Time);
            }
        }
    }
    public void SetUnitColors(Unit Unit, float Time = 0)
    {
        if (Time == 0)
        {
            for (int i = 0; i < Unit.PrimaryMeshRenderers.Length; i++)
            {
                Unit.PrimaryMeshRenderers[i].material.color = Unit.Allegiance == UnitAllegiance.friendly ? CurrentSet.PrimaryFriendlyColor : CurrentSet.PrimaryEnemyColor;
            }
            for (int i = 0; i < Unit.SecondaryMeshRenderers.Length; i++)
            {
                Unit.SecondaryMeshRenderers[i].material.color = Unit.Allegiance == UnitAllegiance.friendly ? CurrentSet.SecondaryFriendlyColor : CurrentSet.SecondaryEnemyColor;
            }
        }
        else
        {
            for (int i = 0; i < Unit.PrimaryMeshRenderers.Length; i++)
            {
                DOTween.Kill(Unit.PrimaryMeshRenderers[i].material.GetInstanceID());
                Unit.PrimaryMeshRenderers[i].material.DOColor(Unit.Allegiance == UnitAllegiance.friendly ? CurrentSet.PrimaryFriendlyColor : CurrentSet.PrimaryEnemyColor, Time);
            }
            for (int i = 0; i < Unit.SecondaryMeshRenderers.Length; i++)
            {
                DOTween.Kill(Unit.SecondaryMeshRenderers[i].material.GetInstanceID());

                Unit.SecondaryMeshRenderers[i].material.DOColor(Unit.Allegiance == UnitAllegiance.friendly ? CurrentSet.SecondaryFriendlyColor : CurrentSet.SecondaryEnemyColor, Time);
            }
        }
    }

    public void SetProjectileColors(Projectile Proj, float Time = 0)
    {
        if (Time == 0)
        {
            for (int i = 0; i < Proj.PrimaryMeshRenderers.Length; i++)
            {
                Proj.PrimaryMeshRenderers[i].material.color = Proj.Owner.Allegiance == UnitAllegiance.friendly ? CurrentSet.PrimaryFriendlyColor : CurrentSet.PrimaryEnemyColor;
            }
            for (int i = 0; i < Proj.SecondaryMeshRenderers.Length; i++)
            {
                Proj.SecondaryMeshRenderers[i].material.color = Proj.Owner.Allegiance == UnitAllegiance.friendly ? CurrentSet.SecondaryFriendlyColor : CurrentSet.SecondaryEnemyColor;
            }
        }
        else
        {
            for (int i = 0; i < Proj.PrimaryMeshRenderers.Length; i++)
            {
                DOTween.Kill(Proj.PrimaryMeshRenderers[i].material.GetInstanceID());
                Proj.PrimaryMeshRenderers[i].material.DOColor(Proj.Owner.Allegiance == UnitAllegiance.friendly ? CurrentSet.PrimaryFriendlyColor : CurrentSet.PrimaryEnemyColor, Time);
            }
            for (int i = 0; i < Proj.SecondaryMeshRenderers.Length; i++)
            {
                DOTween.Kill(Proj.SecondaryMeshRenderers[i].material.GetInstanceID());

                Proj.SecondaryMeshRenderers[i].material.DOColor(Proj.Owner.Allegiance == UnitAllegiance.friendly ? CurrentSet.SecondaryFriendlyColor : CurrentSet.SecondaryEnemyColor, Time);
            }
        }
    }
}
