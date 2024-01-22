using FluentValidation;
using GenZStyleApp.BAL.Helpers;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Validators.Products
{
    public class AddProductValidation: AbstractValidator<AddProductRequest>
    {
        private const int MAX_BYTES = 2048000;

        public AddProductValidation()
        {
            #region Name
            RuleFor(p => p.Name)
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .Length(5, 120).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            #endregion

            #region Color
            RuleFor(p => p.Color)
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .Length(2, 120).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            #endregion

            #region Size
            RuleFor(p => p.Size)
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .Length(1, 120).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            #endregion

            #region Gender
            RuleFor(c => c.Gender)
                .Must(x => x == true || x == false).WithMessage("{PropertyName} must be either true or false.");
            #endregion

            #region Height
            RuleFor(p => p.Cost)
                   .NotNull().WithMessage("{PropertyName} is null.")
                   .NotEmpty().WithMessage("{PropertyName} is empty.")
                   .ExclusiveBetween(0, 1000000000).WithMessage("{PropertyName} must be greater than 0 cm.");
            #endregion

            #region Image
            RuleFor(p => p.Image)
                   .ChildRules(pro => pro.RuleFor(img => img.Length).ExclusiveBetween(0, MAX_BYTES)
                   .WithMessage($"Avatar is required file length greater than 0 and less than {MAX_BYTES / 1024 / 1024} MB"));
            RuleFor(p => p.Image)
                   .ChildRules(pro => pro.RuleFor(img => img.FileName).Must(FileHelper.HaveSupportedFileType).WithMessage("Avatar is required extension type .png, .jpg, .jpeg"));
            #endregion
        }
    }
}
