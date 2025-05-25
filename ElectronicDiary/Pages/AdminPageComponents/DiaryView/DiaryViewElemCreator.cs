using ElectronicDiary.Pages.AdminPageComponents.SheduleView;
using ElectronicDiary.Pages.Components.Elems;

namespace ElectronicDiary.Pages.AdminPageComponents.DiaryView
{
    public class DiaryViewElemCreator : SheduleViewElemCreator<DiaryViewObjectCreator>
    {
        protected override void CreateDataUI()
        {
            _grid.ColumnDefinitions[0] = new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) };
            _grid.ColumnDefinitions[2] = new ColumnDefinition { Width = new GridLength(2.0, GridUnitType.Star) };
            _grid.ColumnDefinitions[3] = new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) };
            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: 1,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"№")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"Предмет")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"Информация")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"Оценка")
                    },
                ]
            );
            foreach (var lesson in _baseResponse.Lessons)
            {
                var diary = lesson?.DiaryList?.FirstOrDefault();
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
                            Elem = BaseElemsCreator.CreateLabel(diary != null ? $"{diary.Topic}\n{diary.Homework}" : $"")
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(diary != null ?
                                                                     diary.Attendance ? "Н" :
                                                                     diary.Score > 0 ? diary.Score.ToString() : "" : "")
                        },
                    ]
                );
            }
        }
    }
}
