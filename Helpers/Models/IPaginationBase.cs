using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Helpers.Controls.ValueObjects;

namespace Helpers.Controls.ValueObjects
{
    public interface IPaginationBase
    {
        string ColumnName { get; set; }
        string Filter { get; set; }
        int Page { get; set; }
        int Show { get; set; }
        bool SortingAscending { get; set; }
        int Top { get; set; }

         Component Component { get; set; }

        public string EntityType { get; set; }
    }
}