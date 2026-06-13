namespace Lime
{
    public class Tokenizer
    {
        public Tokenizer() { }

        enum TokenTrigger
        { 
        }

        public struct Token
        {

        }

        public static List<Token> Tokenize(string contents)
        {
            List<Token> tokens = [];

            string tokenbuffer = "";
            foreach (char c in contents)
            {
                tokenbuffer += c;
                
            }

            return tokens;
        }
    }
}