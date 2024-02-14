using System;
using System.Collections.Generic;
using System.Linq;
using Projektw67656.Klasy;

namespace Projektw67656.Klasy
{
    public class Board
    {
        public const int xCoordinator = 10;
        public const int yCoordinator = 10;
        private Dictionary<Coordinate, ShotHistory> ShotHistory;
        private int _currentShipIndex;

        public Ship[] Ships { get; private set; }

        public Board()
        {
            ShotHistory = new Dictionary<Coordinate, ShotHistory>();
            Ships = new Ship[5];
            _currentShipIndex = 0;
        }

        public FireShotResponse FireShot(Coordinate coordinate)
        {
            var response = new FireShotResponse();

            if (!IsValidCoordinate(coordinate))
            {
                response.ShotStatus = ShotStatus.Nieprawidlowy;
                return response;
            }

            if (ShotHistory.ContainsKey(coordinate))
            {
                response.ShotStatus = ShotStatus.Duplikat;
                return response;
            }

            CheckShipsForHit(coordinate, response);
            CheckForVictory(response);

            return response;
        }

        public ShotHistory CheckCoordinate(Coordinate coordinate)
        {
            if (ShotHistory.ContainsKey(coordinate))
            {
                return ShotHistory[coordinate];
            }
            else
            {
                return Projektw67656.Klasy.ShotHistory.Unknown;
            }
        }

        public ShipPlacement PlaceShip(PlaceShipRequest request)
        {
            if (_currentShipIndex > 4)
                throw new Exception("Nie mozesz dodac wiecej statkow, limit to 5!");

            if (!IsValidCoordinate(request.Coordinate))
                return ShipPlacement.BrakMiejsca;

            Ship newShip = ShipCreator.CreateShip(request.ShipType);
            switch (request.Direction)
            {
                case ShipDirection.Dol:
                    return PlaceShipDown(request.Coordinate, newShip);
                case ShipDirection.Gora:
                    return PlaceShipUp(request.Coordinate, newShip);
                case ShipDirection.Lewo:
                    return PlaceShipLeft(request.Coordinate, newShip);
                default:
                    return PlaceShipRight(request.Coordinate, newShip);
            }
        }

        private void CheckForVictory(FireShotResponse response)
        {
            if (response.ShotStatus == ShotStatus.TrafionyZatopiony)
            {
                if (Ships.All(s => s.IsSunk))
                    response.ShotStatus = ShotStatus.Zwyciestwo;
            }
        }

        private void CheckShipsForHit(Coordinate coordinate, FireShotResponse response)
        {
            response.ShotStatus = ShotStatus.Pudlo;

            foreach (var ship in Ships)
            {
                if (ship.IsSunk)
                    continue;

                ShotStatus status = ship.FireAtShip(coordinate);

                switch (status)
                {
                    case ShotStatus.TrafionyZatopiony:
                        response.ShotStatus = ShotStatus.TrafionyZatopiony;
                        response.ShipImpacted = ship.ShipName;
                        ShotHistory.Add(coordinate, Projektw67656.Klasy.ShotHistory.Trafienie);
                        break;
                    case ShotStatus.Trafienie:
                        response.ShotStatus = ShotStatus.Trafienie;
                        response.ShipImpacted = ship.ShipName;
                        ShotHistory.Add(coordinate, Projektw67656.Klasy.ShotHistory.Trafienie);
                        break;
                }

                if (status != ShotStatus.Pudlo)
                    break;
            }

            if (response.ShotStatus == ShotStatus.Pudlo)
            {
                ShotHistory.Add(coordinate, Projektw67656.Klasy.ShotHistory.Pudlo);
            }
        }

        private bool IsValidCoordinate(Coordinate coordinate)
        {
            return coordinate.XCoordinate >= 1 && coordinate.XCoordinate <= xCoordinator &&
            coordinate.YCoordinate >= 1 && coordinate.YCoordinate <= yCoordinator;
        }

        private ShipPlacement PlaceShipRight(Coordinate coordinate, Ship newShip)
        {
            int positionIndex = 0;
            int maxY = coordinate.YCoordinate + newShip.BoardPositions.Length;

            for (int i = coordinate.YCoordinate; i < maxY; i++)
            {
                var currentCoordinate = new Coordinate(coordinate.XCoordinate, i);
                if (!IsValidCoordinate(currentCoordinate))
                    return ShipPlacement.BrakMiejsca;

                if (OverlapsAnotherShip(currentCoordinate))
                    return ShipPlacement.Nakladanie;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return ShipPlacement.Ok;
        }

        private ShipPlacement PlaceShipLeft(Coordinate coordinate, Ship newShip)
        {
            int positionIndex = 0;
            int minY = coordinate.YCoordinate - newShip.BoardPositions.Length;

            for (int i = coordinate.YCoordinate; i > minY; i--)
            {
                var currentCoordinate = new Coordinate(coordinate.XCoordinate, i);

                if (!IsValidCoordinate(currentCoordinate))
                    return ShipPlacement.BrakMiejsca;

                if (OverlapsAnotherShip(currentCoordinate))
                    return ShipPlacement.Nakladanie;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return ShipPlacement.Ok;
        }

        private ShipPlacement PlaceShipUp(Coordinate coordinate, Ship newShip)
        {
            int positionIndex = 0;
            int minX = coordinate.XCoordinate - newShip.BoardPositions.Length;

            for (int i = coordinate.XCoordinate; i > minX; i--)
            {
                var currentCoordinate = new Coordinate(i, coordinate.YCoordinate);

                if (!IsValidCoordinate(currentCoordinate))
                    return ShipPlacement.BrakMiejsca;

                if (OverlapsAnotherShip(currentCoordinate))
                    return ShipPlacement.Nakladanie;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return ShipPlacement.Ok;
        }

        private ShipPlacement PlaceShipDown(Coordinate coordinate, Ship newShip)
        {
            int positionIndex = 0;
            int maxX = coordinate.XCoordinate + newShip.BoardPositions.Length;

            for (int i = coordinate.XCoordinate; i < maxX; i++)
            {
                var currentCoordinate = new Coordinate(i, coordinate.YCoordinate);

                if (!IsValidCoordinate(currentCoordinate))
                    return ShipPlacement.BrakMiejsca;

                if (OverlapsAnotherShip(currentCoordinate))
                    return ShipPlacement.Nakladanie;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return ShipPlacement.Ok;
        }

        private void AddShipToBoard(Ship newShip)
        {
            Ships[_currentShipIndex] = newShip;
            _currentShipIndex++;
        }

        private bool OverlapsAnotherShip(Coordinate coordinate)
        {
            foreach (var ship in Ships)
            {
                if (ship != null)
                {
                    if (ship.BoardPositions.Contains(coordinate))
                        return true;
                }
            }

            return false;
        }
    }
}
