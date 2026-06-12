using Microsoft.VisualBasic;

namespace Lime
{
    public class FileManager
    {
        public static readonly string InterpreterResultPath = "./workspaces/lime/src/Lime";
        public static readonly string MainCSCompileScript = "interpreted.cs";
        public static readonly string MainCSCompileScriptPath = Path.Join(InterpreterResultPath, MainCSCompileScript);
        public enum CleanResults
        {
            Success,
            NoFiles,
        }
        public enum ReadResults
        {
            Success,
            NotFound,
        }

        public FileManager() { }

        // deletes interpreted.cs just to ensure a proper reset
        public static CleanResults Clean()
        {
            if (File.Exists(MainCSCompileScriptPath))
            {
                File.Delete(MainCSCompileScriptPath);
                return CleanResults.Success;
            }
            return CleanResults.NoFiles;
        }

        // writes fileText to interpreted.cs
        public static void WriteFile(string fileText)
        {
            if (!Path.Exists(InterpreterResultPath)) FileSystem.MkDir(InterpreterResultPath);
            using StreamWriter streamWriter = File.CreateText(MainCSCompileScriptPath);
            streamWriter.Write(fileText);
        }

        // reads a given lime file line by line and returns the lines separated by \n
        //why are youy doing it like that if all contents are concatenated anyway. also phrased wrong, it doesnt return a list of a string seperated by \n it returns a string
        public static Tuple<ReadResults,string> ReadEntireFile(string filePath)
        {
            // string contents = "";
            // string? line;

            // using (StreamReader streamReader = new(filePath))
            // {
            //     while ((line = streamReader.ReadLine()) != null)
            //     {
            //         contents += line + '\n';
            //     }
            // }

            // return new(ReadResults.Success,contents);

            if(!File.Exists(filePath)) return new(ReadResults.NotFound,File.ReadAllText(filePath));
            return new(ReadResults.Success,File.ReadAllText(filePath).Replace("\r\n","\n"));
        }
    }
}