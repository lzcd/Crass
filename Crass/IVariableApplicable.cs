using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crass
{
    interface IVariableApplicable
    {
        void Apply(Dictionary<string, Ast.Node> valueByName);
    }
}
