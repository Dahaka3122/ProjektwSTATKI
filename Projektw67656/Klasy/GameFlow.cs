using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Projektw67656.Klasy;

namespace Projektw67656.Klasy
{
    class GameFlow
    {
        GameState gm;
        public GameFlow()
        {
            gm = new GameState() { IsPlayer1 = false, Player1 = new Player(), Player2 = new Player() };
        }

        public void Start()
        {
            GameSetup GameSetup = new GameSetup(gm);
            GameSetup.Setup();

            do
            {
                GameSetup.SetBoard();
                FireShotResponse shotresponse;
                do
                {
                    ControlOutput.ResetScreen(new Player[] { gm.Player1, gm.Player2 });
                    ControlOutput.ShowWhoseTurn(gm.IsPlayer1 ? gm.Player1 : gm.Player2);
                    ControlOutput.DrawHistory(gm.IsPlayer1 ? gm.Player2 : gm.Player1);
                    Coordinate ShotPoint = new Coordinate(1, 1);
                    shotresponse = Shot(gm.IsPlayer1 ? gm.Player2 : gm.Player1, gm.IsPlayer1 ? gm.Player1 : gm.Player2, out ShotPoint);

                    ControlOutput.ResetScreen(new Player[] { gm.Player1, gm.Player2 });
                    ControlOutput.ShowWhoseTurn(gm.IsPlayer1 ? gm.Player1 : gm.Player2);
                    ControlOutput.DrawHistory(gm.IsPlayer1 ? gm.Player2 : gm.Player1);
                    ControlOutput.ShowShotResult(shotresponse, ShotPoint, gm.IsPlayer1 ? gm.Player1.Name : gm.Player2.Name);
                    if (shotresponse.ShotStatus != ShotStatus.Zwyciestwo)
                    {
                        Console.WriteLine("Wcisnij dowolny przycisk by przelaczyc na gracza: " + (gm.IsPlayer1 ? gm.Player2.Name : gm.Player1.Name));
                        gm.IsPlayer1 = !gm.IsPlayer1;
                        Console.ReadKey();
                    }
                } while (shotresponse.ShotStatus != ShotStatus.Zwyciestwo);

            } while (ControlInput.CheckQuit());
        }


        private FireShotResponse Shot(Player victim, Player Shoter, out Coordinate ShotPoint)
        {
            FireShotResponse fire; Coordinate WhereToShot;
            do
            {
                if (!Shoter.IsPC)
                {
                    WhereToShot = ControlInput.GetShotLocationFromUser();
                    fire = victim.PlayerBoard.FireShot(WhereToShot);
                    if (fire.ShotStatus == ShotStatus.Nieprawidlowy || fire.ShotStatus == ShotStatus.Duplikat)
                        ControlOutput.ShowShotResult(fire, WhereToShot, "");
                }
                else
                {
                    WhereToShot = ControlInput.GetShotLocationFromComputer(victim.PlayerBoard, Shoter.GameLevel);
                    fire = victim.PlayerBoard.FireShot(WhereToShot);
                }
                if (fire.ShotStatus == ShotStatus.Zwyciestwo)
                {
                    if (gm.IsPlayer1) gm.Player1.Win += 1;
                    else gm.Player2.Win += 1;
                }
            } while (fire.ShotStatus == ShotStatus.Duplikat || fire.ShotStatus == ShotStatus.Nieprawidlowy);
            ShotPoint = WhereToShot;
            return fire;
        }
    }
}
