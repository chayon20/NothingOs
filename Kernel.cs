using Cosmos.HAL.BlockDevice;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using Sys = Cosmos.System;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        private MemoryManager memoryManager;
        private Sys.FileSystem.CosmosVFS fs;
        private string current_directory = "0:\\";

        protected override void BeforeRun()
        {
            // Initialize memory manager with 1000 blocks
            memoryManager = new MemoryManager(1000);
            memoryManager.InitializeMemoryBlocks();

            Console.WriteLine("Welcome to 'NOTHING'- A Lightweight Operating System.");
            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
        }

        protected override void Run()
        {
            string input;
            Console.Write(">");
            input = Console.ReadLine();

            if (input == "help")
            {
                DisplayHelp();
            }
            else if (input == "allocate")
            {
                AllocateMemory();
            }
            else if (input == "free")
            {
                FreeMemory();
            }
            else if (input == "hi")
            {
                Console.WriteLine("Hello User!\n This is NOTHING Operating System!\n");
            }
            else if (input == "about")
            {
                Console.WriteLine("NOTHING Operating System. Made by Chayon Kumar Das, Md. Tanjib Riasat, Suvro Kumar Das\n");
            }
            else if (input == "date")
            {
                Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy\n"));
            }
            else if (input == "time")
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss\n"));
            }
            else if (input == "day")
            {
                Console.WriteLine(DateTime.Now.ToString("dddd\n"));
            }
            else if (input == "shutdown")
            {
                Sys.Power.Shutdown();
            }
            else if (input == "clear")
            {
                Console.Clear();
            }
            else if (input == "listfile" || input == "ls")
            {
                ListFiles();
            }
            else if (input == "deletefile" || input == "rm")
            {
                DeleteFile();
            }
            else if (input == "writefile")
            {
                WriteFile();
            }
            else if (input == "readfile" || input == "cat")
            {
                ReadFile();
            }
            else if (input == "createdirectory" || input == "mkdir")
            {
                CreateDirectory();
            }
            else if (input == "change_directory" || input == "cd")
            {
                ChangeDirectory();
            }
            else if (input == "pwd")
            {
                Console.WriteLine($"Current directory: {current_directory}\n");
            }
            else if (input == "createfile" || input == "touch")
            {
                CreateFile();
            }
            else if (input == "meminfo")
            {
                DisplayMemoryInfo();
            }
            else if (input == "schedule")
            {
             
            }
            else
            {
                Console.WriteLine("Invalid command!!!");
            }
        }

        private void DisplayHelp()
        {
            Console.WriteLine("\nabout-Learn about us.\nhelp-what are the commands? \ncreatefile-create a file \ndate -shows date" +
                "\nday-shows the day. \ndeletefile/rm-deletes a file.\nhi-feature\nls/listfile-shows the list of files" +
                "\ncreatefile-creates a file \nshutdown-power off the power \ntime-shows the time \nwritefile-writes on a file" +
                "\ncreatedirectory/mkdir-creates a directory \nmovefile-moves a file \ncopyfile-copies a file \nrenamefile-rename a file " +
                "\nchangedirectory-changes the directory\ncd pwd-\ncp-\nmv-\ntouch-\ncat-\ngrep\nalloc - Allocate memory\ndealloc - Deallocate memory\nmeminfo - Display memory information\n");
        }

        private void AllocateMemory()
        {
            try
            {
                int address = memoryManager.AllocateMemory();
                Console.WriteLine("Memory allocated at address: " + address);
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FreeMemory()
        {
            Console.Write("Enter address to free: ");
            int address = int.Parse(Console.ReadLine());

            try
            {
                memoryManager.FreeMemory(address);
                Console.WriteLine("Memory freed at address: " + address);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ListFiles()
        {
            try
            {
                string[] files = Directory.GetFiles(current_directory);
                string[] directories = Directory.GetDirectories(current_directory);

                Console.WriteLine("Files in the current directory:");
                foreach (string file in files)
                {
                    Console.WriteLine(Path.GetFileName(file));
                }

                Console.WriteLine("\nDirectories in the current directory:");
                foreach (string directory in directories)
                {
                    Console.WriteLine(Path.GetFileName(directory));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing files: {ex.Message}\n");
            }
        }

        private void DeleteFile()
        {
            Console.WriteLine("Enter the file name (including extension) to delete:");
            string fileName = Console.ReadLine();

            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentException("Please provide a valid filename.\n");
                }

                string filePath = Path.Combine(current_directory, fileName);

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"{fileName} does not exist.\n");
                    return;
                }

                File.Delete(filePath);
                Console.WriteLine($"{fileName} deleted successfully!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message} \n");
            }
        }

        private void WriteFile()
        {
            Console.WriteLine("Enter the file name (including extension):");
            string fileName = Console.ReadLine();

            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentException("Please provide a valid filename.\n");
                }

                string filePath = Path.Combine(current_directory, fileName);

                Console.WriteLine("Enter the text to write to the file:");
                string fileContent = Console.ReadLine();

                File.WriteAllText(filePath, fileContent);
                Console.WriteLine($"Content written to {fileName} successfully!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message} \n");
            }
        }

        private void ReadFile()
        {
            Console.WriteLine("Enter the file name (including extension) to display:");
            string fileName = Console.ReadLine();

            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentException("Please provide a valid filename.\n");
                }

                string filePath = Path.Combine(current_directory, fileName);

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"{fileName} does not exist.\n");
                    return;
                }

                string fileContent = File.ReadAllText(filePath);
                Console.WriteLine(fileContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying file content: {ex.Message} \n");
            }
        }

        private void CreateDirectory()
        {
            Console.WriteLine("Enter the directory name:");
            string directoryName = Console.ReadLine();

            try
            {
                if (string.IsNullOrEmpty(directoryName))
                {
                    throw new ArgumentException("Please provide a valid directory name.\n");
                }

                string directoryPath = Path.Combine(current_directory, directoryName);

                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Directory '{directoryName}' created successfully!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"... Error creating directory: {ex.Message}\n");
            }
        }

        private void ChangeDirectory()
        {
            Console.WriteLine("Enter the directory name:");
            string directoryName = Console.ReadLine();

            try
            {
                string newDirectoryPath = Path.Combine(current_directory, directoryName);

                if (!Directory.Exists(newDirectoryPath))
                {
                    Console.WriteLine($"{directoryName} does not exist.\n");
                    return;
                }

                current_directory = newDirectoryPath;
                Console.WriteLine($"Current directory changed to {current_directory}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error changing directory: {ex.Message}\n");
            }
        }

        private void CreateFile()
        {
            Console.WriteLine("Enter the file name (including extension):");
            string fileName = Console.ReadLine();

            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentException("Please provide a valid filename.\n");
                }

                string filePath = Path.Combine(current_directory, fileName);

                using (FileStream fs = File.Create(filePath))
                {
                    Console.WriteLine($"{fileName} created successfully!\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file: {ex.Message}\n");
            }
        }

        private void DisplayMemoryInfo()
        {
            try
            {
                uint availableMemory = Cosmos.Core.CPU.GetAmountOfRAM() * 1024 * 1024;
                uint usedMemory = Cosmos.Core.GCImplementation.GetUsedRAM();
                Console.WriteLine($"Available Memory: {availableMemory} bytes\nUsed Memory: {usedMemory} bytes\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying memory information: {ex.Message}\n");
            }
        }

        public class MemoryManager
        {
            private const int BlockSize = 4096; // Size of each memory block (in bytes)
            private readonly List<bool> memoryMap; // Keeps track of allocated and free memory blocks

            public MemoryManager(int totalBlocks)
            {
                memoryMap = new List<bool>(totalBlocks);

                // Initialize memory map with all blocks marked as free
                for (int i = 0; i < totalBlocks; i++)
                {
                    memoryMap.Add(false);
                }
            }

            public void InitializeMemoryBlocks()
            {
                // This method was added to properly initialize memory blocks
                for (int i = 0; i < memoryMap.Count; i++)
                {
                    memoryMap[i] = false; // Initially, all blocks are free
                }
            }

            public int AllocateMemory()
            {
                for (int i = 0; i < memoryMap.Count; i++)
                {
                    if (!memoryMap[i])
                    {
                        memoryMap[i] = true; // Mark the block as allocated
                        return i * BlockSize; // Return the starting address of the allocated block
                    }
                }
                throw new OutOfMemoryException("Insufficient memory");
            }

            public void FreeMemory(int address)
            {
                int index = address / BlockSize; // Calculate the index of the memory block

                if (index < 0 || index >= memoryMap.Count || !memoryMap[index])
                {
                    throw new ArgumentException("Invalid memory address");
                }

                memoryMap[index] = false; // Mark the block as free
            }
        }
    }
}



