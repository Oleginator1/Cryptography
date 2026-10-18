using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DezvoltareaAplicatii
{  
      
    public static class DesTables
    {
        // PC-1: 64-bit key -> 56 bits (drops the 8 parity bits)
        public static readonly int[,] PC1 = new int[8, 7]
        {
        { 57, 49, 41, 33, 25, 17,  9 },
        {  1, 58, 50, 42, 34, 26, 18 },
        { 10,  2, 59, 51, 43, 35, 27 },
        { 19, 11,  3, 60, 52, 44, 36 },
        { 63, 55, 47, 39, 31, 23, 15 },
        {  7, 62, 54, 46, 38, 30, 22 },
        { 14,  6, 61, 53, 45, 37, 29 },
        { 21, 13,  5, 28, 20, 12,  4 },
        };

        // PC-2: 56 bits (C+D) -> 48-bit round key
        public static readonly int[,] PC2 = new int[8, 6]
        {
        { 14, 17, 11, 24,  1,  5 },
        {  3, 28, 15,  6, 21, 10 },
        { 23, 19, 12,  4, 26,  8 },
        { 16,  7, 27, 20, 13,  2 },
        { 41, 52, 31, 37, 47, 55 },
        { 30, 40, 51, 45, 33, 48 },
        { 44, 49, 39, 56, 34, 53 },
        { 46, 42, 50, 36, 29, 32 },
        };

        // Left shifts of C and D for rounds 1..16
        public static readonly int[] Shifts = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

        // Initial permutation (64 -> 64)
        public static readonly int[,] IP = new int[8, 8]
        {
        { 58, 50, 42, 34, 26, 18, 10,  2 },
        { 60, 52, 44, 36, 28, 20, 12,  4 },
        { 62, 54, 46, 38, 30, 22, 14,  6 },
        { 64, 56, 48, 40, 32, 24, 16,  8 },
        { 57, 49, 41, 33, 25, 17,  9,  1 },
        { 59, 51, 43, 35, 27, 19, 11,  3 },
        { 61, 53, 45, 37, 29, 21, 13,  5 },
        { 63, 55, 47, 39, 31, 23, 15,  7 },
        };

        // Final permutation, the inverse of IP (64 -> 64)
        public static readonly int[,] IPInverse = new int[8, 8]
        {
        { 40,  8, 48, 16, 56, 24, 64, 32 },
        { 39,  7, 47, 15, 55, 23, 63, 31 },
        { 38,  6, 46, 14, 54, 22, 62, 30 },
        { 37,  5, 45, 13, 53, 21, 61, 29 },
        { 36,  4, 44, 12, 52, 20, 60, 28 },
        { 35,  3, 43, 11, 51, 19, 59, 27 },
        { 34,  2, 42, 10, 50, 18, 58, 26 },
        { 33,  1, 41,  9, 49, 17, 57, 25 },
        };

        // Expansion: 32 bits -> 48 bits
        public static readonly int[,] E = new int[8, 6]
        {
        { 32,  1,  2,  3,  4,  5 },
        {  4,  5,  6,  7,  8,  9 },
        {  8,  9, 10, 11, 12, 13 },
        { 12, 13, 14, 15, 16, 17 },
        { 16, 17, 18, 19, 20, 21 },
        { 20, 21, 22, 23, 24, 25 },
        { 24, 25, 26, 27, 28, 29 },
        { 28, 29, 30, 31, 32,  1 },
        };

        // P permutation after the S-boxes (32 -> 32)
        public static readonly int[,] P = new int[8, 4]
        {
        { 16,  7, 20, 21 },
        { 29, 12, 28, 17 },
        {  1, 15, 23, 26 },
        {  5, 18, 31, 10 },
        {  2,  8, 24, 14 },
        { 32, 27,  3,  9 },
        { 19, 13, 30,  6 },
        { 22, 11,  4, 25 },
        };

        // S-boxes: SBoxes[box, row, column], box 0..7 = S1..S8, row 0..3, column 0..15
        public static readonly int[,,] SBoxes = new int[8, 4, 16]
        {
        // S1
        {
            { 14,  4, 13,  1,  2, 15, 11,  8,  3, 10,  6, 12,  5,  9,  0,  7 },
            {  0, 15,  7,  4, 14,  2, 13,  1, 10,  6, 12, 11,  9,  5,  3,  8 },
            {  4,  1, 14,  8, 13,  6,  2, 11, 15, 12,  9,  7,  3, 10,  5,  0 },
            { 15, 12,  8,  2,  4,  9,  1,  7,  5, 11,  3, 14, 10,  0,  6, 13 },
        },
        // S2
        {
            { 15,  1,  8, 14,  6, 11,  3,  4,  9,  7,  2, 13, 12,  0,  5, 10 },
            {  3, 13,  4,  7, 15,  2,  8, 14, 12,  0,  1, 10,  6,  9, 11,  5 },
            {  0, 14,  7, 11, 10,  4, 13,  1,  5,  8, 12,  6,  9,  3,  2, 15 },
            { 13,  8, 10,  1,  3, 15,  4,  2, 11,  6,  7, 12,  0,  5, 14,  9 },
        },
        // S3
        {
            { 10,  0,  9, 14,  6,  3, 15,  5,  1, 13, 12,  7, 11,  4,  2,  8 },
            { 13,  7,  0,  9,  3,  4,  6, 10,  2,  8,  5, 14, 12, 11, 15,  1 },
            { 13,  6,  4,  9,  8, 15,  3,  0, 11,  1,  2, 12,  5, 10, 14,  7 },
            {  1, 10, 13,  0,  6,  9,  8,  7,  4, 15, 14,  3, 11,  5,  2, 12 },
        },
        // S4
        {
            {  7, 13, 14,  3,  0,  6,  9, 10,  1,  2,  8,  5, 11, 12,  4, 15 },
            { 13,  8, 11,  5,  6, 15,  0,  3,  4,  7,  2, 12,  1, 10, 14,  9 },
            { 10,  6,  9,  0, 12, 11,  7, 13, 15,  1,  3, 14,  5,  2,  8,  4 },
            {  3, 15,  0,  6, 10,  1, 13,  8,  9,  4,  5, 11, 12,  7,  2, 14 },
        },
        // S5
        {
            {  2, 12,  4,  1,  7, 10, 11,  6,  8,  5,  3, 15, 13,  0, 14,  9 },
            { 14, 11,  2, 12,  4,  7, 13,  1,  5,  0, 15, 10,  3,  9,  8,  6 },
            {  4,  2,  1, 11, 10, 13,  7,  8, 15,  9, 12,  5,  6,  3,  0, 14 },
            { 11,  8, 12,  7,  1, 14,  2, 13,  6, 15,  0,  9, 10,  4,  5,  3 },
        },
        // S6
        {
            { 12,  1, 10, 15,  9,  2,  6,  8,  0, 13,  3,  4, 14,  7,  5, 11 },
            { 10, 15,  4,  2,  7, 12,  9,  5,  6,  1, 13, 14,  0, 11,  3,  8 },
            {  9, 14, 15,  5,  2,  8, 12,  3,  7,  0,  4, 10,  1, 13, 11,  6 },
            {  4,  3,  2, 12,  9,  5, 15, 10, 11, 14,  1,  7,  6,  0,  8, 13 },
        },
        // S7
        {
            {  4, 11,  2, 14, 15,  0,  8, 13,  3, 12,  9,  7,  5, 10,  6,  1 },
            { 13,  0, 11,  7,  4,  9,  1, 10, 14,  3,  5, 12,  2, 15,  8,  6 },
            {  1,  4, 11, 13, 12,  3,  7, 14, 10, 15,  6,  8,  0,  5,  9,  2 },
            {  6, 11, 13,  8,  1,  4, 10,  7,  9,  5,  0, 15, 14,  2,  3, 12 },
        },
        // S8
        {
            { 13,  2,  8,  4,  6, 15, 11,  1, 10,  9,  3, 14,  5,  0, 12,  7 },
            {  1, 15, 13,  8, 10,  3,  7,  4, 12,  5,  6, 11,  0, 14,  9,  2 },
            {  7, 11,  4,  1,  9, 12, 14,  2,  0,  6, 10, 13, 15,  3,  5,  8 },
            {  2,  1, 14,  7,  4, 10,  8, 13, 15, 12,  9,  0,  3,  5,  6, 11 },
        },
        };

        /// <summary>Flattens a matrix table into the 1-D list that Permute() expects.</summary>
        public static int[] Flatten(int[,] table)
        {
            var flat = new int[table.Length];
            int k = 0;
            foreach (int v in table) flat[k++] = v;   // row by row, left to right
            return flat;
        }
    }


    internal class DESCifer
    {
        public void Start()
        {
            while (true)
            {
                Console.WriteLine("1. Encrypt");
                Console.WriteLine("2. Decrypt");
                Console.WriteLine("3. Exit");
                if (!int.TryParse(Console.ReadLine(), out int opt))
                {
                    Console.WriteLine("Wrong option!");
                    continue;
                }

                switch (opt)
                {
                    case 1:
                        DesEncrypt();
                        break;
                    case 2:
                        //DesDecrypt();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Wrong option");
                        break;
                }

            }
        }


        private bool TryParseKey(string key, out byte[] result)
        {   
            result = Array.Empty<byte>();

            if(string.IsNullOrEmpty(key)) return false;

            if (key.Length == 16 && key.All(Uri.IsHexDigit))
            {
                result = Convert.FromHexString(key);
                return true;
            }

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(key);
            if (bytes.Length != 8)
                return false;
            
            result = bytes;
            return true;
            /*try
            {
                result = Convert.FromHexString(key);
            }
            catch (FormatException)
            {
                result = System.Text.Encoding.UTF8.GetBytes(key);
            }
            return result;*/
        }

        private int[] BytesToBits(byte[] data)
        {
            var bits = new int[data.Length * 8];
            for (int i = 0; i < data.Length; i++)
            {
                for (int b = 0; b < 8; b++)
                {
                    bits[i * 8 + b] = (data[i] >> (7 - b)) & 1;
                }
            }
            return bits;
        }

        private int[] Permute(int[] bits, int[,] table)
        {
            int rows = table.GetLength(0);
            int cols = table.GetLength(1);
            var output = new int[rows * cols];
            int counter = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int num = table[i,j];
                    output[counter] = bits[num - 1];
                    counter++;
                }
            }
            return output;

        }

        private List<int[]> GenerateSubKeys(int[] C_Zero, int[] D_Zero)
        {            
            var c = C_Zero.ToList();            
            var d = D_Zero.ToList();
            List<int[]> subkeys = new List<int[]>();

            for (int i = 0; i < 16; i++)
            {
                /*
                if (DesTables.Shifts[i] == 1)
                {
                    var temp1 = c[0];
                    c.RemoveAt(0);
                    c.Add(temp1);

                    var temp2 = d[0];
                    d.RemoveAt(0);
                    d.Add(temp2);
                }
                else
                {
                    int[] temp1 = new int[2];
                    temp1[0] = c[0];
                    temp1[1] = c[1];
                    c.RemoveAt(0);
                    c.RemoveAt(0);
                    c.Add(temp1[0]);
                    c.Add(temp1[1]);
                    
                    int[] temp2 = new int[2];
                    temp2[0] = d[0];
                    temp2[1] = d[1];
                    d.RemoveAt(0);
                    d.RemoveAt(0);
                    d.Add(temp2[0]);
                    d.Add(temp2[1]);
                }                
                subkeys.Add(Permute(c.Concat(d).ToArray(), DesTables.PC2));    
                */
                int shift = DesTables.Shifts[i];
                c = RotateLeft(c, shift);
                d = RotateLeft(d, shift);
                subkeys.Add(Permute(c.Concat(d).ToArray(), DesTables.PC2));

            }
            return subkeys;
        }

        private static List<int> RotateLeft(List<int> bits, int count)
            => bits.Skip(count).Concat(bits.Take(count)).ToList();

        private bool TryParseMessage(string message, bool isHex, out byte[] output)
        {
            output = Array.Empty<byte>();

            if (string.IsNullOrEmpty(message)) return false;                           
            
            if (isHex)
            {
                if(message.Length % 2  != 0 || !message.All(Uri.IsHexDigit)) return false;
                output = Convert.FromHexString(message);
            }
            else
            {
                output = System.Text.Encoding.UTF8.GetBytes(message);
            }
            return true;
        }

        private byte[] AddBytes(byte[] data)
        {
            int padLen = 8 - (data.Length % 8);             
            var result = new byte[data.Length + padLen];
            Array.Copy(data, result, data.Length);
            for (int i = data.Length; i < result.Length; i++)
                result[i] = (byte)padLen;
            return result;
        }

        private int[] Xor(int[] a, int[] b)
        {
            if(a.Length != b.Length)
                throw new ArgumentException("Arrays must have the same length.");
            var result = new int[a.Length];
            for (int i = 0; i < a.Length; i++)
                 result[i] = a[i] ^ b[i];
            return result;
        }

        private int[] EncryptBlock(int[] block, List<int[]> subkeys)
        {
            if (block.Length != 64)
                throw new ArgumentException("Block must be 64 bits.");

            var ip = Permute(block, DesTables.IP);
            int[] L = ip.Take(32).ToArray();
            int[] R = ip.Skip(32).Take(32).ToArray();

            for (int i = 0; i < 16; i++)
            {                
                var eXorK = Xor(Permute(R, DesTables.E), subkeys[i]);
                List<int> sBits = new List<int>();
                for (int box = 0; box < 8; box++)
                {
                    var bits = eXorK.Skip(6 * box).Take(6).ToArray();
                    Console.WriteLine($"B{box + 1}: {string.Join("", bits)}");
                    string row = bits[0].ToString() + bits[5].ToString();
                    string col = bits[1].ToString() + bits[2].ToString() + bits[3].ToString() + bits[4].ToString();
                    int val = DesTables.SBoxes[box, Convert.ToInt32(row, 2), Convert.ToInt32(col, 2)];
                    string binary = Convert.ToString(val, 2).PadLeft(4, '0');  
                    foreach (char c in binary)
                        sBits.Add(c - '0');                                    
                }
                Console.WriteLine();

                int[] F = Permute(sBits.ToArray(), DesTables.P);
                int[] newR = Xor(L, F);
                L = R;
                R = newR;
            }                        
            return Permute(R.Concat(L).ToArray(), DesTables.IPInverse);
        }

        private byte[] BitsToBytes(int[] bits)
        {
            var bytes = new byte[bits.Length / 8];
            for (int i = 0; i < bits.Length; i++)
                bytes[i / 8] |= (byte)(bits[i] << (7 - i % 8));
            return bytes;
        }

        private void DesEncrypt()
        {
            Console.WriteLine("Enter the key (8 bytes):");
            string key_str = Console.ReadLine();
            if(!TryParseKey(key_str, out byte[] key_byte))
            {
                Console.WriteLine("Key must be 16 hex digits or exactly 8 ASCII characters.");
                return;
            }
            
            int[] key_bit = BytesToBits(key_byte);            
            int[] keyPlus = Permute(key_bit, DesTables.PC1);        
            Console.WriteLine($"K+: {string.Join("",keyPlus)}");
            int[] C_Zero = new int[28];
            int[] D_Zero = new int[28];
            for (int i = 0; i < 28; i++)
            {
                C_Zero[i] = keyPlus[i];
                D_Zero[i] = keyPlus[i + 28];
            }
            var subkeys = GenerateSubKeys(C_Zero, D_Zero);
            for (int i = 0; i < subkeys.Count; i++)
                Console.WriteLine($"K{i + 1}: {string.Join("", subkeys[i])}");


            Console.WriteLine("Do you want to enter hex or text message? (H/T)");
            bool isHex = true;
            if (Console.ReadLine().ToUpper() == "H")
                isHex = true;
            else isHex = false;
            Console.WriteLine("Enter the message:");
            string message = Console.ReadLine();
            if (!TryParseMessage(message, isHex, out byte[] result))
            {
                Console.WriteLine("Wrong format");
                return;
            }
            byte[] padded = AddBytes(result);
            int[] msgBits = BytesToBits(padded);

            var ciferBits = new List<int>();
            for (int i = 0; i < msgBits.Length; i+=64)
            {
                int[] block = msgBits.Skip(i).Take(64).ToArray();
                int[] encrypted = EncryptBlock(block, subkeys);
                ciferBits.AddRange(encrypted);
            }

            Console.WriteLine($"Encrypted (hex): {Convert.ToHexString(BitsToBytes(ciferBits.ToArray()))}");
        }
    }
}
