using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class DirectiveAssignment : Node
    {
        public DirectiveAssignment(Node parent)
            : base(parent)
        {
        }

        public string Name { get; set; }
        public Expression Expression { get; set; }

        internal void TryExtend(Selector targetSelector)
        {
            var parentSelector = this as Node;
            while (parentSelector != null &&
                   !(parentSelector is Selector))
            {
                parentSelector = parentSelector.Parent;
            }

            
            foreach (NamedValue namedValue in Expression.Children)
            {
                var criteria = namedValue.Text;

                foreach (var targetSelectorName in targetSelector.Names)
                {
                    if (!targetSelectorName.StartsWith(criteria))
                    {
                        continue;
                    }

                    var extendedName = targetSelectorName.Substring(criteria.Length);

                }
            }
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            Expression.Find(criteria, matching);
        }

        internal static bool TryParse(Node parent, Queue<string> remainingWords, out DirectiveAssignment directive)
        {
            directive = null;
            if (!remainingWords.Peek().StartsWith("@"))
            {
                return false;
            }

            directive = new DirectiveAssignment(parent);
            directive.Name = remainingWords.Dequeue().Substring(1);

            Expression expression;
            if (!Expression.TryParse(directive, remainingWords, out expression))
            {
                return false;
            }
            // remove ';'
            remainingWords.Dequeue();
            directive.Expression = expression;

            return true;
        }

        
    }
}
