using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_DES
{
    public static class SOFB
    {
        private static BitArray bitArray_char_previous;

        public static BitArray CipherOFB(BitArray bitArray_char, BitArray bitArray_key, BitArray InitVector)
        {
            InitVector = SDES.Cipher(InitVector, bitArray_key, true);
            bitArray_char = SDES.XorBitArrays(bitArray_char, InitVector);
            return bitArray_char;
        }

        public static string OFB(bool is_checked, char[] data, int key)
        {
            string temp = "";

            int counter = 0;
            string InitVectorStr = Microsoft.VisualBasic.Interaction.InputBox("Введите вектор инициализации(трехзначное дестиячное число)", "Окно ввода");
            int InitVectorInt;

            bool isNumber = int.TryParse(InitVectorStr, out InitVectorInt);

            BitArray InitVector = Converters.IntToBit(InitVectorInt, 8);
            if (!isNumber || InitVectorStr.Length != 3)
            {
                MessageBox.Show("Введенный вектор инициализации должен быть трехзначным числом!");
                return "";
            }

            foreach(char c in data)
            {
                BitArray bitArray_char;
                BitArray bitArray_key;
                bitArray_char = Converters.CharToBit(c);
                bitArray_key = Converters.IntToBit(key, 10);

                if (counter == 0)
                {
                    bitArray_char = SOFB.CipherOFB(bitArray_char, bitArray_key, InitVector);

                    bitArray_char_previous = SDES.Cipher(InitVector, bitArray_key, true);
                }   
                else
                {
                    bitArray_char = SOFB.CipherOFB(bitArray_char, bitArray_key, bitArray_char_previous);
                }

                char encryptedChar = Converters.BitArrayToChar(bitArray_char);

                // Посимвольно добавляем в текстовое поле результат вывода
                temp += encryptedChar;

                counter++;
            }

            return temp;
        }
    }
}
