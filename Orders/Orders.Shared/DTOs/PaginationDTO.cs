using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.DTOs
{
    public class PaginationDTO
    {
        //DTO-> Data Transformation Object -> Pagination
        public int Id { get; set; }
        public int Page { get; set; } = 1; // Initial page
        public int RecordsNumber { get; set; } = 10; // Number of records per page
    }
}
