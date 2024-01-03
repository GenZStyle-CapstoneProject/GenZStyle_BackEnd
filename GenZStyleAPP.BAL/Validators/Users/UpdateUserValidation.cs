using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Validators.Users
{
    public class UpdateUserValidation: AbstractValidator<UpdateUserRequest>
    {
        private const int MAX_BYTES = 2048000;
        public UpdateUserValidation()
        {
            #region City
            RuleFor(p => p.City)
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .Length(5, 120).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            #endregion

            #region Address
            RuleFor(p => p.Address)
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .Length(5, 1000).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters.");
            #endregion

            

            #region Dob
            RuleFor(p => p.Dob)
                   .NotNull().WithMessage("{PropertyName} is null.")
                   .Custom((expiredDate, context) =>
                   {
                       if (expiredDate.Date.CompareTo(DateTime.Now.Date) < 0)
                       {
                           context.AddFailure("Expired Date must be greater than or equal today.");
                       }
                   });
            #endregion

            

           

            
        }
    }
}
