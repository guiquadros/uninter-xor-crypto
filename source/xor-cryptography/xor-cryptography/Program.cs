//#define DEBUG_BIN_CONVERSION

using System;
using System.Collections.Generic;

namespace xor_cryptography
{
    public class Program
    {
        private const string TEXT_TO_ENCRYPT = "APROVADO";
        private const int RU = 3282910;
        
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            Console.WriteLine("Uninter - Matematica Computacional: AP Criptografia Simetrica com XOR");
            Console.WriteLine("Autor: Guilherme Quadros da Silva");
            Console.WriteLine();

            Console.WriteLine($"Encriptando '{TEXT_TO_ENCRYPT}' com o RU '{RU}'...");
            Console.WriteLine();
            
            Console.WriteLine($"Buscando valores de '{TEXT_TO_ENCRYPT}' na tabela ASCII:");
            Console.WriteLine("{character} = {valor ASCII em decimal} ({valor ASCII em binario})");
            List<string> binCharsListStr = new List<string>();
            foreach (char character in TEXT_TO_ENCRYPT)
            {
                int charAsDec = character;
                string charAsBinStr = ConvertDecToBinStr(charAsDec);
                WriteLineInDifferentColor($"'{character}' = '{charAsDec} (10)' e '{charAsBinStr} (2)'", ConsoleColor.Gray);
                
                binCharsListStr.Add(charAsBinStr);
            }

            string binTxtStr = string.Join(string.Empty, binCharsListStr);
            WriteLineInDifferentColor($"'{TEXT_TO_ENCRYPT}' = '{binTxtStr} (2)'", ConsoleColor.Red);
            
            ulong keyInDec = GetCriptoKeyFromRU(RU, binTxtStr);
            string keyInBinStr = ConvertDecToBinStr(keyInDec);
            WriteLineInDifferentColor($"Chave obtida = '{keyInBinStr} (2)'", ConsoleColor.Red);
            Console.WriteLine();

            // logic of the cryptography with XOR
            int diffKeyAndTxt = keyInBinStr.Length - binTxtStr.Length;

            if (diffKeyAndTxt > 0)
            {
                int diffWithPartSize = binCharsListStr[0].Length - diffKeyAndTxt;

                if (diffWithPartSize > 0)
                {
                    diffKeyAndTxt += diffWithPartSize;

                    for (int i = 0; i < diffWithPartSize; i++)
                    {
                        // complete key with "0" on the left
                        keyInBinStr = $"0{keyInBinStr}";
                    }
                }

                string strToConcat = string.Empty;
                for (int i = 0; i < diffKeyAndTxt; i++)
                {
                    // complete bin of the str with "0" on the left
                    strToConcat = $"0{strToConcat}";
                }
                
                binTxtStr = $"{strToConcat}{binTxtStr}";
                binCharsListStr.Insert(0, strToConcat);
            }
            
            Console.WriteLine("Criptografando com XOR:");
            string charXORSeparator = "    ";
            foreach (string binChar in binCharsListStr)
            {
                WriteInDifferentColor(charXORSeparator + binChar, ConsoleColor.Gray);
            }
            WriteInDifferentColor($"{charXORSeparator}('{TEXT_TO_ENCRYPT}')", ConsoleColor.Cyan);
            
            Console.WriteLine();
            WriteInDifferentColor("XOR ", ConsoleColor.Cyan);
            for (var i = 0; i < binCharsListStr.Count; i++)
            {
                string keyPart = keyInBinStr.Substring(i * binCharsListStr[i].Length, binCharsListStr[i].Length) + charXORSeparator;
                WriteInDifferentColor(keyPart, ConsoleColor.Gray);
            }
            WriteInDifferentColor($"('{keyInDec}')", ConsoleColor.Cyan);

