using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Specify the directory to traverse
            string directoryPath = @"C:\Users\iamma\OneDrive\Desktop\gRPC Time Server\gRPC Time Server\bin\x64\Debug\net6.0\win-x64\publish";
            int countedFiles = 0;
            // Create the GroupComponents file on the desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string groupComponentsFilePath = Path.Combine(desktopPath, "GroupComponents.txt");
            string groupComponentRefFilePath = Path.Combine(desktopPath, "GroupRef.txt");

            // Open or create the GroupComponents file
            using (StreamWriter writer = new StreamWriter(groupComponentsFilePath, true)) // 'true' appends to an existing file
            {
                // Traverse the specified directory
               countedFiles = TraverseDirectory(directoryPath, writer);
            }

            // Open or create the GroupComponents file
            using (StreamWriter writer = new StreamWriter(groupComponentRefFilePath, true)) // 'true' appends to an existing file
            {
                // Traverse the specified directory
                WriteRefs(writer,countedFiles);
            }

            Console.WriteLine($"GroupComponents file created at: {groupComponentsFilePath}");
        }

        static int TraverseDirectory(string directoryPath, StreamWriter writer)
        {
            // Get all files in the directory
            string[] files = Directory.GetFiles(directoryPath);
            var iterationNumber = 1;

            // Iterate through each file
            foreach (string filePath in files)
            {
                // Generate a new GUID for each component
                string componentGuid = Guid.NewGuid().ToString();

                // Extract file name and extension
                string fileName = Path.GetFileName(filePath);

                var givenFileName = $"TestFile{iterationNumber}";

                // Write the WiX component for each file using the specified template
                writer.WriteLine($"<Component Id={givenFileName} Guid=\"{componentGuid}\">");
                writer.WriteLine($"  <File Id=File{givenFileName} Name=\"{fileName}\" KeyPath=\"yes\" />");
                writer.WriteLine("</Component>");

                iterationNumber++;
            }

            // Traverse subdirectories
            string[] subdirectories = Directory.GetDirectories(directoryPath);
            foreach (string subdirectory in subdirectories)
            {
                TraverseDirectory(subdirectory, writer);
            }

            return iterationNumber;
        }

        static void WriteRefs(StreamWriter writer,int fileNumber)
        {
            for (int i = 1; i < fileNumber+1; i++)
            {
                // Generate a new ID for each ComponentRef
                string componentRefId = "TestFile" + i;

                // Write the ComponentRef element for each file
                writer.WriteLine($"<ComponentRef Id=\"{componentRefId}\" />");
            }
        }
    }
}
