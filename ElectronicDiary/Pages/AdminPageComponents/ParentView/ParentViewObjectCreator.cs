using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.ParentView
{
    public class ParentViewObjectCreator<TResponse, TRequest, TController> : UserViewObjectCreator<TResponse, TRequest, TController>
        where TResponse : UserResponse, new()
        where TRequest : ParentRequest, new()
        where TController : IController, new()
    {

        private ParentSchoolStudentCreator? _parentSchoolStudent = null;
        protected override void CreateUI()
        {
            base.CreateUI();

            _parentSchoolStudent = new ParentSchoolStudentCreator(_baseResponse.Id, true);
            _infoStack.Add(_parentSchoolStudent.Grid);
            if (_componentState == AdminPageStatic.ComponentState.New)
            {
                _parentSchoolStudent.AddSchoolStudent(_baseRequest, _educationalInstitutionId);
            }
            else
            {
                _ = _parentSchoolStudent.ShowList();
            }
        }
    }
}
