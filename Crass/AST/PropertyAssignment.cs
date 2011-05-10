using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class PropertyAssignment : Node
    {
        internal static bool TryParse(Queue<string> remainingWords, out PropertyAssignment property)
        {
            if (!remainingWords.Peek().EndsWith(":"))
            {
                property = null;
                return false;
            }

            property = new PropertyAssignment();
            property.Name = remainingWords.Dequeue();

            Block block;
            if (Block.TryParse(remainingWords, out block))
            {
                property.Value = block;
                return true;
            }
            Expression expression;
            if (Expression.TryParse(remainingWords, out expression))
            {
                property.Value = expression;
                return true;
            }

            property = null;
            return false;
        }

        public string Name { get; set; }
        public Node Value { get; set; }

       
    }
}
