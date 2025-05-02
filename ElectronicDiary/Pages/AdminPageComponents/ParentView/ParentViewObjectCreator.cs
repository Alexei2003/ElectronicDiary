using ElectronicDiary.Pages.AdminPageComponents.ParentSchoolStudentView.ParentWithStudents;
using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.ParentView
{
    public class ParentViewObjectCreator : UserViewObjectCreator<UserResponse, ParentRequest, ParentController>
    {
        protected override void CreateUI()
        {
            base.CreateUI();

            var view = new ParentWithStudentsViewListCreator();
            var scrollView = view.Create([], [], _baseResponse.Id, true);
            _infoStack.Add(scrollView);
        }
    }
}
