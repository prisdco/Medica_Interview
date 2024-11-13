using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medica_Interview.Infrastructure.Entities
{
    public class DataSourceReader
    {
        public string? Title { get; set; } = "";
        public List<ExcelDataSourceReader> ExcelDataSourceReader { get; set; }
    }
}
