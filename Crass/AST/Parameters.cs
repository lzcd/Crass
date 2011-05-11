using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    public class Parameters : Node
    {
        public List<Node> Children { get; private set; }

        public Parameters()
        {
            Children = new List<Node>();
        }

        public override void Emit(Context context, StringBuilder output)
        {
            output.Append("(");
            bool firstParameter = true;
            foreach (var parameter in Children)
            {
                if (firstParameter)
                {
                    firstParameter = false;
                }
                else
                {
                    output.Append(", ");
                }
                parameter.Emit(context, output);
            }
            output.Append(")");
        }

        internal static bool TryParse(Queue<string> remainingWords, out Parameters parameters)
        {
            if (remainingWords.Peek() != "(")
            {
                parameters = null;
                return false;
            }
            remainingWords.Dequeue();

            parameters = new Parameters();
            while (remainingWords.Peek() != ")")
            {
                if (remainingWords.Peek() == ",")
                {
                    remainingWords.Dequeue();
                    continue;
                }

                Expression expression;
                if (Expression.TryParse(remainingWords, out expression))
                {
                    parameters.Children.Add(expression);
                    continue;
                }
            }
            remainingWords.Dequeue();

            return true;
        }

       
    }
}
