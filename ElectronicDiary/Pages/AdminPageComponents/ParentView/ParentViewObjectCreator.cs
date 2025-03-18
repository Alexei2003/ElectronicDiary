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

namespace ElectronicDiary.Pages.AdminPageComponents.ParentView
{
    public class ParentViewObjectCreator<TResponse, TRequest, TController> : UserViewObjectCreator<TResponse, TRequest, TController>
        where TResponse : UserResponse, new()
        where TRequest : ParentRequest, new()
        where TController : IController, new()
    {
        protected Grid _parentInfoGrid = [];
        protected int _parentInfoGridRowIndex = 0;
        protected int _gridParentInfoRowOffset;
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
                        Title = "Дети:"
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
            if (_componentState != ComponentState.New)
            {
                if (_baseResponse.Id != null)
                {
                    var response = await ParentController.GetParentStudents(_baseResponse.Id.Value);
                    _parentList = [];
                    if (!string.IsNullOrEmpty(response)) _parentList = JsonSerializer.Deserialize<List<StudentParentResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];
                }

                for (var i = 0; i < _parentList.Count; i++)
                {
                    var studentParent = _parentList[i];
                    LineElemsCreator.AddLineElems(
                        grid: _parentInfoGrid,
                        rowIndex: _gridParentInfoRowOffset + i,
                        objectList: [
                            new LineElemsCreator.LabelData{
                            Title = studentParent.ParentType?.Name,
                    },
                        new LineElemsCreator.LabelData{
                            Title =  $"{studentParent.SchoolStudent?.LastName} {studentParent.SchoolStudent?.FirstName} {studentParent.SchoolStudent?.Patronymic}"
                        }
                        ]
                    );
                }
            }

            if (_componentState == ComponentState.New)
            {
                var response = await ParentController.GetParentType();
                _parentTypeList = [];
                if (!string.IsNullOrEmpty(response)) _parentTypeList = JsonSerializer.Deserialize<List<TypeResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];


                var changeRowIndex = _gridParentInfoRowOffset + _parentList.Count;
                LineElemsCreator.ClearGridRows(_parentInfoGrid, changeRowIndex);

                var searchElems = LineElemsCreator.AddLineElems(
                    grid: _parentInfoGrid,
                    rowIndex: changeRowIndex++,
                    objectList:
                    [
                        new LineElemsCreator.PickerData{
                            Items = _parentTypeList,
                            IdChangedAction = selectedIndex => _baseRequest.ParentType = selectedIndex
                        },
                        new LineElemsCreator.SearchData{
                            IdChangedAction = selectedIndex => _baseRequest.SchoolStudentId = selectedIndex
                        }
                    ]
                );

                if (searchElems[^1] is TapGestureRecognizer searchParent)
                {

                    var schoolStudentController = new SchoolStudentController();
                    response = await schoolStudentController.GetAll(_educationalInstitutionId);
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
        }
    }
}
