using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class VariableExpression : Node
    {
        internal static bool TryParse(Queue<string> remainingWords, out VariableExpression node)
        {
            node = new VariableExpression();
            var nextWord = remainingWords.Peek();
            while (nextWord != ";" && remainingWords.Count > 0)
            {
                var word = remainingWords.Dequeue();
                node.Children.Add(new Node());
                nextWord = remainingWords.Peek();
            }
            if (nextWord == ";")
            {
                remainingWords.Dequeue();
            }
            return true;
        }

        public List<Node> Children { get; private set; }

        public VariableExpression()
        {
            Children = new List<Node>();
        }
    }
}
