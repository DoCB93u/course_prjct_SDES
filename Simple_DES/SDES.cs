using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_DES
{
    public static class SDES
    {
        private readonly static int[] P10_arr = { 2, 4, 1, 6, 3, 9, 0, 8, 7, 5 }; //Перестановка Р10: { 3 5 2 7 4 10 1 9 8 6 }
        private readonly static int[] P8_arr = { 5, 2, 6, 3, 7, 4, 9, 8 }; //Перестановка Р8: { 6 3 7 4 8 5 10 9 - - }
        private readonly static int[] IP_arr = { 1, 5, 2, 0, 3, 7, 4, 6 }; //Перестановка IP: { 2 6 3 1 4 8 5 7 }
        private readonly static int[] E_P_arr = { 3, 0, 1, 2, 1, 2, 3, 0 }; //E/P: { 4 1 2 3 2 3 4 1}
        private readonly static int[] IP_M_1_arr = { 3, 0, 2, 4, 6, 1, 7, 5 }; //Перестановка IР-1: { 4 1 3 5 7 2 8 6 }

        private static readonly int[,] MatrixL = new int[,]
        {
            { 1, 0, 3, 2 },
            { 3, 2, 1, 0 },
            { 0, 2, 1, 3 },
            { 3, 1, 3, 1 },
        };

        private static readonly int[,] MatrixR = new int[,]
        {
            { 1, 1, 2, 3 },
            { 2, 0, 1, 3 },
            { 3, 0, 1, 0 },
            { 2, 1, 0, 3 },
        };

        public static BitArray P10(BitArray bitArray)
        {
            int length = bitArray.Length;
            BitArray bitArray_temp = new BitArray(length);

            for (int i = 0; i < length; i++)
            {
                bitArray_temp[i] = bitArray[P10_arr[i]];
            }

            return bitArray_temp;
        }

        public static BitArray P8(BitArray bitArray)
        {
            int length = bitArray.Length;
            BitArray bitArray_temp = new BitArray(length - 2);

            for (int i = 0; i < length - 2; i++)
            {
                bitArray_temp[i] = bitArray[P8_arr[i]];
            }

            return bitArray_temp;
        }

        public static BitArray IP(BitArray bitArray)
        {
            int length = bitArray.Length;
            BitArray bitArray_temp = new BitArray(length);

            for (int i = 0; i < length; i++)
            {
                bitArray_temp[i] = bitArray[IP_arr[i]];
            }

            return bitArray_temp;
        }

        public static BitArray E_P(BitArray bitArray)
        {
            int length = bitArray.Length;
            BitArray bitArray_temp = new BitArray(length * 2);

            for (int i = 0; i < length * 2; i++)
            {
                bitArray_temp[i] = bitArray[E_P_arr[i]];
            }

            return bitArray_temp;
        }
        public static BitArray IP_M_1(BitArray bitArray)
        {
            BitArray bitArray_temp = new BitArray(bitArray.Length);
            for (int i = 0; i < bitArray.Length; i++)
            {
                bitArray_temp[i] = bitArray[IP_M_1_arr[i]];
            }

            return bitArray_temp;
        }

        public static BitArray CSlideLeft(BitArray bitArray)
        {
            int length = bitArray.Length;
            BitArray bitArray_temp = new BitArray(length);
            int halfLength = length / 2;

            if (length % 2 != 0)
            {
                throw new ArgumentException("BitArray length must be even.");
            }

            // Циклический сдвиг первой половины
            bool firstBitFirstHalf = bitArray[0];
            for (int i = 1; i < halfLength; i++)
            {
                bitArray_temp[i - 1] = bitArray[i];
            }
            bitArray_temp[halfLength - 1] = firstBitFirstHalf;

            // Циклический сдвиг второй половины
            bool firstBitSecondHalf = bitArray[halfLength];
            for (int i = halfLength + 1; i < length; i++)
            {
                bitArray_temp[i - 1] = bitArray[i];
            }
            bitArray_temp[length - 1] = firstBitSecondHalf;

            return bitArray_temp; // Возвращаем измененный массив
        }

        public static BitArray XorBitArrays(BitArray bitArray1, BitArray bitArray2)
        {
            if (bitArray1.Length != bitArray2.Length)
            {
                throw new ArgumentException("BitArrays must be of the same length.");
            }

            BitArray result = new BitArray(bitArray1.Length);
            for (int i = 0; i < bitArray1.Length; i++)
            {
                result[i] = bitArray1[i] ^ bitArray2[i];
            }

            return result;
        }

        public static BitArray Reverse(BitArray bitArray)
        {
            int length = bitArray.Length;
            BitArray bitArray_temp = new BitArray(length);

            for (int i = 0; i < length; i++)
            {
                bitArray_temp[i] = bitArray[length - 1 - i];
            }

            return bitArray_temp;
        }

        public static BitArray SBox(BitArray value, int[,] S)
        {
            if (value.Length != 4)
            {
                throw new ArgumentException("Input BitArray must be of length 4.");
            }

            // Получаем двоичную строку (первый бит + последний бит)
            string rowBits = (value[0] ? "1" : "0") + (value[3] ? "1" : "0");
            int row = Convert.ToInt32(rowBits, 2);

            // Получаем двоичный столбец (2 бита по-середине)
            string colBits = (value[1] ? "1" : "0") + (value[2] ? "1" : "0");
            int col = Convert.ToInt32(colBits, 2);

            int output = S[row, col];


            BitArray bitArray = new BitArray(2);
            bitArray[0] = (output & 0b01) != 0;
            bitArray[1] = (output & 0b10) != 0;

            bitArray = Reverse(bitArray);

            return bitArray;
        }

        public static BitArray SumArrays(BitArray bitArray1, BitArray bitArray2)
        {
            if (bitArray1.Length != bitArray2.Length)
            {
                throw new ArgumentException("Массивы должны быть равно по размерности");
            }
            int length = bitArray1.Length;
            BitArray bitArray_temp = new BitArray(length * 2);

            for (int i = 0; i < length * 2; i++)
            {
                if (i < length)
                {
                    bitArray_temp[i] = bitArray1[i];
                }
                else
                {
                    bitArray_temp[i] = bitArray2[i - length];
                }
            }

            return bitArray_temp;
        }

        public static BitArray SW(BitArray left, BitArray right)
        {
            if (left.Length != right.Length)
            {
                throw new ArgumentException("Invalid matrix type. Use '0' or '1'.");
            }

            BitArray bitArray_temp = new BitArray(left.Length * 2);
            for (int i = 0; i < left.Length * 2; i++)
            {
                if (i < left.Length)
                {
                    bitArray_temp[i] = right[i];
                }
                else
                {
                    bitArray_temp[i] = left[i - left.Length];
                }
            }

            return bitArray_temp;
        }

        public static BitArray FeistelAlgorithm(BitArray bitArray, BitArray key, BitArray RIGHT, BitArray LEFT)
        {
            int length = bitArray.Length;
            BitArray bitArrayLeft = new BitArray(length / 2);
            BitArray bitArrayRight = new BitArray(length / 2);

            for (int i = 0; i < length / 2; i++)
            {
                bitArrayRight[i] = bitArray[i + length / 2];
            }

            bitArray = E_P(bitArrayRight);

            bitArray = XorBitArrays(bitArray, key); //сохранили значения для XOR-инга в bitArray чтобы не менять длину bAR или bAL

            for (int i = 0; i < length; i++) //порезали bA на bAL и bAR
            {
                if (i < length / 2)
                {
                    bitArrayLeft[i] = bitArray[i];
                }

                else
                {
                    bitArrayRight[i - length / 2] = bitArray[i];
                }
            }

            bitArrayLeft = SBox(bitArrayLeft, MatrixL);
            bitArrayRight = SBox(bitArrayRight, MatrixR);

            bitArray = SumArrays(bitArrayLeft, bitArrayRight);

            LEFT = XorBitArrays(LEFT, bitArray);

            return LEFT;
        }

        public static BitArray Cipher(BitArray bitArray_char, BitArray bitArray_key, bool cond)
        {
            //Подготовка массива битов для финальных ключей, юзнем больше памяти чем дозволено, но повысим читабельность 
            BitArray bitArray_key_final1;
            BitArray bitArray_key_final2;

            //Перестановка Р10
            bitArray_key = P10(bitArray_key);
            //Циклический сдвиг влево
            bitArray_key = CSlideLeft(bitArray_key);
            //Перестановка Р8
            bitArray_key_final1 = P8(bitArray_key);

            //Двойной циклический сдвиг влево
            bitArray_key = CSlideLeft(bitArray_key);
            bitArray_key = CSlideLeft(bitArray_key);
            //Перестановка Р8
            bitArray_key_final2 = P8(bitArray_key);

            bitArray_char = IP(bitArray_char);

            BitArray RIGHT = new BitArray(bitArray_char.Length / 2); //Для сохранения первичной правой части до XOR
            BitArray LEFT = new BitArray(bitArray_char.Length / 2); //Для сохранения первичной левой части

            for (int i = 0; i < bitArray_char.Length; i++)
            {
                if (i < bitArray_char.Length / 2)
                {
                    LEFT[i] = bitArray_char[i];
                }
                else
                {
                    RIGHT[i - bitArray_char.Length / 2] = bitArray_char[i];
                }
            }

            switch (cond)
            {
                case true: LEFT = FeistelAlgorithm(bitArray_char, bitArray_key_final1, RIGHT, LEFT); break;
                case false: LEFT = FeistelAlgorithm(bitArray_char, bitArray_key_final2, RIGHT, LEFT); break;
            }


            bitArray_char = SW(LEFT, RIGHT);

            for (int i = 0; i < bitArray_char.Length; i++)
            {
                if (i < bitArray_char.Length / 2)
                {
                    LEFT[i] = bitArray_char[i];
                }

                else
                {
                    RIGHT[i - bitArray_char.Length / 2] = bitArray_char[i];
                }
            }

            switch (cond)
            {
                case true: LEFT = FeistelAlgorithm(bitArray_char, bitArray_key_final2, RIGHT, LEFT); break;
                case false: LEFT = FeistelAlgorithm(bitArray_char, bitArray_key_final1, RIGHT, LEFT); break;
            }

            bitArray_char = SumArrays(LEFT, RIGHT);

            bitArray_char = IP_M_1(bitArray_char);

            return bitArray_char;
        }
    }
}
