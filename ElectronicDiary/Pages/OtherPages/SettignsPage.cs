using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class SettignsPage : ContentPage
    {
        public SettignsPage()
        {
            Title = "Настройки";
            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_PAGE_COLOR;

            Content = BaseElemsCreator.CreateVerticalStackLayout();
        }
    }
}
