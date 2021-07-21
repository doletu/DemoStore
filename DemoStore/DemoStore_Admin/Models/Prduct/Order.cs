using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoStore_Admin.Models.Prduct
{
    public class Order
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        [UserValidate(ErrorMessage ="User not avaible")]
       
        [ForeignKey("ApplicationUser")]
        [StringLength(128)]
        public string UserId { get; set; }
        public ApplicationUser  ApplicationUser { get; set; }


        [Required]
        public string ShipName { get; set; }
        [Required]
        public string ShipAddress { get; set; }
        [Required]
        public string ShipPhoneNumber { get; set; }

        [Required]
        public int status { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }
    }



}