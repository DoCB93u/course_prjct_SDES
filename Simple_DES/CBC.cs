﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows;

namespace Simple_DES
{
    public static class SCBC
    {
        private static BitArray bitArray_char_previous;

        public static BitArray CipherCBC(BitArray bitArray_char, BitArray bitArray_key, BitArray InitVector, bool cond)
        {
            if (cond)
            {
                bitArray_char = SDES.XorBitArrays(InitVector, bitArray_char);

                bitArray_char = SDES.Cipher(bitArray_char, bitArray_key, cond);
            }

            else
            {
                bitArray_char = SDES.Cipher(bitArray_char, bitArray_key, cond);

                bitArray_char = SDES.XorBitArrays(InitVector, bitArray_char);
            }

            return bitArray_char;
        }

        public static string CBC(bool is_checked, char[] data, int key)
        {
            
            string temp = "";

            int counter = 0;
            string InitVectorStr = Microsoft.VisualBasic.Interaction.InputBox("Введите вектор инициализации(трехзначное дестиячное число)", "Окно ввода");
            int InitVectorInt;

            bool isNumber = int.TryParse(InitVectorStr, out InitVectorInt);

            if (!isNumber || InitVectorStr.Length != 3)
            {
                MessageBox.Show("Введенный вектор инициализации должен быть трехзначным числом!");
                return "";
            }

            BitArray InitVector = Converters.IntToBit(InitVectorInt, 8);

            if (is_checked)
            {
                int length = data.Length;
                for (int i = 0; i < length; i++)
                {
                    BitArray bitArray_char;
                    BitArray bitArray_key;

                    bitArray_char = Converters.CharToBit(data[length - i - 1]);
                    bitArray_key = Converters.IntToBit(key, 10);

                    if (counter != length - 1)
                    {
                        bitArray_char_previous = Converters.CharToBit(data[length - i - 2]);
                        bitArray_char = SCBC.CipherCBC(bitArray_char, bitArray_key, bitArray_char_previous, false);
                        counter++;
                    }
                    else
                    {
                        bitArray_char = SCBC.CipherCBC(bitArray_char, bitArray_key, InitVector, false);
                    }

                    bitArray_char_previous = bitArray_char;

                    char encryptedChar = Converters.BitArrayToChar(bitArray_char);

                    temp += encryptedChar;
                }

                temp = Converters.ReverseString(temp);

                return temp;
            }
            else
            {
                foreach (char c in data)
                {

                    BitArray bitArray_char;
                    BitArray bitArray_key;
                    bitArray_char = Converters.CharToBit(c);
                    bitArray_key = Converters.IntToBit(key, 10);

                    if (counter == 0)
                    {
                        bitArray_char = SCBC.CipherCBC(bitArray_char, bitArray_key, InitVector, true);

                        MessageBox.Show("Ваш вектор инициализации: " + InitVectorInt.ToString());
                    }
                    else
                    {
                        bitArray_char = SCBC.CipherCBC(bitArray_char, bitArray_key, bitArray_char_previous, true);
                    }

                    bitArray_char_previous = bitArray_char;

                    char encryptedChar = Converters.BitArrayToChar(bitArray_char);

                    // Посимвольно добавляем в текстовое поле результат вывода
                    temp += encryptedChar;

                    counter++;
                }

                return temp;
            }
        }
    }
}
