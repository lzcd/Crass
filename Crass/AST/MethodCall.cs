using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class MethodCall : Node
    {
        public MethodCall(Node parent)
            : base(parent)
        {
        }

        public string Name { get; set; }
        public Parameters Parameters { get; set; }
        public Node CalculatedValue { get; set; }

        internal void Call(Host.TryCallMethodHandler methodCallHandler)
        {
            Node result;
            if (!methodCallHandler(Name, Parameters, out result))
            {
                throw new Exception("wheeeee");
            }
        }


        internal override void Emit(StringBuilder output)
        {
            CalculatedValue.Emit(output);
        }

        public override Node Clone(Node newParent)
        {
            var newMethodCall = new MethodCall(newParent);
            newMethodCall.Name = Name;
            newMethodCall.Parameters = (Parameters)Parameters.Clone(newMethodCall);
            newMethodCall.CalculatedValue = CalculatedValue.Clone(newParent);
            return newMethodCall;
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            Parameters.Find(criteria, matching);
        }

        internal static bool TryParse(Node parent, Queue<Word> remainingWords, out MethodCall methodCall)
        {
            if (remainingWords.Count < 2 ||
                remainingWords.Skip(1).First().Text != "(")
            {
                methodCall = null;
                return false;
            }

            methodCall = new MethodCall(parent);

            methodCall.Name = remainingWords.Dequeue().Text;
            Parameters parameters;
            if (!Parameters.TryParse(methodCall, remainingWords, out parameters))
            {
                methodCall = null;
                return false;
            }

            methodCall.Parameters = parameters;

            return true;
        }



      
    }
}
