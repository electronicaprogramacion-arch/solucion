using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazed.Controls
{
    public partial class MultiComponent<TTYPE> : ComponentBase where TTYPE : new()
    {
        public enum Condition
        {
            All,
            OnlyOne
        }

        [Parameter]
        public Condition RulesCondition { get; set; }


        [Parameter]
        public string Policy { get; set; } = "HasAccess";

        [Parameter]
        public bool OnlyFirstTime { get; set; } = false;


        [Parameter]
        public RenderFragment ChildContent { get; set; }

        List<TTYPE> list = new List<TTYPE>();

        TTYPE _Instance = new TTYPE();
        [Parameter]
        public TTYPE Instance
        {

            get
            {

                return _Instance;
            }

            set
            {

                list.Clear();
                list.Add(value);
                Data = list.ToList();


                //Data = list;



            }
        }

        [Parameter]
        public IEnumerable<TTYPE> Data { get; set; } = new List<TTYPE>();

        //[Parameter]
        //public bool CheckBoxSelection { get; set; }

        internal List<Rule<TTYPE>> Rules { get; set; } = new List<Rule<TTYPE>>();

        protected override void OnParametersSet()
        {
            //if (Rules.Count > 0)
            //    columnWidth = 100 / Rules.Count;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //List<TTYPE> lis = new List<TTYPE>();
            //if(Instance != null)
            //{
            //    lis.Add(Instance);

            //    Data = lis;
            //}



            await base.OnAfterRenderAsync(firstRender);
        }

    }
}
