using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Bogus.Extensions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Bogus
{
   public delegate Faker<T> RuleForDelegate<T, TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, T, Task<ViewProperty<TProperty>>> setter,
      Func<Faker, T, Task<ViewProperty<TProperty>>> Default);


   public partial class PopulateAction<T> : Rule<Func<Faker, T, object>>
   {

      public Expression<Func<T, bool>> Expression { get; set; }

   }


   public static class Extensions2
   {


      public static MethodInfo GetMethodInfo<Tpropert>(Expression<Tpropert> expression)
      {
         var member = expression.Body as MethodCallExpression;

         if (member != null)
            return member.Method;

         throw new ArgumentException("Expression is not a method", "expression");
      }


      public static Task<ViewProperty<string>> When2<Tpropert>(this Task<ViewProperty<string>> rule, Expression<Func<Tpropert, bool>> expression)
      {



         return null;
      }
      public static Faker<Tpropert> When<T, Tpropert>(this RuleForDelegate<T, Tpropert> rule, Expression<Func<Tpropert, bool>> expression)
      {

         Console.WriteLine("when......................................");



         var a = rule.GetType().GetMethods();// rule.Parameters;

         foreach (var a2 in a)
         {
            var c = a2.GetParameters();
            Console.WriteLine("xxxx" + a2.Name);

            foreach (var c2 in c)
            {
               Console.WriteLine(c2.Name);
            }
         }
         //var c=((MethodBase)a.meth).GetParameters();
         //var al= expression.Compile()(obj);



         return new Faker<Tpropert>();



      }


   }



   public partial class Faker<T> : IFakerTInternal, ILocaleAware, IRuleSet<T>
   {


      public bool IsVisible { get; set; }

      public bool IsFirstGenerate { get; set; }

      public int GenerationNumber { get; set; }

      public ViewModel<T> ViewModel { get; set; } = new ViewModel<T>();

      protected internal Dictionary<string, Func<Faker, Task<T>>> CreateActionsAsync = new Dictionary<string, Func<Faker, Task<T>>>(StringComparer.OrdinalIgnoreCase);

      /// <summary>
      /// Instructs <seealso cref="Faker{T}"/> to use the factory method as a source
      /// for new instances of <typeparamref name="T"/>.
      /// </summary>
      public virtual Faker<T> CustomInstantiatorAsync(Func<Faker, Task<T>> factoryMethod)
      {
         this.CreateActionsAsync[currentRuleSet] = factoryMethod;
         return this;
      }

      /// <summary>
      /// Creates a rule for a compound property and providing access to the instance being generated.
      /// </summary>
      //public virtual Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property,string MethodName="", params Func<Faker, T, Task<ViewProperty<TProperty>>>[] prop)
      //{
      //   var propName = PropertyName.For(property);
      //   var Index = 0;
      //   foreach(var propi in prop)
      //   {
      //      var s=propi.Method.Name;
      //      Console.WriteLine(s);
      //      if (MethodName == s)
      //      {
      //         return AddRule(propName, (f, t) => prop[Index](f, t));
      //      }
      //      Index++;
      //   }

      //   return AddRule(propName, (f, t) => prop[0](f, t));



      //}




      /// <summary>
      /// Creates a rule for a compound property and providing access to the instance being generated.
      /// </summary>
      public virtual Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, T, Task<ViewProperty<TProperty>>> setter)
      {
         var propName = PropertyName.For(property);

         return AddRule(propName, (f, t) => setter(f, t));


      }

      /// <summary>
      /// Creates a rule for a compound property and providing access to the instance being generated.
      /// </summary>
      public virtual Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, T, Task<ViewPropertyBase>> setter)
      {
         var propName = PropertyName.For(property);

         return AddRule(propName, (f, t) => setter(f, t));


      }


      /// <summary>
      /// Creates a rule for a compound property and providing access to the instance being generated.
      /// </summary>
      public virtual Faker<T> RuleFor2<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, T, Task<ViewProperty<TProperty>>> setter,
         Expression<Func<T, bool>> expression, Func<Faker, T, Task<ViewProperty<TProperty>>> fault = null)
      {


         var propName = PropertyName.For(property);

         return AddRule(propName, (f, t) => setter(f, t), expression);




      }

      /// <summary>
      /// Generates a fake object of <typeparamref name="T"/> using the specified rules in this
      /// <seealso cref="Faker{T}"/>.
      /// </summary>
      /// <param name="ruleSets">A comma separated list of rule sets to execute.
      /// Note: The name `default` is the name of all rules defined without an explicit rule set.
      /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
      /// the `default` rules will not run. If you want rules without an explicit rule set to run
      /// you'll need to include the `default` rule set name in the comma separated
      /// list of rules to run. (ex: "ruleSetA, ruleSetB, default")
      /// </param>
      public virtual async Task<T> GenerateAsync(string ruleSets = null)
      {
         Func<Faker, Task<T>> createRule = null;
         var cleanRules = ParseDirtyRulesSets(ruleSets);

         if (string.IsNullOrWhiteSpace(ruleSets))
         {
            createRule = CreateActionsAsync[Default];
         }
         else
         {
            var firstRule = cleanRules[0];
            createRule = CreateActionsAsync.TryGetValue(firstRule, out createRule) ? createRule : CreateActionsAsync[Default];
         }

         //Issue 143 - We need a new FakerHub context before calling the
         //            constructor. Associated Issue 57: Again, before any
         //            rules execute, we need a context to capture IndexGlobal
         //            and IndexFaker variables.
         FakerHub.NewContext();


         T instance = await createRule(this.FakerHub).ConfigureAwait(false);


         await PopulateInternalAsync(instance, cleanRules).ConfigureAwait(false);

         IsFirstGenerate = true;

         GenerationNumber++;

         ViewModel.Value = instance;

         return instance;
      }

      /// <summary>
      /// Populates an instance of <typeparamref name="T"/> according to the rules
      /// defined in this <seealso cref="Faker{T}"/>.
      /// </summary>
      /// <param name="instance">The instance of <typeparamref name="T"/> to populate.</param>
      /// <param name="ruleSets">A comma separated list of rule sets to execute.
      /// Note: The name `default` is the name of all rules defined without an explicit rule set.
      /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
      /// the `default` rules will not run. If you want rules without an explicit rule set to run
      /// you'll need to include the `default` rule set name in the comma separated
      /// list of rules to run. (ex: "ruleSetA, ruleSetB, default")
      /// </param>
      protected virtual async Task PopulateInternalAsync(T instance, string[] ruleSets)
      {


         ValidationResult vr = null;
         if (!IsValid.HasValue)
         {
            //run validation
            vr = ValidateInternal(ruleSets);
            this.IsValid = vr.IsValid;
         }
         if (!IsValid.GetValueOrDefault())
         {
            throw MakeValidationException(vr ?? ValidateInternal(ruleSets));
         }

         //lock( Randomizer.Locker.Value )
         //{
         //Issue 57 - Make sure you generate a new context
         //           before executing any rules.
         //Issue 143 - If the FakerHub doesn't have any context
         //            (eg NewContext() has never been called), then call it
         //            so we can increment IndexGlobal and IndexFaker.
         if (!this.FakerHub.HasContext) FakerHub.NewContext();

         foreach (var ruleSet in ruleSets)
         {
            if (this.Actions.TryGetValue(ruleSet, out var populateActions))
            {
               foreach (var action in populateActions.Values)
               {
                  await PopulatePropertyAsync(instance, action).ConfigureAwait(false);
               }
            }
         }

         foreach (var ruleSet in ruleSets)
         {
            if (this.FinalizeActions.TryGetValue(ruleSet, out FinalizeAction<T> finalizer))
            {
               finalizer.Action(this.FakerHub, instance);
            }
         }
         //}
      }

      private async Task PopulatePropertyAsync(T instance, PopulateAction<T> action)
      {
         object vp = null;
         bool isview = false;
         bool istask = false;
         var valueFactory = action.Action;
         Type propertyType = null;


         if (action.Expression != null)
         {
            var resexpre = action.Expression.Compile()(instance);

            if (!resexpre)
            {
               return;
            }
         }



         if (valueFactory is null) return; // An .Ignore() rule.

         string aaa = valueFactory.GetType().GetGenericTypeDefinition().Name;
         //Console.WriteLine("type");
         //string bbb=valueFactory.GetType().Name;
         //Console.WriteLine(aaa); 
         //Console.WriteLine(bbb);

         //if (valueFactory.GetType().GetGenericTypeDefinition().Name == typeof(Task<>))
         //{
         //   istask = true;
         //}

         isview = true;
         //var value = valueFactory(FakerHub, instance);

         var value2 = valueFactory(FakerHub, instance);
         object value = new object();

         Type type = value2.GetType();

         if (type.IsGenericType || type.FullName.Contains(typeof(ViewPropertyBase).ToString()))
         {
            istask = true;
            Console.WriteLine("is task ........................................");
         }

         Console.WriteLine(type.FullName);
         if (istask)//(value.GetType().Name == typeof(ViewProperty<T>).Name)
         {
            Type[] typeArguments = type.GetGenericArguments();
            Console.WriteLine("\tList type arguments ({0}):", typeArguments.Length);

            if (typeArguments.Count() > 0)
            {
               foreach (Type tParam in typeArguments)
               {
                  Console.WriteLine("\t\t{0}", tParam);

                  var ty = tParam.GetGenericArguments();

                  foreach (var ttt in ty)
                  {
                     Console.WriteLine("\tList type arguments xxxx");
                     Console.WriteLine("\t\t{0}", ttt);
                     propertyType = ttt;
                     if (propertyType == null)
                     {
                        propertyType = ((object)value2).GetType();
                     }

                  }
               }
               if (propertyType == typeof(System.String))
               {
                  value = await (Task<ViewProperty<string>>)value2;
               }
               else
                if (propertyType == typeof(System.Int32) || propertyType == typeof(System.Int16))
               {
                  value = await (Task<ViewProperty<int>>)value2;
               }
               else
                if (propertyType == typeof(System.Double))
               {
                  value = await (Task<ViewProperty<double>>)value2;
               }
               else
                if (propertyType == typeof(System.Decimal))
               {
                  value = await (Task<ViewProperty<decimal>>)value2;
               }
               else

               {
                  value = await (Task<ViewProperty<dynamic>>)value2;
               }
               //value = await value;
               Console.WriteLine("is task pp");
               vp = value;
               isview = true;
               value = value.GetType().GetProperty("Value").GetValue(value, null);

            }
            else
            {
               value = await (Task<ViewPropertyBase>)value2;

               Console.WriteLine("is task pp");
               vp = value;
               isview = true;
               value = value.GetType().GetProperty("Result").GetValue(value, null);
            }






            //object[] attrs = prop.GetCustomAttributes(true);
         }

         if (SetterCache.TryGetValue(action.PropertyName, out var setter))
         {
            setter(instance, value);
            return;
         }

         if (!TypeProperties.TryGetValue(action.PropertyName, out var member)) return;
         if (member == null) return; // Member would be null if this was a .Rules()
                                     // The valueFactory is already invoked
                                     // which does not select a property or field.

         lock (_setterCreateLock)
         {
            if (SetterCache.TryGetValue(action.PropertyName, out setter))
            {
               setter(instance, value);
               return;
            }

            if (member is PropertyInfo prop)
            {
               if (isview)
               {
                  //object[] attrs = prop.GetCustomAttributes(true);

                  ViewModel.PropertiesViewModel.Add(prop.Name, vp);
               }

               setter = prop.CreateSetter<T>();
               // TODO FieldInfo will need to rely on ILEmit to create a delegate 
            }
            else if (member is FieldInfo field)
               setter = (i, v) => field?.SetValue(i, v);
            if (setter == null) return;

            SetterCache.Add(action.PropertyName, setter);
            setter(instance, value);
            ViewModel.Value = instance;
         }
      }

   }


   [DataContract]
   public class SurveyConfiguration
   {
      /////calibration subtipe

      // 1. calibration type id

      // 2. Type standard or poe - poe-stadard

      // 3. Add more grids - calibration subtypes

      // 4. add new delete rows

      // 5. defined Type UoM

      // 5. add Tolerance

      // 6. Component - if wod - poe

      // 7. One row or multiple row

      // 8. Modal - panel - form- grid

      // 12. Header

      // 9. if poe use work order detail standard - validation

      //9.1 config wod select component- calibrationtype equipmenttype fields key name

      //campos obligatorios

      //create y get obligatorio

      //validaciones en campos


      //10 if poe use standard asign

      //9.2 config select class

      //10.1  map  asign

      //10.2  multiple or only one

      //11. if poe header (new) row
      //12. if poe configure create


   }


   public class ViewLayout<TProperty>
   {




   }

   public class list
   {

      public list(string key, object value,  bool boolValue = false)
      {
         Key = key;
         Value = value;
       
         BoolValue = boolValue;
        


         //ControlType = controlType;
      }


      public list(string key, object value, DynamicProperty dynamicProperty,bool useHelper, bool boolValue = false)
      {
         Key = key;
         Value = value;
         DynamicProperty = dynamicProperty;
         BoolValue = boolValue;
         UseHelper = useHelper;


         //ControlType = controlType;
      }

      public string Key { get; set; }

      public object Value { get; set; }

      public bool HasBoolValue { get; set; }

      public bool BoolValue { get; set; }

      public DynamicProperty DynamicProperty { get; set; }

      public ViewPropertyBase CurrentView { get; set; }

      public bool UseHelper { get; set; }



   }



   [DataContract]
   public class DynamicProperty
   {
      [Key]
      [DataMember(Order = 1)]

      public int DynamicPropertyID { get; set; }

      [DataMember(Order = 2)]
      public int ColPosition { get; set; }

      //[DataMember(Order = 3)]
      //public string Value { get; set; }

      [DataMember(Order = 4)]
      public string Name { get; set; }

      [DataMember(Order = 5)]
      public string DataType { get; set; }

      [DataMember(Order = 6)]
      public int CalibrationSubtype { get; set; }

      [DataMember(Order = 7)]
      public string DefaultValue { get; set; }

      [System.ComponentModel.DataAnnotations.Editable(false)]
      [NotMapped]
      [DataMember(Order = 8)]
      [ForeignKey("ViewPropertyBaseID")]
      public virtual ViewPropertyBase ViewPropertyBase { get; set; }

      [DataMember(Order = 9)]
      public int ViewPropertyBaseID { get; set; }


      [DataMember(Order = 10)]
      public string Formula { get; set; }


      [DataMember(Order = 11)]
      public int Version { get; set; }

      [NotMapped]
      //[DataMember(Order = 12)]
      public object ValidationResult { get; set; }


      [DataMember(Order = 12)]
      public string ValidationFormula { get; set; }


      [DataMember(Order = 13)]
      public bool Enable { get; set; }


      //[DataMember(Order = 14)]
      //public string DefaultValueFormula { get; set; }

      /// <summary>
      /// map a propperty in genericalibration result witth dynamic object
      /// </summary>
      [DataMember(Order = 15)]
      public string Map { get; set; }


      [DataMember(Order = 16)]
      public string FormulaClass { get; set; }


      //[DataMember(Order = 17)]
      //public string ActionFormula { get; set; }

      [DataMember(Order = 18)]
      public string GridLocation { get; set; }

      [NotMapped]
      [DataMember(Order = 19)]
      public string OtherResult { get; set; }

      [DataMember(Order = 20)]
      public bool? IsMaxField { get; set; }


      [DataMember(Order = 21)]
      public bool? isRequired { get; set; }


      [DataMember(Order = 22)]
      public bool? unique { get; set; }


      [DataMember(Order = 23)]
      public string Pattern { get; set; }


      private string _JSONConfiguration;


      //[NotMapped]
      //[System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
      //[DataMember(Order = 24)]
      //public string JSONConfiguration
      //{
      //   get
      //   {
      //      string Options;
      //      var errors = new List<string>();

      //      Options = Newtonsoft.Json.JsonConvert.SerializeObject(this, new JsonSerializerSettings()
      //      {

      //         Error = (sender, error) =>
      //         {
      //            errors.Add(error.ErrorContext.Error.Message);
      //            error.ErrorContext.Handled = true;

      //            Console.WriteLine("Select error " + error.ErrorContext.Error.Message);


      //         }//error.ErrorContext.Handled = true


      //      });

      //      if (errors.Count > 0)
      //      {
      //         foreach(var item in errors)
      //         {
      //            Console.WriteLine("JSONConfiguration" + item);
      //         }               
      //      }

      //      _JSONConfiguration = Options;

      //      return _JSONConfiguration;

      //   }
      //   set
      //   {
      //      _JSONConfiguration = value;
      //   }
      //}


      [DataMember(Order = 24)]
      public string JSONConfiguration { get; set; }

      //[NotMapped]
      //[JsonIgnore]
      //[DataMember(Order = 25)]
      //public bool UseHelper { get; set; }

      

      

   }


   /// <summary>
   /// {
   //  "passWord": null,
   //  "failWord": null
   //  }
   /// </summary>
