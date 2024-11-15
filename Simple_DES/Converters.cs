using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_DES
{
    public class Converters
    {
        // Проверено*** (Reverse не НУЖЕН)
        public static BitArray CharToBit(char ch)
        {
            int value = Convert.ToInt32(ch);
            string binaryString = Convert.ToString(value, 2); // Преобразование в двоичную строку
            binaryString = binaryString.PadLeft(8, '0');
            BitArray bitArray = new BitArray(binaryString.Select(c => c == '1').ToArray());

            return bitArray;
        }


        // ПРОВЕРЕНО (REVERSE УЖЕ ИСПОЛЬЗУЕТСЯ ВНУТРИ)
        public static BitArray IntToBit(int number, int length)
        {
            BitArray bitArray = new BitArray(length);
            for (int i = 0; i < length; i++)
            {
                bitArray[i] = (number & (1 << i)) != 0;
            }

            bitArray = SDES.Reverse(bitArray);

            return bitArray;
        }


        // ПРОВЕРЕНО (REVERSE НЕ НУЖЕН)
        public static int BitArrayToInt(BitArray bits)
        {
            int decimalNumber = 0;
            for (int i = 0; i < 8; i++)
            {
                decimalNumber += Convert.ToInt32(bits[i]) * Convert.ToInt32(Math.Pow(2, 7 - i));
            }

            return decimalNumber;
        }


        // ПРОВЕРЕНО (REVERSE НЕ НУЖЕН)
        public static char IntToChar(int value)
        {
            if (value < 0 || value > 255)
            {
                throw new ArgumentException("Значение должно быть в диапазоне ASCII (0-255)");
            }

            char symbol = Convert.ToChar(value);

            return symbol;
        }


        // ПРОВЕРЕНО (REVERSE не нужен)
        public static char BitArrayToChar(BitArray bits)
        {
            int tempValue = BitArrayToInt(bits);
            char ch = IntToChar(tempValue);
            return ch;
        }

        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
