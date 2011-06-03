using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class IncludeDirective : Node
    {
        public IncludeDirective(Node parent)
            : base(parent)
        {
        }

        public Node Name { get; set; }


        internal void TryInclude(MixinDefinition definition)
        {

        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            if (Name != null)
            {
                Name.Find(criteria, matching);
            }

        }

        internal static bool TryParse(Node parent, Queue<string> remainingWords, out IncludeDirective directive)
        {
            directive = null;
            if (remainingWords.Peek() != "@include")
            {
                return false;
            }

            directive = new IncludeDirective(parent);
            // remove '@include'
            remainingWords.Dequeue();


            Expression expression;
            if (!Expression.TryParse(directive, remainingWords, out expression))
            {
                throw new Exception("errp?");
            }
            // remove ';'
            if (remainingWords.Peek() == ";")
            {
                remainingWords.Dequeue();
            }
            directive.Name = expression;


            return true;
        }



       
    }
}
