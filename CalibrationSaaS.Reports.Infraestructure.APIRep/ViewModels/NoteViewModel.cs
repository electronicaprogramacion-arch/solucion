using System;
using System.Collections.Generic;
using System.Linq;
using CalibrationSaaS.Domain.Aggregates.Entities;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep.ViewModels
{
    public class NoteViewModel
    {
       
        public ICollection<Note> NotesList { get; set; }
        public ICollection<NoteWOD> NotesWODList { get; set; }
    }
      
    
}