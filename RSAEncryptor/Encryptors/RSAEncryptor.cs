using Encryptors.Interfaces;
using System;
using System.Collections.Generic;
using Encryptors.Utilities;
using System.IO;

namespace Encryptors.Encryptors
{
    public class RSAEncryptor : IEncryptor
    {
        #region Variables
        public int n = 0;
        int fi = 0;
        int P = 0;
        int Q = 0;
        int e = 0;
        public long d = 0;
        int[] PrimeNumbers = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97};
        public void SetVariables(int p, int q)
        {
            P = p;
            Q = q;
            n = P * Q;
            fi = (P - 1) * (Q - 1);
            Random rnd = new Random();
            e = PrimeNumbers[rnd.Next(0, 25)];
            while ((e > fi) || (fi % e == 0))
            {
                e = PrimeNumbers[rnd.Next(0, 25)];
            }
            d = EuclidesAlgorithm();
        }
        #endregion

        #region Functions
        public int EuclidesAlgorithm()
        {
            int k = fi;
            int X = e;
            int Y = fi;
            int Z = 1;
            int aux = 0;
            while (X != 1)
            {
                aux = Z;
                Z = Y - ((k / X) * Z);
                Y = aux;
                while(Z < 0)
                {
                    Z = Z + fi;
                }
                aux = X;
                X = k - ((k / X) * X);
                k = aux;
            }
            return Z;
        }
        public void GetKeys(string p, string q, string keyPath)
        {
            SetVariables(int.Parse(p), int.Parse(q));
            var publickeypath = $"{keyPath}/{"public.key"}";
            using var publicfileForWriting = new FileStream(publickeypath, FileMode.OpenOrCreate);
            using var publicwriter = new StreamWriter(publicfileForWriting);
            publicwriter.Write(n.ToString() + "," + e.ToString());
            publicwriter.Close();
            publicfileForWriting.Close();
            var privatekeypath = $"{keyPath}/{"private.key"}";
            using var privatefileForWriting = new FileStream(privatekeypath, FileMode.OpenOrCreate);
            using var privatewriter = new StreamWriter(privatefileForWriting);
            privatewriter.Write(n.ToString() + "," + d.ToString());
            privatewriter.Close();
            privatefileForWriting.Close();
            ResetVariables();
        }
        public void ResetVariables()
        {
            int n = 0;
            int e = 0;
            long d = 0;
        }

        #endregion

        #region Encryption
        public string EncryptString(int p, int q, string message)
        {
            SetVariables(p, q);
            List<string> numbers = new List<string>();
            List<int> numberst = new List<int>();
            string emessage = "";
            byte[] bytes = new byte[1];
            int cbyte;
            int bbyte;
            int maxL = Convert.ToString(n, 2).Length;
            foreach (var cha in message)
            {
                cbyte = Convert.ToInt32(Convert.ToByte(cha));
                bbyte = cbyte;
                for (int i = 1; i < e; i++)
                {
                    cbyte = (cbyte * bbyte) % n;
                }
                numbers.Add(Convert.ToString(cbyte, 2));
                numberst.Add(cbyte);
            }
            string number = "";
            string bchar = "";
            foreach (var num in numbers)
            {
                number = num;
                while (number.Length < maxL)
                {
                    number = "0" + number;
                }
                bchar += number;
            }
            while(bchar.Length >= 8)
            {
                bytes[0] = Convert.ToByte(bchar.Substring(0, 8), 2);
                emessage += ByteConverter.ConvertToString(bytes);
                bchar = bchar.Remove(0, 8);
            }
            if (bchar.Length != 0)
            {
                while (bchar.Length < 8)
                {
                    bchar += "0";
                }
                bytes[0] = Convert.ToByte(bchar, 2);
                emessage += ByteConverter.ConvertToString(bytes);
            }
            return emessage;
        }

