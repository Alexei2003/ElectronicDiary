using ElectronicDiary.Web.Api.Users;

namespace ElectronicDiary.UI.Views.Lists.ParentSchoolStudentView.ParentWithStudents
{
    public class ParentWithStudentsViewListCreator : ParentStudentViewListCreator<ParentWithStudentsController, ParentWithtStudentsElemCreator>
    {
        public ParentWithStudentsViewListCreator() : base()
        {
            _maxCountViews = 2;
            _titleView = "Список детей";
        }
    }
}
