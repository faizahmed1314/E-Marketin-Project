using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EMarketing.Models.Extended
{
    [MetadataType(typeof(AdminMetaData))]
    public partial class tbl_admin
    {

    }
    public class AdminMetaData
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "User name is required")]
        [DataType(DataType.Password)]
        
        public string ad_username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        
        public string ad_password { get; set; }
    }
}