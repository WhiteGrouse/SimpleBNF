using System;
using System.Collections.Generic;

namespace SimpleBNF
{
    public interface ITag
    {
        string Name { get; }

        bool TryMatch(ParseContext context, out Token token);
    }
}
