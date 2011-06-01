using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Selector : Node
    {
        public List<string> Names { get; private set; }
        public Block Block { get; set; }

        public Selector(Node parent)
            : base(parent)
        {
            Names = new List<string>();
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }
            Block.Find(criteria, matching);
        }

        internal static bool TryParse(Node parent, Queue<string> remainingWords, out Selector selector)
        {
            selector = new Selector(parent);
            while (remainingWords.Peek() != "{")
            {
                selector.Names.Add(remainingWords.Dequeue());
            }
            Block block;
            if (!Block.TryParse(selector, remainingWords, out block))
            {
                selector = null;
                return false;
            }
            selector.Block = block;
            return true;
        }


    }
}
