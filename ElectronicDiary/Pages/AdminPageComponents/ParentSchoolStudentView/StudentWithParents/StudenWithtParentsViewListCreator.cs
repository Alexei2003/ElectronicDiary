using ElectronicDiary.Web.Api.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.ParentSchoolStudentView.StudentWithParents
{
    public class StudenWithtParentsViewListCreator : ParentStudentViewListCreator<StudentWithParentsController, StudenWithtParentsViewElemCreator>
    {
        public StudenWithtParentsViewListCreator() : base()
        {
            _maxCountViews = 2;
            _titleView = "Список родителей";
        }
    }
}
