using BMOS.BAL.Helpers;
using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.Heplers;
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

            #region Phone
            RuleFor(c => c.Phone)
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .Length(10).WithMessage("Phone must be 10 characters.");
            #endregion

            #region Gender
            RuleFor(c => c.Gender)
                .Must(x => x == true || x == false).WithMessage("{PropertyName} must be either true or false.");
            #endregion

            #region Dob
            RuleFor(c => c.Dob)
                   .NotEmpty().WithMessage("{PropertyName} is empty.")
                   .NotNull().WithMessage("{PropertyName} is null.")
                   .Must(date => DateHelper.IsValidBirthday(date)).WithMessage("{PropertyName} must greater than 16 year old.")
                   .Must(date => DateHelper.IsValidBirthday(date)).WithMessage("{PropertyName} must greater than 16 year old.");
            #endregion

            #region Avatar
            RuleFor(p => p.AvatarUrl)
                   .ChildRules(pro => pro.RuleFor(img => img.Length).ExclusiveBetween(0, MAX_BYTES)
                   .WithMessage($"Avatar is required file length greater than 0 and less than {MAX_BYTES / 1024 / 1024} MB"));
            RuleFor(p => p.AvatarUrl)
                   .ChildRules(pro => pro.RuleFor(img => img.FileName).Must(FileHelper.HaveSupportedFileType).WithMessage("Avatar is required extension type .png, .jpg, .jpeg"));
            #endregion




        }
    }
}
