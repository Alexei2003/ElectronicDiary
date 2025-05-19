using ElectronicDiary.Pages.AdminPageComponents.SheduleView;
using ElectronicDiary.Pages.Components.Elems;

namespace ElectronicDiary.Pages.AdminPageComponents.DiaryView
{
    public class DiaryViewElemCreator : SheduleViewElemCreator<DiaryViewObjectCreator>
    {
        protected override void CreateDataUI()
        {
            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: 1,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"Номер урока")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"Предмет")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"Оценка")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"Информация")
                    },
                ]
            );
            foreach (var lesson in _baseResponse.Lessons)
            {
                LineElemsCreator.AddLineElems(
                    grid: _grid,
                    rowIndex: (int)lesson.Number + 1,
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
                            Elem = BaseElemsCreator.CreateLabel(lesson?.Diary !=null ?
                                                                        lesson.Diary.Attendance ? "Н" :
                                                                                lesson.Diary.Score > 0 ? lesson.Diary.Score.ToString() : "" : "")
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel($"")
                        },
                    ]
                );
            }
        }
    }
}
