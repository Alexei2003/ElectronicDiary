using System.Collections.ObjectModel;
using System.Text.Json;

using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Tables.BaseTable;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.UI.Views.Tables.JournalTable
{
    public class GradebookStudentViewTableCreator : BaseViewTableCreator<long, DateTime>
    {
        protected long _quarterId = 1;

        protected override View CreateHeaderUI()
        {
            var item = new ObservableCollection<TypeResponse>()
            {
                new TypeResponse(1, "1"),
                new TypeResponse(2, "2"),
                new TypeResponse(3, "3"),
                new TypeResponse(4, "4")
            };

            return BaseElemsCreator.CreatePicker(item, newIndex => {_quarterId = newIndex; CreateUI(); }, _quarterId);
        }

        protected override async Task GetData()
        {
            var response = await DiaryController.GetAll(_id1, _quarterId);
            if (!string.IsNullOrEmpty(response))
            {
                var arr = JsonSerializer.Deserialize<DiaryResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                _headerRowArr = [.. arr
                    .Where(d => d.SchoolSubject != null)
                    .Select(d => d.SchoolSubject!)
                    .DistinctBy(s => s.Id)
                    .Select(s => s.Id)];

                _headerColumnArr = [.. arr
                    .Where(d => d.DateTime.HasValue)
                    .Select(d => d.DateTime.Value.Date)
                    .Distinct()];

                _headerStrColumnArr = [.. _headerColumnArr.Select(s => s.ToString())];
                _headerStrRowArr = [.. arr
                    .Where(d => d.SchoolSubject != null)
                    .Select(d => d.SchoolSubject!)
                    .DistinctBy(s => s.Id)
                    .Select(s => s.Name)];

                _dataTableArr = new string[_headerRowArr.Count(), _headerColumnArr.Count()];
                for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; rowIndex++)
                {
                    for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length; columnIndex++)
                    {
                        _dataTableArr[rowIndex, columnIndex] = "";
                    }
                }

                foreach(var diary in arr)
                {
                    var row = -1;
                    var column = -1;

                    for(var i = 0; i < _headerRowArr.Length; i++)
                    {
                        if (_headerRowArr[i] == (diary.SchoolSubject?.Id ?? 0))
                        {
                            row = i;
                        }
                    }

                    for (var i = 0; i < _headerColumnArr.Length; i++)
                    {
                        if (diary.DateTime != null && _headerColumnArr[i] == diary.DateTime.Value.AddHours(- diary.DateTime.Value.Hour))
                        {
                            column = i;
                        }
                    }

                    if (row != -1 && column != -1)
                    {
                        if (diary.Score != null)
                        {
                            _dataTableArr[row, column] = diary.Score.ToString();
                        }
                        if (diary.Attendance)
                        {
                            _dataTableArr[row, column] = "Н";
                        }
                    }
                }
            }
        }
    }
}
