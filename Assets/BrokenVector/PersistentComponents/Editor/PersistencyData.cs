using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PersistencyData : ScriptableObject
{
    public Component[] persistentComponents;

    public static PersistencyData CreateInstance(Component[] c)
    {
        var instance = ScriptableObject.CreateInstance<PersistencyData>();
        instance.persistentComponents = c;

        AssetDatabase.CreateAsset(instance, GetAssetLocation());
        AssetDatabase.SaveAssets();

        return instance;
    }

    public void Save()
    {
        AssetDatabase.SaveAssets();
    }

    public static string GetAssetLocation()
    {
        return "Assets/BrokenVector/PersistentComponents/Editor/PersistencyData.asset";
    }

}
