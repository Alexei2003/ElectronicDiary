using System.Text.Json;

using ElectronicDiary.Pages.Components;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Responses;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.General
{
    public class ParentSchoolStudent
    {
        private readonly long _userId;
        private readonly bool _isParent;
        public Grid Grid { get; set; }

        public ParentSchoolStudent(long? userId, bool isParent)
        {
            _userId = userId ?? -1;
            _isParent = isParent;

            Grid = BaseElemsCreator.CreateGrid();
        }

        private int _startChangedRow = 0;
        private void BaseShow()
        {
            var rowIndex = 0;
            LineElemsCreator.AddLineElems(
                Grid,
                rowIndex++,
                [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel( _isParent ? "Дети" : "Родители")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel( "ФИО")
                    }
                ]
            );
            _startChangedRow = rowIndex;
        }

        private TypeResponse[] _typesList = [];
        private StudentParentResponse[] _studentParentResponse = [];
        public async void ShowList()
        {
            BaseShow();

            var response = await ParentController.GetParentType();
            if (!string.IsNullOrEmpty(response)) _typesList = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

            if (_isParent)
            {
                response = await ParentController.GetParentStudents(_userId);
            }
            else
            {
                response = await ParentController.GetStudentParents(_userId);
            }
            if (!string.IsNullOrEmpty(response)) _studentParentResponse = JsonSerializer.Deserialize<StudentParentResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];


            for (var i = 0; i < _studentParentResponse.Length; i++)
            {
                var elem = _isParent ? _studentParentResponse[i].SchoolStudent : _studentParentResponse[i].Parent;
                LineElemsCreator.AddLineElems(
                    Grid,
                    _startChangedRow + i,
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_studentParentResponse[i].ParentType?.Name)
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel($"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}")
                        }
                    ]
                );
            }
        }

        public async void Add()
        {

        }

        public async void AddSchoolStudent()
        {

        }

        public async void AddParent()
        {

        }
    }
}
