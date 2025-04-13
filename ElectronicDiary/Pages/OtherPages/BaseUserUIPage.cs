using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Navigation;
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
            BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;

            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = GridLength.Auto }
                }
            };

            Application.Current.Dispatcher.Dispatch(() =>
            {
                AdminPageStatic.CalcViewWidth(out double width, out _);
                grid.MaximumWidthRequest = width;
            });

            grid.Add(scrollView, 0, 0);

            var hStack = new HorizontalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Padding = UserData.Settings.Sizes.PADDING_ALL_PAGES,
                BackgroundColor = UserData.Settings.Theme.NavigationPageColor,
            };
            hStack.Add(BaseElemsCreator.CreateImageClicked("loading_image.png"));
            hStack.Add(BaseElemsCreator.CreateImageClicked("loading_image.png"));
            hStack.Add(BaseElemsCreator.CreateImageClicked("loading_image.png"));
            hStack.Add(BaseElemsCreator.CreateImageClicked("loading_image.png"));
            hStack.Add(BaseElemsCreator.CreateImageClicked("loading_image.png"));
            hStack.Add(BaseElemsCreator.CreateImageClicked("loading_image.png"));
            hStack.Add(BaseElemsCreator.CreateImageClicked("loading_image.png"));

            grid.Add(hStack, 0, 1);

            Content = grid;
        }
    }
}