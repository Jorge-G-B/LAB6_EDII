using Encryptors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encryptors.Utilities;

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
        public int d = 0;
        int[] PrimeNumbers = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
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
            e = 17;
            d = EuclidesAlgorithm();
        }
        #endregion

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
                if (Z < 0)
                {
                    Z = Z + fi;
                }
                aux = X;
                X = k - ((k / X) * X);
                k = aux;
            }
            return Z;
        }
        public string[] GetKeys(string p, string q)
        {
            return new string[] { string.Empty, string.Empty };
        }
        public string EncryptString(int p, int q, string message)
        {
            SetVariables(p, q);
            List<long> numbers = new List<long>();
            string emessage = "";
            byte[] bytes = new byte[1];
            long cbyte;
            long bbyte;
            long maxL = -1;
            foreach (var cha in message)
            {
                cbyte = Convert.ToInt32(Convert.ToByte(cha));
                bbyte = cbyte;
                for (int i = 1; i < e; i++)
                {
                    cbyte = (cbyte * bbyte) % n;
                }
                if (cbyte > maxL)
                {
                    maxL = cbyte;
                }
                numbers.Add(cbyte);
            }
            maxL = maxL.ToString().Length;
            string bnum;
            string bchar = "";
            string number;
            bytes[0] = Convert.ToByte(maxL);
            emessage += ByteConverter.ConvertToString(bytes);
            foreach (var num in numbers)
            {
                number = num.ToString();
                while (number.Length < maxL)
                {
                    number = "0" + number;
                }
                for (int i = 0; i < maxL; i++)
                {
                    bnum = Convert.ToString(Convert.ToInt32(number[i]), 2);
                    while (bnum.Length < 4)
                    {
                        bnum = "0" + bnum;
                    }
                    bchar += bnum;
                    while (bchar.Length >= 8)
                    {
                        bytes[0] = Convert.ToByte(bchar.Substring(0, 8), 2);
                        emessage += ByteConverter.ConvertToString(bytes);
                        bchar = bchar.Remove(0, 8);
                    }
                }
                   
            }
            if (bchar.Length != 0)
            {
                while (bchar.Length != 8)
                {
                    bchar += "0";
                }
                bytes[0] = Convert.ToByte(bchar, 2);
                emessage += ByteConverter.ConvertToString(bytes);
            }
            return emessage;
        }
        public string DecryptString(string cmessage, int N, int D)
        {
            n = N;
            d = D;
            string message = "";
            List<int> numbers= new List<int>(); 
            string nchar;
            byte[] bytes = new byte[1];
            string number = "";
            int cbyte = 0;
            int bbyte = 0;
            int maxl = Convert.ToInt32(ByteConverter.ConvertToByte(cmessage[0]));
            cmessage = cmessage.Remove(0, 1);
            foreach (var c in cmessage)
            {
               nchar = Convert.ToString(ByteConverter.ConvertToByte(c),2);
               while(nchar.Length < 8)
               {
                    nchar = "0" + nchar;
               }
                number += Convert.ToString(Convert.ToInt32(nchar.Substring(0, 4),2));
                nchar = nchar.Remove(0, 4);
                number += Convert.ToString(Convert.ToInt32(nchar, 2));
                if (number.Length >= maxl)
                {
                    numbers.Add(Convert.ToInt32(number.Substring(0, maxl)));
                }
            }
            foreach (var numb in numbers)
            {
                cbyte = numb;
                bbyte = numb;
                for (int j = 1; j < d; j++)
                {
                    cbyte = (bbyte * cbyte) % n;
                }
                bytes[0] = Convert.ToByte(cbyte);
                message += ByteConverter.ConvertToString(bytes);
            }
            return message;
        }
            public string EncryptFile(string keyPath, string filePath, string savingPath, string nombre)
            {
                return string.Empty;
            }
        }
    }

