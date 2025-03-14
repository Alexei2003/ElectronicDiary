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

        protected override void CreateObjectInfoView(Grid grid, ref int rowIndex, bool edit = false)
        {
            base.CreateObjectInfoView(grid, ref rowIndex, edit);

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Дети:"
                    }
                ]
            );


        }
        private static async Task<List<LineElemsAdder.ItemPicker>> GetParantTypes()
        {
            List<LineElemsAdder.ItemPicker>? list = null;
            var response = await ParentController.GetParentType();
            if (response != null)
            {
                list = JsonSerializer.Deserialize<List<LineElemsAdder.ItemPicker>>(response, PageConstants.JsonSerializerOptions);
            }
            return list ?? [];
        }

        private async Task GetSchoolStudents()
        {

        }

        protected async void AddScoolStudent(object? sender, EventArgs e)
        {

        }
        protected async void RemoveScoolStudent(object? sender, EventArgs e)
        {

        }

        protected override async void SaveButtonClicked(object? sender, EventArgs e)
        {
            base.SaveButtonClicked(sender, e);
        }
    }
}
