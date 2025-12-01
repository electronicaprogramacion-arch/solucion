using Blazored.Modal;
using Blazored.Modal.Services;
using Blazed.Controls.Toast;
using Bogus;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Helpers;
namespace Blazed.Controls
{
    public  class TextInput2Base<TValue, TInstance> : InputBase<TValue>, IDisposable 
   
    {
        
        [Parameter]
        public ViewProperty<TValue> ViewProperty { get; set; }

        [Parameter]
        public Faker<TInstance> Fake { get; set; }

        public bool WasRounded { get; set; }

        [Parameter]
        public bool EnableToastMessage { get; set; }

        [Parameter]
        public string ToastMessage { get; set; }

        [Inject] public Blazed.Controls.Toast.IToastService Toast { get; set; }

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

        public string PropertyName { get; set; }

        protected override async Task OnInitializedAsync()
        {

            if (ValidationFor == null)
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

           PropertyName = body.Member.Name;

            var ob = Fake.ViewModel.PropertiesViewModel.Where(x => x.Key == PropertyName).FirstOrDefault();
            
            ViewProperty = (ViewProperty<TValue>) ob.Value;
            //var res = ob.Value.IsVisible();

        }

        protected override async Task OnParametersSetAsync()

        {

            return;

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


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                return;
            }
            else
            {

            }
            await base.OnAfterRenderAsync(firstRender);



        }
        public Type TypeValue { get; set; }


        protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string validationErrorMessage)
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

//        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
//        {
//            //var field = this.FieldIdentifier;
//            //var _fieldIdentifier = FieldIdentifier.Create(ValidationFor);

//            TypeValue = typeof(TValue);

//            TValue result1 = default(TValue);

//            if (typeof(TValue) == typeof(string))
//            {
//                result1 = (TValue)(object)value;
//                validationErrorMessage = null;

//            }
//            else if (typeof(TValue) == typeof(decimal))
//            {
//                var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
//                clone.NumberFormat.NumberDecimalSeparator = ".";
//                clone.NumberFormat.CurrencyDecimalSeparator = ".";
//                CultureInfo.CurrentUICulture = clone;
//                //clone.NumberFormat.NumberGroupSeparator = ".";
//                //Logger.LogDebug("CultureInfo.DefaultThreadCurrentCulture.NumberFormat.NumberDecimalSeparator");
//                //Logger.LogDebug(CultureInfo.DefaultThreadCurrentCulture.NumberFormat.NumberDecimalSeparator);

//                decimal.TryParse(value, NumberStyles.Any, CultureInfo.DefaultThreadCurrentCulture, out var parsedValue);
//                result1 = (TValue)(object)parsedValue;
//                validationErrorMessage = null;

//            }

//            else if (typeof(TValue) == typeof(int))
//            {
//                int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
//                result1 = (TValue)(object)parsedValue;
//                validationErrorMessage = null;


//            }
//            else if (typeof(TValue) == typeof(Guid))
//            {
//                Guid.TryParse(value, out var parsedValue);
//                result1 = (TValue)(object)parsedValue;
//                validationErrorMessage = null;


//            }
//            else if (typeof(TValue) == typeof(double))
//            {
//                //string a = String.Format("{0,12:N3}", value);
//                double.TryParse(value, NumberStyles.Any, CultureInfo.DefaultThreadCurrentCulture, out var parsedValue);

//                if (parsedValue == 0)
//                {
//                    parsedValue = 0.00;
//                }
//                result1 = (TValue)(object)parsedValue;


//                validationErrorMessage = null;

//            }
//            else if (typeof(TValue) == typeof(int?))
//            {

//                int.TryParse(value, NumberStyles.Any, CultureInfo.DefaultThreadCurrentCulture, out var parsedValue);
//                result1 = (TValue)(object)parsedValue;
//                validationErrorMessage = null;

//            }
//            else if (typeof(TValue) == typeof(short))
//            {

//                short.TryParse(value, NumberStyles.Any, CultureInfo.DefaultThreadCurrentCulture, out var parsedValue);
//                result1 = (TValue)(object)parsedValue;
//                validationErrorMessage = null;

//            }
//            else if (typeof(TValue) == typeof(DateTime))
//            {

//                DateTime.TryParse(value, out var parsedValue);
//                result1 = (TValue)(object)parsedValue;
//                validationErrorMessage = null;

