using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using System.Text.Json;

using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Views;

using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.SaveData.Themes;
using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Components.Navigation;
using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Lists.DiaryView;
using ElectronicDiary.UI.Views.Lists.EducationalInstitutionView;
using ElectronicDiary.UI.Views.Lists.General;
using ElectronicDiary.UI.Views.Lists.NewView;
using ElectronicDiary.UI.Views.Lists.SheduleView;
using ElectronicDiary.UI.Views.Tables.BaseTable;
using ElectronicDiary.UI.Views.Tables.JournalTable;
using ElectronicDiary.UI.Views.Tables.QuarterTable;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class BaseUserUIPage : ContentPage
    {
        private readonly PageType _pageName;
        private readonly HorizontalStackLayout _mainStack = [];
        private readonly List<ScrollView> _viewList = [];
        private readonly bool _fullScreen = false;
        public BaseUserUIPage(HorizontalStackLayout mainStack, List<ScrollView> viewList, PageType pageName, bool fullScreen = false)
        {
            _mainStack = mainStack;
            _viewList = viewList;
            _pageName = pageName;
            _fullScreen = fullScreen;

            Title = UserInfo.ConvertEnumRoleToStringRus(UserData.UserInfo.Role);
            ToolbarItemsAdder.AddNotifications(ToolbarItems);
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
            CreateNavigationButtons(gridButtons, 6, pageName);
        }

        private void WindowSizeChanged(object? sender, EventArgs e)
        {
            AdminPageStatic.RepaintPage(_mainStack, _viewList, _fullScreen);
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
            Profile, News, Shedule, Diary, Gradebook, Quarter, AdminPanel
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
                    selectedIndex = elemList.Count;
                    path = colorButtonSelected;
                }

                EventHandler<TappedEventArgs> handler = ProfileTapped;
                switch (type)
                {
                    case PageType.Profile:
                        handler = ProfileTapped;
                        path += "profile_icon.png";
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

                    case PageType.Gradebook:
                        handler = JournalTapped;
                        if (UserData.UserInfo.Role != UserInfo.RoleType.LocalAdmin)
                        {
                            path += "journal_icon.png";
                        }
                        else
                        {
                            path = string.Empty;
                        }
                        break;

                    case PageType.Quarter:
                        handler = QuarterTapped;
                        if (UserData.UserInfo.Role != UserInfo.RoleType.LocalAdmin)
                        {
                            path += "quarter_icon.png";
                        }
                        else
                        {
                            path = string.Empty;
                        }
                        break;


                    case PageType.AdminPanel:
                        handler = AdminPanelTapped;
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

        private long _id = UserData.UserInfo.Id;
        private async void GetId(EventHandler handler)
        {
            if (UserData.UserInfo.Role == UserInfo.RoleType.Parent)
            {
                var controller = new ParentWithStudentsController();
                var response = await controller.GetAll(UserData.UserInfo.Id);
                if (!string.IsNullOrEmpty(response))
                {
                    var arr = JsonSerializer.Deserialize<StudentParentResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];
                    var studentList = new List<TypeResponse>();
                    foreach (var studentParent in arr)
                    {
                        studentList.Add(new TypeResponse(studentParent.SchoolStudent.Id, $"{studentParent.SchoolStudent?.LastName} {studentParent.SchoolStudent?.FirstName} {studentParent.SchoolStudent?.Patronymic}"));
                    }

                    long id = -1;
                    var popup = new SearchPopup(studentList, newText => id = newText);
                    var page = Application.Current?.Windows[0].Page;
                    page?.ShowPopup(popup);

                    popup.Closed += (sender, e) =>
                    {
                        if (popup.AllItems.Count > 0 && id > -1)
                        {
                            _id = id;
                            handler?.Invoke(this, EventArgs.Empty);
                        }
                        Focus();
                    };
                }
            }
        }

        private async void SheduleTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Shedule)
            {
                GetId(MoveShedule);
            }
        }

        private void MoveShedule(object? sender, EventArgs e)
        {
            var viewCreator = new SheduleViewListCreator<SheduleViewElemCreator<SheduleViewObjectCreator>, SheduleViewObjectCreator>();
            var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
            var viewList = new List<ScrollView>();
            var scrollView = viewCreator.Create(mainStack, viewList, _id, true);
            viewList.Add(scrollView);
            Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.Shedule));
        }


        private async void DiaryTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Diary)
            {
                GetId(MoveDiary);

            }
        }
        private void MoveDiary(object? sender, EventArgs e)
        {
            var viewCreator = new DiaryViewListCreator();
            var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
            var viewList = new List<ScrollView>();
            var scrollView = viewCreator.Create(mainStack, viewList, _id, true);
            viewList.Add(scrollView);
            Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.Diary));
        }

        private async void JournalTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Gradebook)
            {
                GetId(MoveJournal);
            }
        }
        private void MoveJournal(object? sender, EventArgs e)
        {
            var viewCreator = new GradebookViewTableCreator();
            var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
            var viewList = new List<ScrollView>();
            var scrollView = viewCreator.Create(_id, -1);
            viewList.Add(scrollView);
            Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.Gradebook, true));
        }

        private async void QuarterTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.Quarter)
            {
                GetId(MoveQuarter);
            }
        }
        private void MoveQuarter(object? sender, EventArgs e)
        {
            var viewCreator = new QuarterScoreStudentViewTableCreator();
            var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
            var viewList = new List<ScrollView>();
            var scrollView = viewCreator.Create(_id, -1);
            viewList.Add(scrollView);
            Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.Quarter, true));
        }

        private void AdminPanelTapped(object? sender, EventArgs e)
        {
            if (_pageName != PageType.AdminPanel)
            {
                var viewCreator = new EducationalInstitutionViewListCreator();
                var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
                var viewList = new List<ScrollView>();
                var scrollView = viewCreator.Create(mainStack, viewList, UserData.UserInfo.EducationId);
                viewList.Add(scrollView);
                Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.AdminPanel));
            }
        }
    }
}