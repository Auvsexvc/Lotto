using System;
using System.Collections.Generic;
using System.Linq;

namespace Lotto
{
    internal static class Program
    {
        private static int cumulation;
        private const int START = 30;
        private static Random rnd = new Random();

        private static void Main(string[] args)
        {
            do
            {
                int money = START;
                int day = 0;
                ConsoleKey input;
                do
                {
                    cumulation = rnd.Next(2, 37) * 1000000;
                    day++;
                    int tickets = 0;
                    List<int[]> coupon = new List<int[]>();

                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"Day : {day}");
                        Console.WriteLine($"Welcome in lotto, today's win is {cumulation} BC");
                        Console.WriteLine($"Wallet: {money} BC");
                        Console.WriteLine();
                        ShowCoupon(coupon);
                        // MENU
                        if (money >= 3 && tickets < 8)
                        {
                            Console.WriteLine($"1 - Buy new ticket (-3BC) [{tickets + 1}/8]");
                        }
                        Console.WriteLine("2 - Check coupon - start lottery");
                        Console.WriteLine("3 - End the game");
                        // MENU
                        input = Console.ReadKey().Key;
                        if (input == ConsoleKey.D1 && money >= 3 && tickets < 8)
                        {
                            coupon.Add(Bet());
                            money -= 3;
                            tickets++;
                        }
                    } while (input == ConsoleKey.D1);
                    Console.Clear();
                    if (coupon.Count > 0)
                    {
                        int win = Drawing(coupon);
                        if (win > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\nCongratulation! You've won {win} BC in this lottery!");
                            Console.ResetColor();
                            money += win;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine($"\nSorry, you haven't won anything");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.WriteLine("U haven't bet anything this time");
                    }
                    Console.WriteLine("Continue...");
                    Console.ReadKey();
                } while (money >= 3 && input != ConsoleKey.D3);

                Console.Clear();
                Console.WriteLine($"Day {day}. \nEnd of game, Your score is: {money - START} BC");
                Console.WriteLine("Enter - play again");
            } while (Console.ReadKey().Key == ConsoleKey.Enter);
        }

        private static int Drawing(List<int[]> coupon)
        {
            int win = 0;
            int[] draft = new int[6];
            for (int i = 0; i < draft.Length; i++)
            {
                int ticket = rnd.Next(1, 50);
                if (!draft.Contains(ticket))
                {
                    draft[i] = ticket;
                }
                else
                {
                    i--;
                }
            }
            Array.Sort(draft);
            Console.WriteLine($"Drawn numbers are:");
            foreach (int number in draft)
            {
                Console.Write(number + ", ");
            }
            int[] hit = CheckCoupon(coupon, draft);
            int value = 0;

            Console.WriteLine();
            if (hit[0] > 0)
            {
                value = hit[0] * 24;
                Console.WriteLine($"3 hits: {hit[0]} +{value}");
                win += value;
            }
            if (hit[1] > 0)
            {
                value = hit[1] * rnd.Next(100, 301);
                Console.WriteLine($"4 hits: {hit[1]} +{value}");
                win += value;
            }
            if (hit[2] > 0)
            {
                value = hit[2] * rnd.Next(4000, 8001); ;
                Console.WriteLine($"5 hits: {hit[2]} +{value}");
                win += value;
            }
            if (hit[3] > 0)
            {
                value = hit[3] * cumulation / (hit[3] + rnd.Next(0, 5));
                Console.WriteLine($"6 hits: {hit[4]} +{value}");
                win += value;
            }

            return win;
        }

        private static int[] CheckCoupon(List<int[]> coupon, int[] draft)
        {
            int[] wins = new int[4];
            int i = 0;
            Console.WriteLine($"\n\nYour coupon:");
            foreach (int[] ticket in coupon)
            {
                i++;
                Console.Write($"{i}: ");
                int hits = 0;
                foreach (int number in ticket)
                {
                    if (draft.Contains(number))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(number + ", ");
                        Console.ResetColor();
                        hits++;
                    }
                    else
                    {
                        Console.Write(number + ", ");
                    }
                }
                switch (hits)
                {
                    case 3:
                        wins[0]++;
                        break;

                    case 4:
                        wins[1]++;
                        break;

                    case 5:
                        wins[2]++;
                        break;

                    case 6:
                        wins[3]++;
                        break;
                }
                Console.WriteLine($" - hits {hits}/6");
            }

            return wins;
        }

        private static int[] Bet()
        {
            int[] numbers = new int[6];
            int number = -1;
            for (int i = 0; i < numbers.Length; i++)
            {
                Console.Clear();
                Console.Write("Bet numbers: ");
                foreach (int l in numbers)
                {
                    if (l > 0)
                    {
                        Console.Write(l + ", ");
                    }
                }
                Console.WriteLine("\n\nPick a number from 1 to 49:");
                Console.Write($"{i + 1}/6: ");
                bool proper = int.TryParse(Console.ReadLine(), out number);
                if (proper && number >= 1 && number <= 49 && !numbers.Contains(number))
                {
                    numbers[i] = number;
                }
                else
                {
                    Console.WriteLine("Incorrect number.");
                    i--;
                    Console.ReadKey();
                }
            }
            Array.Sort(numbers);
            return numbers;
        }

        private static void ShowCoupon(List<int[]> coupon)
        {
            if (coupon.Count == 0)
            {
                Console.WriteLine("You haven't bet any tickets yet.");
            }
            else
            {
                int i = 0;
                Console.WriteLine("\nYour coupon:");
                foreach (int[] ticket in coupon)
                {
                    i++;
                    Console.Write($"{i} : ");
                    foreach (int number in ticket)
                    {
                        Console.Write(number + ", ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}