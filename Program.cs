using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Watki
{

    internal class Program
    {

        public static class Globals
        {
            public static int[,] tableA = new int[1000, 1000];
            public static int[,] tableB = new int[1000, 1000];
            public static int[,] tableResultS = new int[tableA.GetLength(0), tableB.GetLength(1)];
            public static int[,] tableResultW = new int[tableA.GetLength(0), tableB.GetLength(1)];
            public static int threads_num = 0;
        }

        static void Main(string[] args)
        {
            fillTable(Globals.tableA);
            fillTable(Globals.tableB);
        
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            calculateS();

            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("Strukturalnie: " + elapsedMs.ToString() + "ms");

 
            for (int j = 1; j < 5; j++)
            {
                Globals.threads_num = j;
                stopwatch.Reset();
                stopwatch.Start();

                Watki w = new Watki();
                Thread[] threads = new Thread[Globals.threads_num];

                for (int i = 0; i < Globals.threads_num; i++)
                {
                    var temp = i;
                    threads[i] = new Thread(() => w.calculateT(temp));
                }

                foreach (Thread x in threads)
                    x.Start();

                for (int i = 0; i < Globals.threads_num; i++)
                    threads[i].Join();


                elapsedMs = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("Watki: (" + j + ") " + elapsedMs.ToString() + "ms");
            }

            for (int i = 0; i < Globals.tableResultW.GetLength(0); i++)
            {
                for (int j = 0; j < Globals.tableResultW.GetLength(1); j++)
                {
                    if (Globals.tableResultS[i, j] != Globals.tableResultW[i, j])
                    {
                        Console.WriteLine("Macierze nie sa takie same");
                    }
                }
            }

            /*
            for (int i = 0; i < Globals.tableA.GetLength(0); i++)
            {
                for (int j = 0; j < Globals.tableA.GetLength(1); j++)
                    Console.WriteLine(Globals.tableA[i, j]);                
            }
            Console.WriteLine("");
            for (int i = 0; i < Globals.tableB.GetLength(0); i++)
            {
                for (int j = 0; j < Globals.tableB.GetLength(1); j++)
                    Console.WriteLine(Globals.tableB[i, j]);
            }
            */

            /*
            Console.WriteLine("");
            for (int i = 0; i < Globals.tableResultS.GetLength(0); i++)
            {
                for (int j = 0; j < Globals.tableResultS.GetLength(1); j++)
                    Console.WriteLine(Globals.tableResultS[i, j]);
            }
            Console.WriteLine("");
            for (int i = 0; i < Globals.tableResultW.GetLength(0); i++)
            {
                for (int j = 0; j < Globals.tableResultW.GetLength(1); j++)
                    Console.WriteLine(Globals.tableResultW[i, j]);
            }
            */

            Console.Read();
        }

        public static void fillTable(int[,] table)
        {
            Random random = new Random();
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                    table[i, j] = random.Next(1, 10);
            }
        }

        public static void calculateS()
        {
            int result = 0;
            if (Globals.tableA.GetLength(1) != Globals.tableB.GetLength(0))
                Console.WriteLine("Nieprawidlowe wymiary macierzy");
            else
            {
                for (int i = 0; i < Globals.tableA.GetLength(0); i++)
                {
                    for (int j = 0; j < Globals.tableB.GetLength(1); j++)
                    {
                        result = 0;
                        for (int k = 0; k < Globals.tableA.GetLength(1); k++)
                        {
                            result += Globals.tableA[i, k] * Globals.tableB[k, j];
                        }
                        Globals.tableResultS[i, j] = result;
                    }
                }
            }
        }


        public class Watki
        {

            public void calculateT(int x)
            {
                int result = 0;
                if (Globals.tableA.GetLength(1) != Globals.tableB.GetLength(0))
                    Console.WriteLine("Nieprawidlowe wymiary macierzy");
                else
                {
                    for (int i = x; i < Globals.tableA.GetLength(0); i+=Globals.threads_num)
                    {
                        for (int j = 0; j < Globals.tableB.GetLength(1); j++)
                        {
                            result = 0;
                            for (int k = 0; k < Globals.tableA.GetLength(1); k++)
                            {
                                result += Globals.tableA[i, k] * Globals.tableB[k, j];
                            }
                            Globals.tableResultW[i, j] = result;
                        }
                    }
                }
            }
        }

    }











}


















