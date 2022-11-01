using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.Core.ValidationAttributes
{
    public class EmptyGuid : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            Console.WriteLine(value);
            if (value == null ||
                value.GetType() != typeof(Guid) ||
                Guid.Parse(value.ToString()) == Guid.Empty)
            {
                return false;
            }

            return true;
        }
    }
}
