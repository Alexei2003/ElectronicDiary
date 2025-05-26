using ElectronicDiary.SaveData.Static;
using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Components.Navigation;
using ElectronicDiary.UI.Views.Lists.EducationalInstitutionView;
using ElectronicDiary.UI.Views.Lists.General;

namespace ElectronicDiary.Pages.AdminPageComponents.General
{
    public partial class AdminPage : ContentPage
    {
        private readonly HorizontalStackLayout _mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
        private readonly List<ScrollView> _viewList = [];

        public AdminPage()
        {
            Title = "Панель администратора";
            ToolbarItemsAdder.AddSettings(ToolbarItems);
            ToolbarItemsAdder.AddLogOut(ToolbarItems);
            BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;

            var view = new EducationalInstitutionViewListCreator();

            var scrollView = view.Create(_mainStack, _viewList);

            _viewList.Add(scrollView);

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
