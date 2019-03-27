using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SimpleBNF.Syntax;

namespace SimpleBNF
{
    public class GrammarLoader
    {
        public static Grammar Load(string filename)
            => new GrammarLoader(filename).Grammar;

        private GrammarLoader(string filename)
        {
            Grammar = new Grammar();
            using (var reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim() == "" || line[0] == '#')
                        continue;
                    Parse(line.Trim());
                }
            }
        }

        protected Grammar Grammar;

        public void Parse(string line)
        {
            int index = 0;
            var target = ReadTag(line, ref index, false);
            if (line.Substring(index, 3) != "::=")
                throw new Exception("フォーマットエラー");
            var syntax = ReadSyntax(line.Substring(index + 3));
            if(Grammar[target.TagName] == null)
            {
                Grammar.AddTag(new SyntaxTag(target.TagName, new[] { syntax }));
                if(Grammar.SyntaxList.Count() == 1)
                {
                    Grammar.Root = Grammar.SyntaxList.First();
                }
            }
            else
            {
                (Grammar[target.TagName] as SyntaxTag).AddSyntax(syntax);
            }
        }

        private IEnumerable<ISyntaxElement> ReadSyntax(string syntax, bool isOptional = false)
        {
            int index = 0;
            var elements = new List<ISyntaxElement>();
            while(index < syntax.Length)
            {
                if(syntax[index] == '<')
                {
                    elements.Add(ReadTag(syntax, ref index, isOptional));
                }
                else if(syntax[index] == '[')
                {
                    elements.Add(ReadOptional(syntax, ref index));
                }
                else
                {
                    elements.Add(ReadConstant(syntax, ref index));
                }
            }
            return elements;
        }

        private ISyntaxElement ReadOptional(string input, ref int index)
        {
            if (input == null || input.Length == 0)
                throw new Exception("inputが空なんだけど？");
            if (input[index] != '[')
                throw new Exception("Optionalじゃないんだけど？");

            int start = index + 1;
            if (start >= input.Length)
                throw new Exception("Optionalの中身読み取りたいんのに文字列が終わっちゃった…");

            int end = input.IndexOf(']', start);
            if (end == -1)
                throw new Exception("Optionalの終了記号が見つからないんだが…？");

            string content = input.Substring(start, end - start).Trim();
            if (content.Length == 0)
                throw new Exception("Optionalの中身が空なんだけど？");

            index = end + 1;
            var syntax = ReadSyntax(content, true);
            if (syntax.Count() == 0)
                throw new Exception("Optionalで定義されているルールが１つも存在しないんだけど？");
            if (syntax.Count() > 1)
                throw new Exception("Optionalの要素は1つまで！2つ以上はサポートしてないよ。");
            var result = syntax.First();

            return result;
        }
        private TagElement ReadTag(string input, ref int index, bool isOptional)
        {
            if (input == null || input.Length == 0)
                throw new Exception("inputが空なんだけど？");
            if (input[index] != '<')
                throw new Exception("TagElementじゃないんだけど？");

            int start = index + 1;
            if (start >= input.Length)
                throw new Exception("TagElementの中身読み取りたいのに文字列が終わっちゃった…");

            int end = input.IndexOf('>', start);
            if (end == -1)
                throw new Exception("TagElementの終了記号が見つからないんだが…？");

            string content = input.Substring(start, end - start).Trim();
            if (content.Length == 0)
                throw new Exception("TagElementの中身が空なんだけど？");

            string tag_name = content.Split(' ').Last();//FullNameはまだサポートしていません

            index = end + 1;
            var repeat = TagElement.RepeatKind.NotRepeat;
            if(index < input.Length)
            {
                if(input[index] == '+')
                {
                    repeat = TagElement.RepeatKind.Plus;
                    index += 1;
                }
                else if(input[index] == '*')
                {
                    repeat = TagElement.RepeatKind.Star;
                    index += 1;
                }
            }

            return new TagElement(tag_name, repeat, isOptional); ;
        }

        public ConstantElement ReadConstant(string input, ref int index)
        {
            if (input == null || input.Length == 0)
                throw new Exception("inputが空なんだけど？");

            int start = index;
            for(;index < input.Length; ++index)
            {
                if(input[index] == '<' || input[index] == '>' || input[index] == '[' || input[index] == ']')
                {
                    break;
                }
            }
            var constant = input.Substring(start, index - start);
            if (constant.Length == 0)
                throw new Exception("Constantの中身が空なんだけど…？");
            return new ConstantElement(constant);
        }
    }
}
