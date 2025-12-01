

using Helpers.Controls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace Blazor.Controls
{
    public class RulesTemplateBase<TTYPE, TValue> : ComponentBase where TTYPE : new()
    {
        [Inject]
        IAuthorizationService AuthorizationService { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> stateTask { get; set; }


        [Parameter]
        public bool Enabled { get; set; }


        [Parameter]
        public string ID { get; set; } = "";


        [Parameter]
        public string Label { get; set; } = "";

        [Parameter]
        public EventCallback<ChangeEventArgs> OnChangeControl { get; set; }

        TValue _Value;
        [Parameter]
        public TValue Value
        {
            get
            {
                return _Value;

            }

            set
            {
                _Value = value;
            }


        }
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        [Parameter] public Expression<Func<TValue>> ValidationFor { get; set; }
        [Parameter] public Expression<Func<int>> ValidationForNumeric { get; set; }

        [Parameter] public Expression<Func<string>> ValidationForString { get; set; }

        //[Parameter] public Expression<Func<string>> Key { get; set; }

        //[Parameter] public Expression<Func<string>> Name { get; set; }


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task ChangeControl(ChangeEventArgs arg)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            //if (ValueChanged.HasDelegate)
            //{
            //    await ValueChanged.InvokeAsync(Value);
            //}

            //if (OnChangeControl.HasDelegate) { 
            //ChangeEventArgs e = new ChangeEventArgs();

            //e.Value = arg.Value;



            //await OnChangeControl.InvokeAsync(e);

            //}



        }

        //[Parameter]
        //public TextInput<T> TextInput { get; set; }


        //[Parameter]
        //public EventCallback<object> ValueChanged { get; set; }

        [Parameter]
        public EventCallback<int> ComponentValueChanged { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> Value1Changed { get; set; }

        private void OnValue1Changed(ChangeEventArgs e)
        {

        }


        private int _value1;

        [Parameter]
        public int Value1
        {
            get { return _value1; }
            set
            {

                _value1 = value;
                ComponentValue = _value1;

            }
        }

        int _ComponentValue;

        [Parameter]
        public int ComponentValue
        {

            get
            {

                return _ComponentValue;
            }


            set
            {

                _ComponentValue = value;

                //_value1 = _ComponentValue;
            }

        }

        [Parameter]
        public string ValidType { get; set; }


        [Parameter]
        public string InvalidType { get; set; }

        public bool ValidTemplate = false;

        public int ActiveRules = 0;

        public int FirstTime = 0;

        public bool InvalidValidationRule = false;

        public List<RenderFragment> ValidationListMsg = new List<RenderFragment>();

        public List<bool> RulesValid;

        [Parameter]
        public RenderFragment<TTYPE> ValidConditionsTemplate { get; set; }

        [Parameter]
        public RenderFragment<TTYPE> InvalidConditionsTemplate { get; set; }


        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter()]
        public MultiComponent<TTYPE> MultiComponent { get; set; }

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }



        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }

        public async Task<bool> IsInPolicy(Policies policy)
        {
            var user = (await stateTask).User;

            //if (user.HasClaim(c => c.Type == "tech"))
            //{

            //}

            //if (Component.GetType() == typeof(CalibrationSaaS.Domain.Aggregates.Entities.Component))
            //{
            //    var ssss = true;
            //}

            //Component.Permission = policy;

            var ed = await AuthorizationService.AuthorizeAsync(user, null, MultiComponent.Policy);

            if (ed.Succeeded)
            {
                return true;
            }
            return false;

        }
        public void SetEnabled()
        {
            //var user= GetUser().ConfigureAwait(false).GetAwaiter().GetResult();


            //if(user.IsInRole())

            var Hasfull = IsInPolicy(Policies.HasFullAccess).ConfigureAwait(false).GetAwaiter().GetResult();

            if (Hasfull)
            {
                Enabled = true;
                return;
            }


            var HasView = IsInPolicy(Policies.HasView).ConfigureAwait(false).GetAwaiter().GetResult();

            var HasEdit = IsInPolicy(Policies.HasEdit).ConfigureAwait(false).GetAwaiter().GetResult();

            if (HasView && !HasEdit)
            {
                Enabled = false;
            }
            else if (HasView && HasEdit)
            {
                Enabled = false;
            }
        }


    }



}