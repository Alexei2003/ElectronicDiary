using ElectronicDiary.Pages.AdminPageComponents.Base;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class TeacherView
        : UserView<BaseUserResponse, BaseUserRequest, TeacherController>
    {
        public TeacherView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList,
            long educationalInstitutionId
        ) : base(mainStack, viewList, educationalInstitutionId)
        {
        }
    }
}
