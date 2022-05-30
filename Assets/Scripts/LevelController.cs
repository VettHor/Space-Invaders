using UnityEditor;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static GameObject Level;
    public static string TextLevel;
    static LevelController()
    {
        Level = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Factories/EasyLevelFactory.prefab");
        TextLevel = "Easy";
    }
}