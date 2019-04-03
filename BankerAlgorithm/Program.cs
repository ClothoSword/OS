using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BankerAlgorithm
{
    public class Program
    {
        public static void Main()
        {

            //Init 
            Random r = new Random();
            int availableTotal = 10;
            int available = availableTotal;
            int allocTotal = availableTotal + 1;
            List<int> max = new List<int> { 6, 5, 4, 7 };
            List<int> allocation = new List<int>();
            List<int> need = new List<int>();
            List<int> safeOrder = new List<int>();

            while (allocTotal >= availableTotal)
            {
                allocTotal = 0;
                allocation.Clear();
                for (int i = 0; i < max.Count; i++)
                {
                    allocation.Add(r.Next(0, max[i]));
                    allocTotal += allocation[i];
                }
            }
            for (int i = 0; i < allocation.Count; i++)
            {
                available -= allocation[i];
            }
            for (int i = 0; i < max.Count; i++)
            {
                need.Add(max[i] - allocation[i]);
            }

            //log
            Console.WriteLine("availableTotal:".PadRight(17) + $"{availableTotal}");
            Console.WriteLine("available:".PadRight(17) + $"{available}");
            Log(max.ToArray(), "max");
            Log(allocation.ToArray(), "allocation");
            Log(need.ToArray(), "need");
            Console.WriteLine();

            //algorithm
            BankerAlgorithm(ref available, need, allocation, safeOrder);
            if (safeOrder.Count > 0)
                Log(safeOrder.ToArray(), "safeOrder");

            while (true)
            {

            }
        }

        static void Log(int[] iArray,string logName)
        {
            Console.Write(($"{logName}:").PadRight(17));
            foreach (var item in iArray)
            {
                Console.Write((item + " "));
            }
            Console.WriteLine();
        }

        static void BankerAlgorithm(ref int ava,List<int> need,List<int> alloc,List<int> safeOrder)
        {
            int min = int.MaxValue;
            int minIndex = -1;
            for (int i = 0; i < need.Count; i++)
            {
                if (need[i] < min)
                {
                    min = need[i];
                    minIndex = i;
                }
            }
            if (minIndex == -1)
            {
                return;
            }
            Console.WriteLine($"min:{min} , minIndex:{minIndex}");
            if (min <= ava)
            {
                ava += alloc[minIndex];
                need[minIndex] = int.MaxValue;
                safeOrder.Add(minIndex);
                BankerAlgorithm(ref ava, need, alloc, safeOrder);
            }
            else if(min>ava)
            {
                Console.WriteLine("minNeed>available,translate to unsafe state");
                return;
            }
        }
    }

}
