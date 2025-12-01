using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    
    public interface INote
    {

        ICollection<Note> Notes { get; set; }

    }
    public interface INoteWOD
    {

        ICollection<NoteWOD> NotesWOD { get; set; }



    }
}
