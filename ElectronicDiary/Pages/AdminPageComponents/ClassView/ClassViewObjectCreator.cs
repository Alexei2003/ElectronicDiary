using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.ClassView
{
    public class ClassViewObjectCreator : BaseViewObjectCreator<ClassResponse, ClassRequest, ClassController>
    {
        protected override void CreateUI()
        {
            base.CreateUI();

            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                _baseRequest.Name = _baseResponse.Name;
                _baseRequest.TeacherId = _baseResponse.Teacher?.Id ?? -1;
            }

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel( "Название")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.Name)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor( newText =>  _baseRequest.Name = newText, "7А",_baseResponse.Name )
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Классный руководитель")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel($"{_baseResponse.Teacher?.LastName} {_baseResponse.Teacher?.FirstName} {_baseResponse.Teacher?.Patronymic}")
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateSearchPopupAsLabel(GetTeachers(), selectedIndex => _baseRequest.TeacherId = selectedIndex)
                        }
                ]
            );
        }

        private List<TypeResponse> GetTeachers()
        {
            var list = new List<TypeResponse>();

            Task.Run(async () =>
            {
                UserResponse[]? arr = null;
                var controller = new TeacherController();
                var response = await controller.GetAll(_objectParentId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<UserResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new TypeResponse(elem.Id, $"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}"));
                }
            });

            return list;
        }
    }
}
