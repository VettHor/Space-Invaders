using UnityEngine;

public class HardLevelFactory : MonoBehaviour, ILevelFactory
{
    public Sprite InvaderSprite => Resources.Load<Sprite>("Invader_02-1");
    public IMysteryShipView GetMysteryShipView()
    {
        return new HardLevelMysteryShipView();
    }
    public IInvaderView GetInvaderView(float percentKilled, AnimationCurve speedCurve)
    {
        return new HardLevelInvaderView(percentKilled, speedCurve, InvaderSprite);
    }
}