using System.Text.Json;

using CommunityToolkit.Maui.Extensions;

using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.SaveData.Themes;
using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Components.Navigation;
using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Lists.DiaryView;
using ElectronicDiary.UI.Views.Lists.General;
using ElectronicDiary.UI.Views.Lists.NewView;
using ElectronicDiary.UI.Views.Lists.SheduleView;
using ElectronicDiary.UI.Views.Tables.GradebookTable;
using ElectronicDiary.UI.Views.Tables.JournalTable;
using ElectronicDiary.UI.Views.Tables.QuarterTable;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class BaseUserUIPage : ContentPage
    {
        private readonly PageType _pageType;
        private readonly HorizontalStackLayout _mainStack = [];
        private readonly List<ScrollView> _viewList = [];
        private readonly bool _fullScreen = false;
        public BaseUserUIPage(HorizontalStackLayout mainStack, List<ScrollView> viewList, PageType pageType, bool fullScreen = false)
        {
            _mainStack = mainStack;
            _viewList = viewList;
            _pageType = pageType;
            _fullScreen = fullScreen;

            switch (_pageType)
            {
                case PageType.Profile:
                    Title = UserInfo.ConvertEnumRoleToStringRus(UserData.UserInfo.Role);
                    break;

                case PageType.News:
                    Title = "Новости";
                    break;

                case PageType.Shedule:
                    Title = "Расписание";
                    break;

                case PageType.Diary:
                    Title = "Дневник";
                    break;

                case PageType.Gradebook:
                    Title = "Журнал";
                    break;

                case PageType.Quarter:
                    Title = "Четвертные";
                    break;

                default:
                    Title = "Неизвестная";
                    break;
            }

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
            CreateNavigationButtons(gridButtons, 6, pageType);
        }

        private void WindowSizeChanged(object? sender, EventArgs e)
        {
            AdminPageStatic.RepaintPage(_mainStack, _viewList, _fullScreen);
        }

        protected override bool OnBackButtonPressed()
        {
            AdminPageStatic.OnBackButtonPressed(_mainStack, _viewList);
            if (_mainStack.Count == 0)
            {
                return base.OnBackButtonPressed();
            }
            return true;
        }

        public enum PageType
        {
            Profile, News, Shedule, Diary, Gradebook, Quarter
        }

        private PageType[] GetButtonsByRole()
        {
            var list = new List<PageType>();
            switch (UserData.UserInfo.Role)
            {
                case UserInfo.RoleType.Teacher:
                    list.Add(PageType.Profile);
                    list.Add(PageType.News);
                    list.Add(PageType.Shedule);
                    list.Add(PageType.Gradebook);
                    list.Add(PageType.Quarter);
                    break;

                case UserInfo.RoleType.SchoolStudent:
                    list.Add(PageType.Profile);
                    list.Add(PageType.News);
                    list.Add(PageType.Shedule);
                    list.Add(PageType.Diary);
                    list.Add(PageType.Gradebook);
                    list.Add(PageType.Quarter);
                    break;

                case UserInfo.RoleType.Parent:
                    list.Add(PageType.Profile);
                    list.Add(PageType.News);
                    list.Add(PageType.Shedule);
                    list.Add(PageType.Diary);
                    list.Add(PageType.Gradebook);
                    list.Add(PageType.Quarter);
                    break;

                case UserInfo.RoleType.Administration:
                    list.Add(PageType.Profile);
                    list.Add(PageType.News);
                    break;

                default:
                    break;
            }

            return [.. list];
        }

        private void CreateNavigationButtons(Grid gridButtons, int countColumns, PageType pageType)
        {
            var elemList = new List<LineElemsCreator.Data>();
            var colorButton = UserData.Settings.Theme.TextIsBlack ? "black_" : "white_";
            var colorButtonSelected = !UserData.Settings.Theme.TextIsBlack ? "black_" : "white_";

            var selectedIndex = 0;
            foreach (var type in GetButtonsByRole())
            {
                var path = colorButton;
                if (type == pageType)
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
                        handler = SheduleTapped;
                        path += "shedule_icon.png";
                        break;

                    case PageType.Diary:
                        handler = DiaryTapped;
                        path += "diary_icon.png";
                        break;

                    case PageType.Gradebook:
                        handler = GradebookTapped;
                        path += "gradebook_icon.png";
                        break;

                    case PageType.Quarter:
                        handler = QuarterTapped;
                        path += "quarter_icon.png";
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

        private const int LoadTime = 300;
        private async void ProfileTapped(object? sender, EventArgs e)
        {
            if (_pageType != PageType.Profile)
            {
                await Navigation.PushAsync(await Navigator.GetProfileByRole(UserData.UserInfo.Role, UserData.UserInfo.Id));
            }
        }

        private void NewsTapped(object? sender, EventArgs e)
        {
            if (_pageType != PageType.News)
            {
                var viewCreator = new NewsViewListCreator(2);
                var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
                var viewList = new List<ScrollView>();
                var scrollView = viewCreator.Create(mainStack, viewList, UserData.UserInfo.EducationId, true);
                viewList.Add(scrollView);
                Thread.Sleep(LoadTime);
                Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.News));
            }
        }

        private long _id = UserData.UserInfo.Id;
        private int _quarterId = -1;
        protected bool _fixFlag = true;
        private async void InvokeMove(EventHandler handler, bool idNoChange = false)
        {
            string? response = string.Empty;
            IController controller = null;
            switch (UserData.UserInfo.Role)
            {
                case UserInfo.RoleType.Parent:
                    controller = new ParentWithStudentsController();
                    response = await controller.GetAll(UserData.UserInfo.Id);
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
                    break;
                case UserInfo.RoleType.Teacher:
                    if (!idNoChange)
                    {
                        BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;
                        string action = "1";
                        if (_fixFlag)
                        {
                            action = await BaseElemsCreator.CreateActionSheet(["1", "2", "3", "4"]);

                        }
                        if (int.TryParse(action, out _quarterId))
                        {
                            response = await SheduleLessonController.GetByTeacher(UserData.UserInfo.Id, _quarterId);
                            if (!string.IsNullOrEmpty(response))
                            {
                                var lessonsDict = JsonSerializer.Deserialize<Dictionary<int, SheduleLessonResponse[]>>(response, PageConstants.JsonSerializerOptions) ?? [];

                                var teacherAssignmentList = new List<TypeResponse>();
                                foreach (var sheduleLesson in lessonsDict.First().Value)
                                {
                                    if (teacherAssignmentList.Where(ta => ta.Id == sheduleLesson.TeacherAssignment.Id).Count() == 0 && sheduleLesson.TeacherAssignment.Teacher.Id == UserData.UserInfo.Id)
                                    {
                                        teacherAssignmentList.Add(new(sheduleLesson.TeacherAssignment.Id, $"{sheduleLesson.Group.ClassRoom.Name} - {sheduleLesson.Group.GroupName} - {sheduleLesson.TeacherAssignment.SchoolSubject.Name}"));
                                    }
                                }

                                long id = -1;
                                var popup = new SearchPopup(teacherAssignmentList, newText => id = newText);
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
                    else
                    {
                        _id = UserData.UserInfo.Id;
                        handler?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                default:
                    _id = UserData.UserInfo.Id;
                    handler?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private async void SheduleTapped(object? sender, EventArgs e)
        {
            if (_pageType != PageType.Shedule)
            {
                InvokeMove(MoveShedule, true);
            }
        }

        private void MoveShedule(object? sender, EventArgs e)
        {
            var viewCreator = new SheduleViewListCreator<SheduleViewElemCreator<SheduleViewObjectCreator>, SheduleViewObjectCreator>();
            var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
            var viewList = new List<ScrollView>();
            var scrollView = viewCreator.Create(mainStack, viewList, _id, true);
            viewList.Add(scrollView);
            Thread.Sleep(LoadTime);
            Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.Shedule));
        }

        private async void DiaryTapped(object? sender, EventArgs e)
        {
            if (_pageType != PageType.Diary)
            {
                InvokeMove(MoveDiary);

            }
        }
        private void MoveDiary(object? sender, EventArgs e)
        {
            var viewCreator = new DiaryViewListCreator();
            var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
            var viewList = new List<ScrollView>();
            var scrollView = viewCreator.Create(mainStack, viewList, _id, true);
            viewList.Add(scrollView);
            Thread.Sleep(LoadTime * 2);
            Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.Diary));
        }

        private async void GradebookTapped(object? sender, EventArgs e)
        {
            if (_pageType != PageType.Gradebook || UserData.UserInfo.Role == UserInfo.RoleType.Teacher)
            {
                InvokeMove(MoveGradebook);
            }
        }
        private void MoveGradebook(object? sender, EventArgs e)
        {
            ScrollView scrollView;
            var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
            var viewList = new List<ScrollView>();
            if (UserData.UserInfo.Role == UserInfo.RoleType.Teacher)
            {
                var viewCreator = new GradebookTeacherViewTableCreator();
                scrollView = viewCreator.Create(_id, _quarterId, false);
                SizeChanged += (sender, e) => viewCreator.SetSize(this, EventArgs.Empty);
            }
            else
            {
                var viewCreator = new GradebookStudentViewTableCreator();
                scrollView = viewCreator.Create(_id, -1);
            }

            viewList.Add(scrollView);
            Thread.Sleep(LoadTime * 2);
            Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.Gradebook, true));
        }

        private async void QuarterTapped(object? sender, EventArgs e)
        {
            if (_pageType != PageType.Quarter || UserData.UserInfo.Role == UserInfo.RoleType.Teacher)
            {
                _fixFlag = true;
                InvokeMove(MoveQuarter);
                _fixFlag = false;
            }
        }
        private void MoveQuarter(object? sender, EventArgs e)
        {
            ScrollView scrollView;
            var mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
            var viewList = new List<ScrollView>();
            if (UserData.UserInfo.Role == UserInfo.RoleType.Teacher)
            {
                var viewCreator = new QuarterScoreTeacherViewTableCreator();
                scrollView = viewCreator.Create(_id, -1, false);
            }
            else
            {
                var viewCreator = new QuarterScoreStudentViewTableCreator();
                scrollView = viewCreator.Create(_id, -1);
            }
            viewList.Add(scrollView);
            Thread.Sleep(LoadTime);
            Navigation.PushAsync(new BaseUserUIPage(mainStack, viewList, PageType.Quarter, true));
        }
    }
}