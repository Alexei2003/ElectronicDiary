using ElectronicDiary.Pages.AdminPageComponents.Base;
using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Responses;
using ElectronicDiary.Web.DTO.Responses.Users;
using System.Text.Json;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class ParentView
        : UserView<ParentController>
    {
        public ParentView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList,
            long educationalInstitutionId
        ) : base(mainStack, viewList, educationalInstitutionId)
        {
            _controller = new();
        }

        protected int _gridSchoolStudentRowIndex;
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

            _gridSchoolStudentRowIndex = rowIndex;
            GetSchoolStudentInfo();


        }

        protected List<TypeResponse> _parentTypeList = [];
        protected List<StudentParentResponse> _studentParentList = [];
        protected async void GetSchoolStudentInfo()
        {
            List<TypeResponse>? typeList = null;
            var response = await ParentController.GetParentType();
            if (!string.IsNullOrEmpty(response) ) typeList = JsonSerializer.Deserialize<List<TypeResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];
            _parentTypeList = typeList ?? [];


            if (_baseResponse != null)
            {
                List<StudentParentResponse>? list = null;
                response = await ParentController.GetParentStudents(_baseResponse.Id);
                if (!string.IsNullOrEmpty(response)) list = JsonSerializer.Deserialize<List<StudentParentResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];
                _studentParentList = list ?? [];
            }

            RepaintStudentParent();
        }

        protected void RepaintStudentParent()
        {

            for (var i = 0; i < _studentParentList.Count; i++)
            {
                var studentParent = _studentParentList[i];
                LineElemsAdder.AddLineElems(
                    grid: _objectGrid,
                    rowIndex: _gridSchoolStudentRowIndex + i,
                    objectList: [
                        new LineElemsAdder.LabelData{
                            Title = studentParent.ParentType.Name,
                    },
                        new LineElemsAdder.LabelData{
                            Title = studentParent.SchoolStudent.LastName +
                                    studentParent.SchoolStudent.FirstName +
                                    studentParent.SchoolStudent.Patronymic,
                        }
                    ]
                );
            }
        }

        protected async void AddScoolStudent()
        {

        }
        protected async void RemoveScoolStudent()
        {

        }
    }
}