            Console.WriteLine();
            Console.Write(charXORSeparator);
            for (var i = 0; i < binTxtStr.Length + binCharsListStr.Count * charXORSeparator.Length; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine();
            Console.Write(charXORSeparator);
            string encryptedTxt = string.Empty;
            for (int i = 0; i < binTxtStr.Length; i++)
            {
                int txtCharAsDec = binTxtStr[i];
                int keyCharAsDec = keyInBinStr[i];

                string xorResultStr = (txtCharAsDec ^ keyCharAsDec).ToString();
                encryptedTxt += xorResultStr;

                WriteInDifferentColor(xorResultStr, ConsoleColor.Red);
                
                if ((i + 1) % binCharsListStr[0].Length == 0)
                {
                    Console.Write(charXORSeparator);
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            WriteLineInDifferentColor("PRESSIONE QUALQUER TECLA PARA FECHAR A EXECUCAO DO PROGRAMA.", ConsoleColor.DarkBlue);
            Console.ReadKey();
        }

        /// <summary>
        /// Creates the cryptography key with a useful size (compared to the string that will be encrypted) based on RU.
        /// </summary>
        /// <param name="RU">Student RU.</param>
        /// <param name="binCharsListStr"></param>
        /// <returns>The cryptography key.</returns>
        private static ulong GetCriptoKeyFromRU(int RU, string binStr)
        {
            Console.WriteLine();
            Console.WriteLine($"Obtendo a chave de criptografia pelo RU '{RU}':");
            
            ulong keyInDec = (ulong)RU;
            string RUstr = RU.ToString();
            string RUinBinStr = ConvertDecToBinStr(RU);
            string keyInBinStr = RUinBinStr;
            Console.WriteLine($"'{keyInDec} (10)' = '{keyInBinStr} (2)'");

            int tryCount = 0;
            int RUcharPos = -1;
            
            // Repeats this until we find a proper key
            do
            {
                WriteLineInDifferentColor($"{++tryCount}) Comparando o tamanho da chave '{keyInDec} (10)' ('{keyInBinStr} (2)' [{keyInBinStr.Length} bits]) obtida com a string que vai ser criptografada '{TEXT_TO_ENCRYPT}' ('{binStr} (2)' [{binStr.Length} bits]).", ConsoleColor.Cyan);
                
                Console.Write($"    {keyInBinStr.Length} >= {binStr.Length}? --> ");
                if (keyInBinStr.Length >= binStr.Length)
                {
                    WriteInDifferentColor("Sim! ", ConsoleColor.Red);
                    break;
                }
                
                WriteLineInDifferentColor($"Nao! O tamanho da chave obtida nao e suficiente para criptografar com XOR a string '{TEXT_TO_ENCRYPT}'.", ConsoleColor.Blue);
                // concatenates the current key with the RU in the end to generate a bigger keys
                char nextRUDigit = GetNextDigitFromRU(RUstr, ref RUcharPos);
                keyInDec = ulong.Parse($"{keyInDec}{nextRUDigit}");
                Console.Write($"    Concatenando a chave com o proximo digito do RU ("); 
                WriteInDifferentColor($"'{keyInDec} (10)' + '{nextRUDigit} (10)'", ConsoleColor.Gray);
                Console.WriteLine($") para obter uma chave maior:");
                
                keyInBinStr = ConvertDecToBinStr(keyInDec);
                WriteLineInDifferentColor($"    '{keyInDec} (10)' = '{keyInBinStr} (2)'", ConsoleColor.Gray);
            } while (true);

            return keyInDec;
        }

        private static void WriteLineInDifferentColor(string message, ConsoleColor color)
        {
            ConsoleColor oldFgColor = Console.ForegroundColor;
            Console.ForegroundColor = color;    
            Console.WriteLine(message);
            Console.ForegroundColor = oldFgColor;
        }
        
        private static void WriteInDifferentColor(string message, ConsoleColor color)
        {
            ConsoleColor oldFgColor = Console.ForegroundColor;
            Console.ForegroundColor = color;    
            Console.Write(message);
            Console.ForegroundColor = oldFgColor;
        }

        /// <summary>
        /// Return the next digit position in the student's RU.
        /// </summary>
        /// <param name="RUstr">The student's RU.</param>
        /// <param name="digitPos">The digit used in last concatenation.</param>
        /// <returns></returns>
        private static char GetNextDigitFromRU(string RUstr, ref int digitPos)
        {
            if (digitPos < RUstr.Length - 1)
            {
                digitPos++;
            }
            else
            {
                digitPos = 0;
            }
            
            return RUstr[digitPos];
        }

        private static string ConvertDecToBinStr(int decNum)
        {
            return ConvertDecToBinStr((ulong) decNum);
        }

        private static string ConvertDecToBinStr(ulong decNum)
        {
#if DEBUG_BIN_CONVERSION
            Console.WriteLine($"Convertendo '{decNum}' para binario:");
#endif

            ulong result = decNum;
            string binResult = string.Empty;
            
            while (result > 1)
            {
#if DEBUG_BIN_CONVERSION
                Console.Write($"{result} divido por 2: ");
#endif
                
                ulong remaining = result % 2;
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
