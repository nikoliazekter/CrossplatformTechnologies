using System;
using System.Collections.Generic;
using System.Text;

namespace TextEncryptionLib
{
    public class ShiftRowsEncrypter : IEncrypter
    {

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
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    newState[i, j] = state[i, ((j + i) % n + n) % n];
                }
            }
            return newState;
        }
        private byte[,] Decipher(byte[,] state)
        {
            int n = state.GetLength(1);
            byte[,] newState = new byte[4, n];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    newState[i, j] = state[i, ((j - i) % n + n) % n];
                }
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
    }
}
