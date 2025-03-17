using ElectronicDiary.Pages.AdminPageComponents.Base;
using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses;
using ElectronicDiary.Web.DTO.Responses.Users;
using System.Text.Json;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class ParentView
        : UserView<BaseUserResponse, ParentRequest, ParentController>
    {
        public ParentView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList,
            long educationalInstitutionId
        ) : base(mainStack, viewList, educationalInstitutionId)
        {
            _baseRequest = new ParentRequest();
        }

        protected int _gridParentRowOffset;
        protected override void CreateObjectInfoView(ref int rowIndex)
        {
            base.CreateObjectInfoView(ref rowIndex);

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Дети:"
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
                    LineElemsAdder.AddLineElems(
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
                }
            }

            if (_componentState == ComponentState.New)
            {
                var response = await ParentController.GetParentType();
                _parentTypeList = [];
                if (!string.IsNullOrEmpty(response)) _parentTypeList = JsonSerializer.Deserialize<List<TypeResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];


                var changeRowIndex = _gridParentRowOffset + _parentList.Count;
                LineElemsAdder.ClearGridRows(_objectGrid, changeRowIndex);

                var editElems = LineElemsAdder.AddLineElems(
                    grid: _objectGrid,
                    rowIndex: changeRowIndex++,
                    objectList:
                    [
                        new LineElemsAdder.PickerData{
                            Items = _parentTypeList,
                            IdChangedAction = selectedIndex => _baseRequest.ParentType = selectedIndex
                        },
                        new LineElemsAdder.SearchData{
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
        }
    }
}
