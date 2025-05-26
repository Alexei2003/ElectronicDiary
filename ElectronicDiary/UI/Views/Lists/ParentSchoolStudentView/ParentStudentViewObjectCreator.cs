using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.UI.Views.Lists.ParentSchoolStudentView
{
    public class ParentStudentViewObjectCreator<TController> : BaseViewObjectCreator<StudentParentResponse, StudentParentRequest, TController>
        where TController : IController, new()
    {

    }
}
