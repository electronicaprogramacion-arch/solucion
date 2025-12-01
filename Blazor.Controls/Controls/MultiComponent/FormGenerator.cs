
using Bogus;
using Blazor.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
namespace Blazor.Controls.MultiComponent
{
    public partial class FormGenerator<TEntity> : KavokuComponentBase<TEntity>
    
    {

        [Parameter]
        public Faker<TEntity> FEntity { get; set; }
        public int xxx { get; set; }

       public ViewModel<TEntity> ViewModel { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public TEntity Entity { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // var genericType = typeof(int); // You could have a string and Type.GetType here
            // var genericClassType = typeof(Faker<>).MakeGenericType(genericType);
            //var instance = (ICanTellYouMyType)Activator.CreateInstance(genericClassType);
            //Console.WriteLine(instance.WhatsMyGenericType());

            //dynamic c = 1;

            if (!FEntity.IsFirstGenerate)
            {
                 Entity = await FEntity.GenerateAsync();
            }         



            ViewModel=FEntity.ViewModel;



           
        }

       



    }

    public interface ICanTellYouMyType
{
    string WhatsMyGenericType();
}

public class GenericClass<T> : ICanTellYouMyType
{
    public string WhatsMyGenericType()
    {
        return typeof(T).Name;
    }
}
}
