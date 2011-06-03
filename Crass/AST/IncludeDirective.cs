using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass.Ast
{
    class IncludeDirective : Node
    {
        public IncludeDirective(Node parent)
            : base(parent)
        {
        }

        public Expression Name { get; set; }


        internal void TryInclude(MixinDefinition definition)
        {
            MethodCall includeMethodCall;
            MethodCall mixinSignature;
            if (TryMatchMethodCalls(definition, out includeMethodCall, out mixinSignature))
            {
                var targetBlock = Parent as Block;
                var sourceBlock = definition.Value as Block;
                foreach (var child in sourceBlock.Children)
                {
                    var newChild = child.Clone(targetBlock);
                    targetBlock.Children.Add(newChild);
                }
               
            }

            NamedValue includeName;
            NamedValue mixinName;
            if (TryMatchNamedValues(definition, out includeName, out mixinName))
            {
                var targetBlock = Parent as Block;
                var sourceBlock = definition.Value as Block;
                foreach (var child in sourceBlock.Children)
                {
                    var newChild = child.Clone(targetBlock);
                    targetBlock.Children.Add(newChild);
                }

            }
        }

        private bool TryMatchNamedValues(
            MixinDefinition definition,
            out NamedValue includeName,
            out NamedValue mixinName)
        {
            includeName = Name.Children.First() as NamedValue;
            if (includeName == null)
            {
                includeName = null;
                mixinName = null;
                return false;
            }
            mixinName = definition.Name.Children.First() as NamedValue;
            if (mixinName == null)
            {
                includeName = null;
                mixinName = null;
                return false;
            }

            if (includeName.Text != mixinName.Text)
            {
                includeName = null;
                mixinName = null;
                return false;
            }

            return true;
        }

        private bool TryMatchMethodCalls(
            MixinDefinition definition, 
            out MethodCall includeMethodCall, 
            out MethodCall mixinSignature)
        {
            includeMethodCall = Name.Children.First() as MethodCall;
            if (includeMethodCall == null)
            {
                includeMethodCall = null;
                mixinSignature = null;
                return false;
            }
            mixinSignature = definition.Name.Children.First() as MethodCall;
            if (mixinSignature == null)
            {
                includeMethodCall = null;
                mixinSignature = null;
                return false;
            }

            if (includeMethodCall.Name != mixinSignature.Name)
            {
                includeMethodCall = null;
                mixinSignature = null;
                return false;
            }

            return true;
        }

        public override Node Clone(Node newParent)
        {
            var newDirective = new IncludeDirective(newParent);
            newDirective.Name = (Expression)Name.Clone(newDirective);
            return newDirective;
        }

        public override void Find(Func<Node, bool> criteria, List<Node> matching)
        {
            if (criteria(this))
            {
                matching.Add(this);
            }

            if (Name != null)
            {
                Name.Find(criteria, matching);
            }

        }

        internal static bool TryParse(Node parent, Queue<string> remainingWords, out IncludeDirective directive)
        {
            directive = null;
            if (remainingWords.Peek() != "@include")
            {
                return false;
            }

            directive = new IncludeDirective(parent);
            // remove '@include'
            remainingWords.Dequeue();


            Expression expression;
            if (!Expression.TryParse(directive, remainingWords, out expression))
            {
                throw new Exception("errp?");
            }
            // remove ';'
            if (remainingWords.Peek() == ";")
            {
                remainingWords.Dequeue();
            }
            directive.Name = expression;


            return true;
        }



       
    }
}
