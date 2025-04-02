using ElectronicDiary.Pages.AdminPageComponents.EducationalInstitutionView;
using ElectronicDiary.Pages.Components.NavigationPage;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public partial class AdminPage : ContentPage
    {
        private readonly HorizontalStackLayout _mainStack = new()
        {
            // Положение
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Fill,
        };
        private readonly List<ScrollView> _viewList = [];

        public AdminPage()
        {
            Title = "Панель администратора";
            ToolbarItemsAdder.AddSettings(ToolbarItems);
            ToolbarItemsAdder.AddLogOut(ToolbarItems);

            // Цвета
            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_PAGE_COLOR;

            var view = new EducationalInstitutionViewListCreator<EducationalInstitutionResponse, EducationalInstitutionRequest, EducationalInstitutionСontroller,
                        EducationalInstitutionViewElemCreator<EducationalInstitutionResponse, EducationalInstitutionRequest, EducationalInstitutionСontroller,
                        EducationalInstitutionViewObjectCreator<EducationalInstitutionResponse, EducationalInstitutionRequest, EducationalInstitutionСontroller>>,
                        EducationalInstitutionViewObjectCreator<EducationalInstitutionResponse, EducationalInstitutionRequest, EducationalInstitutionСontroller>>();
            _viewList.Add(view.Create(_mainStack, _viewList));

            AdminPageStatic.RepaintPage(_mainStack, _viewList);

            Content = _mainStack;
            SizeChanged += WindowSizeChanged;
        }

        private void WindowSizeChanged(object? sender, EventArgs e)
        {
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }

        // Переопределение возврата
        protected override bool OnBackButtonPressed()
        {
            return AdminPageStatic.OnBackButtonPressed(_mainStack, _viewList);
        }
    }
}
