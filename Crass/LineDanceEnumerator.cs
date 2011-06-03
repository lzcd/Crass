using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Crass
{
    class LineDanceEnumerator : IEnumerator
    {
        private List<IEnumerator> enumerators = new List<IEnumerator>();

        public LineDanceEnumerator(params IEnumerable[] enumerables)
        {
            foreach (var enumerable in enumerables)
            {
                enumerators.Add(enumerable.GetEnumerator());
            }

        }

        public bool MoveNext()
        {
            foreach (var enumerator in enumerators)
            {
                var result = enumerator.MoveNext();
                if (!result)
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<object> Currents
        {
            get
            {
                return from enumerator in enumerators
                       select enumerator.Current;
            }
        }
        public object Current
        {
            get
            {
                return Currents;
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
