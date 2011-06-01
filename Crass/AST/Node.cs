using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    public abstract class Node
    {
        public Node Parent;

        public Node(Node parent)
        {
            Parent = parent;
        }

        protected virtual void AppendChildIndendation(StringBuilder output)
        {
            if (Parent != null)
            {
                Parent.AppendChildIndendation(output);
            }
        }

        public abstract void Find(Func<Node, bool> criteria, List<Node> matching);

        internal virtual void Emit(StringBuilder output)
        {
            
        }
    }
}
