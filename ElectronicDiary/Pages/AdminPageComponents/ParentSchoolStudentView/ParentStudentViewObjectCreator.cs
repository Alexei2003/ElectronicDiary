using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.ParentSchoolStudentView
{
    public class ParentStudentViewObjectCreator<TController> : BaseViewObjectCreator<StudentParentResponse, StudentParentRequest, TController>
        where TController : IController, new()
    {

    }
}
