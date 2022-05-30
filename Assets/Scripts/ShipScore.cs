using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public abstract class ShipScore
    { 
        public abstract int CalculateTotalScore();
    }

    public class SimpleShipScore : ShipScore
    {
        public override int CalculateTotalScore()
        {
            return 300;
        }
    }
    public abstract class ShipScoreDecorator : ShipScore
    {
        protected ShipScore ship;
        public ShipScoreDecorator(ShipScore ship) : base() 
        {
            this.ship = ship;
        }
    }

    public class EasyScore : ShipScoreDecorator
    {
        public EasyScore(ShipScore ship) : base(ship) 
        { }

        public override int CalculateTotalScore()
        {
            return ship.CalculateTotalScore() + 100;
        }
    }

    public class HardScore : ShipScoreDecorator
    {
        public HardScore(ShipScore ship) : base(ship) 
        { }

        public override int CalculateTotalScore()
        {
            return ship.CalculateTotalScore() + 200;
        }
    }
}
