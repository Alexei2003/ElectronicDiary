using ElectronicDiary.Pages.AdminPageComponents.Base;
using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.Web.Api.Users;
using System.Text.Json;

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

        protected override void CreateObjectInfoView(Grid grid, int rowIndex = 0, bool edit = false)
        {
            
            

            if (_elemId != -1)
            {
                _response = _objectsList.FirstOrDefault(x => x.Id == _elemId) ?? new();
            }

            if (edit || _elemId == -1)
            {
                _componentTypeEntity = AdminPageStatic.ComponentType.Entity;
                _componentTypePicker = AdminPageStatic.ComponentType.Picker;
            }
            else
            {
                _componentTypeEntity = AdminPageStatic.ComponentType.Label;
                _componentTypePicker = AdminPageStatic.ComponentType.Label;
            }

            base.CreateObjectInfoView(grid);


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
