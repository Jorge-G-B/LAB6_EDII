using Encryptors.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Encryptors.Encryptors
{
    public class RSAEncryptor : IEncryptor
    {
        #region Variables
        int n = 0;
        int fi = 0;
        int P = 0;
        int Q = 0;
        int e = 0;
        int d = 0;
        int[] PrimeNumbers = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
        public void SetVariables(int p, int q)
        {
            P = p;
            Q = q;
            n = P * Q;
            fi = (P - 1) * (Q - 1);
            Random rnd = new Random();
            e = PrimeNumbers[rnd.Next(0, 25)];
            while((e > fi) || (fi%e == 0))
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
                X = k - ((k/X) * X);
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
            string emessage = "";
            byte[] chara = new byte[1];
            foreach (var c in message)
            {
                chara[0]= Convert.ToByte((Convert.ToInt32(Convert.ToByte(c))^ e) % n);
                emessage += Convert.ToBase64String(chara);
            }
            return emessage;
        }
        public string DecryptString(string cmessage)
        {
            string message = "";
            byte[] chara = new byte[1];
            foreach (var c in cmessage)
            {
                chara[0] = Convert.ToByte((Convert.ToInt32(Convert.ToByte(c)) ^ d) % n);
                message += Convert.ToBase64String(chara);
            }
            return message;
        }


        public string EncryptFile(string keyPath, string filePath, string savingPath, string nombre)
        {
            return string.Empty;
        }
    }
}
