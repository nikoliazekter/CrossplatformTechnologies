using System;
using System.Text;
using TextEncryptionLib;

namespace TestTextEncryption
{
    class Test
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            string plaintext = "The quick brown fox jumps over the lazy dog.";
            Console.WriteLine("Original message: " + plaintext);
            Console.WriteLine();
            TestShiftRowsEncryption(plaintext);
            Console.WriteLine();
            TestSubBytesEncryption(plaintext);
            Console.WriteLine();
            TestMixColumnsEncryption(plaintext);
            Console.ReadKey();

            FileEncrypter fileEncrypter = new FileEncrypter();
            fileEncrypter.EncryptFile(
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\source.txt",
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\encrypted_shift.txt",
                EncryptionType.ShiftRows);
            fileEncrypter.EncryptFile(
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\source.txt",
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\encrypted_sub.txt",
                EncryptionType.SubBytes);
            fileEncrypter.EncryptFile(
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\source.txt",
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\encrypted_mix.txt",
                EncryptionType.MixColumns);

            fileEncrypter.DecryptFile(
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\encrypted_shift.txt",
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\decrypted_shift.txt",
                EncryptionType.ShiftRows);
            fileEncrypter.DecryptFile(
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\encrypted_sub.txt",
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\decrypted_sub.txt",
                EncryptionType.SubBytes);
            fileEncrypter.DecryptFile(
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\encrypted_mix.txt",
                @"C:\Users\nikol\source\repos\TextEncryptionLib\TestTextEncryption\decrypted_mix.txt",
                EncryptionType.MixColumns);
        }

        private static void TestShiftRowsEncryption(string plaintext)
        {
            Console.WriteLine("ShiftRowsEncryption");
            IEncrypter encrypter = new ShiftRowsEncrypter();
            string ciphertext = encrypter.Encrypt(plaintext);
            Console.WriteLine(ciphertext);
            string deciphered = encrypter.Decrypt(ciphertext);
            Console.WriteLine("Deciphered message: " + deciphered);
        }

        private static void TestSubBytesEncryption(string plaintext)
        {
            Console.WriteLine("SubBytesEncryption");
            IEncrypter encrypter = new SubBytesEncrypter();
            string ciphertext = encrypter.Encrypt(plaintext);
            Console.WriteLine(ciphertext);
            string deciphered = encrypter.Decrypt(ciphertext);
            Console.WriteLine("Deciphered message: " + deciphered);
        }

        private static void TestMixColumnsEncryption(string plaintext)
        {
            Console.WriteLine("MixColumnsEncryption");
            IEncrypter encrypter = new MixColumnsEncrypter();
            string ciphertext = encrypter.Encrypt(plaintext);
            Console.WriteLine(ciphertext);
            string deciphered = encrypter.Decrypt(ciphertext);
            Console.WriteLine("Deciphered message: " + deciphered);
        }
    }
}
