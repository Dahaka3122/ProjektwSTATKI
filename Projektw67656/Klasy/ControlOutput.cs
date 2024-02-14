using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Numerics;

namespace Projektw67656.Klasy
{
    class ControlOutput
    {
        public static void ShowAllPlayer(Player[] player)
        {
            string str = "Gracz 1: " + player[0].Name + "(Wygral: " + player[0].Win + ")\t Gracz 2: " + player[1].Name + "(Wygral: " + player[1].Win + ")";
            if (player[1].IsPC)
                str += "\tPoziom trudnosci: " + player[1].GameLevel.ToString();
            Console.WriteLine(str);
            Console.WriteLine("");
        }




        public static void ShowWhoseTurn(Player player)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tura: " + player.Name);
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;

        }

        static string GetLetterFromNumber(int number)
        {
            string result = "";
            switch (number)
            {
                case 1:
                    result = "A";
                    break;
                case 2:
                    result = "B";
                    break;
                case 3:
                    result = "C";
                    break;
                case 4:
                    result = "D";
                    break;
                case 5:
                    result = "E";
                    break;
                case 6:
                    result = "F";
                    break;
                case 7:
                    result = "G";
                    break;
                case 8:
                    result = "H";
                    break;
                case 9:
                    result = "I";
                    break;
                case 10:
                    result = "J";
                    break;
                default:
                    break;
            }
            return result;
        }

        public static void DrawHistory(Player player)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write("  ");
            for (int y = 1; y <= 10; y++)
            {
                Console.Write(y);
                Console.Write(" ");
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            for (int x = 1; x <= 10; x++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(GetLetterFromNumber(x) + " ");
                Console.ForegroundColor = ConsoleColor.White;
                for (int y = 1; y <= 10; y++)
                {
                    //Console.Write(y);
                    ShotHistory history = player.PlayerBoard.CheckCoordinate(new Coordinate(x, y));
                    switch (history)
                    {
                        case ShotHistory.Trafienie:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case ShotHistory.Pudlo:
                            Console.Write("P");
                            break;
                        case ShotHistory.Unknown:
                            Console.Write(" ");
                            break;
                    }
                    Console.Write("|");
                }
                Console.WriteLine();
            }
            Console.WriteLine("");
        }

        public static void ShowShotResult(FireShotResponse shotresponse, Coordinate c, string playername)
        {
            String str = "";
            switch (shotresponse.ShotStatus)
            {
                case ShotStatus.Duplikat:
                    Console.ForegroundColor = ConsoleColor.Red;
                    str = "Cel strzalu: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t Wynik: W to miejsce juz strzelales! ";
                    break;
                case ShotStatus.Trafienie:
                    Console.ForegroundColor = ConsoleColor.Green;
                    str = "Cel strzalu: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t Wynik: Trafiony!";
                    break;
                case ShotStatus.TrafionyZatopiony:
                    Console.ForegroundColor = ConsoleColor.Green;
                    str = "Cel strzalu: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t Wynik: Trafiony zatopiony, " + shotresponse.ShipImpacted + "!";
                    break;
                case ShotStatus.Nieprawidlowy:
                    Console.ForegroundColor = ConsoleColor.Red;
                    str = "Cel strzalu: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t Wynik: Nieprawidlowy cel! ";
                    break;
                case ShotStatus.Pudlo:
                    Console.ForegroundColor = ConsoleColor.White;
                    str = "Cel strzalu: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t Wynik: Pudlo! ";
                    break;
                case ShotStatus.Zwyciestwo:
                    Console.ForegroundColor = ConsoleColor.Green;
                    str = "Cel strzalu: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t Wynik; Trafiony zatopiony, " + shotresponse.ShipImpacted + "! \n\n";
                    str += "Koniec gry!, " + playername + " wygrywa!";
                    break;
            }
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
        }

        static void test()
        {
            List<object> obj = new List<object>();
            Player player1 = new Player();
            Board board1 = new Board();
            obj.Add(board1);
            obj.Add(player1);
        }

        public static void ResetScreen(Player[] player)
        {
            Console.Clear();
            ControlOutput.ShowAllPlayer(player);
        }
    }
}
