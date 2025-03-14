using ElectronicDiary.Pages.AdminPageComponents.Base;
using ElectronicDiary.Web.Api.Users;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class TeacherView 
        : UserView<TeacherController>
    {
        public TeacherView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList,
            long educationalInstitutionId
        ) : base(mainStack, viewList, educationalInstitutionId)
        {
            _controller = new();
        }
    }
}