//            }


//            else if (typeof(TValue).IsEnum)
//            {
//                // There's no non-generic Enum.TryParse (https://github.com/dotnet/corefx/issues/692)
//                try
//                {
//                    result1 = (TValue)Enum.Parse(typeof(TValue), value);
//                    validationErrorMessage = null;


//                }
//                catch (ArgumentException)
//                {
//                    result1 = default;
//                    validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";


//                }
//            }

//            if (!Validate || ValidationFor == null)
//            {
//                result = result1;
//                validationErrorMessage = null;
//                return true;
//            }
//            result = default;

//            var vali = ValidationFor;

//            MemberExpression body = vali.Body as MemberExpression;

//            if (body == null)
//            {
//                UnaryExpression ubody = (UnaryExpression)vali.Body;
//                body = ubody.Operand as MemberExpression;
//            }

//            var PropertyName = body.Member.Name;

//            var ec = this.EditContext;
//            try
//            {

//                var entity = ec.Model.GetType();
//                Type type = entity;
//                PropertyInfo prop = type.GetProperty(PropertyName);

//                if (prop == null)
//                {
//                    result = result1;
//                    validationErrorMessage = "Property Name: " + PropertyName + "Not Exists in context";

//                    //Logger.LogDebug(validationErrorMessage);
//                    return true;
//                }
//                result = result1;
//                prop.SetValue(ec.Model, result1, null);
//                ec.NotifyValidationStateChanged();

//                FieldIdentifier ID = new FieldIdentifier(ec.Model, PropertyName);
//                ec.NotifyFieldChanged(ID);
//                validationErrorMessage = "si se pudo";
//                ErrorMessage = "";

//                var message = ec.GetValidationMessages(ID);
//                string msg = "";
//                foreach (var item in message)
//                {
//                    msg += item + " ";
//                }
//                if (!string.IsNullOrEmpty(msg))
//                {
//                    ErrorMessage = msg;
//                }
//                else
//                {
//                    ErrorMessage = "";
//                }

//                StateHasChanged();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                if (ShowSystemErrorMessages)
//                {
//                    ErrorMessage = ex.Message;
//                }
//                validationErrorMessage = ex.Message;
//                result = default;
//                throw ex;
//#pragma warning disable CS0162 // Se detectó código inaccesible
//                return false;
//#pragma warning restore CS0162 // Se detectó código inaccesible
//                // throw new Exception("Invalid Value");


//            }

