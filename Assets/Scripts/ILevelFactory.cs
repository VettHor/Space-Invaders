using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ILevelFactory
{
    Sprite InvaderSprite { get; }
    IMysteryShipView GetMysteryShipView();
    IInvaderView GetInvaderView(float percentKilled, AnimationCurve speedCurve);
}

