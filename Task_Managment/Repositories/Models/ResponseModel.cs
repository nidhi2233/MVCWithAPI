using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class ResponseModel<T>
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public T? data { get; set; }
    }
}