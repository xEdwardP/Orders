﻿using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.Entities
{
    public class TemporalOrder
    {
        public int Id { get; set; }

        public User? User { get; set; }
        public string? UserId { get; set; }
        public Product? Product { get; set; }
        public int ProductId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")] // Formato con dos decimales
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")] // Formato con unidad monetaria
        public decimal Value => Product == null ? 0 : Product.Price * (decimal)Quantity;
    }
}