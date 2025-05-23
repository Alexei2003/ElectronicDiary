﻿using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Pages.AdminPageComponents.SheduleView
{
    public class SheduleViewElemCreator<TViewObjectCreator> : BaseViewElemCreator<SheduleLessonCustomResponse, SheduleLessonRequest, SheduleLessonController, TViewObjectCreator>
        where TViewObjectCreator : BaseViewObjectCreator<SheduleLessonCustomResponse, SheduleLessonRequest, SheduleLessonController>, new()
    {
        protected override void CreateUI(ref int rowIndex)
        {
            _grid = BaseElemsCreator.CreateGrid(4);
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
                        Elem = BaseElemsCreator.CreateLabel($"Номер урока")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"Предмет")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"ФИО Учителя")
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
                            Elem = BaseElemsCreator.CreateLabel($"{lesson?.TeacherAssignment?.Teacher?.LastName} {lesson?.TeacherAssignment?.Teacher?.FirstName} {lesson?.TeacherAssignment?.Teacher?.Patronymic}")
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
