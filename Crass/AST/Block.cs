using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class Block : Node
    {
        internal static bool TryParse(Queue<string> remainingWords, out Block block)
        {
            if (remainingWords.Peek() != "{")
            {
                block = null;
                return false;
            }

            block = new Block();

            return true;
        }

        public List<Node> Children { get; private set; }

        public Block()
        {
            Children = new List<Node>();
        }
    }
}
