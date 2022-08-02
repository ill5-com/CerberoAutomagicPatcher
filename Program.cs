using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CerberoAutomagicPatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cerbero Automagic Patcher";

            if (args.Length != 1 || !args[0].ToLower().Contains("cerpro.exe"))
            {
                Console.WriteLine("Please drag and drop cerpro.exe onto this EXE!\nPress any key to exit.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Patching, please wait...");

            List<byte> fileBytes = File.ReadAllBytes(args[0]).ToList();
            byte[] signature = { 0xE8, 0xCC, 0xCC, 0xCC, 0xCC, 0x89, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xA8, 0x08 };
            bool patchedSuccessfully = false;
            for (int i = 0; i < fileBytes.Count; i++)
            {
                bool matches = true;
                for (int j = 0; j < signature.Length; j++)
                {
                    if (fileBytes[i + j] != signature[j] && signature[j] != 0xCC)
                    {
                        matches = false;
                        break;
                    }
                }

                if (matches)
                {
                    fileBytes[i] = 0xB8;
                    fileBytes[i + 1] = 0x1F;
                    fileBytes[i + 2] = 0x00;
                    fileBytes[i + 3] = 0x00;
                    fileBytes[i + 4] = 0x00;

                    Console.WriteLine("Found a signature, patched successfully.");
                    patchedSuccessfully = true;
                }
            }

            if (patchedSuccessfully)
            {
                File.WriteAllBytes(args[0], fileBytes.ToArray());
                Console.WriteLine("Wrote patched bytes to file, enjoy!");
            }
            else
                Console.WriteLine("Patching failed, couldn't find the signature!");

            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();
            return;
        }
    }
}
