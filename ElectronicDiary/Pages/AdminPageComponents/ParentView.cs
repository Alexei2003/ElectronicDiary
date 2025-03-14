using ElectronicDiary.Pages.AdminPageComponents.Base;
using ElectronicDiary.Web.Api.Users;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class ParentView
        : UserView<ParentController>
    {
        public ParentView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList,
            long educationalInstitutionId
        ) : base(mainStack, viewList, educationalInstitutionId)
        {
            _controller = new();
        }

        protected override void CreateObjectInfoView(Grid grid, ref int rowIndex, bool edit = false)
        {
            base.CreateObjectInfoView(grid, ref rowIndex, edit);


        }

        protected void AddScoolStudent()
        {

        }
        protected void RemoveScoolStudent()
        {

        }

        protected override async void SaveButtonClicked(object? sender, EventArgs e)
        {
            base.SaveButtonClicked(sender, e);
        }
    }
}
