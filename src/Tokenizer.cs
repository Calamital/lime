using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;

namespace Lime
{
    // do you want me to make a list of regex rules for the different syntax sure honey
    public partial class Tokenizer
    {
#pragma warning disable IDE0044
        private static Dictionary<string, string> RegexRules = [];
#pragma warning restore IDE0044

        public Tokenizer()
        {
            RegexRules.Add("newline", @"\\\n/");
            RegexRules.Add("comment", @"\/\/.*$");
            RegexRules.Add("inlineComment", @"\/\*.*\*\/");
            // idk what other keywords to add
            RegexRules.Add("keyword", @"constructor|return");
            RegexRules.Add("self", @"self");
            RegexRules.Add("functionDefinitionName", @"(?<=int16 |int32 |int6 4|double |long |float |void |public |private |string |constant )\w+(?=\()");
            RegexRules.Add("function", @"(?<=\.).*(?=\(.*;)");
            RegexRules.Add("constructorName", @"(?<=constructor ).*(?=\()");
            // variable name and declaration need to be filtered to make sure its not picking up a keyword
            RegexRules.Add("variableName", @"(?<= )\w+(?!\()");
            RegexRules.Add("variableAssignment", @"(?<= |\.)\w+ = .*(?=;)");
            RegexRules.Add("datatype", @"int16|int32|int64|double|long|float|void|public|private|string|constant");
        }

        // public static Dictionary<string, string> TokenTriggers = new()
        // {
        //     {"Comment","//"} //
        // };

        [method: SetsRequiredMembers]
        public struct Token(string matchkey, string rawtext)
        {
            public required string matcher = matchkey;
            public required string code = rawtext;
            public bool complete = false;
            // public List<string> args = ["", "", "", "", "", "", "", "", ""];
            public string[] args = [];
        }

        public static List<Token> Tokenize(string contents)
        {
            List<Token> tokens = [];

            //Console.WriteLine($"parsing {contents}");
            string tokenbuffer = "";
            KeyValuePair<string, string> currentPair = new();
            bool currentSet = false;
            foreach (char c in contents)
            {
                tokenbuffer += c;
                Console.WriteLine(c);

                // Console.WriteLine($"bih we got {c}");
                foreach (KeyValuePair<string, string> tokenparse in RegexRules)
                {
                    //Console.WriteLine($"buffer {tokenbuffer}");
                    //Console.WriteLine($"Uhm '{tokenparse.Key}' with lock on '{currentPair.Key}'");
                    // Console.WriteLine($"Matching '{tokenparse.Value}' current lock is '{currentPair.Value}'");
                    if (currentSet && (currentPair.Key != tokenparse.Key) && (currentPair.Value != tokenparse.Value)) continue;

                    // bool matches = tokenparse.Value.IsMatch(tokenbuffer);
                    //Console.WriteLine($"comparing with {tokenbuffer}");
                    // bool matches = Regex.IsMatch(tokenbuffer, tokenparse.Value);
                    Match matches = Regex.Match(tokenbuffer, tokenparse.Value);
                    Token previous = tokens.Count == 0 ? new("", "") : tokens[^1];

                    Console.WriteLine($"regex was {matches.Success} L {matches.Length} I {matches.Index} s {matches.ToString()}");

                    if (currentSet)
                    {
                        if (tokenbuffer.Length > matches.Length)
                        {
                            Console.WriteLine($"{{DEBUG}}: Token {tokenparse.Key} is matching {matches.Length} of {tokenbuffer.Length}");
                            /*
                            variableName
                            variableAssignment
                            datatype
                            */
                            bool TokenCompletion = false;
                            switch (tokenparse.Key)
                            {
                                case "variableName":
                                    if (previous.matcher == "datatype")
                                    {
                                        previous.matcher = "variableDefined";
                                        previous.args[0] = previous.code; // variable type
                                        previous.args[1] = tokenbuffer; // variable name
                                        previous.code += " " + tokenbuffer;
                                        TokenCompletion = true;
                                    } // possibly throw error on else
                                    break;
                                case "variableAssignment":
                                    if (previous.matcher == "variableDefined")
                                    {
                                        previous.matcher = "variable";
                                        previous.args[2] = tokenbuffer; // variable assignment
                                        previous.code += "=" + tokenbuffer;
                                        TokenCompletion = true;
                                    } // possibly throw error on else
                                    break;
                            }

                            currentSet = false;
                            if (!TokenCompletion && tokenbuffer.Length > 0)
                            {
                                try
                                {
                                    tokens.Add(
                                        // new Token(tokenparse.Key, tokenbuffer.Substring(0,tokenbuffer.Length-2))
                                        new Token(tokenparse.Key, tokenbuffer)
                                    );

                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    Console.WriteLine(e);
                                }
                            }
                            tokenbuffer = c.ToString();
                        }

                    }
                    else if (matches.Success)
                    {
                        if (tokenparse.Key == "newline")
                        {
                            Console.WriteLine($"{{DEBUG}}: wiping buffer containing {tokenbuffer}");
                            tokenbuffer = "";
                            currentSet = false;
                            break;
                        }

                        switch (tokenparse.Key)
                        {
                            case "variableName":
                                if (previous.matcher != "datatype") continue;
                                break;
                            case "variableAssignment":
                                if (previous.matcher != "variableDefined") continue;
                                break;
                        }

                        tokenbuffer = tokenbuffer[matches.Index..];

                        Console.WriteLine($"{{DEBUG}}: Token {tokenparse.Key} is locked");

                        currentPair = tokenparse;
                        currentSet = true;

                        break;
                    }
                }
            }
            if (currentSet) tokens.Add(
                new Token(currentPair.Key, tokenbuffer)
            );
            return tokens;
        }
    }
}