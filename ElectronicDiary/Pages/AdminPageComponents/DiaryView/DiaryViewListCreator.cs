using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.SheduleView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Pages.AdminPageComponents.DiaryView
{
    public class DiaryViewListCreator : SheduleViewListCreator<DiaryViewElemCreator, DiaryViewObjectCreator>
    {
        private readonly Label _beginLabel = BaseElemsCreator.CreateLabel("");
        private readonly Label _endLabel = BaseElemsCreator.CreateLabel("");
        protected override void CreateFilterUI(ref int rowIndex)
        {
            base.CreateFilterUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = _beginLabel,
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = _endLabel,
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateButton("Назад",PrevButtonClicked)
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateButton("Вперёд",NextButtonClicked)
                    },
                ]
            );
        }

        protected virtual void NextButtonClicked(object? sender, EventArgs e)
        {
            ChangeDate(7);
        }

        protected virtual void PrevButtonClicked(object? sender, EventArgs e)
        {
            ChangeDate(-7);
        }

        protected virtual void ChangeDate(int changeDay)
        {
            var begin = _begin.AddDays(changeDay);
            var end = _end.AddDays(changeDay);
            if (_quarterInfoResponse.DateStartTime <= begin && end <= _quarterInfoResponse.DateEndTime)
            {
                _begin = begin;
                _end = end;
                FilterList();
                CreateListUIWithoutUpdate();
            }
        }


        private DateTime _begin;
        private DateTime _end;
        private SheduleLessonCustomResponse[] _objectsArrSave = [];
        protected override async Task GetList()
        {
            await base.GetList();

            var response = await DiaryController.GetAll(UserData.UserInfo.Id, _quarterId);
            if (!string.IsNullOrEmpty(response))
            {
                var lessonsArr = JsonSerializer.Deserialize<DiaryResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var lesson in lessonsArr)
                {
                    if (lesson.DateTime != null)
                    {
                        var dayWeek = (int)lesson.DateTime.Value.DayOfWeek;
                        var lessonsInSubject = _objectsArr[dayWeek - 1].Lessons.Where(l => (l.TeacherAssignment?.SchoolSubject?.Id ?? -1) == (lesson?.SchoolSubject?.Id ?? -1));
                        if (lessonsInSubject?.Any() ?? false)
                        {
                            var obj = lessonsInSubject.First();
                            if (obj.DiaryList?.Count == null)
                            {
                                obj.DiaryList = [];
                            }
                            lessonsInSubject.First()?.DiaryList?.Add(lesson);
                        }
                    }
                }
            }

            _objectsArrSave = _objectsArr;

            var date = DateTime.Now;
            if (_quarterInfoResponse.DateStartTime > date || date > _quarterInfoResponse.DateEndTime)
            {
                date = _quarterInfoResponse.DateStartTime ?? DateTime.Now;
            }

            var dayIndex = ((int)date.DayOfWeek - 1);
            _begin = date.AddDays(-dayIndex);
            _end = date.AddDays(6 - dayIndex);

            FilterList();
        }

        protected override void FilterList()
        {
            _objectsArr = new SheduleLessonCustomResponse[5];
            for (var i = 0; i < _objectsArrSave.Length; i++)
            {
                _objectsArr[i] = new SheduleLessonCustomResponse()
                {
                    DayNumber = _objectsArrSave[i].DayNumber,
                    Lessons = []
                };
                for (var n = 0; n < (_objectsArrSave[i].Lessons?.Count ?? 0); n++)
                {
                    _objectsArr[i].Lessons.Add(new SheduleLessonElemCustomResponse()
                    {
                        Number = _objectsArrSave[i].Lessons[n].Number,
                        TeacherAssignment = _objectsArrSave[i].Lessons[n].TeacherAssignment,
                        Room = _objectsArrSave[i].Lessons[n].Room,

                        DiaryList = [.. _objectsArrSave[i].Lessons[n].DiaryList?.Where(l => _begin <= l.DateTime && l.DateTime <= _end) ?? []],
                    });
                }
            }

            _beginLabel.Text = _begin.ToString();
            _endLabel.Text = _end.ToString();
        }
    }
}
