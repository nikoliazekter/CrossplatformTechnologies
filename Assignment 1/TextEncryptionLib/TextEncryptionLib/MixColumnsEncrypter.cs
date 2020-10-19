using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace TextEncryptionLib
{
    public class MixColumnsEncrypter : IEncrypter
    {

        private readonly byte[,] multTable = new byte[6, 256];

        public MixColumnsEncrypter()
        {
            for (int i = 0; i < 256; i++)
            {
                multTable[0, i] = Mul02((byte)i);
                multTable[1, i] = Mul03((byte)i);
                multTable[2, i] = Mul09((byte)i);
                multTable[3, i] = Mul0b((byte)i);
                multTable[4, i] = Mul0d((byte)i);
                multTable[5, i] = Mul0e((byte)i);
            }
        }

        public string Encrypt(string plaintext)
        {
            byte[,] state = TextToState(plaintext, true);
            byte[,] newState = Encipher(state);
            return StateToText(newState, false);
        }

        public string Decrypt(string ciphertext)
        {
            byte[,] state = TextToState(ciphertext, false);
            byte[,] newState = Decipher(state);
            return StateToText(newState, true);
        }

        private byte[,] Encipher(byte[,] state)
        {
            int n = state.GetLength(1);
            byte[,] newState = new byte[4, n];
            for (int i = 0; i < n; i++)
            {
                byte s0 = state[0, i];
                byte s1 = state[1, i];
                byte s2 = state[2, i];
                byte s3 = state[3, i];
                newState[0, i] = (byte)(multTable[0, s0] ^ multTable[1, s1] ^ s2 ^ s3);
                newState[1, i] = (byte)(s0 ^ multTable[0, s1] ^ multTable[1, s2] ^ s3);
                newState[2, i] = (byte)(s0 ^ s1 ^ multTable[0, s2] ^ multTable[1, s3]);
                newState[3, i] = (byte)(multTable[1, s0] ^ s1 ^ s2 ^ multTable[0, s3]);
            }
            return newState;
        }
        private byte[,] Decipher(byte[,] state)
        {
            int n = state.GetLength(1);
            byte[,] newState = new byte[4, n];
            for (int i = 0; i < n; i++)
            {
                byte s0 = state[0, i];
                byte s1 = state[1, i];
                byte s2 = state[2, i];
                byte s3 = state[3, i];
                newState[0, i] = (byte)(multTable[5, s0] ^ multTable[3, s1] ^ multTable[4, s2] ^ multTable[2, s3]);
                newState[1, i] = (byte)(multTable[2, s0] ^ multTable[5, s1] ^ multTable[3, s2] ^ multTable[4, s3]);
                newState[2, i] = (byte)(multTable[4, s0] ^ multTable[2, s1] ^ multTable[5, s2] ^ multTable[3, s3]);
                newState[3, i] = (byte)(multTable[3, s0] ^ multTable[4, s1] ^ multTable[2, s2] ^ multTable[5, s3]);
            }
            return newState;
        }

        private byte[,] TextToState(string text, bool fromPlain)
        {
            byte[] message = TextUtils.StringToBytes(text);
            int n = message.Length / 4;
            int numPad = 0;
            if (fromPlain)
            {
                n += 1;
                numPad = 4 * n - message.Length;
            }

            byte[,] state = new byte[4, n];

            if (fromPlain)
            {
                byte firstBlockStart = (byte)((4 - (message.Length) % 4 - 1) % 4 + 1);
                state[0, 0] = firstBlockStart;
            }

            for (int i = 0; i < message.Length; i++)
            {
                state[(i + numPad) % 4, (i + numPad) / 4] = message[i];
            }
            return state;
        }

        private string StateToText(byte[,] state, bool toPlain)
        {
            int n = state.GetLength(1);
            byte firstBlockStart = 0;
            if (toPlain)
            {
                firstBlockStart = state[0, 0];
            }
            byte[] message = new byte[4 * n - firstBlockStart];
            for (int i = firstBlockStart; i < 4 * n; i++)
            {
                message[i - firstBlockStart] = state[i % 4, i / 4];
            }
            return TextUtils.BytesToString(message);
        }

        private byte Mul02(byte num)
        {
            byte res;
            if (num < 0x80)
            {
                res = (byte)(num << 1);
            }
            else
            {
                res = (byte)((num << 1) ^ 0x1b);
            }
            return (byte)(res % 0x100);
        }

        private byte Mul03(byte num)
        {
            return (byte)(Mul02(num) ^ num);
        }


        private byte Mul09(byte num)
        {
            return (byte)(Mul02(Mul02(Mul02(num))) ^ num);
        }

        private byte Mul0b(byte num)
        {
            return (byte)(Mul02(Mul02(Mul02(num))) ^ Mul02(num) ^ num);
        }

        private byte Mul0d(byte num)
        {
            return (byte)(Mul02(Mul02(Mul02(num))) ^ Mul02(Mul02(num)) ^ num);
        }

        private byte Mul0e(byte num)
        {
            return (byte)(Mul02(Mul02(Mul02(num))) ^ Mul02(Mul02(num)) ^ Mul02(num));
        }
    }
}
