using System;
using System.Collections.Generic;
using System.Text;

namespace TextEncryptionLib
{
    public enum EncryptionType
    {
        ShiftRows,
        SubBytes,
        MixColumns
    }
    public class FileEncrypter
    {
        public void EncryptFile(string source, string dest, EncryptionType enc)
        {
            string plaintext = System.IO.File.ReadAllText(source, Encoding.Unicode);
            IEncrypter encrypter = GetEncrypter(enc);
            string ciphertext = encrypter.Encrypt(plaintext);
            System.IO.File.WriteAllBytes(dest, TextUtils.StringToBytes(ciphertext));
        }

        public void DecryptFile(string source, string dest, EncryptionType enc)
        {
            string ciphertext = TextUtils.BytesToString(System.IO.File.ReadAllBytes(source));
            IEncrypter encrypter = GetEncrypter(enc);
            string plaintext = encrypter.Decrypt(ciphertext);
            System.IO.File.WriteAllBytes(dest, TextUtils.StringToBytes(plaintext));
        }


        private IEncrypter GetEncrypter(EncryptionType enc)
        {
            IEncrypter encryptor;
            switch (enc)
            {
                case EncryptionType.ShiftRows:
                    encryptor = new ShiftRowsEncrypter();
                    break;
                case EncryptionType.SubBytes:
                    encryptor = new SubBytesEncrypter();
                    break;
                case EncryptionType.MixColumns:
                    encryptor = new MixColumnsEncrypter();
                    break;
                default:
                    encryptor = new MixColumnsEncrypter();
                    break;
            }
            return encryptor;
        }
    }
}
