using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApplyCheckESosial.ViewModel
{
    public class PassportVM
    {
        public string SeriyaType { get; set; }

        [Required(ErrorMessage = "Mütləq doldurulmalıdır!")]
        [MaxLength(8, ErrorMessage = "Simvol sayı 8-dən çox olmamalıdır!")]
        [MinLength(7, ErrorMessage = "Simvol sayı 7-dən az olmamalıdır!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Ədəd daxil edin!")]
        public string SeriyaNumber { get; set; }

        [Required(ErrorMessage = "Mütləq doldurulmalıdır!")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
    }
}
