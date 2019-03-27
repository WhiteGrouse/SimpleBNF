using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBNF
{
    public class Grammar
    {
        public ITag Root { get; set; }

        protected List<ITag> _SyntaxList { get; } = new List<ITag>();
        public IEnumerable<ITag> SyntaxList => _SyntaxList;

        public Grammar() { }

        public Grammar(ITag root, IEnumerable<ITag> syntaxList)
        {
            Root = root;
            _SyntaxList = syntaxList.ToList();
            //TODO: Validate it
        }

        public ITag this[string tag] => SyntaxList.FirstOrDefault(d=>d.Name == tag);

        public void AddTag(ITag tag)
        {
            if (_SyntaxList.Exists(d => d.Name == tag.Name))
                throw new Exception("同名のTagが既に登録されています");

            _SyntaxList.Add(tag); 
        }
    }
}
