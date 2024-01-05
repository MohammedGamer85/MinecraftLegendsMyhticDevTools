using Newtonsoft.Json;
using System.IO.Compression;

new App();

class ModClass {
    public string Name { get; set; }
    public string Creator { get; set; }
    public string GameMode { get; set; }
    public string Version { get; set; }
    public string Uuid { get; set; }
}

public class App {
    static ModClass Mod = new();

    public App() {
        Console.WriteLine("Hello, World!");
        try {
            GetOption();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
    void GetOption() {
        string TempFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MinecraftLegendsMyhticDevToolsTempFolder");
        if (Directory.Exists(TempFolder)) {
            Directory.Delete(TempFolder, true);
        }

        Console.WriteLine("Please select a option.");
        Console.WriteLine("1-Make a .mcl file.");
        Console.Write("input option number => ");
        string Option = Console.ReadLine();
        if (Option == "1") {
            MakeMCLModFile();
        }
        else {
            Console.Clear();
            Console.WriteLine("Error:[Option unavalable]");
        }
    }

    void MakeMCLModFile() {
        Console.Clear();
        Console.WriteLine("Please Enter RP Folder Path");
        Console.Write("=> ");
        string RPFolderPath = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Please Enter BP Folder Path");
        Console.Write("=> ");
        string BPFolderPath = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Please Enter Mod Name");
        Console.Write("=> ");
        Mod.Name = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Please Enter Mod Auther username");
        Console.Write("=> ");
        Mod.Creator = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Please Enter Mod UUID");
        Console.Write("=> ");
        Mod.Uuid = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Please Enter Mod GameMode");
        Console.Write("=> ");
        Mod.GameMode = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Please Enter Mod Version");
        Console.Write("=> ");
        Mod.Version = Console.ReadLine();
        string TempFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MinecraftLegendsMyhticDevToolsTempFolder");
        if (!Directory.Exists(TempFolderPath)) {
            Directory.CreateDirectory(TempFolderPath);
        }
        DirectoryUtilities.Copy(RPFolderPath, Path.Combine(TempFolderPath, "RP"), true);
        DirectoryUtilities.Copy(BPFolderPath, Path.Combine(TempFolderPath, "BP"), true);
        string SerilizedContent = System.Text.Json.JsonSerializer.Serialize<ModClass>(Mod);
        File.Create(Path.Combine(TempFolderPath, "modInfo.json")).Close();
        File.WriteAllText(Path.Combine(TempFolderPath, "modInfo.json"), SerilizedContent);
        Console.Write("Outpot Directory including FileName => ");
        string outpotDir = Console.ReadLine();
        ZipFile.CreateFromDirectory(TempFolderPath,
            outpotDir + ".mclmod");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"ExportedMod {outpotDir + ".mclmod"}");
        Console.ForegroundColor = ConsoleColor.White;
    }
}

public static class DirectoryUtilities {
    public static void Copy(string sourceDir, string destinationDir, bool recursive) {
        // Get information about the source directory
        var dir = new DirectoryInfo(sourceDir);

        // Check if the source directory exists
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        // Cache directories before we start copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        // Create the destination directory
        Directory.CreateDirectory(destinationDir);

        // Get the files in the source directory and copy to the destination directory
        foreach (FileInfo file in dir.GetFiles()) {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath);
        }

        // If recursive and copying subdirectories, recursively call this method
        if (recursive) {
            foreach (DirectoryInfo subDir in dirs) {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                Copy(subDir.FullName, newDestinationDir, true);
            }
        }

        Console.WriteLine($"Copyed {sourceDir} To {destinationDir} Copy subDir is {recursive}.");
    }
}



