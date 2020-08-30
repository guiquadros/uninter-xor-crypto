//#define DEBUG_BIN_CONVERSION

using System;
using System.Collections.Generic;

namespace xor_cryptography
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            
            Console.WriteLine("Uninter - Matematica Computacional: AP Criptografia Simetrica com XOR");
            Console.WriteLine("Autor: Guilherme Quadros da Silva");
            Console.WriteLine();

            const string TEXT_TO_ENCRYPT = "APROVADO";
            const int RU = 3282910;

            Console.WriteLine($"Encriptando '{TEXT_TO_ENCRYPT}' com o RU '{RU}'...");
            Console.WriteLine();
            
            Console.WriteLine($"Buscando valores de '{TEXT_TO_ENCRYPT}' na tabela ASCII:");
            Console.WriteLine("{character} = {valor ASCII em decimal} ({valor ASCII em binario})");
            List<int> decCharsList = new List<int>();
            List<string> binCharsListStr = new List<string>();
            foreach (char character in TEXT_TO_ENCRYPT)
            {
                int charAsDec = character;
                string charAsBinStr = ConvertDecToBinStr(charAsDec);
                Console.WriteLine($"'{character}' = '{charAsDec} (10)' e '{charAsBinStr} (2)'");
                
                decCharsList.Add(charAsDec);
                binCharsListStr.Add(charAsBinStr);
            }

            string binStr = string.Join(string.Empty, binCharsListStr);
            Console.WriteLine($"'{TEXT_TO_ENCRYPT}' = '{binStr} (2)'");
            string keyInBinStr = GetCriptoKeyFromRU(RU, binStr);

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

        /// <summary>
        /// Creates the cryptography key with a useful size (compared to the string that will be encrypted) based on RU.
        /// </summary>
        /// <param name="RU">Student RU.</param>
        /// <param name="binCharsListStr"></param>
        /// <returns>The cryptography key.</returns>
        private static string GetCriptoKeyFromRU(int RU, string binStr)
        {
            ulong test = 32829103282910328;
            Console.WriteLine(test);

            Console.WriteLine();
            Console.WriteLine($"Obtendo a chave de criptografia pelo RU '{RU}':");
            
            int keyInDec = RU;
            string RUinBinStr = ConvertDecToBinStr(RU);
            
            string keyInBinStr = RUinBinStr;
            Console.WriteLine($"'{keyInDec} (10)' = '{keyInBinStr} (2)'");

            int tryCount = 0;
            // Repeats this until we find a proper key
            do
            {
                Console.WriteLine($"{++tryCount}) Comparando o tamanho da chave '{keyInBinStr} (2)' ({keyInBinStr.Length} bits) obtida com a string que vai ser criptografada '{binStr} (2)' ({binStr.Length} bits).");
                
                Console.Write($"    {keyInBinStr.Length} >= {binStr.Length}? --> ");
                if (keyInBinStr.Length >= binStr.Length)
                {
                    Console.WriteLine("Sim!");
                    break;
                }
                
                Console.WriteLine("Nao!");
                // concatenates the current key with the RU in the end to generate a bigger keys
                Console.Write($"    Concatenando a chave '{keyInDec} (10)' ('{keyInBinStr} (2)') com o RU '{RU} (10)' ('{RUinBinStr} (2)') para obter uma chave maior que possa criptografar corretamente a string: ");
                keyInBinStr += RUinBinStr;
                Console.WriteLine($"'{keyInBinStr} (2)'");
            } while (true);

            ConsoleColor oldFgColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;    
            Console.WriteLine($"Chave obtida = '{keyInBinStr} (2)'");
            Console.ForegroundColor = oldFgColor;
            Console.WriteLine();
            return keyInBinStr;
        }
        
        private static string ConvertDecToBinStr(int decNum)
        {
#if DEBUG_BIN_CONVERSION
            Console.WriteLine($"Convertendo '{decNum}' para binario:");
#endif

            int result = decNum;
            string binResult = string.Empty;
            
            while (result > 1)
            {
#if DEBUG_BIN_CONVERSION
                Console.Write($"{result} divido por 2: ");
#endif
                
                int remaining = result % 2;
                binResult = $"{remaining}{binResult}"; 
                result /= 2;
                
#if DEBUG_BIN_CONVERSION
                Console.WriteLine($"quociente = {result}; resto = {remaining}");
#endif
            }
            
            binResult = $"{result}{binResult}"; 
        
#if DEBUG_BIN_CONVERSION
            Console.WriteLine($"'{decNum}' em binario = '{binResult}'");
#endif

            return binResult;
        }
    }
}
