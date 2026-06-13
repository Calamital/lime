namespace Lime
{
    public class Tokenizer
    {
        public Tokenizer() { }

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
                //should check for token matches here ig
            }

            return tokens;
        }
    }
}