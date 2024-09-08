using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using Jarvis.ProjectJarvis.CommandsFolder;


namespace Jarvis.Project.Settings.PathManager;

public class ExePathManagerClass
{
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public static string ExePaths;

    public static Dictionary<string, string> LoadPathsFromJson()
    {
        try
        {
            string filePath = Properties.Settings.Default.ExePathsJsonPath;
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                return JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString, options);
            }
        }
        catch (Exception ex)
        {
            log.Error($"[EXE PATH MANAGER]: An error occurred while loading paths from JSON: '{ex.Message}'");
        }

        return new Dictionary<string, string>();
    }

    public void SavePathsToJson(Dictionary<string, string> paths)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string jsonString = JsonSerializer.Serialize(paths, options);

            string directoryPath = @"C:\Program Files\Jarvis\Resources";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, "exePaths.json");
            File.WriteAllText(filePath, jsonString);

            log.Info($"[EXE PATH MANAGER]: Paths to exe files have been saved to '{filePath}'");
        }
        catch (Exception ex)
        {
            log.Error($"[EXE PATH MANAGER]: An error occurred while saving paths to JSON: '{ex.Message}'");
        }
    }

    public string FindExePath(string exeName)
    {
        try
        {
            foreach (string drive in Environment.GetLogicalDrives())
            {
                string path = SearchDirectory(drive, exeName);
                if (!string.IsNullOrEmpty(path))
                {
                    return path;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error($"[EXE PATH MANAGER]: Serching path`s: An error occurred while searching for '{exeName}': '{ex.Message}'");
        }

        return null;
    }

    private string SearchDirectory(string directory, string exeName)
    {
        try
        {
            foreach (string file in Directory.GetFiles(directory, exeName, SearchOption.TopDirectoryOnly))
            {
                return file;
            }

            foreach (string subdirectory in Directory.GetDirectories(directory))
            {
                if (subdirectory.StartsWith(@"C:\$RECYCLE.BIN") || subdirectory.StartsWith(@"C:\Windows") || subdirectory.StartsWith(@"D:\$RECYCLE.BIN") || subdirectory.StartsWith(@"D:\Windows") || subdirectory.StartsWith(@"E:\$RECYCLE.BIN") || subdirectory.StartsWith(@"E:\Windows") || subdirectory.StartsWith(@"F:\$RECYCLE.BIN") || subdirectory.StartsWith(@"F:\Windows"))
                {
                    continue;
                }

                string path = SearchDirectory(subdirectory, exeName);
                if (!string.IsNullOrEmpty(path))
                {
                    log.Info($"[EXE PATH MANAGER]: Searching path`s. Found '{exeName}' in '{path}'");
                    return path;
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            // log.Warn($"Access denied to directory {directory}"); // отключено
        }
        catch (Exception ex)
        {
            log.Error($"[EXE PATH MANAGER]: Searching path`s: An error occurred while searching in directory '{directory}': '{ex.Message}'");
        }

        return null;
    }

    public void UpdateExePaths(Dictionary<string, string> exePaths)
    {
        Parallel.ForEach(ExeCommandsClass.exeNameProgramms, kvp =>
        {
            string path = FindExePath(kvp.Value);
            if (!string.IsNullOrEmpty(path))
            {
                lock (exePaths)
                {
                    exePaths[kvp.Value] = path;
                }
            }
            else
            {
                log.Warn($"[EXE PATH MANAGER]: Searching path`s: Path for the program {kvp.Value} not found.");
            }
        });

        SavePathsToJson(exePaths);
    }
}