[DataContract]
   public class PassOrFail
   {
      [DataMember(Order = 1)]
      public string PassWord { get; set; }

      [DataMember(Order = 2)]
      public string FailWord { get; set; }


   }

   /// <summary>
   /// Clase vista 
   /// </summary>
   /// <typeparam name="TProperty"></typeparam>
   [DataContract]
   public class ViewProperty<TProperty> : ViewPropertyBase
   {
      /// <summary>
      /// Property value
      /// </summary>
      public TProperty Value { get; set; }




      [NotMapped]
      [DataMember(Order = 5)]
      public TProperty OldValue { get; set; }


      public Func<ViewProperty<TProperty>, TProperty, Task> Func { get; set; }

      [NotMapped]
      //[DataMember(Order = 13)]
      public TProperty Default { get; set; }



      [NotMapped]
      [DataMember(Order = 16)]
      public Func<object, ViewProperty<TProperty>> FormatResult { get; set; }

      //Select Options
      [NotMapped]
      [DataMember(Order = 17)]
      public Dictionary<TProperty, string> Options { get; set; }

      [NotMapped]
      [DataMember(Order = 6)]
      public Type Type { get; set; }


   }

   [DataContract]
   public class ViewPropertyBase
   {
      [Key]
      [DataMember(Order = 1)]
      public int ViewPropertyID { get; set; }

      [DataMember(Order = 2)]
      public bool IsHide { get; set; }

      [DataMember(Order = 3)]
      public bool? IsVisible { get; set; }

      [DataMember(Order = 4)]
      public bool? IsDisabled { get; set; }

      [DataMember(Order = 5)]
      public string Comment { get; set; }

      [DataMember(Order = 6)]
      public bool? IsValid { get; set; }

      [DataMember(Order = 7)]
      public string ErrorMesage { get; set; }

      [DataMember(Order = 8)]
      public string Display { get; set; }
      [DataMember(Order = 9)]
      public string ToolTipMessage { get; set; }

      [DataMember(Order = 10)]
      public string ControlType { get; set; }

      [DataMember(Order = 11)]
      public bool ReGenerate { get; set; }

      [DataMember(Order = 12)]
      public bool SelectShowDefaultOption { get; set; }

      [DataMember(Order = 13)]
      public string CSSClass { get; set; }

      [DataMember(Order = 14)]
      public int DynamicPropertyID { get; set; }

      [System.Text.Json.Serialization.JsonIgnore]
      [System.ComponentModel.DataAnnotations.Editable(false)]
      [NotMapped]
      [DataMember(Order = 15)]
      public  DynamicProperty DynamicProperty { get; set; }

      [DataMember(Order = 16)]
      public int? Min { get; set; }


      [DataMember(Order = 17)]
      public int? Max { get; set; }

      [DataMember(Order = 18)]
      public int? DecimalRoundType { get; set; }


      [DataMember(Order = 19)]
      public int? DecimalNumbers { get; set; }


      [DataMember(Order = 20)]
      public string StepResol { get; set; }

      [DataMember(Order = 22)]
      public bool ShowControl { get; set; }

      [DataMember(Order = 23)]
      public bool ShowLabel { get; set; }

      [DataMember(Order = 24)]
      public string LabelCSS { get; set; } = "form-control-label";


      [DataMember(Order = 25)]
      public bool EnableToastMessage { get; set; }

      [DataMember(Order = 26)]
      public string ToastMessage { get; set; }


      [DataMember(Order = 27)]
      public bool OnChange { get; set; }

      [NotMapped]
      [DataMember(Order = 28)]
      public string ID { get; set; }

      [NotMapped]
      public object Result { get; set; }

      [DataMember(Order = 29)]
      public string RuleSet { get; set; }

      [DataMember(Order = 30)]
      public string CSSCol { get; set; } = "col";

      [DataMember(Order = 31)]
      public int Version { get; set; }

      [DataMember(Order = 32)]
      public bool HasHeader { get; set; }

      [DataMember(Order = 33)]
      public bool ChangeBackground { get; set; }


      [DataMember(Order = 34)]
      public string SelectOptions { get; set; }

     


      [DataMember(Order = 35)]
      public string FormulaProperty { get; set; }

      [DataMember(Order = 36)]
      public bool ExtendedObject { get; set; }


      [DataMember(Order = 37)]
      public string RowCSSCol { get; set; } = "col";

      //[NotMapped]
      [DataMember(Order = 38)]
      public string ColGroup { get; set; }

      [DataMember(Order = 39)]
      public string ColGroupTitle { get; set; }

      [DataMember(Order = 40)]
      public string ColGroupCSS { get; set; } = "";

      [NotMapped]
      [DataMember(Order = 41)]
      public bool DoubleSep { get; set; } = true;


      [DataMember(Order = 42)]
      public string JSONConfiguration { get; set; }

      [DataMember(Order = 43)]
      public string PassOrFailJSON { get; set; }


      //[DataMember(Order = 25)]
      [NotMapped]
      [DataMember(Order = 44)]
      public bool? PassOrFail { get; set; }

      
      [NotMapped]
      public string? OptionsFilter { get; set; }

      //[DataMember(Order = 46)]

      [NotMapped]
      public string SelectOptionsTmP { get; set; }


      //private string _JSONConfiguration;
      //[NotMapped]
      //[System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
      //[DataMember(Order = 42)]
      //public string JSONConfiguration
      //{
      //   get
      //   {
      //      string Options;
      //      var errors = new List<string>();

      //      Options = Newtonsoft.Json.JsonConvert.SerializeObject(this, new JsonSerializerSettings()
      //      {

      //         Error = (sender, error) =>
      //         {
      //            errors.Add(error.ErrorContext.Error.Message);
      //            error.ErrorContext.Handled = true;

      //            Console.WriteLine("Select error " + error.ErrorContext.Error.Message);


      //         }//error.ErrorContext.Handled = true


      //      });

      //      if (errors.Count > 0)
      //      {
      //         foreach (var item in errors)
      //         {
      //            Console.WriteLine("Faker JSONConfiguration" + item);
      //         }
      //      }

      //      _JSONConfiguration = Options;

      //      return _JSONConfiguration;

      //   }
      //   set
      //   {
      //      _JSONConfiguration = value;
      //   }
      //}

     
      [DataMember(Order = 45)]
      public bool? OnlyAccredited { get; set; }


      [NotMapped]
      public bool? copyAsFoundToAsleft { get; set; } = false;
   }

   [DataContract]
   public class ViewModel<T>
   {
      public T Value { get; set; }

      public bool IsVisible { get; set; }

      public bool IsEnabled { get; set; }

      public string Comment { get; set; }

      public T OldValue { get; set; }

      public Dictionary<string, object> PropertiesViewModel { get; set; } = new Dictionary<string, object>();



   }


   public class Validate
   {
      public bool Required { get; set; }
      public int MinLength { get; set; }
      public int MaxLength { get; set; }

      public string Pattern { get; set; }
      public int Min { get; set; }

      public int Max { get; set; }

      public int Step { get; set; }

   }

   //-conditional: {
   //show: false,
   //when: null,
   //eq: ""
   //},



}
