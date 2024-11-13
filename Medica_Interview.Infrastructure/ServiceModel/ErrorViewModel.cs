using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Infrastructure.ServiceModel
{
    public class ErrorViewModel
    {
        public List<Error> Errors { get; set; } = new List<Error>();
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
