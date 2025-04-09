using System.Collections.ObjectModel;
using System.Text.Json;

using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.General
{
    public class ParentSchoolStudentCreator
    {
        private readonly long _userId;
        private readonly bool _isParent;
        public Grid Grid { get; set; }

        public ParentSchoolStudentCreator(long? userId, bool isParent)
        {
            _userId = userId ?? -1;
            _isParent = isParent;

            Grid = BaseElemsCreator.CreateGrid();
            ShowBase();
        }

        private int _startChangedRow = 0;

        private async Task GetInfo()
        {

            TypeResponse[] typesList = [];
            var response = await ParentController.GetParentType();
            if (!string.IsNullOrEmpty(response)) typesList = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

            if (_isParent)
            {
                response = await ParentController.GetParentStudents(_userId);
            }
            else
            {
                response = await ParentController.GetStudentParents(_userId);
            }
            if (!string.IsNullOrEmpty(response)) _studentParentResponse = JsonSerializer.Deserialize<StudentParentResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

        }

        private void ShowBase()
        {
            var rowIndex = 0;
            LineElemsCreator.AddLineElems(
                Grid,
                rowIndex++,
                [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel( _isParent ? "Дети:" : "Родители:")
                    }
                ]
            );

            LineElemsCreator.AddLineElems(
                Grid,
                rowIndex++,
                [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Тип")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("ФИО")
                    }
                ]
            );
            _startChangedRow = rowIndex;
        }

        private StudentParentResponse[] _studentParentResponse = [];
        public async Task ShowList(bool edit = false)
        {
            await GetInfo();
            LineElemsCreator.ClearGridRows(Grid, _startChangedRow, _studentParentResponse.Length + 4);

            for (var i = 0; i < _studentParentResponse.Length; i++)
            {
                var elem = _isParent ? _studentParentResponse[i].SchoolStudent : _studentParentResponse[i].Parent;

                var label = BaseElemsCreator.CreateLabel($"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}");
                if (edit)
                {
                    var tapGesture = new TapGestureRecognizer();
                    var index = i;
                    tapGesture.Tapped += (sender, e) =>
                    {
                        Delete(_studentParentResponse[index].Id);
                    };
                    label.GestureRecognizers.Add(tapGesture);
                }
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
                            Elem = label
                        }
                    ]
                );
            }
        }

        public void AddSchoolStudent(ParentRequest request, long _educationalInstitutionId)
        {
            ShowBase();

            LineElemsCreator.AddLineElems(
                Grid,
                _startChangedRow + 1,
                [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreatePicker(ParentSchoolStudentCreator.GetParentTypes(),
                                                selectedIndex => request.ParentType = selectedIndex)
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateSearchPopupAsLabel(GetSchoolStudents(_educationalInstitutionId),
                                                selectedIndex => request.SchoolStudentId = selectedIndex)
                    }
                ]
            );
        }

        private StudentParentRequest _studentParentRequest;
        private bool _addParent = false;
        public async Task AddParent()
        {
            await ShowList(true);

            var rowIndex = _startChangedRow + _studentParentResponse.Length;
            if (!_addParent)
            {
                LineElemsCreator.AddLineElems(
                    Grid,
                    rowIndex++,
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateButton("Добавить", AddButtonCliked)
                        },
                    ]
                );
            }
            else
            {
                _studentParentRequest = new();
                LineElemsCreator.AddLineElems(
                    Grid,
                    rowIndex++,
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreatePicker(GetParentTypes(),
                                                    selectedIndex => _studentParentRequest.ParentTypeId = selectedIndex)
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateSearchPopupAsLabel(GetParents(),
                                                    selectedIndex => _studentParentRequest.ParentId = selectedIndex)
                        }
                    ]
                );

                LineElemsCreator.AddLineElems(
                    Grid,
                    rowIndex++,
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateButton("Сохранить", SaveButtonCliked)
                        },
                    ]
                );
            }
        }

        private void AddButtonCliked(object sender, EventArgs e)
        {
            _addParent = true;

            _ = AddParent();
        }

        private void SaveButtonCliked(object sender, EventArgs e)
        {
            _studentParentRequest.SchoolStudentId = _userId;
            Task.Run(async () =>
            {
                var json = JsonSerializer.Serialize(_studentParentRequest, PageConstants.JsonSerializerOptions);
                var response = await ParentController.AddParent(json);
                if (!string.IsNullOrEmpty(response))
                {
                    _addParent = false;
                    _ = AddParent();
                }
            });
        }

        private async void Delete(long id)
        {
            var action = await BaseElemsCreator.CreateActionSheetCreator(["Удалить"]);
            if (action == "Удалить")
            {
                _ = await ParentController.DeleteStudentParent(id);
                _ = AddParent();
            }
        }

        private static ObservableCollection<Item> GetParentTypes()
        {
            var list = new ObservableCollection<Item>();

            Task.Run(async () =>
            {
                TypeResponse[]? arr = null;
                var response = await ParentController.GetParentType();
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                Application.Current.Dispatcher.Dispatch(() =>
                {
                    foreach (var elem in arr ?? [])
                    {
                        list.Add(item: new Item(elem.Id, elem.Name));
                    }
                });
            });

            return list;
        }

        private static List<Item> GetSchoolStudents(long _educationalInstitutionId)
        {
            var list = new List<Item>();

            Task.Run(async () =>
            {
                UserResponse[]? arr = null;
                var controller = new SchoolStudentController();
                var response = await controller.GetAll(_educationalInstitutionId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<UserResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, $"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}"));
                }
            });

            return list;
        }

        private List<Item> GetParents()
        {
            var list = new List<Item>();

            Task.Run(async () =>
            {
                UserResponse[]? arr = null;
                var response = await ParentController.GetParentsWithoutSchoolStudent(_userId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<UserResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, $"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}"));
                }
            });

            return list;
        }
    }
}
