using Blazor.Controls.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
namespace Blazor.Controls
{
    public class TextInputBase<TValue> : InputBase<TValue>, IDisposable
    {

        [Parameter]
        public bool VerticalFocus { get; set; } = true;


        public bool WasRounded { get; set; }

        [Parameter]
        public bool EnableToastMessage { get; set; }

        [Parameter]
        public string ToastMessage { get; set; }

        [Inject] public Blazor.Controls.Toast.IToastService Toast { get; set; }

        [Parameter]
        public int Min { get; set; } = 0;


        [Parameter]
        public int Max { get; set; }

        [Parameter]
        public RoundType DecimalRoundType { get; set; } = RoundType.Normal;


        [Parameter]
        public bool ChangeBackground { get; set; } = true;

        [Parameter]
        public string CssClassDecimal { get; set; }


        [Parameter]
        public int DecimalNumbers { get; set; } = 1;

        [Parameter]
        public string StepResol { get; set; } = "any";

        public ElementReference SelfControl { get; set; }

        [Parameter] public string Style { get; set; } = "top:50%";



        [Parameter] public object Model { get; set; }

        [Parameter] public bool IsDisabled { get; set; } = false;



        DateTime _CurrentDateTime = new DateTime();
        [Parameter]
        public DateTime CurrentDateTime
        {
            get
            {

                return _CurrentDateTime;

            }
            set
            {

                var valor = value;

                //DateTime.TryParse(value.ToString(), DateTimeStyles.AssumeLocal, CultureInfo.DefaultThreadCurrentCulture, out var parsedValue);

                _CurrentDateTime = valor;

                CurrentValueAsString = valor.ToString();


            }
        }

        [Parameter] public string CSSClass { get; set; } = "form-control";

        [Parameter] public string DivCSS { get; set; } = "form-group";//form-control-label

        [Parameter] public string LabelCSS { get; set; } = "form-control-label"; // "form-control-placeholder";//form-control-label

        [Parameter] public bool LabelDown { get; set; } = false;

        //[Parameter] public string Disabled { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public bool ShowDefaultOption { get; set; } = true;

        [Parameter]
        public EventCallback<ChangeEventArgs> OnChangeControl { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnChangeObjectControl { get; set; }

        [Parameter] public string Id { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public Expression<Func<TValue>> ValidationFor { get; set; }

        [Parameter] public string ErrorMessage { get; set; }

        [Parameter] public string Type { get; set; }

        [Parameter] public bool ShowSystemErrorMessages { get; set; } = true;


        [Parameter] public bool Validate { get; set; } = true;

        [Parameter]
        public EventCallback<EventArgs> TextValueChanged { get; set; }
        [Parameter] public string CSSValidationMessage { get; set; } = "validation-message";


        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()

        {
            if (Validate == false || ValidationFor == null || !string.IsNullOrEmpty(ErrorMessage))
            {
                return;
            }


            var vali = ValidationFor;

            MemberExpression body = vali.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)vali.Body;
                body = ubody.Operand as MemberExpression;
            }

            var PropertyName = body.Member.Name;

            var ec = this.EditContext;
            try
            {

                var entity = ec.Model.GetType();
                Type type = entity;
                PropertyInfo prop = type.GetProperty(PropertyName);

                if (prop == null)
                {

                    //Logger.LogDebug("");
                    return;
                }



                FieldIdentifier ID = new FieldIdentifier(ec.Model, PropertyName);

                //ec.NotifyValidationStateChanged();
                //ec.NotifyFieldChanged(ID);


                var message = ec.GetValidationMessages(ID);
                string msg = "";
                foreach (var item in message)
                {
                    msg += item + " ";
                }
                if (!string.IsNullOrEmpty(msg))
                {
                    ErrorMessage = msg;
                }
                else
                {
                    ErrorMessage = "";
                }



            }
            catch (Exception ex)
            {
                if (ShowSystemErrorMessages)
                {
                    ErrorMessage = ex.Message;
                }

                // throw new Exception("Invalid Value");


            }

            await base.OnParametersSetAsync();

            //throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TValue)}'.");






        }


        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        return;
        //    }
        //    await base.OnAfterRenderAsync(firstRender);



        //}
        public Type TypeValue { get; set; }


        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            //var field = this.FieldIdentifier;
            //var _fieldIdentifier = FieldIdentifier.Create(ValidationFor);

            TypeValue = typeof(TValue);

            TValue result1 = default(TValue);

