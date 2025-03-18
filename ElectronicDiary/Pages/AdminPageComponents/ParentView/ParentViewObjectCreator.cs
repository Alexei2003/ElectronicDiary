using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Pages.Components;
using ElectronicDiary.Pages.Others;
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
        protected int _gridParentRowOffset;
        protected override void CreateUI(ref int rowIndex)
        {
            base.CreateUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Дети:"
                    }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Тип"
                    },
                    new LineElemsCreator.LabelData{
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
            if (_componentState != ComponentState.New)
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
                    LineElemsCreator.AddLineElems(
                        grid: _grid,
                        rowIndex: _gridParentRowOffset + i,
                        objectList: [
                            new LineElemsCreator.LabelData{
                            Title = studentParent.ParentType?.Name,
                    },
                        new LineElemsCreator.LabelData{
                            Title =  $"{studentParent.Parent?.LastName} {studentParent.Parent?.FirstName} {studentParent.Parent?.Patronymic}"
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


                var changeRowIndex = _gridParentRowOffset + _parentList.Count;
                LineElemsCreator.ClearGridRows(_grid, changeRowIndex);

                var editElems = LineElemsCreator.AddLineElems(
                    grid: _grid,
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

                if (editElems[^1] is TapGestureRecognizer searchParent)
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
