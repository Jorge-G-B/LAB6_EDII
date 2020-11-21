using System;
using Encryptors;
using Encryptors.Encryptors;

namespace StringEncryptor
{
    class Program
    {
        static void Main(string[] args)
        {
            RSAEncryptor rSA = new RSAEncryptor();
            Console.WriteLine("Escribe el string a cifrar");
            string text = Console.ReadLine();
            Console.WriteLine("Escribe la llave p");
            string p = Console.ReadLine();
            Console.WriteLine("Escribe la llave q");
            string q = Console.ReadLine();
            Console.WriteLine("Se ha guardado el string con éxito para cifrar");
            Console.WriteLine("El resultado del cifrado es el siguiente:");
            text = rSA.EncryptString(Convert.ToInt32(p), Convert.ToInt32(q), text);
            Console.WriteLine(text);
            Console.WriteLine("¿Desea descifrarlo? | Presione 'Y'. De lo contrario, presione cualquier otra tecla.");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                Console.Clear();
                Console.WriteLine("El resultado del descifrado es el siguiente:");
                Console.WriteLine(rSA.DecryptString(text));
                Console.ReadLine();
            }
            Console.WriteLine("Feliz día!");
            Console.ReadLine();
        }
    }
}