            if (typeof(TValue) == typeof(string))
            {
                result1 = (TValue)(object)value;
                validationErrorMessage = null;

            }
            else if (typeof(TValue) == typeof(decimal))
            {
                var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                clone.NumberFormat.NumberDecimalSeparator = ".";
                clone.NumberFormat.CurrencyDecimalSeparator = ".";
                CultureInfo.CurrentUICulture = clone;
                //clone.NumberFormat.NumberGroupSeparator = ".";
                //Logger.LogDebug("CultureInfo.DefaultThreadCurrentCulture.NumberFormat.NumberDecimalSeparator");
                //Logger.LogDebug(CultureInfo.DefaultThreadCurrentCulture.NumberFormat.NumberDecimalSeparator);

                decimal.TryParse(value, NumberStyles.Any, CultureInfo.DefaultThreadCurrentCulture, out var parsedValue);
                result1 = (TValue)(object)parsedValue;
                validationErrorMessage = null;

            }

            else if (typeof(TValue) == typeof(int))
            {
                int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
                result1 = (TValue)(object)parsedValue;
                validationErrorMessage = null;


            }
            else if (typeof(TValue) == typeof(Guid))
            {
                Guid.TryParse(value, out var parsedValue);
                result1 = (TValue)(object)parsedValue;
                validationErrorMessage = null;


            }
            else if (typeof(TValue) == typeof(double))
            {
                //string a = String.Format("{0,12:N3}", value);
                double.TryParse(value, NumberStyles.Any, CultureInfo.DefaultThreadCurrentCulture, out var parsedValue);

                if (parsedValue == 0)
                {
                    parsedValue = 0.00;
                }
                result1 = (TValue)(object)parsedValue;


                validationErrorMessage = null;

            }
            else if (typeof(TValue) == typeof(int?))
            {

                int.TryParse(value, NumberStyles.Any, CultureInfo.DefaultThreadCurrentCulture, out var parsedValue);
                result1 = (TValue)(object)parsedValue;
                validationErrorMessage = null;

            }
            else if (typeof(TValue) == typeof(short))
            {

                short.TryParse(value, NumberStyles.Any, CultureInfo.DefaultThreadCurrentCulture, out var parsedValue);
                result1 = (TValue)(object)parsedValue;
                validationErrorMessage = null;

            }
            else if (typeof(TValue) == typeof(DateTime))
            {

                DateTime.TryParse(value, out var parsedValue);
                result1 = (TValue)(object)parsedValue;
                validationErrorMessage = null;

            }
            else if (typeof(TValue).IsEnum)
            {
                // There's no non-generic Enum.TryParse (https://github.com/dotnet/corefx/issues/692)
                try
                {
                    result1 = (TValue)Enum.Parse(typeof(TValue), value);
                    validationErrorMessage = null;


                }
                catch (ArgumentException)
                {
                    result1 = default;
                    validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";


                }
            }

            


            if (!Validate || ValidationFor == null)
            {
                result = result1;
                validationErrorMessage = null;
                ExecuteCall(result1);
                return true;
            }
            result = default;

            var vali = ValidationFor;

