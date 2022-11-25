using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Assignment.Response
{
    public class AssignmentResponse
    {
        public string AssignedDate { get; set; }
        public string ReturnedDate { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
    }
}
