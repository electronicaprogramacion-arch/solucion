using Microsoft.AspNetCore.Components;
using System;

namespace Blazed.Controls
{
    public class Rule<TTYPE> : ComponentBase where TTYPE : new()
    {


        [Parameter]
        public Func<TTYPE, bool> LambdaCondition { get; set; }



        [Parameter]
        public string Title { get; set; }


        [Parameter]
        public bool OnlyFirsTime { get; set; } = false;


        [Parameter]
        public RenderFragment<TTYPE> ValidationTemplate { get; set; }


        [Parameter]
        public RenderFragment ChildContent { get; set; }


        [Parameter]
        public RenderFragment<dynamic> Template { get; set; }


        [CascadingParameter()]
        public Blazed.Controls.MultiComponent<TTYPE> MultiComponent { get; set; }

        //protected override Task OnInitializedAsync()
        //{

        //    return base.OnInitializedAsync();
        //}


        protected override void OnInitialized()
        {
            
            Rule<TTYPE> rule = this;
            if (MultiComponent != null)
            MultiComponent.Rules.Add(rule);
            base.OnInitialized();
        }


    }
}