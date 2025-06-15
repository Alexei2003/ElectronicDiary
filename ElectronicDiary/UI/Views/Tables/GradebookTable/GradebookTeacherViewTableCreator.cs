using System.Text.Json;

using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Tables.BaseTable;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.UI.Views.Tables.GradebookTable
{
    public class GradebookTeacherViewTableCreator : BaseViewTableCreator<long, long>
    {
        public GradebookTeacherViewTableCreator()
        {
            _hFlag = true;
        }

        private long _schoolSubjectId = -1;
        private long[] _quarterScoreIdArr = [];
        protected override async Task GetData()
        {
            var response = await GroupController.GetGroupMembersByTeacherAssignment(_id1);
            if (!string.IsNullOrEmpty(response))
            {
                var groupMemberArr = JsonSerializer.Deserialize<GroupInfoResponse>(response, PageConstants.JsonSerializerOptions) ?? new();

                var SchoolStudentArr = groupMemberArr.GroupMembers
                    .Where(d => d.SchoolStudent != null)
                    .Select(d => d.SchoolStudent!)
                    .DistinctBy(s => s.Id)
                    .OrderBy(s => s.LastName)
                    .ThenBy(s => s.FirstName)
                    .ThenBy(s => s.Patronymic);

                _headerRowArr = [.. SchoolStudentArr.Select(s => s.Id)];
                _headerStrRowArr = [.. SchoolStudentArr.Select(s => $"{s.LastName} {s.FirstName} {s.Patronymic}")];

                response = await GradebookController.FindByTeacherAssignment(_id1, _id2);
                if (!string.IsNullOrEmpty(response))
                {
                    var gradebook = JsonSerializer.Deserialize<GradebookResponse>(response, PageConstants.JsonSerializerOptions) ?? new();
                    _schoolSubjectId = gradebook.GradebookDayResponseDTOS[0].ScheduleLesson.TeacherAssignment.SchoolSubject.Id;

                    GradebookDayResponse[] arr = [.. gradebook.GradebookDayResponseDTOS
                        .DistinctBy(day => day.DateTime)
                        .OrderBy(day => day.DateTime)];

                    _headerColumnArr = [.. arr.Select(day => day.Id)];
                    _headerStrColumnArr = [.. arr.Select(s => s.DateTime.Value.ToString("dd.MM")), "Итог"];

                    _dataTableArr = new string[_headerRowArr.Count(), _headerColumnArr.Count() + 1];

                    foreach (var score in gradebook.GradebookScoreResponseDTOS)
                    {
                        var row = -1;
                        var column = -1;

                        for (var i = 0; i < _headerRowArr.Length; i++)
                        {
                            if (_headerRowArr[i] == (score.SchoolStudent?.Id ?? 0))
                            {
                                row = i;
                            }
                        }

                        for (var i = 0; i < _headerColumnArr.Length; i++)
                        {
                            if (score.GradebookDay?.DateTime != null && arr[i].DateTime == score.GradebookDay.DateTime.Value)
                            {
                                column = i;
                            }
                        }

                        if (row != -1 && column != -1)
                        {
                            if (score.Score != null)
                            {
                                _dataTableArr[row, column] = score.Score.ToString();
                            }
                        }
                    }

                    foreach (var attendance in gradebook.GradebookAttendanceResponseDTOS)
                    {
                        var row = -1;
                        var column = -1;

                        for (var i = 0; i < _headerRowArr.Length; i++)
                        {
                            if (_headerRowArr[i] == (attendance.SchoolStudent?.Id ?? 0))
                            {
                                row = i;
                            }
                        }

                        for (var i = 0; i < _headerColumnArr.Length; i++)
                        {
                            if (attendance.GradebookDay?.DateTime != null && arr[i].DateTime == attendance.GradebookDay.DateTime.Value)
                            {
                                column = i;
                            }
                        }

                        if (row != -1 && column != -1)
                        {
                            _dataTableArr[row, column] = "Н";
                        }
                    }

                    _quarterScoreIdArr = new long[_headerRowArr.Count()];
                    response = await QuarterScoreController.FindByTeacherAssignment(_id1);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var quarterArr = JsonSerializer.Deserialize<QuarterScoreResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                        foreach (var score in quarterArr)
                        {
                            if (score.QuarterNumber == _id2)
                            {
                                var row = 0;
                                for (var i = 0; i < _headerRowArr.Length; i++)
                                {
                                    if (_headerRowArr[i] == score.SchoolStudentId)
                                    {
                                        row = i;
                                    }
                                }
                                _quarterScoreIdArr[row] = score.Id;
                                _dataTableArr[row, _headerColumnArr.Length] = score.Score.ToString();
                            }
                        }
                    }
                }
            }
        }

        protected override async void Send(int rowIndex, int columnIndex, string value, string oldValue)
        {
            var schoolStudentId = _headerRowArr[rowIndex];
            long gradebookDayId = -1;
            if (columnIndex < _headerColumnArr.Length)
            {
                gradebookDayId = _headerColumnArr[columnIndex];
            }

            if (_hFlag)
            {
                if (columnIndex == _headerStrColumnArr.Length - 1)
                {
                    if (_choseArr.Contains(oldValue))
                    {
                        await QuarterScoreController.UpdateScore(_quarterScoreIdArr[rowIndex], int.Parse(value));
                    }
                    else
                    {
                        await QuarterScoreController.AddScore(schoolStudentId, _schoolSubjectId, _id2, int.Parse(value));
                    }
                }
                else
                {
                    if (_choseArr.Contains(value))
                    {
                        await GradebookController.UpdateGradebookScore(gradebookDayId, schoolStudentId, int.Parse(value));
                    }
                    else
                    {
                        await GradebookController.UpdateGradebookAttendance(gradebookDayId, schoolStudentId);
                    }
                }
            }
            else
            {
                await GradebookController.UpdateGradebookScore(gradebookDayId, schoolStudentId, int.Parse(value));
            }
        }

        protected async Task SendDay(long gradebookDayId, string topic, string homework)
        {
            await GradebookController.UpdateGradebookDay(gradebookDayId, topic, homework);
        }
    }
}
