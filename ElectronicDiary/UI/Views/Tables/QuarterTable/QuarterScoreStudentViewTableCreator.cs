using System.Text.Json;

using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Tables.BaseTable;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.UI.Views.Tables.QuarterTable.cs
{
    public class QuarterScoreStudentViewTableCreator : BaseViewTableCreator<int, int>
    {
        protected override async Task GetData()
        {
            var response = await QuarterScoreController.FindByStudent(_id1);
            if (!string.IsNullOrEmpty(response))
            {
                var arr = JsonSerializer.Deserialize<QuarterScoreResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];


            }
        }
    }
}
