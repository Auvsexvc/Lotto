using System;
using System.Collections.Generic;
using System.Linq;

namespace Lotto
{
    internal static class Program
    {
        private static int kumulacja;
        private static int START = 30;
        private static Random rnd = new Random();

        private static void Main(string[] args)
        {
            int pieniadze = START;
            int dzien = 0;
            do
            {
                pieniadze = START;
                dzien = 0;
                ConsoleKey wybor;
                do
                {
                    kumulacja = rnd.Next(2, 37) * 1000000;
                    dzien++;
                    int losow = 0;
                    List<int[]> kupon = new List<int[]>();

                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"Dzień : {dzien}");
                        Console.WriteLine($"Witaj w grze lotto, dziś do wygrana jest {kumulacja} zł");
                        Console.WriteLine($"Stan konta: {pieniadze} zł");
                        Console.WriteLine();
                        WyswietlKupon(kupon);
                        // MENU
                        if (pieniadze >= 3 && losow < 8)
                        {
                            Console.WriteLine($"1 - Postaw nowy los (-3zł) [{losow + 1}/8]");
                        }
                        Console.WriteLine("2 - Sprawdź kupon - losowanie");
                        Console.WriteLine("3 - Zakończ grę");
                        // MENU
                        wybor = Console.ReadKey().Key;
                        if (wybor == ConsoleKey.D1 && pieniadze >= 3 && losow < 8)
                        {
                            kupon.Add(PostawLos());
                            pieniadze -= 3;
                            losow++;
                        }
                    } while (wybor == ConsoleKey.D1);
                    Console.Clear();
                    if (kupon.Count > 0)
                    {
                        int wygrana = Sprawdz(kupon);
                        if (wygrana > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\nBrawo! wygrałeś {wygrana} złw tym losowaniu!");
                            Console.ResetColor();
                            pieniadze += wygrana;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine($"\nNiestety nic nie wygrałeś");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nie miałeś losów w tym losowaniu");
                    }
                    Console.WriteLine("Kontynuuj...");
                    Console.ReadKey();
                } while (pieniadze >= 3 && wybor != ConsoleKey.D3);

                Console.Clear();
                Console.WriteLine($"Dzień {dzien}. \nKoniec gry, twój wynik to: {pieniadze - START} zł");
                Console.WriteLine("Enter - graj od nowa");
            } while (Console.ReadKey().Key == ConsoleKey.Enter);
        }

        private static int Sprawdz(List<int[]> kupon)
        {
            int wygrana = 0;
            int[] wylosowane = new int[6];
            for (int i = 0; i < wylosowane.Length; i++)
            {
                int los = rnd.Next(1, 50);
                if (!wylosowane.Contains(los))
                {
                    wylosowane[i] = los;
                }
                else
                {
                    i--;
                }
            }
            Array.Sort(wylosowane);
            Console.WriteLine($"Wylosowane liczby to:");
            foreach (int liczba in wylosowane)
            {
                Console.Write(liczba + ", ");
            }
            int[] trafione = SprawdzKupon(kupon, wylosowane);
            int wartosc = 0;

            Console.WriteLine();
            if (trafione[0] > 0)
            {
                wartosc = trafione[0] * 24;
                Console.WriteLine($"3 trafienia: {trafione[0]} +{wartosc}");
                wygrana += wartosc;
            }
            if (trafione[1] > 0)
            {
                wartosc = trafione[1] * rnd.Next(100, 301);
                Console.WriteLine($"4 trafienia: {trafione[1]} +{wartosc}");
                wygrana += wartosc;
            }
            if (trafione[2] > 0)
            {
                wartosc = trafione[2] * rnd.Next(4000, 8001); ;
                Console.WriteLine($"5 trafień: {trafione[2]} +{wartosc}");
                wygrana += wartosc;
            }
            if (trafione[3] > 0)
            {
                wartosc = trafione[3] * kumulacja / (trafione[3] + rnd.Next(0, 5));
                Console.WriteLine($"6 trafień: {trafione[4]} +{wartosc}");
                wygrana += wartosc;
            }

            return wygrana;
        }

        private static int[] SprawdzKupon(List<int[]> kupon, int[] wylosowane)
        {
            int[] wygrane = new int[4];
            int i = 0;
            Console.WriteLine($"\n\nTwój kupon:");
            foreach (int[] los in kupon)
            {
                i++;
                Console.Write($"{i}: ");
                int trafien = 0;
                foreach (int liczba in los)
                {
                    if (wylosowane.Contains(liczba))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(liczba + ", ");
                        Console.ResetColor();
                        trafien++;
                    }
                    else
                    {
                        Console.Write(liczba + ", ");
                    }
                }
                switch (trafien)
                {
                    case 3:
                        wygrane[0]++;
                        break;

                    case 4:
                        wygrane[1]++;
                        break;

                    case 5:
                        wygrane[2]++;
                        break;

                    case 6:
                        wygrane[3]++;
                        break;
                }
                Console.WriteLine($" - trafiono {trafien}/6");
            }

            return wygrane;
        }

        private static int[] PostawLos()
        {
            int[] liczby = new int[6];
            int liczba = -1;
            for (int i = 0; i < liczby.Length; i++)
            {
                Console.Clear();
                Console.Write("Postawione liczby: ");
                foreach (int l in liczby)
                {
                    if (l > 0)
                    {
                        Console.Write(l + ", ");
                    }
                }
                Console.WriteLine("\n\nWybierz liczbę od 1 do 49:");
                Console.Write($"{i + 1}/6: ");
                bool prawidlowa = int.TryParse(Console.ReadLine(), out liczba);
                if (prawidlowa && liczba >= 1 && liczba <= 49 && !liczby.Contains(liczba))
                {
                    liczby[i] = liczba;
                }
                else
                {
                    Console.WriteLine("Błędna liczba.");
                    i--;
                    Console.ReadKey();
                }
            }
            Array.Sort(liczby);
            return liczby;
        }

        private static void WyswietlKupon(List<int[]> kupon)
        {
            if (kupon.Count == 0)
            {
                Console.WriteLine("Nie postawiłeś jeszcze żadnych losów.");
            }
            else
            {
                int i = 0;
                Console.WriteLine("\nTwój kupon:");
                foreach (int[] los in kupon)
                {
                    i++;
                    Console.Write($"{i} : ");
                    foreach (int liczba in los)
                    {
                        Console.Write(liczba + ", ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}