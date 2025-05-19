using ElectronicDiary.Pages.AdminPageComponents.ChatView;
using ElectronicDiary.Pages.AdminPageComponents.EducationalInstitutionView;
using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.AdminPageComponents.NewsView;
using ElectronicDiary.Pages.AdminPageComponents.SheduleView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Navigation;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.SaveData.Themes;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class BaseUserUIPage : ContentPage
    {
        private readonly PageType _pageName;
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

            var gridButtons = BaseElemsCreator.CreateGrid(0, false);
            grid.Add(gridButtons, 0, 1);
            CreateNavigationButtons(gridButtons, 5, pageName);
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
            var elemList = new List<LineElemsCreator.Data>();
            var colorButton = UserData.Settings.Theme.TextIsBlack ? "black_" : "white_";
            var colorButtonSelected = !UserData.Settings.Theme.TextIsBlack ? "black_" : "white_";

            var selectedIndex = 0;
            foreach (var type in Enum.GetValues<PageType>())
            {
                var path = colorButton;
                if (type == pageName)
                {
                    selectedIndex = elemList.Count
                        ;
                    path = colorButtonSelected;
                }

                EventHandler<TappedEventArgs> handler = ProfileTapped;
                switch (type)
                {
                    case PageType.Profile:
                        handler = ProfileTapped;
                        path += "profile_icon.png";
                        break;

                    case PageType.Chats:
                        handler = ChatsTapped;
                        path += "chats_icon.png";
                        break;

                    case PageType.News:
                        handler = NewsTapped;
                        path += "news_icon.png";
                        break;

                    case PageType.Shedule:
                        if (UserData.UserInfo.Role != UserInfo.RoleType.LocalAdmin)
                        {
                            handler = SheduleTapped;
                            path += "shedule_icon.png";
                        }
                        else
                        {
                            path = string.Empty;
                        }
                        break;

                    case PageType.Diary:
                        handler = DiaryTapped;
                        if (UserData.UserInfo.Role != UserInfo.RoleType.LocalAdmin)
                        {
                            path += "diary_icon.png";
                        }
                        else
                        {
                            path = string.Empty;
                        }
                        break;

                    case PageType.Search:
                        handler = SearchTapped;
                        if (UserData.UserInfo.Role == UserInfo.RoleType.LocalAdmin)
                        {
                            path += "search_icon.png";
                        }
                        else
                        {
                            path = string.Empty;
                        }
                        break;

                }

                if (path?.Length != 0)
                {
                    elemList.Add(new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateImageFromFile(path, handler)
                    });
                }
            }

            BaseElemsCreator.GridAddColumn(gridButtons, elemList.Count);

            IAppTheme theme = UserData.Settings.Theme.TextIsBlack ? new DarkTheme() : new LightTheme();
            gridButtons.Add(new BoxView { Color = theme.BackgroundFillColor }, selectedIndex, 0);

            LineElemsCreator.AddLineElems(
                gridButtons,
                0,
                [.. elemList]
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
                var viewCreator = new ChatViewListCreator();
                var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
                var viewList = new List<ScrollView>();
                var scrollView = viewCreator.Create(mainStack, viewList, UserData.UserInfo.UserId);
                viewList.Add(scrollView);
                Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.Chats));
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
                var viewCreator = new SheduleViewListCreator();
                var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
                var viewList = new List<ScrollView>();
                var scrollView = viewCreator.Create(mainStack, viewList, UserData.UserInfo.Id, true);
                viewList.Add(scrollView);
                Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.Shedule));
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
                var viewCreator = new EducationalInstitutionViewListCreator();
                var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
                var viewList = new List<ScrollView>();
                var scrollView = viewCreator.Create(mainStack, viewList, UserData.UserInfo.EducationId);
                viewList.Add(scrollView);
                Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.Search));
            }
        }
    }
}