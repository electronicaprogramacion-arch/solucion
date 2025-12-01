﻿using System.Collections.Generic;

namespace Helpers.Controls.ValueObjects
{
    public class ResultSet<T>
    {
        public ResultSet()
        {
            Items = new List<T>();
            List = new List<T>();
        }

        public List<T> Items { get; set; }
        public List<T> List { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PageTotal { get; set; }
        public int CurrentPage { get; set; }
        public string ColumnName { get; set; }
        public bool SortingAscending { get; set; }
        public bool ClientPagination { get; set; }
        public string JSonDefinitionResult { get; set; }
        public string Component { get; set; }
        public string Route { get; set; }
    }
}
