using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework.Sparse_matrices
{
    // Exeption for matrices that cannot ne added
    internal class CannotBeAdded : Exception
    {
        internal CannotBeAdded() : base("Cannot be added") { }
        internal CannotBeAdded(string expMessage) : base(expMessage) { }

    }
    // Exeption for matrices that cannot ne multiplied
    internal class CannotBeMultiplied : Exception
    {
        internal CannotBeMultiplied() : base("Cannot be multiplied") { }
        internal CannotBeMultiplied(string expMessage) : base(expMessage) { }
    }
}
