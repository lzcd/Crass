using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class VariableAssignment : Node
    {
        public string Name { get; set; }
        public Expression Expression { get; set; }

        public override void Emit(Context context, StringBuilder output)
        {
            context[Name] = Expression;
        }

        internal static bool TryParse(Queue<string> remainingWords, out VariableAssignment assignment)
        {
            assignment = null;
            if (!remainingWords.Peek().StartsWith("$"))
            {
                return false;
            }

            var name = remainingWords.Dequeue();
            // remove ':'
            remainingWords.Dequeue();
            Expression expression;
            if (!Expression.TryParse(remainingWords, out expression))
            {
                return false;
            }
            // remove ';'
            remainingWords.Dequeue();
            assignment = new VariableAssignment() { Name = name, Expression = expression };
            return true;
        }

       
    }
}
