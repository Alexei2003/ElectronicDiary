using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Navigation;
using ElectronicDiary.Pages.OtherViews.NewsView;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.SaveData.Themes;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class BaseUserUIPage : ContentPage
    {
        private PageType _pageName;
        private readonly HorizontalStackLayout _mainStack = [];
        private readonly List<ScrollView> _viewList = [];
        public BaseUserUIPage(HorizontalStackLayout mainStack, List<ScrollView> viewList, PageType pageName)
        {
            _mainStack = mainStack;
            _viewList = viewList;
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
            SizeChanged += WindowSizeChanged;
            grid.Add(_mainStack, 0, 0);

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

        private void WindowSizeChanged(object? sender, EventArgs e)
        {
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }

        protected override bool OnBackButtonPressed()
        {
            AdminPageStatic.OnBackButtonPressed(_mainStack, _viewList);
            if (_mainStack.Count == 1)
            {
                return base.OnBackButtonPressed();
            }
            return true;
        }

        public enum PageType
        {
            Profile, Chats, News, Shedule, Diary, Search
        }

        private void CreateNavigationButtons(Grid gridButtons, int countColumns, PageType pageName)
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
                Elem = BaseElemsCreator.CreateImageFromFile(path, ProfileTapped)
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
                Elem = BaseElemsCreator.CreateImageFromFile(path, ChatsTapped)
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
                Elem = BaseElemsCreator.CreateImageFromFile(path, NewsTapped)
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
                    Elem = BaseElemsCreator.CreateImageFromFile(path, SheduleTapped)
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
                    Elem = BaseElemsCreator.CreateImageFromFile(path, DiaryTapped)
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
                Elem = BaseElemsCreator.CreateImageFromFile(path, SearchTapped)
            };

            LineElemsCreator.AddLineElems(
                gridButtons,
                0,
                elemArr
            );
        }

        private async void ProfileTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Profile)
            {
                await Navigation.PushAsync(await Navigator.GetProfileByRole(UserData.UserInfo.Role, UserData.UserInfo.Id));
            }
        }

        private void ChatsTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Chats)
            {
               // Navigation.PushAsync(new BaseUserUIPage([BaseElemsCreator.CreateVerticalStackLayout()], PageType.Chats));
            }
        }

        private void NewsTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.News)
            {
                var viewCreator = new NewsViewListCreator(2);
                var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
                var viewList = new List<ScrollView>();
                var scrollView = viewCreator.Create(mainStack, viewList, UserData.UserInfo.EducationId, true);
                viewList.Add(scrollView);
                Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.News));
            }
        }

        private void SheduleTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Shedule)
            {
                //Navigation.PushAsync(new BaseUserUIPage([BaseElemsCreator.CreateVerticalStackLayout()], PageType.Shedule));
            }
        }

        private void DiaryTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Diary)
            {
                //Navigation.PushAsync(new BaseUserUIPage([BaseElemsCreator.CreateVerticalStackLayout()], PageType.Diary));
            }
        }

        private void SearchTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Search)
            {
                //Navigation.PushAsync(new BaseUserUIPage([BaseElemsCreator.CreateVerticalStackLayout()], PageType.Search));
            }
        }
    }
}