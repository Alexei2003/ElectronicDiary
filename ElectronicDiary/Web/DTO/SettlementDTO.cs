using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicDiary.Web.DTO
{
    public record SettlementDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public RegionDTO Region { get; set; } = default!;
    }
}
