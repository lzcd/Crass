using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crass.Ast;

namespace Crass
{
    public class Context
    {
        private Dictionary<string, Node> nodeByName;

        public Context()
        {
            nodeByName = new Dictionary<string, Node>();
        }

        protected Context Parent { get; set; }

        protected Context(Context parent)
            : base()
        {
            Parent = parent;
        }
        

        internal Context CreateChild()
        {
            var child = new Context(this);
            return child;
        }

        public Node this[string name]
        {
            get
            {
                Node value;
                if (nodeByName.TryGetValue(name, out value))
                {
                    return value; 
                }

                if (Parent != null)
                {
                    return Parent[name];
                }

                throw new Exception("I dont know the value of " + name);
            }
            set
            {
                nodeByName[name] = value;
            }
        }

        public delegate bool TryCallMethodHandler(string name, Parameters parameters, out Node result);

        public TryCallMethodHandler TryCallMethod { get; set; }

        public Node Execute(string name, Parameters parameters)
        {
            Node result;
            if (TryCallMethod == null ||
                !TryCallMethod(name, parameters, out result))
            {
                throw new Exception("I don't know how to " + name);
            }

            return result;
        }

        public string PropertyPrefix = null;
        public bool EmitBraces = true;
    }
}
