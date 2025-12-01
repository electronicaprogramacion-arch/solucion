using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Controls
{
     public enum Policies : int
    {

        HasFullAccess = 1,
        HasView = 2,
        HasEdit = 3,
        HasDelete = 4,
        HasNew=5,
        HasSelect=6,
        HasSave=7,
        ContractReview = 8,
        ReadyforCalibration=9,
        TechnicalReview=10,
        Override = 11,
        ViewAssignationsOnly=12,
        ConfigurationForms=13

    }
}
