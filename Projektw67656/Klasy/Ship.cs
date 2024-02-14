using System.Linq;
using Projektw67656.Klasy;

namespace Projektw67656.Klasy
{
    public class Ship
    {
        public ShipType ShipType { get; private set; }
        public string ShipName { get { return ShipType.ToString(); } }
        public Coordinate[] BoardPositions { get; set; }
        private int _lifeRemaining;
        public bool IsSunk { get { return _lifeRemaining == 0; } }

        public Ship(ShipType shipType, int numberOfSlots)
        {
            ShipType = shipType;
            _lifeRemaining = numberOfSlots;
            BoardPositions = new Coordinate[numberOfSlots];
        }

        public ShotStatus FireAtShip(Coordinate position)
        {
            if (BoardPositions.Contains(position))
            {
                _lifeRemaining--;

                if (_lifeRemaining == 0)
                    return ShotStatus.TrafionyZatopiony;

                return ShotStatus.Trafienie;
            }

            return ShotStatus.Pudlo;
        }
    }
}
