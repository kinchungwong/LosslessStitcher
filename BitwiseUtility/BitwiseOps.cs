using System;

namespace BitwiseUtility
{
    public static class BitwiseOps
    {
        public static int Add(int a, int b)
        {
            return unchecked(a + b);
        }

        public static uint Add(uint a, uint b)
        {
            return unchecked(a + b);
        }

        public static long Add(long a, long b)
        {
            return unchecked(a + b);
        }

        public static ulong Add(ulong a, ulong b)
        {
            return unchecked(a + b);
        }

        public static int Xor(int a, int b)
        {
            return a ^ b;
        }

        public static uint Xor(uint a, uint b)
        {
            return a ^ b;
        }

        public static long Xor(long a, long b)
        {
            return a ^ b;
        }

        public static ulong Xor(ulong a, ulong b)
        {
            return a ^ b;
        }

        public static int Rotate(int input, int amount)
        {
            unchecked
            { 
                return (int)Rotate((uint)input, amount);
            }
        }

        public static uint Rotate(uint input, int amount)
        {
            unchecked
            {
                uint leftAmount = unchecked((uint)amount) & 31u;
                if (leftAmount == 0u)
                {
                    return input;
                }
                uint rightAmount = 32u - leftAmount;
                return (input << (int)leftAmount) | (input >> (int)rightAmount);
            }
        }

        public static long Rotate(long input, int amount)
        {
            unchecked 
            {
                return (long)Rotate((ulong)input, amount);
            }
        }

        public static ulong Rotate(ulong input, int amount)
        {
            unchecked
            {
                uint leftAmount = unchecked((uint)amount) & 63u;
                if (leftAmount == 0u)
                {
                    return input;
                }
                uint rightAmount = 64u - leftAmount;
                return (input << (int)leftAmount) | (input >> (int)rightAmount);
            }
        }
    }
}
