using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    public record CustomerId
    {
        public Guid Value { get; set; }
        private CustomerId(Guid Id) => Value = Id;

        public static CustomerId Of(Guid value)  {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("CustomerId cannot be empty.");
            }
            return new CustomerId(value);
        }

    }
}
