using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record SheduleLessonCustomResponse : BaseResponse
    {
        public List<SheduleLessonElemCustomResponse> Lessons { get; set; } = [];
    }
}
