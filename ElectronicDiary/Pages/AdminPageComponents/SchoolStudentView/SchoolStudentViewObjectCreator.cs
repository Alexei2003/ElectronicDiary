using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.SchoolStudentView
{
    public class SchoolStudentViewObjectCreator : UserViewObjectCreator<SchoolStudentResponse, SchoolStudentRequest, SchoolStudentController>
    {
        private ParentSchoolStudentCreator? _parentSchoolStudent = null;
        protected override void CreateUI()
        {
            base.CreateUI();

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel( "Класс")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.Class?.Name)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateSearchPopupAsLabel(GetClasses(),
                                                    selectedIndex => _baseRequest.ClassId = selectedIndex)
                        }
                ]
            );

            _parentSchoolStudent = new ParentSchoolStudentCreator(_baseResponse.Id, false);
            _infoStack.Add(_parentSchoolStudent.Grid);
            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                _ = _parentSchoolStudent.AddParent();
            }
            else
            {
                _ = _parentSchoolStudent.ShowList();
            }
        }
        private List<Item> GetClasses()
        {
            var list = new List<Item>();

            Task.Run(async () =>
            {
                ClassResponse[]? arr = null;
                var response = await _controller.GetAll(_educationalInstitutionId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<ClassResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, elem?.Name));
                }
            });

            return list;
        }
    }
}
