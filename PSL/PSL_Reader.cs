using System.IO;


namespace PSL
{
    class PSL_Reader
    {
        readonly StreamReader sr;

        public PSL_Reader(string file)
        {
            sr = new StreamReader(file);
        }

        public string NextChar()
        {
            if (sr.EndOfStream)
                return "-1";

            return ((char)sr.Read()).ToString();
        }

        public string Peek()
        {
            if (sr.EndOfStream)
                return "-1";

            return ((char)sr.Peek()).ToString();
        }
    }
}
