using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBNF
{
    public class Token
    {
        public string Name { get; }

        public object Value { get; }

        public string Comment { get; }

        public IEnumerable<Token> Child { get; set; }

        public bool HasChild => Child?.Any() == true;

        public Token(string name, object value) : this(name, value, "", null) { }
        public Token(string name, object value, string comment) : this(name, value, comment, null) { }
        public Token(string name, object value, IEnumerable<Token> child) : this(name, value, "", child) { }

        public Token(string name, object value, string comment, IEnumerable<Token> child)
        {
            Name = name;
            Value = value;
            Comment = comment;
            Child = child;
        }
    }
}
