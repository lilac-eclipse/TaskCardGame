using System.Text;

namespace Tools;

internal static class ProjToClipboard
{
    private static readonly HashSet<string> DirsToSkip =
        ["bin", "obj", ".idea", ".git", ".godot", "Tools"];

    private static readonly HashSet<string> FileExtensionsToSkipContent =
        [".svg", ".png", ".jpg", ".jpeg", ".tscn", ".import"];

    public static void Run(string[] args)
    {
        string projectRoot = FindProjectRoot(Directory.GetCurrentDirectory());

        if (string.IsNullOrEmpty(projectRoot))
        {
            Console.WriteLine("Could not find the project root directory.");
            return;
        }

        StringBuilder output = new();
        string[] directories = ["src", "tests"];

        foreach (string dir in directories)
        {
            string fullPath = Path.Combine(projectRoot, dir);
            if (!Directory.Exists(fullPath))
            {
                continue;
            }

            output.AppendLine($"Contents of {dir} directory:");
            output.AppendLine(GetDirectoryStructure(fullPath));
            output.AppendLine();
            AddDirectoryContents(fullPath, output);
        }

        try
        {
            Clipboard.SetText(output.ToString());
            Console.WriteLine("Data copied to clipboard!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error copying to clipboard: {ex.Message}");
        }
    }

    private static string FindProjectRoot(string startPath)
    {
        while (!string.IsNullOrEmpty(startPath))
        {
            if (Directory.Exists(Path.Combine(startPath, "src")) &&
                Directory.Exists(Path.Combine(startPath, "tests")))
            {
                return startPath;
            }

            startPath = Directory.GetParent(startPath)?.FullName!;
        }

        return null!;
    }

    private static string GetDirectoryStructure(string path, string indent = "")
    {
        StringBuilder structure = new();

        foreach (string dir in Directory.GetDirectories(path))
        {
            if (ShouldSkipDirectory(dir))
            {
                continue;
            }

            structure.AppendLine($"{indent}{Path.GetFileName(dir)}/");
            structure.Append(GetDirectoryStructure(dir, indent + "  "));
        }

        foreach (string file in Directory.GetFiles(path))
        {
            structure.AppendLine($"{indent}{Path.GetFileName(file)}");
        }

        return structure.ToString();
    }

    private static void AddDirectoryContents(string path, StringBuilder output)
    {
        foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
        {
            if (ShouldSkipDirectory(Path.GetDirectoryName(file)!))
            {
                continue;
            }

            string extension = Path.GetExtension(file);

            if (FileExtensionsToSkipContent.Contains(extension))
            {
                continue;
            }

            output.AppendLine($"File: {file}");
            output.AppendLine(new string('-', 80));
            output.AppendLine(File.ReadAllText(file));
            output.AppendLine(new string('-', 80));
            output.AppendLine();
        }
    }

    private static bool ShouldSkipDirectory(string dir) =>
        dir.Split(Path.DirectorySeparatorChar).Any(segment => DirsToSkip.Contains(segment));
}
