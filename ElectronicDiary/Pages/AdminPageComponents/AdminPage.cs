using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class AdminPage : ContentPage
    {
        private readonly HorizontalStackLayout _mainStack = new()
        {
            // Положение
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Fill,
        };
        private readonly List<ScrollView> _viewList = [];
        private EducationalInstitutionView _educationalInstitutionView;
        public AdminPage()
        {
            Title = "Панель администратора";
            ToolbarItemsAdder.AddSettings(ToolbarItems);
            ToolbarItemsAdder.AddLogOut(ToolbarItems);

            // Цвета
            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_PAGE_COLOR;

            _educationalInstitutionView = new EducationalInstitutionView(_mainStack, _viewList);
            _viewList.Add(_educationalInstitutionView.CreateMainView());

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
