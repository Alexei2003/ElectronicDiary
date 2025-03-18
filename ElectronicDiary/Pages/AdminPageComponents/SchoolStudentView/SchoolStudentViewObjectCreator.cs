using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Pages.Components;
using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses;
using ElectronicDiary.Web.DTO.Responses.Users;
using System.Text.Json;

namespace ElectronicDiary.Pages.AdminPageComponents.SchoolStudentView
{
    public class SchoolStudentViewObjectCreator<TResponse, TRequest, TController> : UserViewObjectCreator<TResponse, TRequest, TController>
        where TResponse : UserResponse, new()
        where TRequest : UserRequest, new()
        where TController : IController, new()
    {
        protected Grid _parentInfoGrid = [];
        protected int _parentInfoGridRowIndex = 0;
        private int _gridParentInfoRowOffset;
        protected override void CreateUI()
        {
            base.CreateUI();

            _parentInfoGrid = new Grid
            {
                // Положение
                ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
                // Положение
                Padding = PageConstants.PADDING_ALL_PAGES,
                ColumnSpacing = PageConstants.SPACING_ALL_PAGES,
                RowSpacing = PageConstants.SPACING_ALL_PAGES,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
            };
            _infoStack.Add(_parentInfoGrid);

            _parentInfoGridRowIndex = 0;
            LineElemsCreator.AddLineElems(
                grid: _parentInfoGrid,
                rowIndex: _parentInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Родители:"
                    }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _parentInfoGrid,
                rowIndex: _parentInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Тип"
                    },
                    new LineElemsCreator.LabelData{
                        Title = "ФИО"
                    }
                ]
            );

            _gridParentInfoRowOffset = _parentInfoGridRowIndex;
            RepaintParentsInfo();
        }

        private List<TypeResponse> _parentTypeList = [];
        private List<StudentParentResponse> _parentList = [];
        private async void RepaintParentsInfo()
        {
            if (_baseResponse.Id != null)
            {
                var response = await ParentController.GetStudentParents(_baseResponse.Id.Value);
                _parentList = [];
                if (!string.IsNullOrEmpty(response)) _parentList = JsonSerializer.Deserialize<List<StudentParentResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];
            }

            for (var i = 0; i < _parentList.Count; i++)
            {
                var studentParent = _parentList[i];
                var parentElems = LineElemsCreator.AddLineElems(
                    grid: _parentInfoGrid,
                    rowIndex: _gridParentInfoRowOffset + i,
                    objectList: [
                        new LineElemsCreator.LabelData{
                            Title = studentParent.ParentType?.Name,
                    },
                        new LineElemsCreator.LabelData{
                            Title =  $"{studentParent.Parent?.LastName} {studentParent.Parent?.FirstName} {studentParent.Parent?.Patronymic}"
                        }
                    ]
                );

                if (parentElems[^1] is Label parentLabel)
                {
                    parentLabel.BindingContext = studentParent.Id;
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += RemoveParentTapped;
                    parentLabel.GestureRecognizers.Add(tapGesture);
                }
            }

            if (_componentState == ComponentState.Edit)
            {
                var response = await ParentController.GetParentType();
                _parentTypeList = [];
                if (!string.IsNullOrEmpty(response)) _parentTypeList = JsonSerializer.Deserialize<List<TypeResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];

                var addParentButton = new Button
                {
                    // Положение
                    HorizontalOptions = LayoutOptions.Fill,

                    // Цвета
                    BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                    TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                    // Текст
                    FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                    Text = "Добавить",
                };
                addParentButton.Clicked += AddParentClicked;
                _parentInfoGrid.Add(addParentButton, 0, _gridParentInfoRowOffset + _parentList.Count);
            }
        }


        private StudentParentRequest _parentRequest = new();
        private bool AddParent = false;
        private async void AddParentClicked(object? sender, EventArgs e)
        {
            AddParent = true;
            var changeRowIndex = _gridParentInfoRowOffset + _parentList.Count;
            LineElemsCreator.ClearGridRows(_parentInfoGrid, changeRowIndex);

            var searchElems = LineElemsCreator.AddLineElems(
                grid: _parentInfoGrid,
                rowIndex: changeRowIndex++,
                objectList:
                [
                    new LineElemsCreator.PickerData{
                        Items = _parentTypeList,
                        IdChangedAction = selectedIndex => _parentRequest.ParentTypeId = selectedIndex,
                    },
                    new LineElemsCreator.SearchData{
                        IdChangedAction = selectedIndex => _parentRequest.ParentId = selectedIndex,
                    }
                ]
            );

            if (searchElems[^1] is TapGestureRecognizer searchParent)
            {
                if (_baseResponse.Id != null)
                {
                    var response = await ParentController.GetParentsWithoutSchoolStudent(_baseResponse.Id.Value);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var list = JsonSerializer.Deserialize<List<UserResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];
                        var idAndFullNameList = list
                            .Select(item => new TypeResponse()
                            {
                                Id = item.Id,
                                Name = $"{item.LastName} {item.FirstName} {item.Patronymic}",
                            })
                            .ToList();

                        searchParent.BindingContext = idAndFullNameList;
                    }
                }
            }

            var saveButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = "Сохранить",
            };
            saveButton.Clicked += SaveParentClicked;
            _parentInfoGrid.Add(saveButton, 0, changeRowIndex++);
            var cancelButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = "Отмена",
            };
            cancelButton.Clicked += (sender, e) =>
            {
                var changeRowIndex = _gridParentInfoRowOffset + _parentList.Count;
                LineElemsCreator.ClearGridRows(_parentInfoGrid, changeRowIndex, changeRowIndex + 1);

                _parentRequest = new();
                AddParent = false;

                var addParentButton = new Button
                {
                    // Положение
                    HorizontalOptions = LayoutOptions.Fill,

                    // Цвета
                    BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                    TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                    // Текст
                    FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                    Text = "Добавить",
                };
                addParentButton.Clicked += AddParentClicked;
                _parentInfoGrid.Add(addParentButton, 0, _gridParentInfoRowOffset + _parentList.Count);

            };
            _parentInfoGrid.Add(cancelButton, 1, changeRowIndex - 1);
        }

        private async void SaveParentClicked(object? sender, EventArgs e)
        {
            _parentRequest.SchoolStudentId = _baseResponse.Id;
            var json = JsonSerializer.Serialize(_parentRequest, PageConstants.JsonSerializerOptions);
            var response = await ParentController.AddParent(json);
            if (!string.IsNullOrEmpty(response))
            {
                var changeRowIndex = _gridParentInfoRowOffset + _parentList.Count;
                LineElemsCreator.ClearGridRows(_parentInfoGrid, _gridParentInfoRowOffset, changeRowIndex + 1);

                _parentRequest = new();
                RepaintParentsInfo();
                AddParent = false;
            }
        }

        private async void RemoveParentTapped(object? sender, EventArgs e)
        {
            if (!AddParent)
            {
                string action = string.Empty;
                var page = Application.Current?.Windows[0].Page;
                if (page != null) action = await page.DisplayActionSheet(
                    "Выберите действие",    // Заголовок
                    "Отмена",               // Кнопка отмены
                    null,                   // Кнопка деструктивного действия (например, удаление)
                    "Удалить");             // Остальные кнопки

                if (action == "Удалить")
                {
                    if (sender is Label label)
                    {
                        var response = await ParentController.DeleteStudentParent((long)label.BindingContext);
                        if (!string.IsNullOrEmpty(response))
                        {
                            LineElemsCreator.ClearGridRows(_parentInfoGrid, _gridParentInfoRowOffset, _gridParentInfoRowOffset + _parentList.Count);
                            RepaintParentsInfo();
                        }
                    }
                }
            }
        }
    }
}
