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
        protected override void CreateFilterUI(ref int rowIndex)
        {
            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("<----")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("----->")
                    },
                ]
            );
        }



        protected override async Task GetList()
        {
            await base.GetList();

            var response = await DiaryController.GetAll(UserData.UserInfo.Id, 1);
            response = response.Replace("null", "-1");
            if (!string.IsNullOrEmpty(response))
            {
                var lessonsArr = JsonSerializer.Deserialize<DiaryResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var lesson in lessonsArr) 
                {
                    if(lesson.DateTime != null)
                    {
                        var dayWeek = (int)lesson.DateTime.Value.DayOfWeek;
                        var lessonsInSubject = _objectsArr[dayWeek - 1].Lessons.Where(l=>l.TeacherAssignment.SchoolSubject.Id == lesson.SchoolSubject.Id);
                        if (lessonsInSubject?.Any() ?? false)
                        {
                            lessonsInSubject.First().Diary = lesson;
                        }
                    }
                }
            }
        }
    }
}
