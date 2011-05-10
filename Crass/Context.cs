using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crass.AST;

namespace Crass
{
    class Context
    {
        private Dictionary<string, Node> nodeByName;

        public Context()
        {
            nodeByName = new Dictionary<string, Node>();
        }

        public Node this[string name]
        {
            get
            {
                return nodeByName[name];
            }
            set
            {
                nodeByName[name] = value;
            }
        }
    }
}
