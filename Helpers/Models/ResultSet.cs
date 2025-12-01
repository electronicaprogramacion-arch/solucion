using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace Helpers.Controls.ValueObjects
{


    [DataContract]
    public class ResultSet<T>
    {
        [DataMember(Order = 1)]
        public List<T> List { get; set; }

        [DataMember(Order = 2)]
        public int Count { get; set; }

        [DataMember(Order = 3)]
        public int PageTotal { get; set; }

        [DataMember(Order = 4)]
        public int CurrentPage { get; set; }

        [DataMember(Order = 5)]
        public string ColumnName { get; set; }

        [DataMember(Order = 6)]
        public bool SortingAscending { get; set; }

        [DataMember(Order = 7)]
        public int Shown { get; set; }


        [DataMember(Order = 8)]
        public bool ClientPagination { get; set; }

        [DataMember(Order = 9)]
        public string  Message { get; set; }

        [DataMember(Order = 10)]
        public string JSONObject { get; set; }

        [DataMember(Order = 11)]
        public string HeadeClassName { get; set; }

        [DataMember(Order = 12)]
        public string HeaderJSON { get; set; }



    }



    [DataContract]
    public class TableChanges<T>
    {
        [DataMember(Order = 1)]
        public List<T> DeleteList { get; set; } = new List<T>();


         [DataMember(Order = 2)]
        public List<T> AddList { get; set; } = new List<T>();


         [DataMember(Order = 3)]
        public List<T> EditList { get; set; } = new List<T>();

        public void Clear()
        {
            DeleteList.Clear();
            DeleteList = new List<T>();
            AddList.Clear();
            AddList= new List<T>();
            EditList.Clear();
            EditList= new List<T>();
        }


        public bool HasChanges()
        {
            if(DeleteList != null && DeleteList.Count > 0)
            {
                return true;
            }
            if(AddList != null && AddList.Count > 0)
            {
                 return true;
            }

            if(EditList != null && EditList.Count > 0)
            {
                 return true;
            }

            return false;
        }


    }


}
