using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace Helpers.Controls.ValueObjects
{
    public class TodoDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public bool Completed { get; set; }

        public int UserId { get; set; }
    }
}
