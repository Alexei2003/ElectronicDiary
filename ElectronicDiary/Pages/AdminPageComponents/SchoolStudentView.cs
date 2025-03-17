using ElectronicDiary.Pages.AdminPageComponents.Base;
using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses;
using ElectronicDiary.Web.DTO.Responses.Users;
using System.Text.Json;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class SchoolStudentView
        : UserView<BaseUserResponse, BaseUserRequest, SchoolStudentController>
    {
        public SchoolStudentView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList,
            long educationalInstitutionId
        ) : base(mainStack, viewList, educationalInstitutionId)
        {
        }

        private int _gridParentRowOffset;
        protected override void CreateObjectInfoView(ref int rowIndex)
        {
            base.CreateObjectInfoView(ref rowIndex);

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Родители:"
                    }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Тип"
                    },
                    new LineElemsAdder.LabelData{
                        Title = "ФИО"
                    }
                ]
            );

            _gridParentRowOffset = rowIndex;
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
                var parentElems = LineElemsAdder.AddLineElems(
                    grid: _objectGrid,
                    rowIndex: _gridParentRowOffset + i,
                    objectList: [
                        new LineElemsAdder.LabelData{
                            Title = studentParent.ParentType?.Name,
                    },
                        new LineElemsAdder.LabelData{
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
                _objectGrid.Add(addParentButton, 0, _gridParentRowOffset + _parentList.Count);
            }
        }


        private StudentParentRequest _parentRequest = new();
        private bool AddParent = false;
        private async void AddParentClicked(object? sender, EventArgs e)
        {
            AddParent = true;
            var changeRowIndex = _gridParentRowOffset + _parentList.Count;
            LineElemsAdder.ClearGridRows(_objectGrid, changeRowIndex);

            var editElems = LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: changeRowIndex++,
                objectList:
                [
                    new LineElemsAdder.PickerData{
                        Items = _parentTypeList,
                        IdChangedAction = selectedIndex => _parentRequest.ParentTypeId = selectedIndex,
                    },
                    new LineElemsAdder.SearchData{
                        IdChangedAction = selectedIndex => _parentRequest.ParentId = selectedIndex,
                    }
                ]
            );

            if (editElems[^1] is TapGestureRecognizer searchParent)
            {
                if (_baseResponse.Id != null)
                {
                    var response = await ParentController.GetParentsWithoutSchoolStudent(_baseResponse.Id.Value);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var list = JsonSerializer.Deserialize<List<BaseUserResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];
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
            _objectGrid.Add(saveButton, 0, changeRowIndex++);
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
                var changeRowIndex = _gridParentRowOffset + _parentList.Count;
                LineElemsAdder.ClearGridRows(_objectGrid, changeRowIndex, changeRowIndex + 1);

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
                _objectGrid.Add(addParentButton, 0, _gridParentRowOffset + _parentList.Count);

            };
            _objectGrid.Add(cancelButton, 1, changeRowIndex - 1);
        }

        private async void SaveParentClicked(object? sender, EventArgs e)
        {
            _parentRequest.SchoolStudentId = _elemId;
            var json = JsonSerializer.Serialize(_parentRequest, PageConstants.JsonSerializerOptions);
            var response = await ParentController.AddParent(json);
            if (!string.IsNullOrEmpty(response))
            {
                var changeRowIndex = _gridParentRowOffset + _parentList.Count;
                LineElemsAdder.ClearGridRows(_objectGrid, _gridParentRowOffset, changeRowIndex + 1);

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
                            LineElemsAdder.ClearGridRows(_objectGrid, _gridParentRowOffset, _gridParentRowOffset + _parentList.Count);
                            RepaintParentsInfo();
                        }
                    }
                }
            }
        }
    }
}
