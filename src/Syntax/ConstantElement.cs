using System;
using System.Collections.Generic;

namespace SimpleBNF.Syntax
{
    public class ConstantElement : ISyntaxElement
    {
        public string Value { get; }

        public string Comment { get; }

        public bool IsOptional { get; }

        public ConstantElement(string value) : this(value, "", false) { }
        public ConstantElement(string value, string comment) : this(value, comment, false) { }
        public ConstantElement(string value, bool isOptional) : this(value, "", isOptional) { }
        
        public ConstantElement(string value, string comment, bool isOptional)
        {
            Value = value;
            Comment = comment;
            IsOptional = isOptional;
        }
    }
}
