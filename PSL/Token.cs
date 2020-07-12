namespace PSL
{
    class Token
    {
        public string Type { get; set; }
        public string Literal { get; set; }

        public Token(string type, string literal)
        {
            if (type == "IDENT")
                Type = ClassifyToken(type, literal);
            else
                Type = type;

            Literal = literal;
        }

        private string ClassifyToken(string type, string literal)
        {
            if (int.TryParse(literal, out int result))
                return "NUM";

            switch (literal.ToLower())
            {
                case "if":
                case "else":
                case "then":
                case "endif":
                case "repeat":
                case "exitrepeat":
                case "endrepeat":
                case "dim":
                case "as":
                    return "KEYWORD";
                default:
                    break;
            }
            return "IDENT";
        }

        public string DebugPrint()
        {
            string tabs = "\t";

            if (Type.Length < 6)
                tabs = "\t\t";

            return $"*{Type}*{tabs}*{Literal}*";
        }
    }
}
