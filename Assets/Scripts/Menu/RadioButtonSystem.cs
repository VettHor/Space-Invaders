using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using TMPro;

public class RadioButtonSystem : MonoBehaviour
{
    ToggleGroup toggleGroup;
    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }

    public void Confirm()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        string levelText = toggle.GetComponentInChildren<TextMeshProUGUI>().text;
        LevelController.Level = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Factories/{levelText}LevelFactory.prefab");
        LevelController.TextLevel = levelText;
    }
}
