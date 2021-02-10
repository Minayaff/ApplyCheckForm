using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyCheckESosial.Extensions
{
    public static class Config
    {
        public static string CheckNull(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return "--";
            }
            else
            {
                return data;
            }
        }
    }
}
