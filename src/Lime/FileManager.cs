namespace Lime
{
    public class FileManager
    {
        public static readonly string InterpreterResultPath = "/workspaces/lime/src/Lime/interpreted.cs";

        public FileManager() {}

        // deletes interpreted.cs just to ensure a proper reset
        public static void Clean()
        {
            if (File.Exists(InterpreterResultPath))
            {
                File.Delete(InterpreterResultPath);
            }
        }

        // writes fileText to interpreted.cs
        public static void WriteFile(string fileText)
        {
            using StreamWriter streamWriter = File.CreateText(InterpreterResultPath);
            streamWriter.Write(fileText);
        }

        // reads a given lime file line by line and returns the lines separated by \n
        public static string ReadEntireFile(string filePath)
        {
            string contents = "";
            string? line;

            using (StreamReader streamReader = new(filePath))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    contents += line + '\n';
                }
            }

            return contents;
        }
    }
}