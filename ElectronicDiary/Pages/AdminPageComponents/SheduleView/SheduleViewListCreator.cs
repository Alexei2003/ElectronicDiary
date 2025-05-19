using System.Collections.ObjectModel;
using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Pages.AdminPageComponents.SheduleView
{
    public class SheduleViewListCreator<TViewElemCreator, TViewObjectCreator> : BaseViewListCreator<SheduleLessonCustomResponse, SheduleLessonRequest, SheduleLessonController, TViewElemCreator, TViewObjectCreator>
        where TViewElemCreator : BaseViewElemCreator<SheduleLessonCustomResponse, SheduleLessonRequest, SheduleLessonController, TViewObjectCreator>, new()
        where TViewObjectCreator : BaseViewObjectCreator<SheduleLessonCustomResponse, SheduleLessonRequest, SheduleLessonController>, new()
    {
        private long _quarterId = 1;

        protected override void CreateFilterUI(ref int rowIndex)
        {
            base.CreateFilterUI(ref rowIndex);
            var item = new ObservableCollection<TypeResponse>()
            {
                new TypeResponse(1, "1"),
                new TypeResponse(2, "2"),
                new TypeResponse(3, "3"),
                new TypeResponse(4, "4")
            };

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Номер четверти")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreatePicker(item, newIndex => {_quarterId = newIndex; CreateListUI(); }, 1)
                    },
                ]
            );
        }

        protected override async Task GetList()
        {
            var response = await _controller.GetAll(_objectParentId, _quarterId);
            if (!string.IsNullOrEmpty(response))
            {
                var lessonsDict = JsonSerializer.Deserialize<Dictionary<int, SheduleLessonResponse[]>>(response, PageConstants.JsonSerializerOptions) ?? [];

                _objectsArr = new SheduleLessonCustomResponse[5];
                foreach (var lessonArr in lessonsDict.Values)
                {
                    foreach (var lesson in lessonArr)
                    {
                        if (_objectsArr[lesson.DayNumber - 1] == null)
                        {
                            _objectsArr[lesson.DayNumber - 1] = new();
                        }
                        else
                        {
                            var flag = _objectsArr[lesson.DayNumber - 1].Lessons.Where(l => l.Number == lesson.LessonNumber);
                            if (!flag.Any())
                            {
                                _objectsArr[lesson.DayNumber - 1].Lessons.Add(new SheduleLessonElemCustomResponse()
                                {
                                    Number = lesson.LessonNumber,
                                    TeacherAssignment = lesson.TeacherAssignment,
                                    Room = lesson.Room,
                                });
                                _objectsArr[lesson.DayNumber - 1].DayNumber = lesson.DayNumber;
                            }
                        }
                    }
                }

                var objectsList = new List<SheduleLessonCustomResponse>();
                foreach (var obj in _objectsArr)
                {
                    if (obj?.Lessons?.Count > 0)
                    {
                        objectsList.Add(obj);
                    }
                }

                _objectsArr = objectsList.ToArray();
            }
        }
    }
}
