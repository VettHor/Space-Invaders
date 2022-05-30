using UnityEngine;

public class EasyLevelFactory : MonoBehaviour, ILevelFactory
{
    public Sprite InvaderSprite => Resources.Load<Sprite>("Invader_01-1");
    public IMysteryShipView GetMysteryShipView()
    {
        return new EasyLevelMysteryShipView();
    }
    public IInvaderView GetInvaderView(float percentKilled, AnimationCurve speedCurve)
    {
        return new EasyLevelInvaderView(percentKilled, speedCurve, InvaderSprite);
    }
}