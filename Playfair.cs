using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace DezvoltareaAplicatii
{
    internal class Playfair
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool ReadConsoleW(IntPtr hConsoleInput, [Out] char[] lpBuffer,
            uint nNumberOfCharsToRead, out uint lpNumberOfCharsRead, IntPtr pInputControl);

        private string ReadLineUnicode()
        {
            try
            {
                IntPtr h = GetStdHandle(-10); // STD_INPUT_HANDLE
                char[] buf = new char[1024];
                if (ReadConsoleW(h, buf, (uint)buf.Length, out uint read, IntPtr.Zero) && read > 0)
                    return new string(buf, 0, (int)read).TrimEnd('\r', '\n');
            }
            catch { }
            return ReadLineUnicode(); // rezervă, dacă intrarea e redirecționată
        }
        public void Start()
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            while (true)
            {
                Console.WriteLine("1. Encrypt");
                Console.WriteLine("2. Decrypt");
                Console.WriteLine("3. Exit");
                if (!int.TryParse(ReadLineUnicode(), out int opt))
                {
                    Console.WriteLine("Introdu un număr valid.");
                    continue;
                }

                switch (opt)
                {
                    case 1:
                        PlayfairEncrypt();
                        break;
                    case 2:
                        PlayfairDecrypt();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Optiune incorecta");
                        break;
                }
            }
        }

        private bool CheckText(string text)
        {
            if(string.IsNullOrEmpty(text)) return false;
                      
            string pattern = "^[A-Za-zăâîșțĂÂÎȘȚşţŞŢ ]+$";
            return Regex.IsMatch(text, pattern);

        }

        private string[] PrepareText(string text, char l)
        {
            {
                text = text.ToUpper()
                          .Replace(" ", "")
                          .Replace("W", "V")
                          .Replace('Ş', 'Ș')
                          .Replace('Ţ', 'Ț')
                          .Replace('ş', 'Ș')
                          .Replace('ţ', 'Ț');
                
                List<string> formatedText = new List<string>();

                if (l == 'e')
                {
                    bool useQ = true;
                    for (int i = 0; i < text.Length; i += 2)
                    {
                        char first = text[i];
                        if (i + 1 == text.Length)
                        {
                            formatedText.Add($"{first}Q");
                        }
                        else if (first == text[i + 1])
                        {
                            char filler = useQ ? 'Q' : 'Y';
                            if (filler == first) filler = useQ ? 'Y' : 'Q';
                            useQ = !useQ;
                            formatedText.Add($"{first}{filler}");
                            i--;
                        }
                        else
                        {
                            formatedText.Add($"{first}{text[i + 1]}");
                        }
                    }
                }

                else
                {

                    for (int i = 0; i < text.Length; i += 2)
                    {
                        if (i + 1 < text.Length)
                        {
                            formatedText.Add($"{text[i]}{text[i + 1]}");
                        }
                        else
                        {
                            formatedText.Add($"{text[i]}Q");
                        }
                    }
                }

                return formatedText.ToArray();

            }
        }

        private char[,] GenerateMatrix(string key)
        {
            string alphabet = "AĂÂBCDEFGHIÎJKLMNOPQRSȘTȚUVXYZ";
            key = key.Replace(" ", "").Replace("W","V");

            string combinedText = key + alphabet;
            List<char> uniqueLetters = combinedText.Distinct().ToList();

            char[,] matrix = new char[5, 6];

            int index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    matrix[i,j] = uniqueLetters[index];
                    index++;
                }
            }
            return matrix;
        }

        private (int line, int col) GetCoordonates(char[,] matrix, char letter)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (letter == matrix[i, j])
                    {
                        return (i, j);
                    }
                }
            }
            return(0, 0);
        }

        private string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == 'Q' || text[i] == 'Y')
                {
                    if (i > 0 && i < text.Length - 1 && text[i + 1] == text[i - 1])
                    {
                        continue;
                    }

                    if (text[i] == 'Q' && i == text.Length - 1)
                    {
                        continue;
                    }
                }

                result += text[i];
            }
            return result;
        }

        public void PlayfairEncrypt()
        {
            Console.WriteLine("Introduceti cheia: ");
            string key = ReadLineUnicode();
            key = key.ToUpper().Replace(" ", "").Replace("W", "V")
                    .Replace('Ş', 'Ș').Replace('Ţ', 'Ț');
            key = Regex.Replace(key, @"[\p{P}\s]", "");
            if (!CheckText(key.ToUpper().Trim()) || key.Length < 7)
            {
                Console.WriteLine("Cheia trebuie sa fie in alfabetul roman(A-Za-zăâîșțĂÂÎȘȚşţŞŢ) si minim de 7 litere");
                ReadLineUnicode();
                return;
            }
           

            Console.WriteLine("Introduceti textul: ");
            string message = ReadLineUnicode();
            message = Regex.Replace(message, @"[\p{P}\s]", "");

            if (!CheckText(message.ToUpper().Trim()))
            {
                Console.WriteLine("Textul trebuie sa fie in alfabetul roman(A-Za-zăâîșțĂÂÎȘȚşţŞŢ)");
                ReadLineUnicode();
                return;
            }
            
            string[] formatedMessage = PrepareText(message, 'e');
            char[,] matrix = GenerateMatrix(key);
            string result = "";

            foreach (string m in formatedMessage)
            {
                var pos1 = GetCoordonates(matrix, m[0]);
                var pos2 = GetCoordonates(matrix, m[1]);
                int l1 = pos1.line;
                int l2 = pos2.line;
                int c1 = pos1.col; 
                int c2 = pos2.col;

                if (l1 == l2)
                {
                    if (c1 == 5)  c1 = 0;
                    else c1++;

                    if (c2 == 5)  c2 = 0;
                    
                    else c2++;
                }
                else if (c1 == c2)
                {
                    if (l1 == 4) l1 = 0;
                    else l1++;

                    if(l2 == 4) l2 = 0; 
                    else l2++;
                }
                else
                {
                    int temp = c1;
                    c1 = c2;
                    c2 = temp;
                }

                result += $"{matrix[l1, c1]}{matrix[l2, c2]}";
            }

            Console.WriteLine("Mesajul criptat:");
            Console.WriteLine(result);

        }
        public void PlayfairDecrypt()
        {
            Console.WriteLine("Introduceti cheia: ");
            string key = ReadLineUnicode();
            key = key.ToUpper().Replace(" ", "").Replace("W", "V")
                    .Replace('Ş', 'Ș').Replace('Ţ', 'Ț');
            key = Regex.Replace(key, @"[\p{P}\s]", "");
            if (!CheckText(key.ToUpper().Trim()) || key.Length < 7)
            {
                Console.WriteLine("Cheia trebuie sa fie in alfabetul roman(A-Za-zăâîșțĂÂÎȘȚşţŞŢ) si minim de 7 litere");
                ReadLineUnicode();
                return;
            }
            

            Console.WriteLine("Introduceti textul: ");
            string message = ReadLineUnicode();
            Console.WriteLine(message.Length);
            foreach (char c in message) Console.Write((int)c + " ");
            message = Regex.Replace(message, @"[\p{P}\s]", "");
            

            if (!CheckText(message.ToUpper().Trim()))
            {
                Console.WriteLine("Textul trebuie sa fie in alfabetul roman(A-Za-zăâîșțĂÂÎȘȚşţŞŢ)");
                ReadLineUnicode();
                return;
            }
            if (message.Replace(" ", "").Length % 2 != 0)
            {
                Console.WriteLine("Criptograma trebuie sa aiba numar par de litere");
                return;
            }
            

            string[] formatedMessage = PrepareText(message, 'd');
            char[,] matrix = GenerateMatrix(key);
            string result = "";

            foreach (string m in formatedMessage)
            {
                var pos1 = GetCoordonates(matrix, m[0]);
                var pos2 = GetCoordonates(matrix, m[1]);
                int l1 = pos1.line;
                int l2 = pos2.line;
                int c1 = pos1.col;
                int c2 = pos2.col;

                if (l1 == l2)
                {
                    if (c1 == 0) c1 = 5;
                    else c1--;

                    if (c2 == 0) c2 = 5;

                    else c2--;
                }
                else if (c1 == c2)
                {
                    if (l1 == 0) l1 = 4;
                    else l1--;

                    if (l2 == 0) l2 = 4;
                    else l2--;
                }
                else
                {
                    int temp = c1;
                    c1 = c2;
                    c2 = temp;
                }

                result += $"{matrix[l1, c1]}{matrix[l2, c2]}";
            }
            result = CleanText(result);
            Console.WriteLine("Mesajul decriptat:");
            Console.WriteLine(result);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var Playfair = new Playfair();

            Playfair.Start();
        }


    }
}
