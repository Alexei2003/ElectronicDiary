using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicDiary.Web.DTO
{
    public record InstitutionTypeDTO
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
    }
}
