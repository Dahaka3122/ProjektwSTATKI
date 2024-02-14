using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Projektw67656.Klasy;

namespace Projektw67656.Klasy
{
    public class GameSetup
    {
        GameState _gm;
        public GameSetup(GameState gm)
        {
            _gm = gm;
        }

        public void Setup()
        {
            Console.ForegroundColor = ConsoleColor.White;

            GameLevel gamelevel = GameLevel.Latwy;
            string[] userSetUp = ControlInput.GetNameFromUser();
            switch (userSetUp[2])
            {
                case "t":
                    gamelevel = GameLevel.Trudny;
                    break;
                case "s":
                    gamelevel = GameLevel.Sredni;
                    break;
                default:
                    gamelevel = GameLevel.Latwy;
                    break;
            }

            _gm.Player1.Name = userSetUp[0];
            _gm.Player1.IsPC = false;
            _gm.Player1.Win = 0;
            _gm.Player1.GameLevel = gamelevel;

            _gm.Player2.Name = userSetUp[1];
            _gm.Player2.Win = 0;
            _gm.Player2.GameLevel = gamelevel;

            //vs Computer
            if (userSetUp[1] == "")
            {
                _gm.Player2.Name = "Komputer";
                _gm.Player2.IsPC = true;
            }
        }

        public void SetBoard()
        {
            ControlOutput.ResetScreen(new Player[] { _gm.Player1, _gm.Player2 });

            _gm.IsPlayer1 = Projektw67656.Klasy.GetRandom.WhoseFirst();

            _gm.Player1.PlayerBoard = new Board();
            PlaceShipOnBoard(_gm.Player1);
            ControlOutput.ResetScreen(new Player[] { _gm.Player1, _gm.Player2 });

            _gm.Player2.PlayerBoard = new Board();
            PlaceShipOnBoard(_gm.Player2);
            Console.WriteLine("Wszystkie statki zostaly rozmieszczone! Wcisnij dowolny przycisk by kontynuowac...");
            Console.ReadKey();
        }

        public void PlaceShipOnBoard(Player player)
        {
            bool IsPlaceBoardAuto = false;
            if (player.IsPC != true)
            {
                ControlOutput.ShowWhoseTurn(player);
                IsPlaceBoardAuto = ControlInput.IsPlaceBoardAuto();
                if (!IsPlaceBoardAuto)
                    Console.WriteLine("Prosze wprowadzic najpierw pole, pozniej kierunek (l - lewo, r - prawo, u - gora, d - dol), np. f8, l");
            }
            for (ShipType s = ShipType.Destroyer; s <= ShipType.Carrier; s++)
            {
                PlaceShipRequest ShipToPlace = new PlaceShipRequest();
                ShipPlacement result;
                do
                {
                    if (!player.IsPC && !IsPlaceBoardAuto)
                    {
                        ShipToPlace = ControlInput.GetLocationFromUser(s.ToString());
                        ShipToPlace.ShipType = s;
                        result = player.PlayerBoard.PlaceShip(ShipToPlace);
                        if (result == ShipPlacement.BrakMiejsca)
                            Console.WriteLine("Niewystarczajaco duzo miejsca!");
                        else if (result == ShipPlacement.Nakladanie)
                            Console.WriteLine("Statki sie na siebie nakladaja!");
                    }
                    else
                    {
                        ShipToPlace = ControlInput.GetLocationFromComputer();
                        ShipToPlace.ShipType = s;
                        result = player.PlayerBoard.PlaceShip(ShipToPlace);
                    }

                } while (result != ShipPlacement.Ok);
            }
        }
    }
}
