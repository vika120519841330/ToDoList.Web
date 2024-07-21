﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using ToDoList.Web.Data.DTO;
using ToDoList.Web.Enums;
using ToDoList.Web.Helpers;

namespace ToDoList.Web.Components.Pages.Base;

public class EditFormBase<T> : ComponentBaseClass<T> 
    where T : class, ICloneable, new()
{
    [Parameter]
    public T TItemFromParent { get; set; }

    protected EditContext EditContext;

    private T model;
    public T Model
    {
        get => model;
        set
        {
            model = value;
            if (model != null)
                EditContext = new(model);
            else
                EditContext = null;
        }
    }

    protected Type ModelsType => Model?.GetType();

    [Parameter]
    public ActionType? ActionTypeValue { get; set; }

    [Parameter]
    public EventCallback InvokeParentHandlerCancel { get; set; }

    [Parameter]
    public EventCallback<T> InvokeParentHandlerSuccess { get; set; }

    [Parameter]
    public EventCallback<string> InvokeParentHandlerError { get; set; }

    public virtual bool IsCreate => ActionTypeValue.HasValue && (byte)ActionTypeValue.Value == (byte)ActionType.create;

    protected string LabelListEmpty => "Список пустой";

    protected string LabelNotSelected => "Ничего не выбрано";

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        ResetValues();
        IsRender = true;
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        if (parameters.TryGetValue<ActionType?>(nameof(ActionTypeValue), out var value))
        {
            if (value is null)
                ActionTypeValue = ActionType.update;
            else
                ActionTypeValue = value;
        }

        await base.SetParametersAsync(parameters);
    }

    protected virtual void ResetValues()
    {
        InitModel();
        ValidateModel();
    }

    protected virtual void InitModel()
    {
        if (IsCreate)
            Model = new();
        else
            Model = TItemFromParent != null ? TItemFromParent.Clone() as T : new();
    }

    protected virtual void ValidateModel()
    {
        var validResult = Model?.ValidateModel();
        var errors = string.Empty;
        if(validResult.HasValue)
        {
            validResult.Value.errors?.ForEach(error => errors += error);
        }

        IsModelValid = validResult.HasValue && validResult.Value.isValid;
        ValidErrors = validResult.HasValue ? errors : "Модель не валидная";
    }

    protected virtual bool IsModelValid { get; private set; }

    protected virtual string ValidErrors { get; private set; }

    protected virtual async Task DoAction() => await Task.FromResult(true);
}