using Encryptors.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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
        int[] PrimeNumbers = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97};
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
            int cbyte = 0;
            int bbyte = 0;
            foreach (var c in message)
            {
                cbyte = Convert.ToInt32(Convert.ToByte(c));
                bbyte = cbyte;
                for (int i = 0; i < e; i++)
                {
                    cbyte = (cbyte * bbyte)% n;
                }
                //1992 -> 1 - 9 - 9 - 2
                chara[0] = Convert.ToByte(cbyte);
                emessage += Convert.ToBase64String(chara);
            }
            return emessage;
        }
        public string DecryptString(string cmessage, int N, int D)
        {
            n = N;
            d = D;
            string message = "";
            byte[] chara = new byte[1]; 
            int cbyte = 0;
            int bbyte = 0;
            for (int i = 0; i < cmessage.Length; i++)
            {
                cbyte = Convert.ToInt32(Convert.ToByte(cmessage[i]));
                while (cmessage[i] == 255)
                {
                    cbyte += 255; 
                    i++;
                }
                cbyte += message[i];
                bbyte = cbyte;
                i++;
                for (int j = 0; j < d; j++)
                {
                    cbyte = (bbyte * cbyte) % n;
                }
                chara[0] = Convert.ToByte(cbyte);
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