            MemberExpression body = vali.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)vali.Body;
                body = ubody.Operand as MemberExpression;
            }

            var PropertyName = body.Member.Name;

            var ec = this.EditContext;
            try
            {

                var entity = ec.Model.GetType();
                Type type = entity;
                PropertyInfo prop = type.GetProperty(PropertyName);

                if (prop == null)
                {
                    result = result1;
                    validationErrorMessage = "Property Name: " + PropertyName + "Not Exists in context";

                    //Logger.LogDebug(validationErrorMessage);
                    ExecuteCall(result1);
                    return true;
                }
                result = result1;
                prop.SetValue(ec.Model, result1, null);
                ec.NotifyValidationStateChanged();

                FieldIdentifier ID = new FieldIdentifier(ec.Model, PropertyName);
                ec.NotifyFieldChanged(ID);
                validationErrorMessage = "si se pudo";
                ErrorMessage = "";

                var message = ec.GetValidationMessages(ID);
                string msg = "";
                foreach (var item in message)
                {
                    msg += item + " ";
                }
                if (!string.IsNullOrEmpty(msg))
                {
                    ErrorMessage = msg;
                }
                else
                {
                    ErrorMessage = "";
                }

                StateHasChanged();
                ExecuteCall(result1);
                return true;
            }
            catch (Exception ex)
            {
                if (ShowSystemErrorMessages)
                {
                    ErrorMessage = ex.Message;
                }
                validationErrorMessage = ex.Message;
                result = default;
                throw ex;
#pragma warning disable CS0162 // Se detectó código inaccesible
                return false;
#pragma warning restore CS0162 // Se detectó código inaccesible
                // throw new Exception("Invalid Value");


            }

            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TValue)}'.");
        }

        bool isSubmitting;

        protected async Task ChangeWindow(ChangeEventArgs item)
        {

            await ExecuteCall2(item.Value);

            //if (OnChangeControl.HasDelegate && !isSubmitting && item != null)
            //{

            //    try
            //    {

            //        ChangeEventArgs e = new ChangeEventArgs();

            //        e.Value = item.Value;
            //        isSubmitting = true;



            //        await OnChangeControl.InvokeAsync(e);

            //        await Task.Delay(300);

            //        //await SelfControl.FocusAsync();

            //    }

            //    catch
            //    {
            //        try
            //        {
            //            if (OnChangeControl.HasDelegate)
            //            {
            //                await OnChangeControl.InvokeAsync(default);
            //            }
            //        }
            //        catch
            //        {

            //        }

            //        //throw new NotImplementedException();
            //    }
            //    finally
            //    {
            //        isSubmitting = false;
            //    }


            //}
        }

        private async Task ExecuteCall(object? Value)
        {

             if (OnChangeControl.HasDelegate && !isSubmitting && Value != null)
            {

                try
                {

                    ChangeEventArgs e = new ChangeEventArgs();
                    
                    e.Value = Value;
                    isSubmitting = true;

//                    Console.WriteLine("ExecuteCall");

                    await SetControlFocus();

                    await OnChangeControl.InvokeAsync(e);

                    await Task.Delay(30);

                    //await SelfControl.FocusAsync();

                }

                catch
                {
                    try
                    {
                        if (OnChangeControl.HasDelegate)
                        {
                            await OnChangeControl.InvokeAsync(default);
                        }
                    }
                    catch
                    {

                    }

                    //throw new NotImplementedException();
                }
                finally
                {
                    isSubmitting = false;
                }


            }


        }

        private async Task ExecuteCall2(object? Value)
        {

             if (OnChangeControl.HasDelegate && !isSubmitting && Value != null)
            {

                try
                {

                    ChangeEventArgs e = new ChangeEventArgs();

                    e.Value = Value;
                    isSubmitting = true;



                    await OnChangeControl.InvokeAsync(e);

                    await Task.Delay(100);

                    //await SelfControl.FocusAsync();

                }

                catch
                {
                    try
                    {
                        if (OnChangeControl.HasDelegate)
                        {
                            await OnChangeControl.InvokeAsync(default);
                        }
                    }
                    catch
                    {

                    }

                    //throw new NotImplementedException();
                }
                finally
                {
                    isSubmitting = false;
                }


            }


        }

        protected async Task ChangeObject(ChangeEventArgs item)
        {

            if (OnChangeObjectControl.HasDelegate && !isSubmitting)
            {

                try
                {

                    ChangeEventArgs e = new ChangeEventArgs();
                    e.Value = item.Value;
                    if (Model != null)
                    {
                        e.Value = Model;
                    }


                    isSubmitting = true;

                    await OnChangeObjectControl.InvokeAsync(e);

                    await Task.Delay(500);

                }

                catch
                {
                    await OnChangeControl.InvokeAsync(default);
                    //throw new NotImplementedException();
                }
                finally
                {
                    isSubmitting = false;
                }


            }
        }




        private Task OnValueChanged(ChangeEventArgs e)
        {
            try
            {
                Value = (TValue)(object)(e.Value);
                return ValueChanged.InvokeAsync(Value);
            }
            catch
            {
                return TextValueChanged.InvokeAsync(default);
                //throw new NotImplementedException();
            }

            //TextLength = Value.Length;

        }






        public RenderFragment GetErrorMessage(string errormessage)
        {
            if (!string.IsNullOrEmpty(errormessage))
            {
                return builder =>
                {
                    builder.OpenElement(0, "div");
                    builder.AddAttribute(1, "class", @CSSValidationMessage);
                    builder.AddAttribute(1, "style", "display:block !important");
                    builder.AddContent(2, errormessage);
                    builder.CloseElement();
                };


                //return "<div class='validation-message'>" + ValidationMessages[PropertyName] + "</div>";
            }
            else
            {
                return null;
            }
        }



        protected override void Dispose(bool disposing)
        {
        }

        void IDisposable.Dispose()
        {
            if (EditContext != null)
            {
                //EditContext.OnValidationStateChanged -= _validationStateChangedHandler;
            }

            Dispose(disposing: true);
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task ShowToast(string message, ToastLevel level)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            if (level == ToastLevel.Success)
            {

                Toast.ShowSuccess(message);
            }
            if (level == ToastLevel.Error)
            {
                Toast.ShowError(message);
            }
            if (level == ToastLevel.Info)
            {
                Toast.ShowInfo(message);
            }
            if (level == ToastLevel.Warning)
            {
                Toast.ShowWarning(message);
            }

        }

        [Inject] public IJSRuntime JSRuntime { get; set; }
        public async Task SetControlFocus()
        {
            
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("SetControlFocus", Id,VerticalFocus);
               
            }
          
        }

    }


    public enum RoundType
    {
        RoundToResolution,
        Normal,
        Ceiling,
        Floor,
        Without
    }


}
