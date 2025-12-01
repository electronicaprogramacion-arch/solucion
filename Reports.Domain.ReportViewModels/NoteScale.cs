using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{

    public class NoteScale
    {
        
            public ICollection<NoteEqScale> NotesEqScaleList { get; set; }
            public ICollection<NoteWODScale> NoteWODScaleList { get; set; }
        
    }

    public class NoteEqScale
    { 
      public string Text { get; set; }
    }

    public class NoteWODScale
    { 
      public string Text { get; set; }
    }

}


