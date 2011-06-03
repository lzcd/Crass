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


        public abstract void Find(Func<Node, bool> criteria, List<Node> matching);
        public abstract Node Clone(Node newParent);

        internal virtual void Emit(StringBuilder output)
        {
            
        }
    }
}
