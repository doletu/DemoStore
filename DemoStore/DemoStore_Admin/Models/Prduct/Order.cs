using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string UserId { get; set; }

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