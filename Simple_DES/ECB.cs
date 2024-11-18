using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;

namespace Simple_DES
{
    public static class SECB
    {
        public static string ECB(bool is_checked, char[] data, int key)
        {
            string temp = "";

            foreach (char c in data)
            {
                char encryptedChar;
                BitArray bitArray_char;
                BitArray bitArray_key;
                bitArray_char = Converters.CharToBit(c);

                bitArray_key = Converters.IntToBit(key, 10);

                if (is_checked) { bitArray_char = SDES.Cipher(bitArray_char, bitArray_key, false); }
                else { bitArray_char = SDES.Cipher(bitArray_char, bitArray_key, true); }

                encryptedChar = Converters.BitArrayToChar(bitArray_char);

                // Посимвольно добавляем в текстовое поле результат вывода
                temp += encryptedChar;
            }

            return temp;
        }
    }
}
