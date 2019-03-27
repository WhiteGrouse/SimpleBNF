using System;
using System.Collections.Generic;

namespace SimpleBNF.Syntax
{
    public class TagElement : ISyntaxElement
    {
        public enum RepeatKind
        {
            NotRepeat,
            Star,
            Plus
        }

        public string TagName { get; }

        public RepeatKind Repeat { get; }

        public bool IsOptional { get; }

        public TagElement(string tagName) : this(tagName, RepeatKind.NotRepeat, false) { }
        public TagElement(string tagName, RepeatKind repeat) : this(tagName, repeat, false) { }
        public TagElement(string tagName, bool isOptional) : this(tagName, RepeatKind.NotRepeat, isOptional) { }

        public TagElement(string tagName, RepeatKind repeat, bool isOptional)
        {
            TagName = tagName;
            Repeat = repeat;
            IsOptional = isOptional;

            if(Repeat == RepeatKind.Star)
            {
                IsOptional = true;
            }
            if(repeat == RepeatKind.Plus && IsOptional)
            {
                Repeat = RepeatKind.Star;
            }
        }
    }
}
