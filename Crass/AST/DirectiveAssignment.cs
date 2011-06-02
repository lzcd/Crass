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
        public Node Value { get; set; }
        
        internal void TryExtend(Selector targetSelector)
        {
            var searchNode = this as Node;
            while (searchNode != null &&
                   !(searchNode is Selector))
            {
                searchNode = searchNode.Parent;
            }

            var parentSelector = searchNode as Selector;
            var expressionValue = Value as Expression;
            foreach (NamedValue namedValue in expressionValue.Children)
            {
                var criteria = namedValue.Text;

                var originalTargetSelectorNames = new List<string>(targetSelector.Names);
                foreach (var targetSelectorName in originalTargetSelectorNames)
                {
                    if (!targetSelectorName.StartsWith(criteria))
                    {
                        continue;
                    }

                    var extendedNameSuffix = targetSelectorName.Substring(criteria.Length);
                    foreach (var parentName in parentSelector.Names)
                    {
                        var extendedName = parentName + extendedNameSuffix;
                        targetSelector.Names.Add(",");
                        targetSelector.Names.Add(extendedName);
                    }
                }
            }
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            if (Value != null)
            {
                Value.Find(criteria, matching);
            }

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

            Selector selector;
            if (Selector.TryParse(directive, remainingWords, out selector))
            {
                directive.Value = selector;
            }
            else
            {
                Expression expression;
                if (!Expression.TryParse(directive, remainingWords, out expression))
                {
                    return false;
                }
                // remove ';'
                remainingWords.Dequeue();
                directive.Value = expression;
            }

            return true;
        }

        
    }
}
