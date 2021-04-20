using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DbBicyclesLab.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
    }
}
