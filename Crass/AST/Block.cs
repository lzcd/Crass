using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class Block : Node
    {
        public List<Node> Children { get; private set; }

        public Block()
        {
            Children = new List<Node>();
        }

        public override void Emit(Context context, StringBuilder output)
        {
            throw new NotImplementedException();
        }

        internal static bool TryParse(Queue<string> remainingWords, out Block block)
        {
            if (remainingWords.Peek() != "{")
            {
                block = null;
                return false;
            }
            remainingWords.Dequeue();

            block = new Block();
            while (remainingWords.Peek() != "}")
            {
                PropertyAssignment property;
                if (PropertyAssignment.TryParse(remainingWords, out property))
                {
                    block.Children.Add(property);
                    continue;
                }
            }
            remainingWords.Dequeue();

            return true;
        }

       
    }
}
