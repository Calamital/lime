using Microsoft.VisualBasic;

namespace Lime
{
    public class Program
    {
        /*
        ARGS:
        [0] = command (build|clean)
        [1] = file/project path
        */
        public static void Main(string[] args)
        {
            Tokenizer tokenizer = new();
            Console.WriteLine($"{{PO}}: {args[0]} on {args[1]}");
            if (File.Exists(args[1])) //validate path
            {
                switch (args[0].ToLower())
                {
                    case "clean":
                        switch (FileManager.Clean())
                        {
                            case FileManager.CleanResults.Success:
                                Console.WriteLine($"Cleared '{FileManager.InterpreterResultPath}/*' !");
                                break;
                            case FileManager.CleanResults.NoFiles:
                                Console.WriteLine("Nothing to clean.");
                                break;
                        }
                        break;
                    case "build":
                    default:
                        Console.WriteLine("Beginning Build.");
                        (FileManager.ReadResults result, string contents) = FileManager.ReadEntireFile(args[1]);
                        if (result != FileManager.ReadResults.Success) { FileManager.HandleReadError(result); return; } //validate successful read

                        List<Tokenizer.Token> tokens = Tokenizer.Tokenize(contents);

                        Console.WriteLine("Beginning printout");
                        foreach (Tokenizer.Token token in tokens)
                        {
                            Console.WriteLine($"Token: {token.matcher} Contents: {token.code}");
                        }

                        break;
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