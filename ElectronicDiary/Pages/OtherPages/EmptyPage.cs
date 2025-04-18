using CommunityToolkit.Maui.Views;

using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class EmptyPage : ContentPage
    {
        //public EmptyPage()
        //{
        //    BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;
        //    var image = new Image()
        //    {
        //        Source = ImageSource.FromFile("dotnet_bot.png")
        //    };
        //    var vStack = BaseElemsCreator.CreateVerticalStackLayout();
        //    vStack.Add(image);
        //    Content = vStack;
        //}

        public EmptyPage()
        {
            BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;

            var mediaElement = new MediaElement
            {
                Source = MediaSource.FromFile("D:\\WPS\\ElectronicDiary\\ElectronicDiary\\Resources\\Raw\\jane_doe.mp4"),
                //Source = MediaSource.FromUri("https://sample-videos.com/video321/mp4/720/big_buck_bunny_720p_1mb.mp4"),
                ShouldAutoPlay = true,
                ShouldShowPlaybackControls = false,
                MaximumHeightRequest = 700
            };

            var vStack = BaseElemsCreator.CreateVerticalStackLayout();
            vStack.Add(mediaElement);
            Content = vStack;
        }
    }
}
