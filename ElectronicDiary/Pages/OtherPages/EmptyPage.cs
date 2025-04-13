using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class EmptyPage : ContentPage
    {
        public EmptyPage()
        {
            BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;
            var image = new Image()
            {
                Source = ImageSource.FromFile("dotnet_bot.png")
            };
            var vStack = BaseElemsCreator.CreateVerticalStackLayout();
            vStack.Add(image);
            Content = vStack;
        }
    }
}
