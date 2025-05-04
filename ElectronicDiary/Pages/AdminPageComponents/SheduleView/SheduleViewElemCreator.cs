using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Pages.AdminPageComponents.SheduleView
{
    public class SheduleViewElemCreator : BaseViewElemCreator<SheduleLessonCustomResponse, SheduleLessonRequest, SheduleLessonController, SheduleViewObjectCreator>
    {
        protected override void CreateUI(ref int rowIndex)
        {
            _grid = BaseElemsCreator.CreateGrid(3);
            foreach (var lesson in _baseResponse.Lessons)
            {
                LineElemsCreator.AddLineElems(
                    grid: _grid,
                    rowIndex: (int)lesson.Number-1,
                    objectList: [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel($"{lesson.Number}")
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel($"{lesson?.TeacherAssignment?.SchoolSubject?.Name}")
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel($"{lesson?.TeacherAssignment?.Teacher?.LastName} {lesson?.TeacherAssignment?.Teacher?.FirstName} {lesson?.TeacherAssignment?.Teacher?.Patronymic}")
                        },
                    ]
                );
            }
        }

        protected override void GestureTapped(object? sender, EventArgs e)
        {

        }
    }
}
