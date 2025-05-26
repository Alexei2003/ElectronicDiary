using ElectronicDiary.UI.Views.Lists.ParentSchoolStudentView.ParentWithStudents;
using ElectronicDiary.UI.Views.Lists.UserView;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.UI.Views.Lists.ParentView
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
