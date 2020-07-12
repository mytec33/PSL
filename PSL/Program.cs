using System;
using System.Collections.Generic;

namespace PSL
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Token> tokens = new List<Token>();

            if (args.Length != 1)
            {
                PrintHelp();
                Environment.Exit(1);
            }

            tokens = TokenizeFile(args[0]);

            PrintTokens(tokens);
        }

        static private List<Token> TokenizeFile(string file)
        {
            List<Token> Tokens = new List<Token>();
            PSL_Reader reader = new PSL_Reader(file);
            string operators = "+/*<>,=[]{}()";
            string numbers = "1234567890";

            string ch;
            string newToken = "";
            while ((ch = reader.NextChar()) != "-1")
            {
                switch (ch)
                {
                    case "'":
                        newToken = ch;
                        while ((ch = reader.NextChar()) != "\r")
                            newToken += ch;
                        reader.NextChar(); // Consume \n
                        Tokens.Add(new Token("COMMENT", newToken));
                        newToken = "";
                        break;
                    case "\"":
                        newToken = ch;
                        while ((ch = reader.NextChar()) != "\"")
                            newToken += ch;
                        newToken += ch;
                        Tokens.Add(new Token("STRING", newToken));
                        newToken = "";
                        break;
                    case "\t":
                        // Consume it
                        break;
                    case "+":
                        if (reader.Peek() == "=")
                        {
                            reader.NextChar();
                            Tokens.Add(new Token("ADD", "+="));
                        }
                        else
                            Tokens.Add(new Token("ADD", "+"));
                        break;
                    case "-":
                        if (numbers.Contains(reader.Peek()))
                            newToken += ch;
                        else if (reader.Peek() == "=")
                        {
                            reader.NextChar();
                            Tokens.Add(new Token("SUB", "-="));
                        }
                        else
                            Tokens.Add(new Token("SUB", "-"));
                        break;
                    case "/":
                        if (reader.Peek() == "=")
                        {
                            reader.NextChar();
                            Tokens.Add(new Token("DIV", "/="));
                        }
                        else
                            Tokens.Add(new Token("DIV", "/"));
                        break;
                    case "*":
                        if (reader.Peek() == "=")
                        {
                            reader.NextChar();
                            Tokens.Add(new Token("MUL", "*="));
                        }
                        else
                            Tokens.Add(new Token("MUL", "*"));
                        break;
                    case "<":
                        Tokens.Add(new Token("LTHAN", "<"));
                        break;
                    case ">":
                        Tokens.Add(new Token("GTHAN", ">"));
                        break;
                    case ",":
                        Tokens.Add(new Token("COMMA", ","));
                        break;
                    case "=":
                        if (reader.Peek() == "=")
                        {
                            reader.NextChar();
                            Tokens.Add(new Token("EQUAL", "=="));
                        }
                        else
                            Tokens.Add(new Token("ASSIGN", "="));
                        break;
                    case "[":
                        Tokens.Add(new Token("LBRACKET", "["));
                        break;
                    case "]":
                        Tokens.Add(new Token("RBRACKET", "]"));
                        break;
                    case "(":
                        Tokens.Add(new Token("LPAREN", "("));
                        break;
                    case ")":
                        Tokens.Add(new Token("RPAREN", ")"));
                        break;
                    default:
                        string peek = reader.Peek();

                        if (string.IsNullOrWhiteSpace(ch))
                        {
                            if (newToken.Length == 0)
                                continue; // Consume -- just a space betweeen tokens or tab

                            Tokens.Add(new Token("IDENT", newToken));
                            newToken = "";
                        }
                        else if (operators.Contains(peek))
                        {
                            newToken += ch;
                            Tokens.Add(new Token("IDENT", newToken));
                            newToken = "";
                        }
                        else
                            newToken += ch;

                        break;
                }
            }

            return Tokens;
        }

        static private void PrintHelp()
        {
            Console.WriteLine("You must provide a path and file name");
        }

        static private void PrintTokens(List<Token> tokens)
        {
            foreach (Token token in tokens)
            {
                Console.WriteLine(token.DebugPrint());
            }
        }
    }
}
