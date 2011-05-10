using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.AST
{
    class Expression : Node
    {
        internal static bool TryParse(Queue<string> remainingWords, out Expression expression)
        {
            expression = new Expression();
            while (remainingWords.Peek() != ";" &&
                   remainingWords.Peek() != "," &&
                   remainingWords.Peek() != ")" &&
                   remainingWords.Peek() != "}")
            {
                Variable variable;
                if (Variable.TryParse(remainingWords, out variable))
                {
                    expression.Children.Add(variable);
                    continue;
                }

                Colour colour;
                if (Colour.TryParse(remainingWords, out colour))
                {
                    expression.Children.Add(colour);
                    continue;
                }

                Unit unit;
                if (Unit.TryParse(remainingWords, out unit))
                {
                    expression.Children.Add(unit);
                    continue;
                }

                MethodCall methodCall;
                if (MethodCall.TryParse(remainingWords, out methodCall))
                {
                    expression.Children.Add(methodCall);
                    continue;
                }

                var namedValue = new NamedValue() { Text = remainingWords.Dequeue() };
                expression.Children.Add(namedValue);
            }

            

            return true;
        }

        public List<Node> Children { get; private set; }

        public Expression()
        {
            Children = new List<Node>();
        }
    }
}
