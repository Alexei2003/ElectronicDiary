using System.Text.Json;

using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Tables.BaseTable;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.UI.Views.Tables.JournalTable
{
    public class GradebookViewTableCreator : BaseViewTableCreator<long, DateTime>
    {
        protected long quarterId = 1;
        protected override async Task GetData()
        {
            var response = await DiaryController.GetAll(_id1, quarterId);
            if (!string.IsNullOrEmpty(response))
            {
                var arr = JsonSerializer.Deserialize<DiaryResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                _headerColumnArr = [.. arr
                    .Where(d => d.DateTime.HasValue)
                    .Select(d => d.DateTime.Value.Date) 
                    .Distinct()];

                _headerRowArr = [.. arr
                    .Where(d => d.SchoolSubject != null)
                    .Select(d => d.SchoolSubject!)
                    .DistinctBy(s => s.Id)
                    .Select(s => s.Id)];

                _headerStrColumnArr = [.. _headerColumnArr.Select(s => s.ToString())];
                _headerStrRowArr = [.. _headerRowArr.Select(s => s.ToString())];

                _dataTableArr = new string[_headerRowArr.Count(), _headerColumnArr.Count()];
                for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; rowIndex++)
                {
                    for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length; columnIndex++)
                    {
                        _dataTableArr[rowIndex, columnIndex] = "";
                    }
                }
            }
        }
    }
}
