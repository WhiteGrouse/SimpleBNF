using System;
using SimpleBNF;
using static SimpleBNF.Parser;

namespace CppMangledParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var grammar = GrammarLoader.Load("mangle-rule.txt");
            grammar.AddTag(new Number());
            grammar.AddTag(new SourceName());
            var parser = new Parser(grammar);
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                var tree = parser.Parse(input);
                if (tree.Status == ParseTree.ParseStatus.Success)
                {
                    print(tree.Tree, 0);
                }
                else
                {
                    Console.WriteLine("Parse fail...");
                }
            }
        }

        static void print(Token token, int indent)
        {
            string space = new string(' ', indent * 4);
            Console.WriteLine($"{space}{token.Name} => {token.Value.ToString()}");
            if (token.HasChild)
            {
                Console.WriteLine($"{space}{{");
                foreach(var childToken in token.Child)
                {
                    print(childToken, indent + 1);
                }
                Console.WriteLine($"{space}}}");
            }
        }
    }
}
