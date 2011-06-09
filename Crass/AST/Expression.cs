using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class Expression : Node
    {
        public List<Node> Children { get; private set; }

        public Expression(Node parent)
            : base(parent)
        {
            Children = new List<Node>();
        }

        public override Node Clone(Node newParent)
        {
            var newExpression = new Expression(newParent);
            foreach (var child in Children)
            {
                newExpression.Children.Add(child.Clone(newExpression));
            }
            return newExpression;
        }

        internal override void Emit(StringBuilder output)
        {
            bool isFirst = true;
            foreach (var child in Children)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    output.Append(" ");
                }

                child.Emit(output);
            }
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            foreach (var child in Children)
            {
                child.Find(criteria, matching);
            }
        }

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out Expression expression)
        {
            expression = new Expression(parent);
            while (remainingWords.Count > 0 &&
                    remainingWords.Peek().Text != ";" &&
                   remainingWords.Peek().Text != "," &&
                   remainingWords.Peek().Text != ")" &&
                   remainingWords.Peek().Text != "}")
            {
                Add add;
                if (Add.TryParse(expression, remainingWords, out add))
                {
                    expression.Children.Add(add);
                }

                Subtract subtract;
                if (Subtract.TryParse(expression, remainingWords, out subtract))
                {
                    expression.Children.Add(subtract);
                }
                
                Multiply multiply;
                if (Multiply.TryParse(expression, remainingWords, out multiply))
                {
                    expression.Children.Add(multiply);
                }
                 
                Divide divide;
                if (Divide.TryParse(expression, remainingWords, out divide))
                {
                    expression.Children.Add(divide);
                }

                Variable variable;
                if (Variable.TryParse(expression, remainingWords, out variable))
                {
                    expression.Children.Add(variable);
                    continue;
                }

                Colour colour;
                if (Colour.TryParse(expression, remainingWords, out colour))
                {
                    expression.Children.Add(colour);
                    continue;
                }

                Unit unit;
                if (Unit.TryParse(expression, remainingWords, out unit))
                {
                    expression.Children.Add(unit);
                    continue;
                }

                MethodCall methodCall;
                if (MethodCall.TryParse(expression, remainingWords, out methodCall))
                {
                    expression.Children.Add(methodCall);
                    continue;
                }

                if (remainingWords.Count > 0)
                {
                    var namedValue = new NamedValue(expression) { Text = remainingWords.Dequeue().Text };
                    expression.Children.Add(namedValue);
                }
            }



            return true;
        }


    }
}
