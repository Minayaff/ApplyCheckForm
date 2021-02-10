using ApplyCheckESosial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyCheckESosial.Services
{
    public interface IESosialService
    {
        Task<List<ESosialServiceData>> GetAsync(string idNumber, DateTime bithdate);
        Task<List<ESosialServiceData>> GetAsync(string docNumber);
    }
}
