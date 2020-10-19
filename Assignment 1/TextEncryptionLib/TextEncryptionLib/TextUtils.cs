using System;
using System.Collections.Generic;
using System.Text;

namespace TextEncryptionLib
{
    class TextUtils
    {
        public static byte[] StringToBytes(string text)
        {
            byte[] bytes = new byte[text.Length * sizeof(char)];
            Buffer.BlockCopy(text.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
