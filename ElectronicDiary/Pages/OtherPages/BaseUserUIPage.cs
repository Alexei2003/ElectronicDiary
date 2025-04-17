using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Navigation;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.SaveData.Themes;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class BaseUserUIPage : ContentPage
    {
        private PageType _pageName;
        public BaseUserUIPage(IView hStack, PageType pageName)
        {
            _pageName = pageName;

            Title = UserInfo.ConvertEnumRoleToStringRus(UserData.UserInfo.Role);
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
            Content = grid;
            grid.Add(hStack, 0, 0);

            var countColumns = 6;
            switch (UserData.UserInfo.Role)
            {
                case UserInfo.RoleType.LocalAdmin:
                    countColumns = 4;
                    break;
                case UserInfo.RoleType.Parent:
                    countColumns = 5;
                    break;
            }
            var gridButtons = BaseElemsCreator.CreateGrid(countColumns, false);
            grid.Add(gridButtons, 0, 1);
            CreateNavigationButtons(gridButtons, countColumns, pageName);
        }

        public enum PageType
        {
            Profile, Chats, News, Shedule, Diary, Search
        }

        public void CreateNavigationButtons(Grid gridButtons, int countColumns, PageType pageName)
        {
            var elemArr = new LineElemsCreator.Data[countColumns];
            var colorButton = UserData.Settings.Theme.TextIsBlack ? "black_" : "white_";
            var colorButtonSelected = !UserData.Settings.Theme.TextIsBlack ? "black_" : "white_";

            var indexColumn = 0;
            string path = "profile_icon.png";
            if (pageName == PageType.Profile)
            {
                IAppTheme theme = UserData.Settings.Theme.TextIsBlack ? new DarkTheme() : new LightTheme();
                gridButtons.Add(new BoxView { Color = theme.BackgroundFillColor }, indexColumn, 0);
                path = colorButtonSelected + path;
            }
            else
            {
                path = colorButton + path;
            }
            elemArr[indexColumn++] = new LineElemsCreator.Data
            {
                Elem = BaseElemsCreator.CreateImageClicked(path, ProfileTapped)
            };

            path = "chats_icon.png";
            if (pageName == PageType.Chats)
            {
                IAppTheme theme = UserData.Settings.Theme.TextIsBlack ? new DarkTheme() : new LightTheme();
                gridButtons.Add(new BoxView { Color = theme.BackgroundFillColor }, indexColumn, 0);
                path = colorButtonSelected + path;
            }
            else
            {
                path = colorButton + path;
            }
            elemArr[indexColumn++] = new LineElemsCreator.Data
            {
                Elem = BaseElemsCreator.CreateImageClicked(path, ChatsTapped)
            };

            path = "news_icon.png";
            if (pageName == PageType.News)
            {
                IAppTheme theme = UserData.Settings.Theme.TextIsBlack ? new DarkTheme() : new LightTheme();
                gridButtons.Add(new BoxView { Color = theme.BackgroundFillColor }, indexColumn, 0);
                path = colorButtonSelected + path;
            }
            else
            {
                path = colorButton + path;
            }
            elemArr[indexColumn++] = new LineElemsCreator.Data
            {
                Elem = BaseElemsCreator.CreateImageClicked(path, NewsTapped)
            };

            if (countColumns == 6)
            {
                path = "shedule_icon.png";
                if (pageName == PageType.Shedule)
                {
                    IAppTheme theme = UserData.Settings.Theme.TextIsBlack ? new DarkTheme() : new LightTheme();
                    gridButtons.Add(new BoxView { Color = theme.BackgroundFillColor }, indexColumn, 0);
                    path = colorButtonSelected + path;
                }
                else
                {
                    path = colorButton + path;
                }
                elemArr[indexColumn++] = new LineElemsCreator.Data
                {
                    Elem = BaseElemsCreator.CreateImageClicked(path, SheduleTapped)
                };
            }

            if (countColumns > 4)
            {
                path = "diary_icon.png";
                if (pageName == PageType.Diary)
                {
                    IAppTheme theme = UserData.Settings.Theme.TextIsBlack ? new DarkTheme() : new LightTheme();
                    gridButtons.Add(new BoxView { Color = theme.BackgroundFillColor }, indexColumn, 0);
                    path = colorButtonSelected + path;
                }
                else
                {
                    path = colorButton + path;
                }
                elemArr[indexColumn++] = new LineElemsCreator.Data
                {
                    Elem = BaseElemsCreator.CreateImageClicked(path, DiaryTapped)
                };
            }

            path = "search_icon.png";
            if (pageName == PageType.Search)
            {
                IAppTheme theme = UserData.Settings.Theme.TextIsBlack ? new DarkTheme() : new LightTheme();
                gridButtons.Add(new BoxView { Color = theme.BackgroundFillColor }, indexColumn, 0);
                path = colorButtonSelected + path;
            }
            else
            {
                path = colorButton + path;
            }
            elemArr[indexColumn++] = new LineElemsCreator.Data
            {
                Elem = BaseElemsCreator.CreateImageClicked(path, SearchTapped)
            };

            LineElemsCreator.AddLineElems(
                gridButtons,
                0,
                elemArr
            );
        }

        public void ProfileTapped(object? sender, EventArgs e)
        {
            if(_pageName != PageType.Profile)
            {
                Navigation.PushAsync(new BaseUserUIPage(BaseElemsCreator.CreateVerticalStackLayout(), PageType.Profile));
            }
        }

        public void ChatsTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Chats)
            {
                Navigation.PushAsync(new BaseUserUIPage(BaseElemsCreator.CreateVerticalStackLayout(), PageType.Chats));
            }
        }

        public void NewsTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.News)
            {
                Navigation.PushAsync(new BaseUserUIPage(BaseElemsCreator.CreateVerticalStackLayout(), PageType.News));
            }
        }

        public void SheduleTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Shedule)
            {
                Navigation.PushAsync(new BaseUserUIPage(BaseElemsCreator.CreateVerticalStackLayout(), PageType.Shedule));
            }
        }

        public void DiaryTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Diary)
            {
                Navigation.PushAsync(new BaseUserUIPage(BaseElemsCreator.CreateVerticalStackLayout(), PageType.Diary));
            }
        }

        public void SearchTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Search)
            {
                Navigation.PushAsync(new BaseUserUIPage(BaseElemsCreator.CreateVerticalStackLayout(), PageType.Search));
            }
        }
    }
}