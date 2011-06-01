using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    public abstract class Node
    {
        public abstract void Find(Func<Node, bool> criteria, List<Node> matching);
    }
}
