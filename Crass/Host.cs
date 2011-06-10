using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crass.Ast;

namespace Crass
{
    public class Host
    {
        public delegate bool TryCallMethodHandler(string name, Parameters parameters, out Node result);
        public TryCallMethodHandler TryCallMethod { get; set; }

        public string Execute(string source, TryCallMethodHandler methodCallHandler)
        {
            Script script;
            if (!Parser.TryParse(source, out script))
            {
                return null;
            }

            Include(script);
            Extend(script);
            ApplyVariableValues(script);
            CallMethods(script, methodCallHandler);
            ApplyOperators(script);
            return Emit(script);
        }

       

        private static string Emit(Script script)
        {
            var selectors = new List<Node>();
            script.Find(n => (n is Selector), selectors);

            var output = new StringBuilder();

            foreach (var selector in selectors)
            {
                selector.Emit(output);
            }
            return output.ToString();
        }

        private void ApplyOperators(Script script)
        {
            var ops = new List<Node>();
            script.Find(n => (n is IOperator), ops);

            foreach (IOperator op in ops)
            {
                op.Operate();
            }
        }

        private static void Include(Script script)
        {
            var mixinDefintions = new List<Node>();
            script.Find(n => (n is MixinDefinition), mixinDefintions);

            var includeDirectives = new List<Node>();
            script.Find(n => (n is IncludeDirective), includeDirectives);

            foreach (IncludeDirective directive in includeDirectives)
            {
                foreach (MixinDefinition definition in mixinDefintions)
                {
                    directive.TryInclude(definition);
                }
            }

            foreach (var definition in mixinDefintions)
            {
                var scriptParent = (Script)definition.Parent;
                var mixinIndex = scriptParent.Children.IndexOf(definition);
                scriptParent.Children.RemoveAt(mixinIndex);
            }

            foreach (var directive in includeDirectives)
            {
                var blockParent = (Block)directive.Parent;
                var directiveIndex = blockParent.Children.IndexOf(directive);
                blockParent.Children.RemoveAt(directiveIndex);
            }
        }

        private static void Extend(Script script)
        {
            var selectors = new List<Node>();
            script.Find(n => (n is Selector), selectors);

            var extensions = new List<Node>();
            script.Find(n => (n is ExtendDirective), extensions);

            foreach (ExtendDirective extension in extensions)
            {
                foreach (Selector selector in selectors)
                {
                    extension.TryExtend(selector);
                }
            }
        }

        private static void ApplyVariableValues(Script script)
        {
            var nodes = new List<Node>();
            script.Find(n => (n is IVariableApplicable), nodes);
            var variableApplicables = nodes.Cast<IVariableApplicable>();

            var valueByName = new Dictionary<string, Node>();
            foreach (var variableApplicable in variableApplicables)
            {
                variableApplicable.Apply(valueByName);
            }
        }

        private static void CallMethods(Script script, TryCallMethodHandler methodCallHandler)
        {
            var methodCalls = new List<Node>();
            script.Find(n => (n is MethodCall), methodCalls);

            foreach (MethodCall methodCall in methodCalls)
            {
                methodCall.Call(methodCallHandler);
            }
        }
    }
}
