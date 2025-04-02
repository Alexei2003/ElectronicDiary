using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.SchoolStudentView
{
    public class SchoolStudentViewObjectCreator<TResponse, TRequest, TController> : UserViewObjectCreator<TResponse, TRequest, TController>
        where TResponse : UserResponse, new()
        where TRequest : UserRequest, new()
        where TController : IController, new()
    {
        protected override void CreateUI()
        {
            base.CreateUI();

        }
    }
}
