namespace Lime
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // checks if it is about to interpret a file that doesn't exist
            if (File.Exists(args[0]))
            {
                // cleaning up interpreted.cs before written to; only happens if "clean" is passed in build.sh
                if (args[1] == "clean")
                {
                    FileManager.Clean();
                    Console.WriteLine("Cleared /interpreted.cs!");
                }

                // idk how this part is going to fully work yet but its something like

                // converts the lime code at args[0] (buildsettings.json/buildFile) to tokens
                // Tokenizer.Tokenize(args[0]);

                // this will probably convert the tokens into an ast and then into c#
                // string CSharpCode = Tokenizer.ConvertTokensToCSharp();

                // this function creates interpreted.cs with the code
                // FileManager.WriteFile(CSharpCode)

                // then build.sh moves on to run interpreted.cs

            }
            else
            {
                Console.WriteLine("File given does not exist!");
            }
        }
    }
}