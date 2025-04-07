using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.NavigationPage;
using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class BaseUserUIPage : ContentPage
    {
        public BaseUserUIPage(ScrollView scrollView, string title)
        {
            Title = title;
            ToolbarItemsAdder.AddSettings(ToolbarItems);
            ToolbarItemsAdder.AddLogOut(ToolbarItems);
            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_PAGE_COLOR;

            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = GridLength.Auto }
                }
            };
            grid.Add(scrollView, 0, 0);

            var hStack = new HorizontalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
            };
            hStack.Add(BaseElemsCreator.CreateButton("Главная", delegate { }, true));
            hStack.Add(BaseElemsCreator.CreateButton("Профиль", delegate { }, true));
            hStack.Add(BaseElemsCreator.CreateButton("Новости", delegate { }, true));
            hStack.Add(BaseElemsCreator.CreateButton("Дневник", delegate { }, true));

            grid.Add(hStack, 0, 1);

            Content = grid;
        }
    }
}