
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Helpers.Controls.ValueObjects
{
    [DataContract]
    public class PaginationBase : IPaginationBase
    {
        [DataMember(Order = 1)]
        public int Page { get; set; } = 1;
        [DataMember(Order = 2)]
        public int Show { get; set; } = 1;
        [DataMember(Order = 3)]
        public string Filter { get; set; }

        [DataMember(Order = 4)]
        public string ColumnName { get; set; }

        [DataMember(Order = 5)]
        public bool SortingAscending { get; set; }

        

        [DataMember(Order = 6)]
        public int Top { get; set; } = 100000;

        [DataMember(Order = 7)]
        public Component Component { get; set; }

        public string EntityType { get; set; }

    }

    

    [DataContract]
    public class Pagination<T,S>: IPaginationBase
    {

        string _EntityType;

        public string EntityType { get
            {
                _EntityType = typeof(T).Name;   
                
                return _EntityType;

            } set 
            {
                _EntityType = value;
            } 
        
        
        }


        [DataMember(Order = 1)]
        public int Page { get; set; } = 1;
        [DataMember(Order = 2)]
        public int Show { get; set; } = 1;


        [DataMember(Order = 3)]
        public string Filter { get; set; }

        [DataMember(Order = 4)]
        public string ColumnName { get; set; }

        [DataMember(Order = 5)]
        public bool SortingAscending { get; set; }


        [DataMember(Order = 6)]
        public int Top { get; set; } = 100000;

        [DataMember(Order = 7)]
        public T Entity { get; set; }

        [DataMember(Order = 8)]
        public FilterObject<T> Object { get; set; }

        [DataMember(Order = 9)]
        public Component Component { get; set; }

        [DataMember(Order = 10)]
        public bool SaveCache { get; set; } = true;


       [DataMember(Order = 11)]
        public S Other { get; set; } 


    }

     [DataContract]
    public class Pagination<T>: IPaginationBase
    {

        string _EntityType;


        [DataMember(Order = 18)]
        public string EntityType { 
            get
            {
                if (string.IsNullOrEmpty(_EntityType))
                {
                    _EntityType = typeof(T).Name;
                }
              
                
                return _EntityType;

            } set 
            {
                _EntityType = value;
            } 
        
        
        }


        [DataMember(Order = 1)]
        public int Page { get; set; } = 1;
        [DataMember(Order = 2)]
        public int Show { get; set; } = -1;


        [DataMember(Order = 3)]
        public string Filter { get; set; }

        [DataMember(Order = 4)]
        public string ColumnName { get; set; }

        [DataMember(Order = 5)]
        public bool SortingAscending { get; set; }


        [DataMember(Order = 6)]
        public int Top { get; set; } = 100000;

        [DataMember(Order = 7)]
        public T Entity { get; set; }

        [DataMember(Order = 8)]
        public FilterObject<T> Object { get; set; }

        [DataMember(Order = 9)]
        public Component Component { get; set; }

        [DataMember(Order = 10)]
        public bool SaveCache { get; set; } = true;

        
         [DataMember(Order = 11)]
        public int Other { get; set; }

        [DataMember(Order = 12)]
        public bool ShowProgress { get; set; } = true;

        [DataMember(Order = 13)]
        public int? Min { get; set; }

        [DataMember(Order = 14)]
        public int? Max { get; set; }

        [DataMember(Order = 15)]
        public string JSonDefinitionResult{ get; set; }


        [DataMember(Order = 16)]
        public ReportView ReportView { get; set; }

        [DataMember(Order = 17)]
        public bool? LoadDynamic { get; set; } 



    }

    [DataContract]
    public class FilterObject<T>
    {
        [DataMember(Order = 1)]
        public T EntityFilter { get; set; } 

        [DataMember(Order = 2)]
        public DateTime? InitDate { get; set; } = null;
        [DataMember(Order = 3)]
        public DateTime? EndDate { get; set; } = null;
        [DataMember(Order = 4)]
        public bool Advanced { get; set; }
    }

    //[DataContract]
    //public class Pagination2<T>
    //{
    //    [DataMember(Order = 1)]
    //    public int Page { get; set; } = 1;
    //    [DataMember(Order = 2)]
    //    public int Show { get; set; } = 1;
    //    [DataMember(Order = 3)]
    //    public string Filter { get; set; }

    //    [DataMember(Order = 4)]
    //    public string ColumnName { get; set; }

    //    [DataMember(Order = 5)]
    //    public bool SortingAscending { get; set; }

    //    [DataMember(Order = 6)]
    //    public T Entity { get; set; }


    //}

    [DataContract]
    public class ReportView
    {

        [DataMember(Order = 1)]
        public string Title { get; set; }

        [DataMember(Order = 2)]
        public string Query { get; set; }

        [DataMember(Order = 3)]
        public string QueryDescription { get; set; }

        [DataMember(Order = 4)]
        public ICollection<Column> Columns { get; set; }

        [DataMember(Order = 5)]
        public ICollection<ParameterClass> Parameters { get; set; }

        [DataMember(Order = 6)]
        public HeaderClass Header { get; set; }

        [DataMember(Order = 7)]
        public DateTime? BeginDate { get; set; }
        [DataMember(Order = 8)]
        public DateTime? EndDate { get; set; }

        [DataMember(Order = 9)]
        public ICollection<string> Includes{ get; set; }

        [DataMember(Order = 10)]
        public string ClassName { get; set; }

        [DataContract]
        public class Column
        {
            [DataMember(Order = 1)]
            public string Title { get; set; }

            [DataMember(Order = 2)]
            public string Field { get; set; }

            [DataMember(Order = 3)]
            public string CSSClass { get; set; }

            [DataMember(Order = 4)]
            public string Value { get; set; }

            [DataMember(Order = 5)]
            public string Format { get; set; }

            [DataMember(Order = 6)]
            public int Position { get; set; }

        }

        [DataContract]
        public class HeaderClass
        {
            [DataMember(Order = 1)]
            public ICollection<Column> Columns { get; set; }
            [DataMember(Order = 2)]
            public string Description { get; set; }


            /// <summary>
            /// ex: Pieceofequipment etc...
            /// </summary>
            [DataMember(Order = 3)]
            public string PropertyName { get; set; }

            [DataMember(Order = 4)]
            public string Query { get; set; }

            [DataMember(Order = 5)]
            public ICollection<ParameterClass> Parameters { get; set; }

            [DataMember(Order = 6)]
            public string ClassName { get; set; }


            [DataMember(Order = 7)]
            public ICollection<string> Includes { get; set; }

        }

        [DataContract]
        public class ParameterClass
        {
            [DataMember(Order = 1)]
            public string Name { get; set; }

            [DataMember(Order = 2)]
            public string ColumnName { get; set; }

            [DataMember(Order = 3)]
            public string Operator { get; set; } = "=";

            [DataMember(Order = 4)]
            public string Value { get; set; }

        }

        }





}
