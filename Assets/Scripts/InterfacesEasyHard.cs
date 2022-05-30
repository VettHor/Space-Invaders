using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public interface IInvaderView
{
    Invader Invader { get; }
    float Speed { get; }
    int Rows { get; }
    int Columns { get; }
}

public interface IMysteryShipView
{
    Sprite MysteryShip { get; }
}

public class EasyLevelInvaderView : IInvaderView
{
    public Invader Invader => AssetDatabase.LoadAssetAtPath<Invader>("Assets/Prefabs/Invader_01.prefab");
    public int Rows => 3;
    public int Columns => 10;
    private readonly float _speed;
    public float Speed => _speed;
    public EasyLevelInvaderView(float percentKilled, AnimationCurve speedCurve, Sprite sprite)
    {
        _speed = speedCurve.Evaluate(percentKilled);
        Invader.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}

public class EasyLevelMysteryShipView : IMysteryShipView
{
    public Sprite MysteryShip => Resources.Load<Sprite>("Invader_03-1");
}

public class HardLevelInvaderView : IInvaderView
{
    public Invader Invader => AssetDatabase.LoadAssetAtPath<Invader>("Assets/Prefabs/Invader_02.prefab");
    public int Rows => 5;
    public int Columns => 11;
    private readonly float _speed;
    public float Speed => _speed;
    public HardLevelInvaderView(float percentKilled, AnimationCurve speedCurve, Sprite sprite)
    {
        _speed = speedCurve.Evaluate(percentKilled) * 1.5f;
        Invader.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}

public class HardLevelMysteryShipView : IMysteryShipView
{
    public Sprite MysteryShip => Resources.Load<Sprite>("MysteryShip");
}