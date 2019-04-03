using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitmap
{
    class Program
    {
        struct coordinate
        {
            public int x;
            public int y;

            public coordinate(int x,int y) { this.x = x;this.y = y; }
        }
        static void Main(string[] args)
        {

            Random r = new Random();

            //init
            int allocBlockCountTotal = 20;
            int needAllocBlockCount = allocBlockCountTotal;
            int[,] coordinates = new int[30, 2];
            List<coordinate> co = new List<coordinate>();
            UInt32[] bitmap = new UInt32[10];
            UInt32[] mirroredBitmap = new UInt32[10];
            for (int i = 0; i < coordinates.GetLength(0); i++)
            {
                coordinates[i, 0] = r.Next(0, 10);
                coordinates[i, 1] = r.Next(0, 32);
            }
            for (int i = 0; i < coordinates.GetLength(0); i++)
            {
                UInt32 op = 1U << coordinates[i,1];
                bitmap[coordinates[i,0]] |= op;
            }

            //mirror bitmap,origin from upper right to upper left
            for (int i = 0; i < bitmap.Length; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    UInt32 temp = 1U << j;
                    if ((bitmap[i] & temp) != 0)
                    {
                        temp = 1U << (31 - j);
                        mirroredBitmap[i] |= temp;
                    }
                }
            }

            //log bitmap before allocating
            Console.WriteLine("before allocating:");
            foreach (var item in mirroredBitmap)
            {
                Console.WriteLine(Convert.ToString(item, 2).PadLeft(32, '0'));
            }
            Console.WriteLine();

            //emulation alloc, according to row-order matrix's coordinate that alloc memory
            for (int i = 0; i < bitmap.Length; i++)
            {
                for (int j = 31; j >= 0; j--)
                {
                    UInt32 temp = 1U << j;
                    if ((mirroredBitmap[i] & temp) == 0)
                    {
                        mirroredBitmap[i] |= temp;
                        co.Add(new coordinate(i, 31 - j));
                        needAllocBlockCount--;
                    }
                    if (needAllocBlockCount <= 0)
                    {
                        break;
                    }
                }
                if (needAllocBlockCount <= 0)
                {
                    break;
                }
            }

            //log allocated coord
            Console.WriteLine("allocated coordinate:");
            foreach (var item in co)
            {
                Console.Write($"({item.x},{item.y}) ");
            }
            Console.WriteLine();

            //log bitmap after allocating
            Console.WriteLine();
            Console.WriteLine("after allocating:");
            foreach (var item in mirroredBitmap)
            {
                Console.WriteLine(Convert.ToString(item, 2).PadLeft(32, '0'));
            }
            Console.WriteLine();

            //emulation recycle
            Console.WriteLine("recycled coordinate:");
            for (int x = 0; x < allocBlockCountTotal; x++)
            {
                int index = r.Next(0, co.Count-1);
                for (int i = 0; i < bitmap.Length; i++)
                {
                    if (co[index].x != i)
                    {
                        continue;
                    }
                    for (int j = 0; j < 32; j++)
                    {
                        if (co[index].y != j)
                        {
                            continue;
                        }
                        UInt32 temp = ~(1U << (31 - co[index].y));
                        mirroredBitmap[i] &= temp;
                        Console.Write($"({co[index].x},{co[index].y}) ");
                        co.RemoveAt(index);
                        break;
                    }
                    if (co.Count == 0)
                    {
                        break;
                    }
                }
            }
            Console.WriteLine();

            //log bitmap after recycling
            Console.WriteLine();
            Console.WriteLine("after recycling:");
            foreach (var item in mirroredBitmap)
            {
                Console.WriteLine(Convert.ToString(item, 2).PadLeft(32, '0'));
            }  

            while (true)
            {

            }
        }
    }
}