        public string EncryptFile(string keyPath, string filePath, string savingPath, string nombre)
        {
            using var fileForReadingKey = new FileStream(keyPath, FileMode.Open);
            using var readerKey = new StreamReader(fileForReadingKey);
            string keyf = readerKey.ReadToEnd();
            string[] Keys = keyf.Split(',');
            int M = Convert.ToInt32(Keys[0]);
            int P = Convert.ToInt32(Keys[1]);
            readerKey.Close();
            fileForReadingKey.Close();

            using var fileForReading = new FileStream(filePath, FileMode.Open);
            using var reader = new BinaryReader(fileForReading);
            var buffer = new byte[2000];

            var fileRoute = $"{savingPath}/{nombre + ".txt"}";
            using var fileForWriting = new FileStream(fileRoute, FileMode.OpenOrCreate);
            using var writer = new BinaryWriter(fileForWriting);

            List<string> numbers = new List<string>();
            List<int> numberst = new List<int>();
            byte[] bytes = new byte[1];
            int cbyte;
            int bbyte;
            string bchar = "";
            int maxL = Convert.ToString(M, 2).Length;

            while (fileForReading.Position != fileForReading.Length)
            {
                buffer = reader.ReadBytes(buffer.Length);

                foreach (var cha in buffer)
                {
                    cbyte = Convert.ToInt32(cha);
                    bbyte = cbyte;
                    for (int i = 1; i < P; i++)
                    {
                        cbyte = (cbyte * bbyte) % M;
                    }
                    numbers.Add(Convert.ToString(cbyte, 2));
                    numberst.Add(cbyte);
                }
                string number = "";

                foreach (var num in numbers)
                {
                    number = num;
                    while (number.Length < maxL)
                    {
                        number = "0" + number;
                    }
                    bchar += number;
                }
                while (bchar.Length >= 8)
                {
                    bytes[0] = Convert.ToByte(bchar.Substring(0, 8), 2);
                    writer.Write(bytes);
                    bchar = bchar.Remove(0, 8);
                }
                numbers.Clear();
            }
            if (bchar.Length != 0)
            {

                while (bchar.Length < 8)
                {
                    bchar += "0";
                }
                bytes[0] = Convert.ToByte(bchar, 2);
                writer.Write(bytes);
            }

            return fileRoute;
        }

        #endregion

        #region Decryption
        public string DecryptString(string cmessage)
        {
            string message = "";
            List<int> numbers = new List<int>();
            string nchar;
            byte[] bytes = new byte[1];
            string number = "";
            int cbyte = 0;
            int bbyte = 0;
            int maxl = Convert.ToString(n, 2).Length;
            foreach (var c in cmessage)
            {
                nchar = Convert.ToString(ByteConverter.ConvertToByte(c), 2);
                while (nchar.Length < 8)
                {
                    nchar = "0" + nchar;
                }
                number += nchar;
                if (number.Length >= maxl)
                {
                    numbers.Add(Convert.ToInt32(number.Substring(0, maxl), 2)); 
                    number = number.Remove(0, maxl);
                }
            }
            foreach (var numb in numbers)
            {
                cbyte = numb;
                bbyte = numb;
                for (int i = 1; i < d; i++)
                {
                    cbyte = (cbyte * bbyte) % n;
                }
                bytes[0] = Convert.ToByte(cbyte);
                message += ByteConverter.ConvertToString(bytes);
            }
            return message;
        }

        public string DecryptFile(string keyPath, string filePath, string savingPath, string nombre)
        {
            using var fileForReadingKey = new FileStream(keyPath, FileMode.Open);
            using var readerKey = new StreamReader(fileForReadingKey);
            string keyf = readerKey.ReadToEnd();
            string[] Keys = keyf.Split(',');
            int M = Convert.ToInt32(Keys[0]);
            int P = Convert.ToInt32(Keys[1]);
            readerKey.Close();
            fileForReadingKey.Close();

            using var fileForReading = new FileStream(filePath, FileMode.Open);
            using var reader = new BinaryReader(fileForReading);
            var buffer = new byte[2000];

            var fileRoute = $"{savingPath}/{nombre + ".txt"}";
            using var fileForWriting = new FileStream(fileRoute, FileMode.OpenOrCreate);
            using var writer = new BinaryWriter(fileForWriting);

            List<int> numbers = new List<int>();
            string nchar;
            byte[] bytes = new byte[1];
            string number = "";
            int cbyte = 0;
            int bbyte = 0;
            int maxl = Convert.ToString(M, 2).Length;

            while (fileForReading.Position != fileForReading.Length)
            {
                buffer = reader.ReadBytes(buffer.Length);

                foreach (var c in buffer)
                {
                    nchar = Convert.ToString(c, 2);
                    while (nchar.Length < 8)
                    {
                        nchar = "0" + nchar;
                    }
                    number += nchar;
                    if (number.Length >= maxl)
                    {
                        numbers.Add(Convert.ToInt32(number.Substring(0, maxl), 2));
                        number = number.Remove(0, maxl);
                    }
                }

                foreach (var numb in numbers)
                {
                    cbyte = numb;
                    bbyte = numb;
                    for (int i = 1; i < P; i++)
                    {
                        cbyte = (cbyte * bbyte) % M;
                    }
                    bytes[0] = Convert.ToByte(cbyte);
                    writer.Write(bytes);
                }
                numbers.Clear();
            }

            ResetVariables();
            reader.Close();
            readerKey.Close();
            fileForReading.Close();
            fileForReadingKey.Close();
            writer.Close();
            fileForWriting.Close();

            return fileRoute;
        }


        #endregion
    }
}

