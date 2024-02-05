﻿using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Validators.Reports
{
    public class AddReportValidation: AbstractValidator<AddReportRequest>
    {
        private const int MAX_BYTES = 2048000;

        public AddReportValidation()
        {
            #region ReportName
            RuleFor(p => p.ReportName)
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .Length(5, 120).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            #endregion

            /*#region CreateTime
            RuleFor(p => p.CreateTime)
                   .NotNull().WithMessage("{PropertyName} is null.")
                   .Custom((expiredDate, context) =>
                   {
                       if (expiredDate.Date.CompareTo(DateTime.Now.Date) < 0)
                       {
                           context.AddFailure("CreateTime must be greater than or equal today.");
                       }
                   });
            #endregion*/

            /*#region UpdateTime
            RuleFor(p => p.UpdateTime)
                   .NotNull().WithMessage("{PropertyName} is null.")
                   .Custom((expiredDate, context) =>
                   {
                       if (expiredDate.Date.CompareTo(DateTime.Now.Date) < 0)
                       {
                           context.AddFailure("UpdateTime must be greater than or equal today.");
                       }
                   });
            #endregion*/

            //#region ProductImages
            //RuleFor(p => p.Image)
            //       .NotNull().WithMessage("{PropertyName} is null.")
            //       .NotEmpty().WithMessage("{PropertyName} is empty.");
            //RuleForEach(p => p.Image)
            //       .ChildRules(pro => pro.RuleFor(img => img.Length).ExclusiveBetween(0, MAX_BYTES)
            //       .WithMessage($"Image is required file length greater than 0 and less than {MAX_BYTES / 1024 / 1024} MB"));
            //RuleForEach(p => p.Image)
            //       .ChildRules(pro => pro.RuleFor(img => img.FileName).Must(FileHelper.HaveSupportedFileType).WithMessage("Product Image is required extension type .png, .jpg, .jpeg"));
            //#endregion


        }
    }
}
