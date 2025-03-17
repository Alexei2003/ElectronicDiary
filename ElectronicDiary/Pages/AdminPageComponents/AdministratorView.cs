using ElectronicDiary.Pages.AdminPageComponents.Base;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class AdministratorView
        : UserView<BaseUserResponse, BaseUserRequest, AdministratorController>
    {
        public AdministratorView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList,
            long educationalInstitutionId
        ) : base(mainStack, viewList, educationalInstitutionId)
        {
        }
    }
}
