using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class VariableAssignment : Node
    {
        internal static bool TryParse(Queue<string> remainingWords, out VariableAssignment assignment)
        {
            assignment = null;
            if (!remainingWords.Peek().StartsWith("$"))
            {
                return false;
            }

            var name = remainingWords.Dequeue();

            VariableExpression expression;
            if (!VariableExpression.TryParse(remainingWords, out expression))
            {
                return false;
            }

            assignment = new VariableAssignment() { Name = name, Expression = expression };
            return true;
        }

        public string Name { get; set; }

        public VariableExpression Expression { get; set; }

    }
}
