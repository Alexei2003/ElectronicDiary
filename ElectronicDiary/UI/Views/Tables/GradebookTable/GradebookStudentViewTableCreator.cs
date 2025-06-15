using System.Collections.ObjectModel;
using System.Text.Json;

using ElectronicDiary.SaveData.Static;
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
            var elem = BaseElemsCreator.CreatePicker(item, newIndex => { _quarterId = newIndex; CreateUI(); }, _quarterId);
            elem.WidthRequest = UserData.Settings.Sizes.CellWidthText;
            elem.Background = UserData.Settings.Theme.BackgroundPageColor;
            return elem;
        }

        protected override async Task GetData()
        {
            var response = await GradebookController.FindBySchoolStudent(_id1, _quarterId);
            if (!string.IsNullOrEmpty(response))
            {
                var arr = JsonSerializer.Deserialize<DiaryResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                var SchoolSubjectArr = arr
                    .Where(d => d.SchoolSubject != null)
                    .Select(d => d.SchoolSubject!)
                    .DistinctBy(s => s.Id)
                    .OrderBy(s => s.Name);

                _headerRowArr = [.. SchoolSubjectArr.Select(s => s.Id)];
                _headerStrRowArr = [.. SchoolSubjectArr.Select(s => s.Name)];

                _headerColumnArr = [.. arr
                    .Where(d => d.DateTime.HasValue)
                    .Select(d => d.DateTime.Value.Date)
                    .Distinct()
                    .OrderBy(date => date)];
                _headerStrColumnArr = [.. _headerColumnArr.Select(s => s.ToString("dd.MM")), "Итог"];

                _dataTableArr = new string[_headerRowArr.Count(), _headerColumnArr.Count() + 1];

                foreach (var diary in arr)
                {
                    var row = -1;
                    var column = -1;

                    for (var i = 0; i < _headerRowArr.Length; i++)
                    {
                        if (_headerRowArr[i] == (diary.SchoolSubject?.Id ?? 0))
                        {
                            row = i;
                        }
                    }

                    for (var i = 0; i < _headerColumnArr.Length; i++)
                    {
                        if (diary.DateTime != null && _headerColumnArr[i] == diary.DateTime.Value.AddHours(-diary.DateTime.Value.Hour))
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

                response = await QuarterScoreController.FindByStudent(_id1);
                if (!string.IsNullOrEmpty(response))
                {
                    var QuarterArr = JsonSerializer.Deserialize<QuarterScoreResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                    foreach (var score in QuarterArr)
                    {
                        if (score.QuarterNumber == _quarterId)
                        {
                            var row = 0;
                            for (var i = 0; i < _headerRowArr.Length; i++)
                            {
                                if (_headerRowArr[i] == (score.SchoolSubject?.Id ?? 0))
                                {
                                    row = i;
                                }
                            }
                            _dataTableArr[row, _headerColumnArr.Length] = score.Score.ToString();
                        }
                    }
                }
            }
        }
    }
}
