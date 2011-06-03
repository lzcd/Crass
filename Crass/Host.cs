using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crass.Ast;

namespace Crass
{
    public class Host
    {
        public string Execute(string source, Context.TryCallMethodHandler methodCallHandler)
        {
            Script script;
            if (!Parser.TryParse(source, out script))
            {
                return null;
            }

            var context = new Context() { TryCallMethod = methodCallHandler };

           

            Include(script);

            Extend(script);

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

       

    }
}
