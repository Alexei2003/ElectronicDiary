using System.Text.Json;

using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Tables.BaseTable;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.UI.Views.Tables.QuarterTable
{
    public class QuarterScoreStudentViewTableCreator : BaseViewTableCreator<long, long>
    {
        public QuarterScoreStudentViewTableCreator()
        {
            _attendance = false;
        }

        protected override async Task GetData()
        {
            var response = await QuarterScoreController.FindByStudent(_id1);
            if (!string.IsNullOrEmpty(response))
            {
                var QuarterArr = JsonSerializer.Deserialize<QuarterScoreResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                response = await YearScoreController.FindByStudent(_id1);
                if (!string.IsNullOrEmpty(response))
                {
                    var YearArr = JsonSerializer.Deserialize<YearScoreResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                    _headerRowArr = [.. QuarterArr
                    .Where(d => d.SchoolSubject != null)
                    .Select(d => d.SchoolSubject!)
                    .DistinctBy(s => s.Id)
                    .Select(s => s.Id)];

                    _headerColumnArr = [1, 2, 3, 4, -1];

                    _headerStrRowArr = [.. QuarterArr
                    .Where(d => d.SchoolSubject != null)
                    .Select(d => d.SchoolSubject!)
                    .DistinctBy(s => s.Id)
                    .Select(s => s.Name)];

                    _headerStrColumnArr = ["1", "2", "3", "4", "Годовая"];

                    _dataTableArr = new string[_headerRowArr.Count(), _headerColumnArr.Count()];

                    for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; rowIndex++)
                    {
                        for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length; columnIndex++)
                        {
                            _dataTableArr[rowIndex, columnIndex] = "";
                        }
                    }

                    foreach(var score in QuarterArr)
                    {
                        var row = 0;
                        for (var i = 0; i < _headerRowArr.Length; i++)
                        {
                            if (_headerRowArr[i] == (score.SchoolSubject?.Id ?? 0))
                            {
                                row = i;
                            }
                        }
                        _dataTableArr[row, score.QuarterNumber - 1] = score.Score.ToString();
                    }

                    foreach (var score in YearArr)
                    {
                        var row = 0;
                        for (var i = 0; i < _headerRowArr.Length; i++)
                        {
                            if (_headerRowArr[i] == (score.SchoolSubject?.Id ?? 0))
                            {
                                row = i;
                            }
                        }
                        _dataTableArr[row, 4] = score.Score.ToString();
                    }

                }
            }
        }
    }
}
