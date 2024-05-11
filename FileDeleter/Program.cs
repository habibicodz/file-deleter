using Microsoft.VisualBasic;

class Program  {
    static int Main(string[] args) {
        if (args.Length == 0) {
            Console.WriteLine("Please Pass argument to the program");
            return 1;
        }

        if (args.Length > 1) {
            Console.WriteLine("Arguments not allowed");
            return 1;
        }

        string filePath = args[0];
        bool isDir = IsDirectory(filePath);

        if (!Exists(filePath))
        {
            Console.WriteLine($"{filePath} does not exists.");
            return 0;
        }

        long size = GetFullLength(filePath);
        Console.WriteLine($"Size: {size}");

        Console.WriteLine("Are you sure want to delete this file? (Y) Yes, or (N) No");
        string? proceed;

        while (!((proceed = Console.ReadLine()?.ToLower()) == "y" || proceed == "n")) {
            Console.WriteLine("Please press (Y) for Yes or (N) for No and press enter");
        }

        if (proceed == "n") {
            Console.WriteLine("Delete operation cancelled");
            return 0;
        }

        var start = DateAndTime.Now;
        ForceDelete(filePath);
        long end = DateAndTime.Now.Microsecond  - start.Microsecond;
        Console.WriteLine($"Time Taken: {end} micro seconds");



        if (!Exists(filePath)) {
            Console.WriteLine($"{((isDir) ? "Folder" : "File")} deleted successfully :)");
        } else {
            Console.WriteLine("Delete operation failed!");
        }

        return 0;
    }

    private static void PrintAllChild (string filePath) {
        if (IsDirectory(filePath)) {
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            Print($"Printing: {directoryInfo.Name}");
            foreach (var child in directoryInfo.GetDirectories()) {
                PrintAllChild(child.FullName);
            }

            foreach(var child in directoryInfo.GetFiles()) {
                PrintAllChild(child.Name);
            }

        } else {
            FileInfo fileInfo = new FileInfo(filePath);
            Print(fileInfo.Name);
        }
    }


    private static void ForceDelete (string filePath) {
        if (IsDirectory(filePath)) {
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

            foreach(var child in directoryInfo.GetFiles()) {
                ForceDelete(child.FullName);
            }
            
            foreach (var dir in directoryInfo.GetDirectories()) {
                ForceDelete(dir.FullName);
                dir.Delete();
            }

            directoryInfo.Delete();

        } else {
            FileInfo fileInfo = new FileInfo(filePath);
            fileInfo.Delete();
        }
    }

    private static long GetFullLength(string filePath)
    {
        long size = 0L;

        if (IsDirectory(filePath))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            Print($"Printing: {directoryInfo.Name}");
            foreach (var child in directoryInfo.GetDirectories())
            {
                size += GetFullLength(child.FullName);
            }

            foreach (var child in directoryInfo.GetFiles())
            {
                size += GetFullLength(child.FullName);
            }

        }
        else
        {
            FileInfo fileInfo = new FileInfo(filePath);
            size += fileInfo.Length;
        }

        return size;
    }

    private static bool IsDirectory (string filePath) {
        var info = new DirectoryInfo(filePath);
        return info != null && info.Exists;
    }

    private static bool Exists(string filePath) {
        return new FileInfo(filePath).Exists || new DirectoryInfo(filePath).Exists;
    }

    private static void Print(string text) {
        Console.WriteLine(text);
    }
}