//            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TValue)}'.");
//        }

        bool isSubmitting;

        protected async Task ChangeWindow(ChangeEventArgs item)
        {



            if (OnChangeControl.HasDelegate && !isSubmitting && item != null)
            {

                try
                {

                    ChangeEventArgs e = new ChangeEventArgs();

                    e.Value = item.Value;
                    isSubmitting = true;



                    await OnChangeControl.InvokeAsync(e);

                    await Task.Delay(300);

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


        public async Task ShowToast(string message, ToastLevel level)

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


         [Parameter]
        public string CSSClassForm { get; set; } = "d-sm-inline-block btn btn-sm";


        [Parameter]
        public string JSid { get; set; }


        [Inject]
        public NavigationManager NavigationManager { get; set; }


        [Parameter]
        public string HideElement { get; set; }

        [Parameter]
        public string ShowElement { get; set; }


        [Parameter]
        public string Link { get; set; }

        public string AllWidthCSS { get; set; } = " btn-sm btn-block";


        [Parameter]
        public string Icon { get; set; }


        [Parameter]
        public bool AllWidth { get; set; } = false;

        [Parameter]
        public bool IsDisabledForm { get; set; } = false;


        [Parameter]
        public EventCallback<ChangeEventArgs> CloseWindow { get; set; }


        [Parameter]
        public string Prefix { get; set; } = "";


        [Parameter]
        public string Text { get; set; }


        [Parameter]
        public string FormSize { get; set; }

        [Parameter]
        public string CSSButton { get; set; }

         [Parameter]
        public string TypeButton { get; set; }       

        [Parameter]
        public string EntityID { get; set; }

        [Parameter]
        public bool Enabled { get; set; } = true;

        [CascadingParameter] public IModalService Modal { get; set; }

        public string _message { get; set; }

        [Parameter]
        public ModalParameters Parameters { get; set; }

        [Parameter]
        public string Tittle { get; set; }

        public object Result { get; set; }

        [Parameter]
        public bool NoModal { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public Type FormType { get; set; }

        private bool LauncModal;

        public Func<object,ViewProperty<TValue>> ModalResult{ get; set; } 
        

        public string modalresult(object obj)
        {
            //Console.WriteLine("modalresult  xxx");
            
            if(ModalResult != null)
            {
                 ViewProperty= ModalResult.Invoke(obj);  
                if(ViewProperty != null)
                {
                     CurrentValueModal = ViewProperty.Value.ToString();
                     return ViewProperty.Value.ToString();;
                }                
               
            }
            else
            {
                //Console.WriteLine("modalresult");
                if(obj != null)
                {
                     CurrentValueModal = obj.ToString();
                return obj.ToString();
                }
               
            }

            return "";
           
        }


        private string _currentValueModal;

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


    private string _currentValue;

    public string CurrentValue
    {
        get
        {
            return CurrentValueAsString;

        }
        set
        {
            CurrentValueAsString = value;

            if (ViewProperty.ReGenerate)
            {
                Fake.Generate();
            }

        }
    }

    public bool ToastLunch { get; set; }

    private string _currentValue2;

    private string _currentValueTemp;
    public string CurrentValue2
    {
        get
        {
            try
            {

                if (StepResol == "any" || StepResol == "0" || DecimalNumbers <= 0)
                {
                    return CurrentValueAsString;
                }

                if (double.TryParse(CurrentValueAsString, NumberStyles.Any, CultureInfo.DefaultThreadCurrentCulture, out var parsedValue))
                {



                    if (DecimalRoundType == RoundType.Without)
                    {
                        parsedValue = parsedValue.ToStringNoTruncate(DecimalNumbers);
                        if (TypeValue == typeof(int) && parsedValue == 0 && EnableToastMessage
                            && !string.IsNullOrEmpty(ToastMessage)
                            && _currentValueTemp == CurrentValueAsString)
                        {
                            ShowToast(ToastMessage, Blazed.Controls.Toast.ToastLevel.Warning).ConfigureAwait(false).GetAwaiter().GetResult();
                            _currentValueTemp = CurrentValueAsString;
                            WasRounded = true;
                        }
                    }



                    var mult = 1;
                    for (int iii = 0; iii < DecimalNumbers; iii++)
                    {
                        mult = mult * 10;
                    }

                    if (DecimalRoundType == RoundType.Ceiling)
                    {
                        //int vall = (int)parsedValue * mult;


                        parsedValue = Math.Ceiling(parsedValue);
                        parsedValue = parsedValue.ToStringNoTruncate(DecimalNumbers);
                    }



                    if (DecimalRoundType == RoundType.Normal)
                    {


                        parsedValue = Math.Round(parsedValue, DecimalNumbers);


                    }

                    if (DecimalRoundType == RoundType.Floor)
                    {


                        parsedValue = Math.Floor(parsedValue);
                        parsedValue = parsedValue.ToStringNoTruncate(DecimalNumbers);
                    }

                    if (!string.IsNullOrEmpty(StepResol) && StepResol != "any" && StepResol != "0" && double.TryParse(StepResol, NumberStyles.Any, CultureInfo.DefaultThreadCurrentCulture, out var parsedValue3))
                    {

                        if (DecimalRoundType == RoundType.RoundToResolution)
                        {
                            parsedValue = (Math.Round(parsedValue / parsedValue3, MidpointRounding.AwayFromZero)) * parsedValue3; // 1.25
                            parsedValue = parsedValue.ToStringNoTruncate(DecimalNumbers);
                        }

                        if (EnableToastMessage && !string.IsNullOrEmpty(ToastMessage) && (Convert.ToDouble(CurrentValueAsString) != parsedValue) && _currentValueTemp != CurrentValueAsString)
                        {
                            var ms = string.Format(ToastMessage, CurrentValueAsString, parsedValue);

                            ShowToast(ms, Blazed.Controls.Toast.ToastLevel.Warning).ConfigureAwait(false).GetAwaiter().GetResult();

                            ToastLunch = true;

                            _currentValueTemp = CurrentValueAsString;

                            WasRounded = true;

                            //Console.WriteLine("showtoast");
                        }

                        var cssinvalid = parsedValue.GetInvalidClass(parsedValue3, DecimalNumbers);

                        if (ChangeBackground && !string.IsNullOrEmpty(cssinvalid))
                        {
                            CssClassDecimal = cssinvalid;
                            //in Testpoint (weight) the typed value XX was rounded to XX



                        }
                        else
                        {
                            CssClassDecimal = "";
                        }

                    }


                    var parsedValue2 = parsedValue.ToString();

                    var position = parsedValue2.IndexOf(".");

                    var lengt = 0;

                    if (position > 0)
                        lengt = parsedValue2.Substring(position + 1).Length;

                    var dif = DecimalNumbers - lengt;




                    //if (parsedValue.IsInt())
                    if (dif > 0)
                    {
                        if (DecimalRoundType == RoundType.RoundToResolution)
                        {

                        }
                        var zeros = "";
                        for (int i = 0; i < dif; i++)
                        {
                            zeros = zeros + "0";
                        }
                        if (!string.IsNullOrEmpty(zeros) && position == -1)
                        {
                            parsedValue2 = parsedValue2 + "." + zeros;
                        }
                        else if (!string.IsNullOrEmpty(zeros) && position > 0)
                        {
                            parsedValue2 = parsedValue2 + zeros;

                        }

                    }

                    //var a = parsedValue2; //String.Format("{0:0.00}", parsedValue2);
                    if (WasRounded)
                    {
                        CssClassDecimal = CssClassDecimal + " warningcolor";
                    }

                    return parsedValue2;


                }
                else
                {
                    return CurrentValueAsString;
                }


            }
            catch (Exception ex)
            {
                //Console.WriteLine("error textinput");
                return CurrentValueAsString;
            }
        }
        set
        {


            CurrentValueAsString = value;
        }
    }



        // public async Task<string> ShowModal()
        //{

        //    //var parameters = new ModalParameters();
        //    //parameters.Add("SelectOnly", true);
        //    //parameters.Add("IsModal", true);
        //    IModalReference messageForm;
        //    ModalResult result;
        //    if (Parameters == null && EntityID == null)
        //    {
        //        messageForm = Modal.Show(FormType,Tittle);
        //        result = await messageForm.Result;
        //         if (!result.Cancelled)
        //    {
        //         _message = result.Data?.ToString() ?? string.Empty;
        //    }
        //    else
        //    {
        //        _message = "";
        //    }
        //        return _message;
        //    }
        //    else
        //    {

        //        if (Parameters == null)
        //        {
        //            Parameters = new ModalParameters();

        //        }

        //        if (!string.IsNullOrEmpty(EntityID))
        //        {
        //            Parameters.Add("EntityID", EntityID);

        //        }

        //        Parameters.Add("Enabled", Enabled);

        //        ModalOptions op = new ModalOptions();

        //        op.HideHeader = false;

        //        op.HideCloseButton = false;

        //        op.Position = ModalPosition.Center;

        //        op.ContentScrollable = true;

        //        //op.Animation = ModalAnimation.FadeIn(200);

        //        if (!string.IsNullOrEmpty(Size))
        //        {
        //            op.Class = "blazored-modal " + Size;  //blazored-modal blazored-modal-scrollable allWindow

        //        }
        //        if (NoModal)
        //        {
        //            op.UseCustomLayout = true;
        //        }
        //        //op.ContentScrollable = true;

        //        if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
        //        {
        //            await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
        //        }


        //        messageForm = Modal.Show(FormType,Tittle, Parameters, op);
                
        //        result = await messageForm.Result;

        //    }

        //    if (!result.Cancelled)
        //    {
        //         _message = result.Data?.ToString() ?? string.Empty;
        //    }
        //    else
        //    {
        //        _message = "";
        //    }

        //    return _message;
        //    //Result = result;

        //    //ChangeEventArgs arg = new ChangeEventArgs();

        //    //arg.Value = Result;


        //    //if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
        //    //{
        //    //    await JSRuntime.InvokeVoidAsync("showElement", HideElement);
        //    //}

        //    //if (CloseWindow.HasDelegate)
        //    //{
        //    //    await CloseWindow.InvokeAsync(arg);
        //    //}

        //}




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
