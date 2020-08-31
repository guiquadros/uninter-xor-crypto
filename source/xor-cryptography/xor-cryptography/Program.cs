//#define DEBUG_BIN_CONVERSION

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace xor_cryptography
{
    public class Program
    {
        private const int BIN_PART_SIZE = 7;
        
        public static void Main(string[] args)
        {
            const string TEXT_TO_ENCRYPT = "APROVADO";
            const int RU = 3282910;
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            Console.WriteLine("Uninter - Matematica Computacional: AP Criptografia Simetrica com XOR");
            Console.WriteLine("Autor: Guilherme Quadros da Silva");
            Console.WriteLine();

            // encryption
            string encryptedString = EncryptStringXor(TEXT_TO_ENCRYPT, RU);
            Console.WriteLine();
            Console.WriteLine();
            WriteInDifferentColor($"'{TEXT_TO_ENCRYPT}' ", ConsoleColor.Cyan);
            WriteInDifferentColor($"foi criptografado para: ", ConsoleColor.Red);
            WriteLineInDifferentColor($"'{encryptedString}'", ConsoleColor.Cyan);

            // decryption
            // TODO: check if just calling the same method, but with the encrypted string, will work.
            string decryptedString = EncryptStringXor(encryptedString, RU);
            Console.WriteLine();
            WriteInDifferentColor($"'{encryptedString}' ", ConsoleColor.Cyan);
            WriteInDifferentColor($"foi descriptografado para: ", ConsoleColor.Red);
            WriteLineInDifferentColor($"'{decryptedString}'", ConsoleColor.Cyan);

            Console.WriteLine();
            Console.WriteLine();
            WriteLineInDifferentColor("PRESSIONE QUALQUER TECLA PARA FECHAR A EXECUCAO DO PROGRAMA.", ConsoleColor.DarkBlue);
            Console.ReadKey();
        }
        
        private static string EncryptStringXor(string textToEncrypt, int RU)
        {
            if (string.IsNullOrEmpty(textToEncrypt)) return string.Empty;
            
            Console.WriteLine($"Encriptando '{textToEncrypt}' com o RU '{RU}'...");
            Console.WriteLine();
            
            Console.WriteLine($"Buscando valores de '{textToEncrypt}' na tabela ASCII:");
            Console.WriteLine("{character} = {valor ASCII em decimal} ({valor ASCII em binario})");
            List<string> binCharsListStr = new List<string>();
            foreach (char character in textToEncrypt)
            {
                int charAsDec = character;
                string charAsBinStr = ConvertDecToBinStr(charAsDec);
                WriteLineInDifferentColor($"'{character}' = '{charAsDec} (10)' e '{charAsBinStr} (2)'", ConsoleColor.Gray);
                
                binCharsListStr.Add(charAsBinStr);
            }

            string binTxtStr = string.Join(string.Empty, binCharsListStr);
            WriteLineInDifferentColor($"'{textToEncrypt}' = '{binTxtStr} (2)'", ConsoleColor.Red);
            
            ulong keyInDec = GetCriptoKeyFromRU(RU, binTxtStr, textToEncrypt);
            string keyInBinStr = ConvertDecToBinStr(keyInDec);
            WriteLineInDifferentColor($"Chave obtida = '{keyInBinStr} (2)'", ConsoleColor.Red);
            Console.WriteLine();

            int diffKeyAndTxt = keyInBinStr.Length - binTxtStr.Length;
            
            // adding additional "0" in the key
            if (diffKeyAndTxt > 0)
            {
                int diffWithPartSize = BIN_PART_SIZE - diffKeyAndTxt;
            
                if (diffWithPartSize > 0)
                {
                    diffKeyAndTxt += diffWithPartSize;

                    // complete key with "0" on the left
                    keyInBinStr = keyInBinStr.PadLeft(keyInBinStr.Length + diffWithPartSize, '0');
                }
            }
            
            // encryption logic started
            Console.WriteLine("Criptografando com XOR:");
            const string XOR_SEPARATOR = "    ";

            for (int i = 0; i < diffKeyAndTxt; i++)
            {
                Console.Write(" ");
            }
            Console.Write(XOR_SEPARATOR);
            foreach (string binChar in binCharsListStr)
            {
                WriteInDifferentColor(XOR_SEPARATOR + binChar, ConsoleColor.Gray);
            }
            WriteInDifferentColor($"{XOR_SEPARATOR}('{textToEncrypt}')", ConsoleColor.Cyan);
            
            Console.WriteLine();
            WriteInDifferentColor("XOR ", ConsoleColor.Cyan);
            for (var i = 0; i < binCharsListStr.Count; i++)
            {
                string keyPart = keyInBinStr.Substring((i * binCharsListStr[i].Length) + diffKeyAndTxt, binCharsListStr[i].Length) + XOR_SEPARATOR;

                if (i == 0 && diffKeyAndTxt > 0)
                {
                    string exceededKeyPart = keyInBinStr.Substring(0, diffKeyAndTxt);
                    WriteInDifferentColor(exceededKeyPart, ConsoleColor.DarkBlue);
                    Console.Write(XOR_SEPARATOR);
                }
                
                WriteInDifferentColor(keyPart, ConsoleColor.Gray);
            }
            WriteInDifferentColor($"('{keyInDec}')", ConsoleColor.Cyan);

            Console.WriteLine();
            Console.Write(XOR_SEPARATOR);
            for (var i = 0; i < binTxtStr.Length + (binCharsListStr.Count * XOR_SEPARATOR.Length) + diffKeyAndTxt + XOR_SEPARATOR.Length; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine();
            Console.Write(XOR_SEPARATOR);
            if (diffKeyAndTxt > 0)
            {
                for (int i = 0; i < diffKeyAndTxt; i++)
                {
                    Console.Write(" ");
                }
                
                Console.Write(XOR_SEPARATOR);
            }
            
            // the actual encryption 
            string encryptedBinTxt = string.Empty;
            for (int i = binTxtStr.Length - 1; i >= 0; i--)
            {
                int txtCharAsDec = int.Parse(binTxtStr[i].ToString());
                int keyCharAsDec = int.Parse(keyInBinStr[i + diffKeyAndTxt].ToString());

                string xorResultStr = (txtCharAsDec ^ keyCharAsDec).ToString();
                encryptedBinTxt = $"{xorResultStr}{encryptedBinTxt}";
            }

            string encryptedTxt = string.Empty;
            string characterInBin = string.Empty;
            // write down the result
            for (int i = 0; i < encryptedBinTxt.Length; i++)
            {
                characterInBin = $"{characterInBin}{encryptedBinTxt[i]}";

                if ((i + 1) % BIN_PART_SIZE == 0)
                {
                    WriteInDifferentColor($"{characterInBin}", ConsoleColor.Red);

                    int characterInDec = ConvertBinToDec(characterInBin);
                    char character = (char) characterInDec;
                    // Console.WriteLine($"'{characterInBin} (2)' = '{characterInDec} (10)' = '{character}' = '{character}'");
                    
                    encryptedTxt = $"{encryptedTxt}{character}";
                    
                    Console.Write(XOR_SEPARATOR);
                    characterInBin = string.Empty;
                }
            }
            WriteInDifferentColor($"('{encryptedTxt}')", ConsoleColor.Cyan);

            // TODO: convert back from txt bin to txt and print the encrypted string ('xxxxxx') and return it
            return encryptedTxt;
        }

        /// <summary>
        /// Creates the cryptography key with a useful size (compared to the string that will be encrypted) based on RU.
        /// </summary>
        /// <param name="RU">Student RU.</param>
        /// <param name="binStr"></param>
        /// <param name="textToEncrypt"></param>
        /// <param name="binCharsListStr"></param>
        /// <returns>The cryptography key.</returns>
        private static ulong GetCriptoKeyFromRU(int RU, string binStr, string textToEncrypt)
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
                WriteInDifferentColor($"{++tryCount}) Comparando o tamanho da chave '{keyInDec} (10)' ('{keyInBinStr} (2)' ", ConsoleColor.Cyan);
                WriteInDifferentColor($"[{keyInBinStr.Length} bits]", ConsoleColor.Yellow); 
                WriteInDifferentColor($") obtida com a string que vai ser criptografada '{textToEncrypt}' ('{binStr} (2)' ", ConsoleColor.Cyan);
                WriteInDifferentColor($"[{binStr.Length} bits]", ConsoleColor.Yellow);
                WriteLineInDifferentColor($").", ConsoleColor.Cyan);
                
                Console.Write($"    {keyInBinStr.Length} >= {binStr.Length}? --> ");
                if (keyInBinStr.Length >= binStr.Length)
                {
                    WriteInDifferentColor("Sim! ", ConsoleColor.Red);
                    break;
                }
                
                WriteLineInDifferentColor($"Nao! O tamanho da chave obtida nao e suficiente para criptografar com XOR a string '{textToEncrypt}'.", ConsoleColor.Blue);
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

            binResult = binResult.PadLeft(BIN_PART_SIZE, '0');
        
#if DEBUG_BIN_CONVERSION
            Console.WriteLine($"'{decNum}' em binario = '{binResult}'");
#endif

            return binResult;
        }

        private static int ConvertBinToDec(string binNum)
        {
            //TODO: implement my own bin to dec algorithm
            return Convert.ToInt32(binNum, 2);
        }
    }
}
