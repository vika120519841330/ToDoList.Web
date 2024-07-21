using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace ToDoList.Web.Helpers;

public static class ValidationHelper
{
    public static (bool isValid, List<string> errors) ValidateModel(this object model)
    {
        if (model == null || model == default)
            return default;

        var isModelValid = true;
        List<string> errorsMessages = new();
        List<ValidationResult> validResults = new();

        ValidationContext context = new(model);

        var modelIsValid = Validator.TryValidateObject(model, context, validResults, true);

        if (!modelIsValid)
        {
            foreach (var error in validResults)
            {
                errorsMessages.Add(error.ErrorMessage);
            }

            isModelValid = false;
        }
        else
        {
            var editContext = new EditContext(model);
            errorsMessages = editContext?.GetValidationMessages().ToList() ?? new();
        }

        return (isModelValid, errorsMessages);
    }
}
