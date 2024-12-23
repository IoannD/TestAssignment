using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TestAssignment.API.Validators;

namespace TestAssignment.API.Filters;

internal class FileValidationFilterAttribute(string[] allowedExtensions, long maxSizeInMb) : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var param = context.ActionArguments.SingleOrDefault(p => p.Value is IFormFile);

        if (param.Value is not IFormFile file || file.Length == 0)
        {
            context.Result = new BadRequestObjectResult("File is null or empty.");
            return;
        }

        if (!FileValidator.IsFileExtensionAllowed(file, allowedExtensions))
        {
            var allowedExtensionsMessage = string.Join(", ", allowedExtensions).Replace(".", "").ToUpper();
            context.Result = new BadRequestObjectResult("Invalid file type. " +
                                                        $"Please upload {allowedExtensionsMessage} file.");
            return;
        }

        if (!FileValidator.IsFileSizeWithinLimit(file, maxSizeInMb))
        {
            var mbSize = (double)maxSizeInMb / 1024 / 1024;
            context.Result = new BadRequestObjectResult($"File size exceeds the maximum allowed size ({mbSize} MB).");
        }
    }
}