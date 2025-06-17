using ElectronicDiary.SaveData.Static;
using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.UI.Views.Lists.SheduleView
{
    public class SheduleViewElemCreator<TViewObjectCreator> : BaseViewElemCreator<SheduleLessonCustomResponse, SheduleLessonRequest, SheduleLessonController, TViewObjectCreator>
        where TViewObjectCreator : BaseViewObjectCreator<SheduleLessonCustomResponse, SheduleLessonRequest, SheduleLessonController>, new()
    {
        protected override void CreateUI(ref int rowIndex)
        {
            BaseElemsCreator.GridAddColumn(_grid, 2);
            _grid.ColumnDefinitions[0] = new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) };
            if (UserData.UserInfo.Role == SaveData.Other.UserInfo.RoleType.Teacher)
            {
                _grid.ColumnDefinitions[1] = new ColumnDefinition { Width = new GridLength(2.0, GridUnitType.Star) };
                _grid.ColumnDefinitions[2] = new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) };
                _grid.ColumnDefinitions[3] = new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) };
            }
            else
            {
                _grid.ColumnDefinitions[2] = new ColumnDefinition { Width = new GridLength(2.0, GridUnitType.Star) };
                _grid.ColumnDefinitions[3] = new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) };
            }

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: 0,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateTitleLabel(
                        _baseResponse.DayNumber switch
                            {
                                1 => "Понедельник",
                                2 => "Вторник",
                                3 => "Среда",
                                4 => "Четверг",
                                5 => "Пятница",
                                6 => "Суббота",
                                7 => "Воскресенье",
                                _ => ""
                            }),
                        CountJoinColumns = 4
                    }
                ]
            );

            CreateDataUI();
        }

        protected virtual void CreateDataUI()
        {
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
                        Elem = UserData.UserInfo.Role == SaveData.Other.UserInfo.RoleType.Teacher ?
                        BaseElemsCreator.CreateLabel($"Класс"):
                        BaseElemsCreator.CreateLabel($"ФИО Учителя")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"Кабинет")
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

                            Elem = UserData.UserInfo.Role == SaveData.Other.UserInfo.RoleType.Teacher ?
                            BaseElemsCreator.CreateLabel($"{lesson?.TeacherAssignment?.Group?.ClassRoom?.Name}"):
                            BaseElemsCreator.CreateLabel($"{lesson?.TeacherAssignment?.Teacher?.LastName} {lesson?.TeacherAssignment?.Teacher?.FirstName} {lesson?.TeacherAssignment?.Teacher?.Patronymic}")
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel($"{lesson?.Room}")
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
