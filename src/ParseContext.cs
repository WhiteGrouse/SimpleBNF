using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleBNF
{
    public class ParseContext
    {
        public Grammar Grammar { get; }

        public string Source { get; }

        public int Position { get; set; }

        public string Current => Source.Substring(Position);

        public char CurrentChar => Source[Position];

        public bool Eof() => !(Position < Source.Length);

        public ParseContext(Grammar grammar, string source)
        {
            Grammar = grammar;
            Source = source;
            Position = 0;
        }
    }
}
