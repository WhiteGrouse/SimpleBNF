using SimpleBNF;
using System;
using System.Collections.Generic;
using System.Text;

namespace CppMangledParser
{
    public class Number : ITag
    {
        public string Name => "number";

        public bool TryMatch(ParseContext context, out Token token)
        {
            if(context.Eof())
            {
                token = null;
                return false;
            }
            bool nega = context.CurrentChar == 'n';
            ++context.Position;
            if (!char.IsDigit(context.CurrentChar))
            {
                --context.Position;
                token = null;
                return false;
            }
            int number = 0;
            while (!context.Eof() && char.IsDigit(context.CurrentChar))
            {
                number = number * 10 + (context.CurrentChar - 0x30);
                ++context.Position;
            }
            if (nega)
            {
                number = -number;
            }
            token = new Token(Name, number);
            return true;
        }
    }
}
