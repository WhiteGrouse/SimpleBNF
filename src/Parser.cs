using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleBNF
{
    public class Parser
    {
        public class ParseTree
        {
            public enum ParseStatus
            {
                Success,
                Fail
            }

            public ParseStatus Status { get; }
            public Token Tree { get; }

            public ParseTree(ParseStatus status, Token tree)
            {
                Status = status;
                Tree = tree;
            }
        }

        public Grammar Grammar { get; }
        
        public Parser(Grammar grammar)
        {
            Grammar = grammar;
        }

        public ParseTree Parse(string target)
        {
            var context = new ParseContext(Grammar, target);
            if(Grammar.Root.TryMatch(context, out Token token))
            {
                return new ParseTree(ParseTree.ParseStatus.Success, token);
            }
            else
            {
                return new ParseTree(ParseTree.ParseStatus.Fail, null);
            }
        }
    }
}
