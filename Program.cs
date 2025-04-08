using OsuParsers.Decoders;
using OsuParsers.Database;
using OsuParsers.Enums.Database;
using System.Windows.Forms;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleCP(uint wCodePageID);
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleOutputCP(uint wCodePageID);

    [STAThread]
    static void Main(string[] args)
    {
        SetConsoleCP(65001);
        SetConsoleOutputCP(65001);
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Console.WriteLine("osu! Database Manager");
        Console.WriteLine("------------------------");

        try
        {
            Console.WriteLine("Please select your osu!.db file...");
            string dbPath = SelectFile();
            
            if (string.IsNullOrEmpty(dbPath))
            {
                Console.WriteLine("No file selected. Exiting...");
                return;
            }

            OsuDatabase osuDb;
            osuDb = DatabaseDecoder.DecodeOsu(dbPath);
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Database: {dbPath}");
                Console.WriteLine("------------------------");
                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Delete user information");
                Console.WriteLine("2. Delete all beatmaps");
                Console.WriteLine("3. Save changes");
                Console.WriteLine("4. Exit");
                Console.Write("\nSelect an option (1-4): ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DeleteUserInfo(osuDb);
                        break;
                    case "2":
                        DeleteAllBeatmaps(osuDb);
                        break;
                    case "3":
                        SaveDatabase(osuDb, dbPath);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    static string SelectFile()
    {
        using (var dialog = new OpenFileDialog())
        {
            dialog.Filter = "osu! Database|*.db";
            dialog.Title = "Select osu!.db file";
            dialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "osu!");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
        }

        return string.Empty;
    }

    static void DeleteUserInfo(OsuDatabase db)
    {
        Console.Clear();
        Console.WriteLine("Delete User Information");
        Console.WriteLine("----------------------");
        Console.WriteLine($"Current username: {db.PlayerName}");
        Console.WriteLine("\nThis will:");
        Console.WriteLine("- Reset username to empty");
        Console.WriteLine("- Set account as unlocked");
        Console.WriteLine("- Reset permissions");
        Console.WriteLine("- Reset unlock date");
        Console.Write("\nAre you sure? (yes/no): ");
        
        string? confirmation = Console.ReadLine()?.ToLower();

        if (confirmation == "yes")
        {
            db.PlayerName = "";
            db.AccountUnlocked = true;
            db.Permissions = Permissions.None;
            db.UnlockDate = DateTime.MinValue;
            Console.WriteLine("\nUser information deleted successfully!");
        }
        else
        {
            Console.WriteLine("\nOperation cancelled.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void DeleteAllBeatmaps(OsuDatabase db)
    {
        Console.Clear();
        Console.WriteLine("Delete All Beatmaps");
        Console.WriteLine("------------------");
        Console.WriteLine($"Current beatmap count: {db.Beatmaps.Count}");
        Console.WriteLine($"Total folders: {db.FolderCount}");
        Console.WriteLine("\nThis will:");
        Console.WriteLine("- Delete all beatmaps from database");
        Console.WriteLine("- Reset beatmap count to 0");
        Console.Write("\nAre you sure? (yes/no): ");
        
        string? confirmation = Console.ReadLine()?.ToLower();

        if (confirmation == "yes")
        {
            int count = db.Beatmaps.Count;
            db.Beatmaps.Clear();
            db.FolderCount = 0;
            db.BeatmapCount = 0;
            Console.WriteLine($"\nDeleted {count} beatmaps successfully!");
            Console.WriteLine("All beatmap information has been cleared.");
        }
        else
        {
            Console.WriteLine("\nOperation cancelled.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void SaveDatabase(OsuDatabase db, string path)
    {
        try
        {
            string backupPath = path + ".backup";
            File.Copy(path, backupPath, true);
            
            db.Save(path);
            
            Console.WriteLine("Database saved successfully!");
            Console.WriteLine($"Backup created at: {backupPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving database: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
} 