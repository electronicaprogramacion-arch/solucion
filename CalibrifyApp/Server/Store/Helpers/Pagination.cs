﻿using System;

namespace Helpers.Controls.ValueObjects
{
    public class Pagination<T>
    {
        public Pagination()
        {
            PageSize = 10;
            PageNumber = 1;
        }

        public T Entity { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string JSonDefinitionResult { get; set; }
        public string Component { get; set; }
        public string Route { get; set; }
    }
}
