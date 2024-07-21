using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Data.Validation;

public class ValidateIntNotZero : ValidationAttribute
{
    public string PropName { get; set; }
    public override bool IsValid(object value)
    {
        var error = $"Поле «{PropName ?? string.Empty}» обязательно к заполнению ";

        if (value != null)
        {
            var stringVal = value.ToString();

            if (!string.IsNullOrEmpty(stringVal) && !string.IsNullOrWhiteSpace(stringVal))
            {

                if (Int32.TryParse(stringVal, out int intVal))
                {
                    if (intVal < 0 || intVal > 0)
                        return true;
                    else
                    {
                        ErrorMessage = error;
                        return false;
                    }
                }
                else
                {
                    ErrorMessage = error;
                    return false;
                }
            }
            else
            {
                ErrorMessage = error;
                return false;
            }
        }
        else
        {
            ErrorMessage = error;
            return false;
        }
    }
}
