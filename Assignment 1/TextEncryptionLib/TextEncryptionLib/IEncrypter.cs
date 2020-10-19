using System;
using System.Collections.Generic;
using System.Text;

namespace TextEncryptionLib
{
    public interface IEncrypter
    {
        string Encrypt(string plaintext);

        string Decrypt(string ciphertext);
    }
}
