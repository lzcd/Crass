using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Script : Node
    {
        public List<Node> Children { get; set; }

        public Script(Node parent)
            : base(parent)
        {
            Children = new List<Node>();
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
            foreach (var child in Children)
            {
                child.Find(criteria, matching);
            }
        }



        internal static bool TryParse(Node parent, Queue<string> remainingWords, out Script script)
        {
            script = new Script(parent);

            do
            {
                DirectiveAssignment directiveAssigment;
                if (DirectiveAssignment.TryParse(script, remainingWords, out directiveAssigment))
                {
                    script.Children.Add(directiveAssigment);
                }

                VariableAssignment variableAssignment;
                if (VariableAssignment.TryParse(script, remainingWords, out variableAssignment))
                {
                    script.Children.Add(variableAssignment);
                    continue;
                }

                Selector selector;
                if (Selector.TryParse(script, remainingWords, out selector))
                {
                    script.Children.Add(selector);
                    continue;
                }
            }
            while (remainingWords.Count > 0);

            return true;
        }




    }
}
