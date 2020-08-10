using System;
using System.Collections.Generic;

namespace xor_cryptography
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Uninter - Matematica Computacional: AP Criptografia Simetrica com XOR");
            Console.WriteLine("Autor: Guilherme Quadros da Silva");
            Console.WriteLine();

            const string textToEncrypt = "APROVADO";
            const int keyInDec = 3282910;

            Console.WriteLine($"Encriptando '{textToEncrypt}' com a chave (RU) {keyInDec}...");
            Console.WriteLine();
            
            Console.WriteLine($"Buscando valores de '{textToEncrypt}' na tabela ASCII:");
            Console.WriteLine("{character} = {valor ASCII em decimal} ({valor ASCII em binario})");
            List<int> decCharsList = new List<int>();
            List<string> binCharsListStr = new List<string>();
            foreach (char character in textToEncrypt)
            {
                int charAsDec = character;
                string charAsBinStr = ConvertDecToBinStr(charAsDec);
                Console.WriteLine($"{character} = {charAsDec} ({charAsBinStr})");
                
                decCharsList.Add(charAsDec);
                binCharsListStr.Add(charAsBinStr);
            }
            
            Console.WriteLine();
            Console.WriteLine($"Convertendo a chave {keyInDec} para binario:");
            string keyInBinStr = ConvertDecToBinStr(keyInDec);
            Console.WriteLine($"{keyInDec} = {keyInBinStr}");
            Console.WriteLine();

            Console.WriteLine("Criptografando com XOR:");
            string charXORSeparator = "    ";
            foreach (string binChar in binCharsListStr)
            {
                Console.Write(charXORSeparator + binChar);
            }
            Console.WriteLine();
            Console.Write("XOR ");
            for (var i = 0; i < binCharsListStr.Count; i++)
            {
                //TODO: fix this. There are missing characters for the whole XOR operation. We are having an exception here due this missing characters.
                string keyPart = keyInBinStr.Substring(i * binCharsListStr[i].Length, binCharsListStr[i].Length);
                Console.Write(keyPart + charXORSeparator);
            }

            Console.ReadKey();
        }

        private static string ConvertDecToBinStr(int charAsDec)
        {
            //TODO: do manually the conversion from dec to bin.
            return Convert.ToString(charAsDec, 2);
        }
    }
}
