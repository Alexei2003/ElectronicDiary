using ElectronicDiary.Web.Api.Users;

namespace ElectronicDiary.UI.Views.Lists.ParentSchoolStudentView.StudentWithParents
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
