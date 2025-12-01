using Microsoft.AspNetCore.Components;
using System;

namespace BlazorApp1.Blazor.Blazor.GenericMethods
{
    public partial class SelectWeightComponent<Target> : ComponentBase, IDisposable where Target : new()
    {
        public void Dispose()
        {



        }
    }
}
