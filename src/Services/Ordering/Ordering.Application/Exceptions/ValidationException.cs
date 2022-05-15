using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
     
        public List<string> Errors { get; } = new List<string>();

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base("One or more validations errors has occured")
        {
            Errors = failures.Select(x => x.PropertyName + " " + x.ErrorMessage).ToList();
        }



    }
}
