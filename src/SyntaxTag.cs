using SimpleBNF.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBNF
{
    using SYNTAX = IEnumerable<ISyntaxElement>;
    public class SyntaxTag : ITag
    {
        public string Name { get; }

        protected List<SYNTAX> _SyntaxCases { get; }
        public IEnumerable<SYNTAX> SyntaxCases => _SyntaxCases;

        public SyntaxTag(string name, IEnumerable<SYNTAX> syntaxCases)
        {
            Name = name;
            _SyntaxCases = syntaxCases.ToList();
        }

        public void AddSyntax(SYNTAX syntax) => _SyntaxCases.Add(syntax);

        public bool TryMatch(ParseContext context, out Token token)
        {
            var position_bk = context.Position;
            foreach(SYNTAX syntax in SyntaxCases)
            {
                context.Position = position_bk;

                var tokens = new List<Token>();
                bool success = true;
                foreach(var element in syntax)
                {
                    if(element is ConstantElement)
                    {
                        var constant = element as ConstantElement;
                        if (context.Current.StartsWith(constant.Value))
                        {
                            context.Position += constant.Value.Length;
                            tokens.Add(new Token("Constant", constant.Value));
                        }
                        else if(!constant.IsOptional)
                        {
                            success = false;
                            break;
                        }
                    }
                    else if(element is TagElement)
                    {
                        var tagElement = element as TagElement;
                        var tag = context.Grammar[tagElement.TagName];
                        if(tag.TryMatch(context, out Token firstTagToken))
                        {
                            tokens.Add(firstTagToken);
                            if(tagElement.Repeat == TagElement.RepeatKind.NotRepeat)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (tagElement.IsOptional)
                            {
                                continue;
                            }
                            else
                            {
                                success = false;
                                break;
                            }
                        }
                        while(tag.TryMatch(context, out Token tagToken))
                        {
                            tokens.Add(tagToken);
                        }
                    }
                }
                if (success)
                {
                    if(tokens.Count > 0)
                    {
                        token = new Token(Name, Name, tokens);
                    }
                    else
                    {
                        token = null;
                    }
                    return true;
                }
            }
            token = null;
            return false;
        }
    }
}
