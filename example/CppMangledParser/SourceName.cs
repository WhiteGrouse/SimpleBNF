using SimpleBNF;
using System;
using System.Collections.Generic;
using System.Text;

namespace CppMangledParser
{
    public class SourceName : ITag
    {
        public string Name => "source-name";

        public bool TryMatch(ParseContext context, out Token token)
        {
            if (TryParse_Length(context, out int length))
            {
                if(context.Position + length >= context.Source.Length)
                {
                    context.Position -= length.ToString().Length;
                    token = null;
                    return false;
                }
                var value = context.Current.Substring(0, length);
                context.Position += length;
                token = new Token(Name, value);
                return true;
            }
            else
            {
                token = null;
                return false;
            }
        }

        protected bool TryParse_Length(ParseContext context, out int length)
        {
            if(context.Eof() || !char.IsDigit(context.CurrentChar))
            {
                length = -1;
                return false;
            }
            length = 0;
            while (!context.Eof() && char.IsDigit(context.CurrentChar))
            {
                length = length * 10 + (context.CurrentChar - 0x30);
                ++context.Position;
            }
            return true;
        }
    }
}
