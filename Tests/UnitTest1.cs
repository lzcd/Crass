using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crass;
using Crass.Ast;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void DoesDoVariableExample()
        {
            var host = new Host();
            var source =
@"$blue: #3bbfce;
$margin: 16px;

.content-navigation {
  border-color: $blue;
  color:
    darken($blue, 9%);
}

.border {
  padding: $margin / 2;
  margin: $margin / 2;
  border-color: $blue;
}
";
            var expected =
@".content-navigation {
  border-color: #3bbfce;
  color: #2b9eab;
}

.border {
  padding: 8px;
  margin: 8px;
  border-color: #3bbfce;
}
";
            var result = host.Execute(source,
                (string name, Parameters parameters, out Node node) =>
                {
                    node = new Colour(parameters.Parent) { Text = "#2b9eab" };
                    return true;
                });
        }

        [TestMethod]
        public void DoesDoNestingExample()
        {
            var host = new Host();
            var source =
@"table.hl {
  margin: 2em 0;
  td.ln {
    text-align: right;
  }
}

li {
  font: {
    family: serif;
    weight: bold;
    size: 1.2em;
  }
}
";
            var expected =
@"table.hl {
  margin: 2em 0;
}
table.hl td.ln {
  text-align: right;
}

li {
  font-family: serif;
  font-weight: bold;
  font-size: 1.2em;
}
";
            var result = host.Execute(source, null);
        }

        [TestMethod]
        public void DoesDoMixinsExample()
        {
            var host = new Host();
            var source =
@"@mixin table-base {
  th {
    text-align: center;
    font-weight: bold;
  }
  td, th {padding: 2px}
}

@mixin left($dist) {
  float: left;
  margin-left: $dist;
}

#data {
  @include left(10px);
  @include table-base;
}
";

            var expected =
@"#data {
  float: left;
  margin-left: 10px;
}
#data th {
  text-align: center;
  font-weight: bold;
}
#data td, #data th {
  padding: 2px;
}
";
        }

        [TestMethod]
        public void DoesDoInheritanceExample()
        {
            var host = new Host();
            var source =
@".error {
  border: 1px #f00;
  background: #fdd;
}
.error.intrusion {
  font-size: 1.3em;
  font-weight: bold;
}

.badError {
  @extend .error;
  border-width: 3px;
}
";

            var expected =
@".error, .badError {
  border: 1px #f00;
  background: #fdd;
}

.error.intrusion,
.badError.intrusion {
  font-size: 1.3em;
  font-weight: bold;
}

.badError {
  border-width: 3px;
}";
        }
    }
}
