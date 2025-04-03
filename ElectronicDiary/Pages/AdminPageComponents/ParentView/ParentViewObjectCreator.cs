using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.ParentView
{
    public class ParentViewObjectCreator<TResponse, TRequest, TController> : UserViewObjectCreator<TResponse, TRequest, TController>
        where TResponse : UserResponse, new()
        where TRequest : ParentRequest, new()
        where TController : IController, new()
    {

        protected override void CreateUI()
        {
            base.CreateUI();

            var parentSchoolStudent = new ParentSchoolStudent(_baseResponse.Id, true);
            _infoStack.Add(parentSchoolStudent.Grid);
            parentSchoolStudent.ShowList();
        }
    }
}
