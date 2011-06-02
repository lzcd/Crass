﻿using System;
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

            var selectors = new List<Node>();
            script.Find(n => (n is Selector), selectors);


            var extensions = new List<Node>();
            script.Find(n => (n is DirectiveAssignment && 
                                ((DirectiveAssignment)n).Name == "extend" ), 
                                extensions);

            foreach (DirectiveAssignment extension in extensions)
            {
                foreach (Selector selector in selectors)
                {
                    extension.TryExtend(selector);
                }
            }

            var output = new StringBuilder();

            foreach (var selector in selectors)
            {
                selector.Emit(output);
            }
            return output.ToString();
        }

       

    }
}
