using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class NamedValue : Node
    {
        public NamedValue(Node parent)
            : base(parent)
        {
        }

        public string Text { get; set; }

        internal override void Emit(StringBuilder output)
        {
            output.Append(Text);
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
        }
    }
}
