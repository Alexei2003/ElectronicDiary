using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Pages.AdminPageComponents.SheduleView
{
    public class SheduleViewListCreator : BaseViewListCreator<SheduleLessonCustomResponse, SheduleLessonRequest, SheduleLessonController, SheduleViewElemCreator, SheduleViewObjectCreator>
    {
        private long _quarterId = 1;
        protected override async Task GetList()
        {
            var response = await _controller.GetAll(_objectParentId, _quarterId);
            if (!string.IsNullOrEmpty(response))
            {
                var lessonsDict = JsonSerializer.Deserialize<Dictionary<int, SheduleLessonResponse[]>>(response, PageConstants.JsonSerializerOptions) ?? [];

                _objectsArr = new SheduleLessonCustomResponse[5];
                foreach(var lessonArr in lessonsDict.Values)
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
                                    TeacherAssignment = lesson.TeacherAssignment
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}
