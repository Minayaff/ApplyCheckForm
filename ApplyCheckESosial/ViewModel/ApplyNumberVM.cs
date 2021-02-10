using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApplyCheckESosial.ViewModel
{
    public class ApplyNumberVM
    {
        [Required(ErrorMessage = "Mütləq doldurulmalıdı!")]
        [MaxLength(11, ErrorMessage = "Simvol sayı 11-dən çox olmamalıdır!")]
        [MinLength(11, ErrorMessage = "Simvol sayı 11-dən az olmamalıdır!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Ədəd daxil edin!")]
        public string DocNumber { get; set; }

    }
}
