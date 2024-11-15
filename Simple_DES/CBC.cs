using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_DES
{
    public static class SCBC
    {
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
    }
}
