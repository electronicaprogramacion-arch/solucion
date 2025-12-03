using Blazed.Controls.Toast;
using Blazored.Modal.Services;
using Bogus;
using Helpers;
using Helpers.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Blazed.Controls
{
    public class TextInputBase<TValue> : InputBase<TValue>, IDisposable
    {


        [Parameter]
        public string SelectFilter { get; set; }

        [Parameter]
        public string  GroupName { get; set; }
        public string? stringValue { get; set; }
        public string? intValue { get; set; }


        private string _CurrentValueMultiple;



        public string OptionsFilter { get; set; }


        [Parameter]
        public string SelectedValue { get; set; } = "-1";


        [Parameter]
        public Action<string> RefreshGrid { get; set; }

        [Parameter]
        public EventCallback OnCopyToClicked { get; set; }

        public async Task PerformSearch()
        {
            //searchResult = await SearchService.FetchAsync(searchText);

            SelectedValue = CurrentValueAsString;

            if (RefreshGrid != null)
            {
                RefreshGrid(SelectedValue);
            }
            //StateHasChanged();

        }

        public bool IsChecked(TValue _value)
        {

            if (_value==null)
            {
                return false;
            }
            else if(_value.ToString()== SelectedValue)
            {

                return true;

            }
            else
            {
                return false;
            }


        }


        public void OnChange(ChangeEventArgs args)
        {
            CurrentValueAsString = args.Value.ToString();
        }

        [Parameter]
        public bool? IsValid { get; set; }

        [Parameter]
        public bool? IsClassMismatch { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public ElementReference myTextInput { get; set; }

        public async Task GetSelectionStart(ElementReference element)
        {
           if(JsRuntime != null)
            {
                CurrentValueAsString = await JsRuntime.InvokeAsync<string>("getSelectedStart", element);
            }
           
        }
        public string Text { get; set; }
        public void UpdateText(ChangeEventArgs e)
        {
            //Console.WriteLine("UpdateText");
            CurrentValueAsString = e.Value.ToString();
        }

        public string Logintest { get; set; }
        public Func<object, object> ModalResult { get; set; }

        public string modalresult(object obj)
        {
            //Console.WriteLine("modalresult  xxx");

            if (ModalResult != null)
            {
                var result = ModalResult.Invoke(obj);
                if (result != null)
                {
                    CurrentValueModal = result.ToString();
                    return result.ToString(); ;
                }

            }
            else
            {
                //Console.WriteLine("modalresult");
                if (obj != null)
                {
                    CurrentValueModal = obj.ToString();
                    return obj.ToString();
                }

            }

            return "";

        }



        [Parameter]
        public bool IsRowView { get; set; }


        [Parameter]
        public string FormSize { get; set; }


        [Parameter]
        public string TypeButton { get; set; }

        [Parameter]
        public Type FormType { get; set; }


        //public string _tempCurrentValueAsString
        //{
        //    get => _tempCurrentValueAsString ?? CurrentValueAsString;
        //    set => _tempCurrentValueAsString = value;
        //}

        public string CurrentValueModal
        {
            get
            {

                //Console.WriteLine("CurrentValueModal get");

                return CurrentValueAsString;

            }
            set
            {
                //Console.WriteLine("CurrentValueModal set");
                CurrentValueAsString = value;



            }
        }


       
        protected Dictionary<string,string> Options { get; set; }



        [Parameter]
        public string CSSControl { get; set; }

        [Parameter]
        public bool IsHeader { get; set; }


        [Parameter]
        public string PropertyName { get; set; }

        [Parameter]
        public bool IsDynamic { get; set; }


        public bool IsInternalRounded { get; set; } =  true;


         [Parameter]
        public bool OnlyHeader { get; set; }


        [Parameter]
        public string Format { get; set; }

        [Parameter] 
        public string ValidateValueMessage { get; set; }

        [Parameter]
        public bool IsRounded { get; set; } = true;


        [Parameter]
        public ViewPropertyBase ViewProperty { get; set; }


        [Parameter]
        public ViewProperty<TValue> ViewPropertyType { get; set; }


        [Parameter] 
        public bool? ValidateValue { get; set; }

        [Parameter]
        public bool VerticalFocus { get; set; } = true;


        public bool WasRounded { get; set; }

        [Inject] public Blazed.Controls.Toast.IToastService Toast { get; set; }

        [Parameter]
        public bool EnableToastMessage { get; set; }

        [Parameter]
        public string ToastMessage { get; set; }

        [Parameter]
        public string ToastMessageForce { get; set; } = string.Empty;

        [Parameter]
        public string MessageForceDifference { get; set; } = string.Empty;

        [Parameter]
        public int Min { get; set; } = 0;


        [Parameter]
        public int Max { get; set; }

        [Parameter]
        public RoundType DecimalRoundType { get; set; } = RoundType.Normal;


        [Parameter]
        public int DecimalNumbers { get; set; } = 1;


        [Parameter]
        public string StepResol { get; set; } = "any";

        [Parameter]
        public bool ShowControl { get; set; } = true;

        [Parameter]
        public bool ShowLabel { get; set; } = true;


        [Parameter]
        public bool ChangeBackground { get; set; } = true;

        [Parameter]
        public string CssClassDecimal { get; set; }       

        

        public ElementReference SelfControl { get; set; }

        [Parameter] public string Style { get; set; } = "top:2rem";



        [Parameter] public object Model { get; set; }

        [Parameter] public bool IsDisabled { get; set; } = false;


        [Parameter] public string DynamicType { get; set; }

        [Parameter]
        public string OnKeyDown { get; set; }

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


        public string Component { get; set; }
        public string Key { get; set; }
        public string Val { get; set; }
        public int? FilterId { get; set; }
    
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

            
            try
            {
                var ec = this.EditContext;

                if (ec == null)
                {
                    return;
                }
                
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


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ////
                    var json = GetJSONConfigurationArray();
                   
                    if (json != null)
                    {

                       Key = json.FirstOrDefault().Key;
                       Val = json.FirstOrDefault().Value;
                       Component = json.FirstOrDefault().Component;
                       
                }
                /////
                if (Type == Types.Coordinates && !IsDisabled)
                {
                    if (JsRuntime != null)
                    {
                         await JsRuntime.InvokeVoidAsync("actualizarCoordenadas", Id);
                    }

                }
                else
                if(Type == Types.Coordinates && IsDisabled)
                {

                    if (JsRuntime != null)
                    {
                        await JsRuntime.InvokeVoidAsync("observefield", Id);
                    }
                    
                }
                //return;
            }
            
            
            await  base.OnAfterRenderAsync(firstRender);

        }

        public List<KeyValueOption> GetJSONConfigurationArray()
        {


            if (!string.IsNullOrEmpty(ViewProperty?.JSONConfiguration))
            {

                var nvc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValueOption>>(ViewProperty?.JSONConfiguration);
                return nvc;

            }

            return null;
        }

        public  bool SetCopyAsFoundToAsLeft()
        {

          ViewProperty.copyAsFoundToAsleft = true;

           return true;

        }


        public Type TypeValue { get; set; }


        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            //var field = this.FieldIdentifier;
            //var _fieldIdentifier = FieldIdentifier.Create(ValidationFor);

          

            TypeValue = typeof(TValue);

            TValue result1 = default(TValue);

            var nuevotipo = "";

            if (typeof(TValue) == typeof(object) && !string.IsNullOrEmpty(DynamicType))
            {

                nuevotipo = DynamicType;
            }

            var ssss= typeof(string).ToString();

            if (typeof(TValue) == typeof(bool) || nuevotipo == typeof(bool).ToString())
            {
                result1 = (TValue)(object)value;
                validationErrorMessage = null;

            }else

            if (typeof(TValue) == typeof(string) || nuevotipo== typeof(string).ToString() )
            {
                result1 = (TValue)(object)value;
                validationErrorMessage = null;

            }
            else if (typeof(TValue) == typeof(decimal) || nuevotipo == typeof(decimal).ToString())
            {
                var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                clone.NumberFormat.NumberDecimalSeparator = ".";
                clone.NumberFormat.CurrencyDecimalSeparator = ".";
                CultureInfo.CurrentUICulture = clone;
                //clone.NumberFormat.NumberGroupSeparator = ".";
                //Logger.LogDebug("CultureInfo.DefaultThreadCurrentCulture.NumberFormat.NumberDecimalSeparator");
                //Logger.LogDebug(CultureInfo.DefaultThreadCurrentCulture.NumberFormat.NumberDecimalSeparator);

                decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue);
                result1 = (TValue)(object)parsedValue;
                validationErrorMessage = null;

            }

            else if (typeof(TValue) == typeof(int) || nuevotipo == typeof(int).ToString())
            {
                int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
                result1 = (TValue)(object)parsedValue;
                validationErrorMessage = null;


            }
            else if (typeof(TValue) == typeof(Guid) || nuevotipo == typeof(int).ToString())
            {
                Guid.TryParse(value, out var parsedValue);
                result1 = (TValue)(object)parsedValue;
                validationErrorMessage = null;


            }
            else if (typeof(TValue) == typeof(double) || nuevotipo == typeof(double).ToString()     )
            {
                 //Console.WriteLine("llega alresul xx yyyy");


                if(IsInternalRounded && IsRounded)
                {
                    //Console.WriteLine("lround double value");
                }
                //string a = String.Format("{0,12:N3}", value);
                double.TryParse( value, NumberStyles.Number , CultureInfo.InvariantCulture, out var parsedValue);

                if (parsedValue == 0)
                {
                    parsedValue = 0.00;
                }


                result1 = (TValue)(object)parsedValue;

                


                validationErrorMessage = null;

            }
            else if (typeof(TValue) == typeof(double?) || nuevotipo == typeof(double?).ToString())
            {
                //Console.WriteLine("llega alresul xx yyyy");


                if (IsInternalRounded && IsRounded)
                {
                    //Console.WriteLine("lround double value");
                }
                //string a = String.Format("{0,12:N3}", value);
                double? parsedValue;

                //double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out  parsedValue);

                bool asi= value.TryParseDouble(out parsedValue);


                if (!asi || !parsedValue.HasValue)
                {
                    parsedValue = 0.00;
                }


                result1 = (TValue)(object)parsedValue;

                validationErrorMessage = null;

            }
            else if (typeof(TValue) == typeof(int?) || nuevotipo == typeof(int?).ToString())
            {

                int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue);
                result1 = (TValue)(object)parsedValue;
                validationErrorMessage = null;

            }
            else if (typeof(TValue) == typeof(short) || nuevotipo == typeof(short).ToString())
            {

                short.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue);
                result1 = (TValue)(object)parsedValue;
                validationErrorMessage = null;

            }
            else if (typeof(TValue) == typeof(DateTime) || nuevotipo == typeof(DateTime).ToString())
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
            else
            {
                var success = BindConverter.TryConvertTo<TValue>(
                value, CultureInfo.CurrentCulture, out var parsedValue);
                if (success)
                {
                    result1 = parsedValue;
                    validationErrorMessage = null;

                    
                }
                else
                {
                    result1 = default;
                    validationErrorMessage = $"{FieldIdentifier.FieldName} field isn't valid.";

                    
                }
                
            }

            


            if (!Validate || ValidationFor == null)
            {
                 //Console.WriteLine("llega alresultxxxxx2");
                result = result1;
                validationErrorMessage = null;
                ExecuteCall(result1);
                return true;
            }
            result = default;

            string propertyName = "";
            
            if (ValidationFor != null)
            {
                var vali = ValidationFor;

                MemberExpression body = vali.Body as MemberExpression;

                if (body == null)
                {
                    UnaryExpression ubody = (UnaryExpression)vali.Body;
                    body = ubody.Operand as MemberExpression;
                }

                PropertyName = body.Member.Name;
            }
            if (Validate)
            {
                propertyName = PropertyName;
            }
           

            var ec = this.EditContext;
            try
            {
                if (ec != null)
                {
                    var entity = ec.Model.GetType();
                    Type type = entity;
                    PropertyInfo prop = type.GetProperty(propertyName);

                    if (prop == null)
                    {
                        result = result1;
                        validationErrorMessage = "Property Name: " + propertyName + "Not Exists in context";

                        //Logger.LogDebug(validationErrorMessage);
                        ExecuteCall(result1);
                        return true;
                    }
                    //Console.WriteLine("llega alresultxxxxx");
                    result = result1;
                    prop.SetValue(ec.Model, result1, null);
                    ec.NotifyValidationStateChanged();

                    FieldIdentifier ID = new FieldIdentifier(ec.Model, propertyName);
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
                }
                else
                {
                    
                    PropertyInfo prop = null;

                    if (prop == null)
                    {
                        result = result1;
                        validationErrorMessage = "Property Name: " + propertyName + "Not Exists in context";

                        //Logger.LogDebug(validationErrorMessage);
                        ExecuteCall(result1);
                        return true;
                    }
                    //Console.WriteLine("llega alresultxxxxx");
                    result = result1;
                    prop.SetValue(ec.Model, result1, null);
                    ec.NotifyValidationStateChanged();

                    FieldIdentifier ID = new FieldIdentifier(ec.Model, propertyName);
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
                }
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

            if (IsDynamic)
            {
                //StateHasChanged();

            }

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

                    //Console.WriteLine("ExecuteCall");

                    await SetControlFocus();

                    await Task.Delay(30);

                    await OnChangeControl.InvokeAsync(e);

                    

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

        public string selectedString { get; set; }
        public async Task DoStuff(ChangeEventArgs e)
        {

            //if(Options.Count > 0)
            //{
            //    var selectedStringkey = Options.ElementAtOrDefault(Convert.ToInt32(e.Value));

            //    selectedString = selectedStringkey.Key;

            //}
            selectedString = e.Value.ToString();

            CurrentValueAsString = selectedString;

            await ExecuteCall2(selectedString);
            //Console.WriteLine("It is definitely: " + selectedString);
        }

        public string GetOptions(string SelectOptions, string OptionsFilter)
        {
            var optionsstring = SelectOptions;

            var errors = new List<string>();

            var errors1 = new List<string>();

            var errors2 = new List<string>();

            if (string.IsNullOrEmpty(SelectOptions))
            {
                return "";
            }


            var Options = JsonConvert.DeserializeObject<Dictionary<string, string>>(optionsstring, new JsonSerializerSettings()
            {
                Error = (sender, error) =>
                {
                    errors.Add(error.ErrorContext.Error.Message);
                    error.ErrorContext.Handled = true;

                    //Console.WriteLine("GetOptions error " + error.ErrorContext.Error.Message);
                }//error.ErrorContext.Handled = true

            });

            if (errors?.Count > 0)
            {

                var optionswl = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(optionsstring
                , new JsonSerializerSettings()
                {
                    Error = (sender, error) =>
                    {
                        errors1.Add(error.ErrorContext.Error.Message);
                        error.ErrorContext.Handled = true;

                        //Console.WriteLine("GetOptions 2 error " + error.ErrorContext.Error.Message);
                    }//error.ErrorContext.Handled = true

                }
                );
                if (errors1.Count == 0 )
                {
                    //var property = OptionsFilter.Replace("header.", "");

                    //var HeaderModel = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(NewItem.Object

                    //   , new JsonSerializerSettings()
                    //   {
                    //       Error = (sender, error) =>
                    //       {
                    //           errors2.Add(error.ErrorContext.Error.Message);
                    //           error.ErrorContext.Handled = true;

                    //           Console.WriteLine("GetOptions 2 error NewItem" + error.ErrorContext.Error.Message);
                    //       }//error.ErrorContext.Handled = true

                    //   }
                    //);
                    if (string.IsNullOrEmpty(OptionsFilter))
                    {
                        OptionsFilter = "0";
                    }

                    if (!string.IsNullOrEmpty(OptionsFilter) && errors2.Count == 0)
                    {
                        string provalue = OptionsFilter; // HeaderModel[property];

                        //Console.WriteLine("GetOptions " + provalue + " " + GroupName + " NewItemGroup: " + NewItem.GroupName);

                        if (optionswl != null && optionswl.Count > 0)
                        {
                            KeyValuePair<string, Dictionary<string, string>> optfilter ;

                            if (provalue.ToLower() == "all" || provalue.ToLower()=="-1")
                            {
                                Dictionary<string, string> all = new Dictionary<string, string>();  
                                
                                foreach (var opt in optionswl)
                                {

                                   foreach (var opt2 in opt.Value)
                                    {
                                        if (!all.ContainsKey(opt2.Key))
                                        {
                                            all.Add(opt2.Key, opt2.Value);
                                        }
                                    }
                                }

                                return Newtonsoft.Json.JsonConvert.SerializeObject(DictionarySortingUtilities.SortByValues(all).ToDictionary()); ;

                            }
                            else
                            {
                                optfilter = optionswl.Where(x => x.Key == provalue).FirstOrDefault();
                            }
                            

                            if (EqualityComparer<KeyValuePair<string, Dictionary<string, string>>>.Default.Equals(optfilter, default(KeyValuePair<string, Dictionary<string, string>>)))
                            {

                            }
                            else
                            {

                                return Newtonsoft.Json.JsonConvert.SerializeObject(DictionarySortingUtilities.SortByValues(optfilter.Value).ToDictionary());



                            }
                        }
                        // Por esta versión corregida:

                    }
                    else
                    {
                        foreach (var err in errors2)
                        {
                            //Console.WriteLine("Error in newitem: " + err);
                        }
                    }
                }
                else
                {
                    //Console.WriteLine("Mal formated Json, review SelectOptions +++ " + OptionsFilter);
                }


            }
            else
            {
                return optionsstring;
            }


            return "";
        }

    }

    
        //public enum RoundType
        //{
        //    RoundToResolution,
        //    Normal,
        //    Ceiling,
        //    Floor,
        //    Without
        //}


    }
