using System;
using TextEncryptionLib;

namespace UseTextEncryptionLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            FileEncrypter fileEncrypter = new FileEncrypter();
            fileEncrypter.EncryptFile(
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\source.txt",
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\encrypted_shift.txt",
                EncryptionType.ShiftRows);
            fileEncrypter.EncryptFile(
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\source.txt",
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\encrypted_sub.txt",
                EncryptionType.SubBytes);
            fileEncrypter.EncryptFile(
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\source.txt",
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\encrypted_mix.txt",
                EncryptionType.MixColumns);

            fileEncrypter.DecryptFile(
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\encrypted_shift.txt",
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\decrypted_shift.txt",
                EncryptionType.ShiftRows);
            fileEncrypter.DecryptFile(
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\encrypted_sub.txt",
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\decrypted_sub.txt",
                EncryptionType.SubBytes);
            fileEncrypter.DecryptFile(
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\encrypted_mix.txt",
                @"C:\Users\nikol\source\repos\UseTextEncryptionLibrary\UseTextEncryptionLibrary\decrypted_mix.txt",
                EncryptionType.MixColumns);
        }
    }
}
