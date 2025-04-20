// Объединенный код C#
// Дата создания: 04/21/2025 02:34:19
// Исключено файлов: 27


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Main\App.cs
////////////////////////////////////////////////////////////////////////////////

using System.Text.Json;

using ElectronicDiary.Pages;
using ElectronicDiary.Pages.Components.Navigation;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Pages.OtherPages;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Main
{
    public partial class App : Application
    {
        protected override Window CreateWindow(IActivationState? activationState)
        {
            UserData.LoadAll();

            Task.Run(async () =>
            {
                await AuthorizationСontroller.LogIn(UserData.UserInfo.Login ?? string.Empty, UserData.UserInfo.Password ?? string.Empty);
                var response = await AuthorizationСontroller.GetUserInfo();
                if (!string.IsNullOrEmpty(response))
                {
                    var obj = JsonSerializer.Deserialize<AuthorizationUserResponse>(response, PageConstants.JsonSerializerOptions);
                    if (obj != null)
                    {
                        if (obj.Id == UserData.UserInfo.Id && UserInfo.ConverStringRoleToEnum(obj.Role) == UserData.UserInfo.Role)
                        {
                            await Task.Delay(3000);
                            Navigator.ChooseRootPageByRole(UserData.UserInfo.Role, UserData.UserInfo.Id);
                            return;
                        }
                    }
                }
                await Task.Delay(3000);
                Navigator.SetAsRoot(new LogPage());
            });

            return new Window(new EmptyPage())
            {
                Title = "Электронный дневник"
            };
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Main\MauiProgram.cs
////////////////////////////////////////////////////////////////////////////////

using CommunityToolkit.Maui;

using Microsoft.Extensions.Logging;

namespace ElectronicDiary.Main;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\BaseView\BaseViewElemCreator.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.AdminPageComponents.ParentView;
using ElectronicDiary.Pages.AdminPageComponents.SchoolStudentView;
using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.OtherViews.NewsView;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Other;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.BaseView
{
    public class BaseViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator>
        where TResponse : BaseResponse, new()
        where TRequest : BaseRequest, new()
        where TController : IController, new()
        where TViewObjectCreator : BaseViewObjectCreator<TResponse, TRequest, TController>, new()
    {
        protected TResponse _baseResponse = new();
        protected TRequest _baseRequest = new();
        protected TController _controller = new();

        protected HorizontalStackLayout _mainStack = [];
        protected List<ScrollView> _viewList = [];
        protected event Action ChageListAction = delegate { };

        protected int _maxCountViews;
        protected long _educationalInstitutionId;
        protected bool _readOnly = false;


        protected Grid _grid = [];
        public Grid Create(HorizontalStackLayout mainStack,
                           List<ScrollView> viewList,
                           Action chageListAction,
                           BaseResponse? baseResponse,
                           int maxCountViews,
                           long educationalInstitutionId,
                           bool readOnly = false)
        {
            _mainStack = mainStack;
            _viewList = viewList;
            ChageListAction = chageListAction;
            _baseResponse = baseResponse as TResponse ?? new();
            _educationalInstitutionId = educationalInstitutionId;
            _maxCountViews = maxCountViews;
            _readOnly = readOnly;

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += GestureTapped;
            _grid = BaseElemsCreator.CreateGrid();
            _grid.GestureRecognizers.Add(tapGesture);

            var rowIndex = 0;
            CreateUI(ref rowIndex);

            return _grid;
        }

        // Пусто
        protected virtual void CreateUI(ref int rowIndex)
        {

        }


        protected virtual async void GestureTapped(object? sender, EventArgs e)
        {
            if (!_readOnly)
            {
                if (_baseResponse.Id > -1)
                {
                    var action = string.Empty;
                    if (_maxCountViews == 2)
                    {
                        action = await BaseElemsCreator.CreateActionSheet(
                           [
                            "Описание",
                            "Перейти",
                            "Редактировать",
                            "Удалить"]);

                    }
                    else
                    {
                        action = await BaseElemsCreator.CreateActionSheet(
                           [
                            "Описание",
                            "Редактировать",
                            "Удалить"]);
                    }

                    switch (action)
                    {
                        case "Описание":
                            ShowInfo(_baseResponse.Id);
                            break;
                        case "Перейти":
                            await MoveTo(_baseResponse.Id);
                            break;
                        case "Редактировать":
                            Edit(_baseResponse.Id);
                            break;
                        case "Удалить":
                            Delete(_baseResponse.Id);
                            break;
                        default:
                            return;
                    }
                }
            }
            else
            {
                ShowInfo(_baseResponse.Id);
            }
        }

        protected virtual void ShowInfo(long id)
        {
            var baseViewObjectCreator = new TViewObjectCreator();
            var scrollView = baseViewObjectCreator.Create(_mainStack, _viewList, ChageListAction, _baseResponse, _educationalInstitutionId);
            AdminPageStatic.DeleteLastView(_mainStack, _viewList, _maxCountViews);
            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }

        protected virtual async Task MoveTo(long id)
        {
            string action = string.Empty;
            action = await BaseElemsCreator.CreateActionSheet(
               [
                "Администраторы",
                "Учителя",
                "Классы",
                "Ученики",
                "Родители",
                "Новости"]);

            IBaseViewListCreator? viewCreator = null;
            switch (action)
            {
                case "Администраторы":
                    viewCreator = new UserViewListCreator<UserResponse, UserRequest, AdministratorController,
                        UserViewElemCreator<UserResponse, UserRequest, AdministratorController,
                        UserViewObjectCreator<UserResponse, UserRequest, AdministratorController>>,
                        UserViewObjectCreator<UserResponse, UserRequest, AdministratorController>>();
                    break;
                case "Учителя":
                    viewCreator = new UserViewListCreator<UserResponse, UserRequest, TeacherController,
                        UserViewElemCreator<UserResponse, UserRequest, TeacherController,
                        UserViewObjectCreator<UserResponse, UserRequest, TeacherController>>,
                        UserViewObjectCreator<UserResponse, UserRequest, TeacherController>>();
                    break;
                case "Классы":
                    //view = new AdministratorView(_mainStack, _viewList, id);
                    break;
                case "Ученики":
                    viewCreator = new UserViewListCreator<SchoolStudentResponse, SchoolStudentRequest, SchoolStudentController,
                        UserViewElemCreator<SchoolStudentResponse, SchoolStudentRequest, SchoolStudentController,
                        SchoolStudentViewObjectCreator>,
                        SchoolStudentViewObjectCreator>();
                    break;
                case "Родители":
                    viewCreator = new UserViewListCreator<UserResponse, ParentRequest, ParentController,
                        UserViewElemCreator<UserResponse, ParentRequest, ParentController,
                        ParentViewObjectCreator>,
                        ParentViewObjectCreator>();
                    break;
                case "Новости":
                    viewCreator = new NewsViewListCreator();
                    break;
                default:
                    return;
            }

            if (viewCreator != null)
            {
                AdminPageStatic.DeleteLastView(_mainStack, _viewList, _maxCountViews);
                var scrollView = new ScrollView()
                {
                    Content = viewCreator.Create(_mainStack, _viewList, _baseResponse.Id)
                };
                _viewList.Add(scrollView);
                AdminPageStatic.RepaintPage(_mainStack, _viewList);
            }
        }

        protected virtual void Edit(long id)
        {
            var baseViewObjectCreator = new TViewObjectCreator();
            var scrollView = baseViewObjectCreator.Create(_mainStack, _viewList, ChageListAction, _baseResponse, _educationalInstitutionId, true);
            AdminPageStatic.DeleteLastView(_mainStack, _viewList, _maxCountViews);
            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);

            ChageListAction.Invoke();
        }

        protected virtual async void Delete(long id)
        {
            if (_controller != null)
            {
                await _controller.Delete(id);
                ChageListAction.Invoke();
            }
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\BaseView\BaseViewListCreator.cs
////////////////////////////////////////////////////////////////////////////////

using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Requests.Other;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Pages.AdminPageComponents.BaseView
{
    public interface IBaseViewListCreator
    {
        ScrollView Create(HorizontalStackLayout mainStack,
                                   List<ScrollView> viewList,
                                   long educationalInstitutionId = -1,
                                   bool edit = false);
    }

    public class BaseViewListCreator<TResponse, TRequest, TController, TViewElemCreator, TViewObjectCreator> : IBaseViewListCreator
        where TResponse : BaseResponse, new()
        where TRequest : BaseRequest, new()
        where TController : IController, new()
        where TViewElemCreator : BaseViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator>, new()
        where TViewObjectCreator : BaseViewObjectCreator<TResponse, TRequest, TController>, new()
    {
        protected TController _controller = new();

        protected HorizontalStackLayout _mainStack = [];
        protected List<ScrollView> _viewList = [];
        protected event Action ChageListAction;

        protected int _maxCountViews;
        protected VerticalStackLayout _listVerticalStack = new()
        {
            Spacing = UserData.Settings.Sizes.SPACING_ALL_PAGES
        };
        protected long _educationalInstitutionId;

        public BaseViewListCreator()
        {
            _maxCountViews = 0;
            ChageListAction += ChageListHandler;
        }

        protected Grid _grid = [];
        protected bool _readOnly = false;
        public ScrollView Create(HorizontalStackLayout mainStack,
                                          List<ScrollView> viewList,
                                          long educationalInstitutionId = -1,
                                          bool readOnly = false)
        {
            _mainStack = mainStack;
            _viewList = viewList;
            _educationalInstitutionId = educationalInstitutionId;
            _readOnly = readOnly;

            _ = CreateListUI();

            var verticalStack = new VerticalStackLayout
            {
                // Положение
                Padding = UserData.Settings.Sizes.PADDING_ALL_PAGES,
                Spacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,
            };

            var scrollView = new ScrollView()
            {
                Content = verticalStack
            };

            _grid = BaseElemsCreator.CreateGrid();

            var rowIndex = 0;
            CreateFilterUI(ref rowIndex);
            verticalStack.Add(_grid);

            var getButton = BaseElemsCreator.CreateButton("Найти", GetButtonClicked);
            verticalStack.Add(getButton);

            if (!readOnly)
            {
                var addButton = BaseElemsCreator.CreateButton("Добавить", AddButtonClicked);
                verticalStack.Add(addButton);
            }
            verticalStack.Add(_listVerticalStack);

            return new ScrollView() { Content = verticalStack };
        }

        protected string _titleView = string.Empty;
        protected virtual void CreateFilterUI(ref int rowIndex)
        {
            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel(_titleView),
                    },
                ]
            );
        }

        protected virtual async void GetButtonClicked(object? sender, EventArgs e)
        {
            await CreateListUI();
        }

        protected virtual void AddButtonClicked(object? sender, EventArgs e)
        {
            var viewObjectCreator = new TViewObjectCreator();
            var scrollView = viewObjectCreator.Create(_mainStack, _viewList, ChageListAction, null, _educationalInstitutionId);
            AdminPageStatic.DeleteLastView(_mainStack, _viewList, _maxCountViews);
            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }

        // Получение списка объектов
        protected TResponse[] _objectsArr = [];
        protected virtual async Task CreateListUI()
        {
            await GetList();

            Application.Current?.Dispatcher.Dispatch(() =>
            {
                _listVerticalStack.Clear();

                for (var i = 0; i < _objectsArr.Length; i++)
                {
                    var baseViewElemCreator = new TViewElemCreator();
                    var grid = baseViewElemCreator.Create(_mainStack, _viewList, ChageListAction, _objectsArr[i], _maxCountViews, _educationalInstitutionId, _readOnly);
                    _listVerticalStack.Add(grid);
                }
            });
        }

        protected virtual void ChageListHandler()
        {
            _ = CreateListUI();
        }

        protected virtual async Task GetList()
        {
            var response = await _controller.GetAll(_educationalInstitutionId);
            if (!string.IsNullOrEmpty(response)) _objectsArr = JsonSerializer.Deserialize<TResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];
            FilterList();
        }

        // Пусто
        protected virtual void FilterList()
        {

        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\BaseView\BaseViewObjectCreator.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Requests.Other;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Pages.AdminPageComponents.BaseView
{
    public class BaseViewObjectCreator<TResponse, TRequest, TController>
        where TResponse : BaseResponse, new()
        where TRequest : BaseRequest, new()
        where TController : IController, new()
    {
        protected TResponse _baseResponse = new();
        protected TRequest _baseRequest = new();
        protected TController _controller = new();

        protected HorizontalStackLayout _mainStack = [];
        protected List<ScrollView> _viewList = [];
        protected event Action ChageListAction = delegate { };

        protected long _educationalInstitutionId;


        protected AdminPageStatic.ComponentState _componentState;

        // Вид объекта
        protected VerticalStackLayout _infoStack = [];
        protected Grid _baseInfoGrid = [];
        protected int _baseInfoGridRowIndex = 0;

        public ScrollView Create(HorizontalStackLayout? mainStack,
                                    List<ScrollView>? viewList,
                                    Action? chageListAction,
                                    BaseResponse? baseResponse,
                                    long educationalInstitutionId,
                                    bool edit = false)
        {

            _mainStack = mainStack ?? [];
            _viewList = viewList ?? [];
            ChageListAction = chageListAction ?? delegate { };
            _baseResponse = (baseResponse as TResponse) ?? new();
            _educationalInstitutionId = educationalInstitutionId;

            if (edit)
            {
                _componentState = AdminPageStatic.ComponentState.Edit;
            }
            else
            {
                if (_baseResponse.Id > -1)
                {
                    _componentState = AdminPageStatic.ComponentState.Read;
                }
                else
                {
                    _componentState = AdminPageStatic.ComponentState.New;
                }
            }

            var verticalStack = new VerticalStackLayout
            {
                // Положение
                Padding = UserData.Settings.Sizes.PADDING_ALL_PAGES,
                Spacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,
            };

            _infoStack = new VerticalStackLayout
            {
                // Положение
                Spacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,
            };
            verticalStack.Add(_infoStack);

            _baseInfoGrid = BaseElemsCreator.CreateGrid();
            _infoStack.Add(_baseInfoGrid);

            CreateUI();

            if (_componentState != AdminPageStatic.ComponentState.Read)
            {
                var saveButton = BaseElemsCreator.CreateButton(text: "Сохранить", SaveButtonClicked);
                verticalStack.Add(saveButton);
            }

            return new ScrollView() { Content = verticalStack };
        }

        // Пусто
        protected virtual void CreateUI()
        {
            _baseInfoGridRowIndex = 0;

            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                _baseRequest = new();
                _baseRequest.Id = _baseResponse.Id;
            }
        }

        protected virtual async void SaveButtonClicked(object? sender, EventArgs e)
        {
            var response = _componentState == AdminPageStatic.ComponentState.New ? await _controller.Add(_baseRequest) : await _controller.Edit(_baseRequest);
            if (response != null)
            {
                ChageListAction.Invoke();
                AdminPageStatic.OnBackButtonPressed(_mainStack, _viewList);
            }
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\EducationalInstitutionView\EducationalInstitutionViewElemCreator.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Pages.AdminPageComponents.EducationalInstitutionView
{
    public class EducationalInstitutionViewElemCreator : BaseViewElemCreator<EducationalInstitutionResponse, EducationalInstitutionRequest, EducationalInstitutionСontroller, EducationalInstitutionViewObjectCreator>
    {

        protected override void CreateUI(ref int rowIndex)
        {
            base.CreateUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Название")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Name)
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Регион")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Settlement?.Region?.Name)
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Населённый пункт")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Settlement?.Name)
                    },
                ]
            );
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\EducationalInstitutionView\EducationalInstitutionViewListCreator.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Pages.AdminPageComponents.EducationalInstitutionView
{
    public sealed class EducationalInstitutionViewListCreator : BaseViewListCreator<EducationalInstitutionResponse, EducationalInstitutionRequest, EducationalInstitutionСontroller, EducationalInstitutionViewElemCreator, EducationalInstitutionViewObjectCreator>
    {
        public EducationalInstitutionViewListCreator()
        {
            _maxCountViews = 2;
            _titleView = "Список учебных заведений";
        }

        private string _regionFilter = string.Empty;
        private string _settlementFilter = string.Empty;
        private string _nameFilter = string.Empty;

        protected override void CreateFilterUI(ref int rowIndex)
        {
            base.CreateFilterUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Область")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem  = BaseElemsCreator.CreateEditor(newText => _regionFilter = newText, "Минская область")
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Населённый пункт")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem  = BaseElemsCreator.CreateEditor(newText => _settlementFilter = newText, "г.Солигорск")
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Название"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem  = BaseElemsCreator.CreateEditor(newText => _nameFilter = newText, "ГУО ...")
                    },
                ]
            );
        }

        // Получение списка объектов
        protected override void FilterList()
        {
            bool regionFilter = string.IsNullOrEmpty(_regionFilter);
            bool settlementFilter = string.IsNullOrEmpty(_settlementFilter);
            bool nameFilter = string.IsNullOrEmpty(_nameFilter);

            _objectsArr = [.. _objectsArr
                .Where(e =>
                    (!regionFilter || (e.Settlement?.Region?.Name ?? string.Empty).Contains(_regionFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (!settlementFilter || (e.Settlement?.Name ?? string.Empty).Contains(_settlementFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (!nameFilter || (e.Name ?? string.Empty).Contains(_nameFilter!, StringComparison.OrdinalIgnoreCase)))];
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\EducationalInstitutionView\EducationalInstitutionViewObjectCreator.cs
////////////////////////////////////////////////////////////////////////////////

using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Pages.AdminPageComponents.EducationalInstitutionView
{
    public class EducationalInstitutionViewObjectCreator : BaseViewObjectCreator<EducationalInstitutionResponse, EducationalInstitutionRequest, EducationalInstitutionСontroller>
    {
        private VerticalStackLayout? _imageStack = null;
        protected override void CreateUI()
        {
            base.CreateUI();

            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                _baseRequest.Name = _baseResponse.Name;
                _baseRequest.Address = _baseResponse.Address;
                _baseRequest.Email = _baseResponse.Email;
                _baseRequest.PhoneNumber = _baseResponse.PhoneNumber;
                _baseRequest.RegionId = _baseResponse.Settlement?.Region?.Id ?? -1;
                _baseRequest.SettlementId = _baseResponse.Settlement?.Id ?? -1;
            }

            _imageStack = BaseElemsCreator.CreateImageFromUrl(_baseResponse.PathImage,
                _componentState == AdminPageStatic.ComponentState.Edit ? AddImageTapped : null);
            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                     new LineElemsCreator.Data
                     {
                         CountJoinColumns = 2,
                         Elem = _imageStack
                     }
                ]);

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel( "Название")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.Name)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor( newText =>  _baseRequest.Name = newText, "ГУО ...",_baseResponse.Name )
                        }
                ]
            );

            List<Item> settlementList = [];
            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Регион")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Settlement ?.Region ?.Name)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateSearchPopupAsLabel(GetRegions(),
                                selectedIndex =>
                                {
                                    _baseRequest.RegionId = selectedIndex;
                                    if(_baseRequest.RegionId > -1)
                                    {
                                        GetSettlements(settlementList, _baseRequest.RegionId);
                                    }
                                }
                            )
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Населённый пункт")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Settlement ?.Name)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateSearchPopupAsLabel(settlementList, selectedIndex => _baseRequest.SettlementId = selectedIndex)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Адресс")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Address)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.Address = newText, "ул. Ленина, 12",_baseResponse.Address )
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Email")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Email)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor( newText => _baseRequest.Email = newText, "sh4@edus.by", _baseResponse.Email)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Телефон")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.PhoneNumber)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.PhoneNumber = newText, "+375 17 433-09-02", _baseResponse.PhoneNumber)
                        }
                ]
            );
        }

        private Stream? _image_stream = null;
        private FileResult? _imageFile = null;
        private async void AddImageTapped(object? sender, EventArgs e)
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите изображение",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null) return;
            _imageFile = result;

            if (_imageStack != null)
            {
                _image_stream?.Dispose();
                _image_stream = await result.OpenReadAsync();
                Image image = (Image)_imageStack[^1];
                image.Source = ImageSource.FromStream(() => _image_stream);
            }
        }

        protected override void SaveButtonClicked(object? sender, EventArgs e)
        {
            base.SaveButtonClicked(sender, e);

            if (_imageFile != null)
            {
                _controller.AddImage(_baseResponse.Id, _imageFile);
            }
        }

        private static List<Item> GetRegions()
        {
            var list = new List<Item>();

            Task.Run(async () =>
            {
                TypeResponse[]? arr = null;
                var response = await AddressСontroller.GetRegions();
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, elem.Name));
                }
            });

            return list;
        }

        private static void GetSettlements(List<Item> list, long regionId)
        {
            list.Clear();
            Task.Run(async () =>
            {
                TypeResponse[]? arr = null;
                var response = await AddressСontroller.GetSettlements(regionId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, elem.Name));
                }
            });
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\General\AdminPage.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.AdminPageComponents.EducationalInstitutionView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Navigation;
using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.AdminPageComponents.General
{
    public partial class AdminPage : ContentPage
    {
        private readonly HorizontalStackLayout _mainStack = BaseElemsCreator.CreateHorizontalStackLayout();
        private readonly List<ScrollView> _viewList = [];

        public AdminPage()
        {
            Title = "Панель администратора";
            ToolbarItemsAdder.AddSettings(ToolbarItems);
            ToolbarItemsAdder.AddLogOut(ToolbarItems);
            BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;

            var view = new EducationalInstitutionViewListCreator();

            var scrollView = view.Create(_mainStack, _viewList);

            _viewList.Add(scrollView);

            AdminPageStatic.RepaintPage(_mainStack, _viewList);
            Content = _mainStack;
            SizeChanged += WindowSizeChanged;
        }

        private void WindowSizeChanged(object? sender, EventArgs e)
        {
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }

        // Переопределение возврата
        protected override bool OnBackButtonPressed()
        {
            return AdminPageStatic.OnBackButtonPressed(_mainStack, _viewList);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\General\AdminPageStatic.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Pages.AdminPageComponents.General
{
    public static class AdminPageStatic
    {
        // Обновление видов
        public static void RepaintPage(HorizontalStackLayout mainStack, List<ScrollView> viewList)
        {
            Application.Current?.Dispatcher.Dispatch(() =>
            {
                mainStack.Clear();

                CalcViewWidth(out var width, out var countColumn);

                for (var i = int.Max(viewList.Count - countColumn, 0); i < viewList.Count; i++)
                {
                    viewList[i].MinimumWidthRequest = width;
                    viewList[i].MaximumWidthRequest = width;
                    mainStack.Add(viewList[i]);
                }
            });
        }

        public static void CalcViewWidth(out double width, out int countColumn)
        {
            double dpi = DeviceDisplay.MainDisplayInfo.Density * 640;
            var widthWindow = 0d;
            if (Application.Current?.Windows.Count > 0)
            {
                widthWindow = Application.Current?.Windows[0].Width ?? 0d;
            }

#if WINDOWS
            const double coeffFixAndroidWidth = 1;
#else
            const double coeffFixAndroidWidth = 3.3;
#endif
            countColumn = int.Max(int.Min((int)(widthWindow * coeffFixAndroidWidth / dpi), 3), 1);

            width = 0.9 * dpi / (coeffFixAndroidWidth);
        }

        public static bool OnBackButtonPressed(HorizontalStackLayout mainStack, List<ScrollView> viewList)
        {
            if (viewList.Count > 1)
            {
                viewList.RemoveAt(viewList.Count - 1);
                RepaintPage(mainStack, viewList);
            }

            return true;
        }

        public static void DeleteLastView(HorizontalStackLayout mainStack, List<ScrollView> viewList, int maxCountViews, int indexDel = 1)
        {
            while (viewList.Count >= maxCountViews)
            {
                viewList.RemoveAt(viewList.Count - indexDel);
            }
        }
        public enum ComponentState
        {
            Read, New, Edit
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\General\ParentSchoolStudentCreator.cs
////////////////////////////////////////////////////////////////////////////////

using System.Collections.ObjectModel;
using System.Text.Json;

using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.General
{
    public class ParentSchoolStudentCreator
    {
        private readonly long _userId;
        private readonly bool _isParent;
        public Grid Grid { get; set; }

        public ParentSchoolStudentCreator(long? userId, bool isParent)
        {
            _userId = userId ?? -1;
            _isParent = isParent;

            Grid = BaseElemsCreator.CreateGrid();
            ShowBase();
        }

        private int _startChangedRow = 0;

        private async Task GetInfo()
        {

            TypeResponse[] typesList = [];
            var response = await ParentController.GetParentType();
            if (!string.IsNullOrEmpty(response)) typesList = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

            if (_isParent)
            {
                response = await ParentController.GetParentStudents(_userId);
            }
            else
            {
                response = await ParentController.GetStudentParents(_userId);
            }
            if (!string.IsNullOrEmpty(response)) _studentParentResponse = JsonSerializer.Deserialize<StudentParentResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

        }

        private void ShowBase()
        {
            var rowIndex = 0;
            LineElemsCreator.AddLineElems(
                Grid,
                rowIndex++,
                [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel( _isParent ? "Дети:" : "Родители:")
                    }
                ]
            );

            LineElemsCreator.AddLineElems(
                Grid,
                rowIndex++,
                [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Тип")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("ФИО")
                    }
                ]
            );
            _startChangedRow = rowIndex;
        }

        private StudentParentResponse[] _studentParentResponse = [];
        public async Task ShowList(bool edit = false)
        {
            await GetInfo();
            LineElemsCreator.ClearGridRows(Grid, _startChangedRow, _studentParentResponse.Length + 4);

            for (var i = 0; i < _studentParentResponse.Length; i++)
            {
                var elem = _isParent ? _studentParentResponse[i].SchoolStudent : _studentParentResponse[i].Parent;

                var label = BaseElemsCreator.CreateLabel($"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}");
                if (edit)
                {
                    var tapGesture = new TapGestureRecognizer();
                    var index = i;
                    tapGesture.Tapped += (sender, e) =>
                    {
                        Delete(_studentParentResponse[index].Id);
                    };
                    label.GestureRecognizers.Add(tapGesture);
                }
                LineElemsCreator.AddLineElems(
                    Grid,
                    _startChangedRow + i,
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_studentParentResponse[i].ParentType?.Name)
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = label
                        }
                    ]
                );
            }
        }

        public void AddSchoolStudent(ParentRequest request, long _educationalInstitutionId)
        {
            ShowBase();

            LineElemsCreator.AddLineElems(
                Grid,
                _startChangedRow + 1,
                [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreatePicker(ParentSchoolStudentCreator.GetParentTypes(),
                                                selectedIndex => request.ParentType = selectedIndex)
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateSearchPopupAsLabel(GetSchoolStudents(_educationalInstitutionId),
                                                selectedIndex => request.SchoolStudentId = selectedIndex)
                    }
                ]
            );
        }

        private StudentParentRequest _studentParentRequest;
        private bool _addParent = false;
        public async Task AddParent()
        {
            await ShowList(true);

            var rowIndex = _startChangedRow + _studentParentResponse.Length;
            if (!_addParent)
            {
                LineElemsCreator.AddLineElems(
                    Grid,
                    rowIndex++,
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateButton("Добавить", AddButtonCliked)
                        },
                    ]
                );
            }
            else
            {
                _studentParentRequest = new();
                LineElemsCreator.AddLineElems(
                    Grid,
                    rowIndex++,
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreatePicker(GetParentTypes(),
                                                    selectedIndex => _studentParentRequest.ParentTypeId = selectedIndex)
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateSearchPopupAsLabel(GetParents(),
                                                    selectedIndex => _studentParentRequest.ParentId = selectedIndex)
                        }
                    ]
                );

                LineElemsCreator.AddLineElems(
                    Grid,
                    rowIndex++,
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateButton("Сохранить", SaveButtonCliked)
                        },
                    ]
                );
            }
        }

        private void AddButtonCliked(object sender, EventArgs e)
        {
            _addParent = true;

            _ = AddParent();
        }

        private void SaveButtonCliked(object sender, EventArgs e)
        {
            _studentParentRequest.SchoolStudentId = _userId;
            Task.Run(async () =>
            {
                var response = await ParentController.AddParent(_studentParentRequest);
                if (!string.IsNullOrEmpty(response))
                {
                    _addParent = false;
                    _ = AddParent();
                }
            });
        }

        private async void Delete(long id)
        {
            var action = await BaseElemsCreator.CreateActionSheet(["Удалить"]);
            if (action == "Удалить")
            {
                _ = await ParentController.DeleteStudentParent(id);
                _ = AddParent();
            }
        }

        private static ObservableCollection<Item> GetParentTypes()
        {
            var list = new ObservableCollection<Item>();

            Task.Run(async () =>
            {
                TypeResponse[]? arr = null;
                var response = await ParentController.GetParentType();
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                Application.Current?.Dispatcher.Dispatch(() =>
                {
                    foreach (var elem in arr ?? [])
                    {
                        list.Add(item: new Item(elem.Id, elem.Name));
                    }
                });
            });

            return list;
        }

        private static List<Item> GetSchoolStudents(long _educationalInstitutionId)
        {
            var list = new List<Item>();

            Task.Run(async () =>
            {
                UserResponse[]? arr = null;
                var controller = new SchoolStudentController();
                var response = await controller.GetAll(_educationalInstitutionId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<UserResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, $"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}"));
                }
            });

            return list;
        }

        private List<Item> GetParents()
        {
            var list = new List<Item>();

            Task.Run(async () =>
            {
                UserResponse[]? arr = null;
                var response = await ParentController.GetParentsWithoutSchoolStudent(_userId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<UserResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, $"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}"));
                }
            });

            return list;
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\General\PreAdminPage.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Pages.AdminPageComponents.General
{
    public partial class PreAdminPage : ContentPage
    {
        public PreAdminPage()
        {
            Navigation.PushAsync(new AdminPage());
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\ParentView\ParentViewObjectCreator.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.ParentView
{
    public class ParentViewObjectCreator : UserViewObjectCreator<UserResponse, ParentRequest, ParentController>
    {

        private ParentSchoolStudentCreator? _parentSchoolStudent = null;
        protected override void CreateUI()
        {
            base.CreateUI();

            _parentSchoolStudent = new ParentSchoolStudentCreator(_baseResponse.Id, true);
            _infoStack.Add(_parentSchoolStudent.Grid);
            if (_componentState == AdminPageStatic.ComponentState.New)
            {
                _parentSchoolStudent.AddSchoolStudent(_baseRequest, _educationalInstitutionId);
            }
            else
            {
                _ = _parentSchoolStudent.ShowList();
            }
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\SchoolStudentView\SchoolStudentViewObjectCreator.cs
////////////////////////////////////////////////////////////////////////////////

using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.SchoolStudentView
{
    public class SchoolStudentViewObjectCreator : UserViewObjectCreator<SchoolStudentResponse, SchoolStudentRequest, SchoolStudentController>
    {
        private ParentSchoolStudentCreator? _parentSchoolStudent = null;
        protected override void CreateUI()
        {
            base.CreateUI();

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel( "Класс")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.Class?.Name)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateSearchPopupAsLabel(GetClasses(),
                                                    selectedIndex => _baseRequest.ClassId = selectedIndex)
                        }
                ]
            );

            _parentSchoolStudent = new ParentSchoolStudentCreator(_baseResponse.Id, false);
            _infoStack.Add(_parentSchoolStudent.Grid);
            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                _ = _parentSchoolStudent.AddParent();
            }
            else
            {
                _ = _parentSchoolStudent.ShowList();
            }
        }
        private List<Item> GetClasses()
        {
            var list = new List<Item>();

            Task.Run(async () =>
            {
                ClassResponse[]? arr = null;
                var response = await _controller.GetAll(_educationalInstitutionId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<ClassResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, elem?.Name));
                }
            });

            return list;
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\UserView\UserViewElemCreator.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.UserView
{
    public class UserViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator> : BaseViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator>
        where TResponse : UserResponse, new()
        where TRequest : UserRequest, new()
        where TController : IController, new()
        where TViewObjectCreator : BaseViewObjectCreator<TResponse, TRequest, TController>, new()
    {

        protected override void CreateUI(ref int rowIndex)
        {
            base.CreateUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Фамилия"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.LastName),
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Имя"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.FirstName)
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Отчество"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Patronymic)
                    },
                ]
            );
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\UserView\UserViewListCreator.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.UserView
{
    public class UserViewListCreator<TResponse, TRequest, TController, TViewElemCreator, TViewObjectCreator> : BaseViewListCreator<TResponse, TRequest, TController, TViewElemCreator, TViewObjectCreator>
        where TResponse : UserResponse, new()
        where TRequest : UserRequest, new()
        where TController : IController, new()
        where TViewElemCreator : BaseViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator>, new()
        where TViewObjectCreator : BaseViewObjectCreator<TResponse, TRequest, TController>, new()
    {
        public UserViewListCreator()
        {
            _titleView = "Список пользователй";
            _maxCountViews = 3;
        }

        private string _lastNameFilter = string.Empty;
        private string _firstNameFilter = string.Empty;
        private string _patronymicFilter = string.Empty;

        protected override void CreateFilterUI(ref int rowIndex)
        {
            base.CreateFilterUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Фамилия"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateEditor(newText => _lastNameFilter = newText, "Дубовский")
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Имя"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateEditor(newText => _firstNameFilter = newText, "Алексей")
                    },
                ]
            );


            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Отчество"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateEditor(newText => _patronymicFilter = newText,  "Владимирович")
                    },
                ]
            );
        }

        // Получение списка объектов
        protected override void FilterList()
        {
            bool lastNameFilter = string.IsNullOrEmpty(_lastNameFilter);
            bool firstNameFilter = string.IsNullOrEmpty(_firstNameFilter);
            bool patronymicFilter = string.IsNullOrEmpty(_patronymicFilter);

            _objectsArr = [.. _objectsArr
                .Where(e =>
                    (!lastNameFilter || (e.LastName ?? string.Empty).Contains(_lastNameFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (!firstNameFilter || (e.FirstName ?? string.Empty).Contains(_firstNameFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (!patronymicFilter || (e.Patronymic ?? string.Empty).Contains(_patronymicFilter!, StringComparison.OrdinalIgnoreCase)))];
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\AdminPageComponents\UserView\UserViewObjectCreator.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.UserView
{
    public class UserViewObjectCreator<TResponse, TRequest, TController> : BaseViewObjectCreator<TResponse, TRequest, TController>
        where TResponse : UserResponse, new()
        where TRequest : UserRequest, new()
        where TController : IController, new()
    {
        private VerticalStackLayout? _imageStack = null;
        protected override void CreateUI()
        {
            base.CreateUI();

            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                _baseRequest.FirstName = _baseResponse.FirstName;
                _baseRequest.LastName = _baseResponse.LastName;
                _baseRequest.Patronymic = _baseResponse.Patronymic;
                _baseRequest.Email = _baseResponse.Email;
                _baseRequest.PhoneNumber = _baseResponse.PhoneNumber;
            }

            _imageStack = BaseElemsCreator.CreateImageFromUrl(_baseResponse.PathImage,
                _componentState == AdminPageStatic.ComponentState.Edit ? AddImageTapped : null);
            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                     new LineElemsCreator.Data
                     {
                         CountJoinColumns = 2,
                         Elem = _imageStack
                     }
                ]);

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Фамилия")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.LastName),
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.LastName = newText, "Дубовский", _baseResponse.LastName)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Имя")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.FirstName)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.FirstName = newText, "Алексей", _baseResponse.FirstName)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Отчество")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Patronymic)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.Patronymic = newText, "Владимирович", _baseResponse.Patronymic)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Email")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Email)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.Email = newText, "sh4@edus.by", _baseResponse.Email)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Телефон")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.PhoneNumber)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor( newText => _baseRequest.PhoneNumber = newText, "+375 17 433-09-02", _baseResponse.PhoneNumber)
                        }
                ]
            );
            if (_componentState == AdminPageStatic.ComponentState.New)
            {
                LineElemsCreator.AddLineElems(
                    grid: _baseInfoGrid,
                    rowIndex: _baseInfoGridRowIndex++,
                    objectList:
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel("Логин")
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.Login = newText, "admin")
                        }
                    ]
                );
                LineElemsCreator.AddLineElems(
                    grid: _baseInfoGrid,
                    rowIndex: _baseInfoGridRowIndex++,
                    objectList:
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel("Пароль")
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.Password = newText, "4Af7@adf")
                        }
                    ]
                );

                _baseRequest.UniversityId = _educationalInstitutionId;
            }
        }

        private Stream? _image_stream = null;
        private FileResult? _imageFile = null;
        private async void AddImageTapped(object? sender, EventArgs e)
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите изображение",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null) return;
            _imageFile = result;

            if (_imageStack != null)
            {
                _image_stream?.Dispose();
                _image_stream = await result.OpenReadAsync();
                Image image = (Image)_imageStack[^1];
                image.Source = ImageSource.FromStream(() => _image_stream);
            }
        }

        protected override void SaveButtonClicked(object? sender, EventArgs e)
        {
            base.SaveButtonClicked(sender, e);

            if (_imageFile != null)
            {
                _controller.AddImage(_baseResponse.Id, _imageFile);
            }
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\Components\Elems\BaseElemsCreator.cs
////////////////////////////////////////////////////////////////////////////////

using System.Collections.ObjectModel;

using CommunityToolkit.Maui.Views;

using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.Components.Elems
{
    public static class BaseElemsCreator
    {
        public static Button CreateButton(string text, EventHandler handler, bool navigation = false)
        {
            var button = new Button()
            {
                HorizontalOptions = LayoutOptions.Fill,

                BackgroundColor = navigation ? UserData.Settings.Theme.NavigationPageColor : UserData.Settings.Theme.AccentColor,
                TextColor = UserData.Settings.Theme.TextColor,

                FontSize = UserData.Settings.Fonts.BASE_FONT_SIZE,
                Text = text,
            };

            button.Clicked += handler;
            return button;
        }

        public static async Task<string> CreateActionSheet(string[] actionList)
        {
            var page = Application.Current?.Windows[0].Page;
            var action = string.Empty;
            if (page != null) action = await page.DisplayActionSheet(
                "Выберите действие",
                "Отмена",
                null,
                actionList);

            return action;
        }

        public static Image CreateImageFromFile(string? imagePath, EventHandler<TappedEventArgs> handler)
        {
            var image = new Image
            {
                MaximumWidthRequest = UserData.Settings.Sizes.IMAGE_BUTTON_SIZE,
                MaximumHeightRequest = UserData.Settings.Sizes.IMAGE_BUTTON_SIZE,
                Source = ImageSource.FromFile(imagePath)
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += handler;
            image.GestureRecognizers.Add(tapGesture);

            return image;
        }

        public static VerticalStackLayout CreateImageFromUrl(string? url, EventHandler<TappedEventArgs>? handler)
        {
            var vStack = new VerticalStackLayout();

            var loadImage = new Image
            {
                MaximumWidthRequest = UserData.Settings.Sizes.IMAGE_SIZE,
                MaximumHeightRequest = UserData.Settings.Sizes.IMAGE_SIZE,
                Source = ImageSource.FromFile(url == null ? "no_image.png" : "loading_image.png")
            };
            if (handler != null)
            {
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += handler;
                loadImage.GestureRecognizers.Add(tapGesture);
            }

            vStack.Add(loadImage);

            if (url != null)
            {
                var mainImage = new Image
                {
                    MaximumHeightRequest = UserData.Settings.Sizes.IMAGE_SIZE,
                    MinimumHeightRequest = UserData.Settings.Sizes.IMAGE_SIZE,
                    Source = ImageSource.FromUri(new Uri(url + $"?t={DateTime.Now.Ticks}")),
                };
                if (handler != null)
                {
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += handler;
                    mainImage.GestureRecognizers.Add(tapGesture);
                }

                vStack.Add(mainImage);

                loadImage.IsVisible = true;
                mainImage.IsVisible = false;
                Task.Run(async () =>
                {
                    while (!mainImage.IsVisible)
                    {
                        await Task.Delay(1000);
                        Application.Current?.Dispatcher.Dispatch(() =>
                        {
                            loadImage.IsVisible = mainImage.IsLoading;
                            mainImage.IsVisible = !mainImage.IsLoading;
                        });
                    }
                });
            }

            return vStack;
        }

        public static Editor CreateEditor(Action<string>? textChangedAction, string? placeholder, string? text = null)
        {
            var editor = new Editor
            {
                BackgroundColor = UserData.Settings.Theme.AccentColorFields,
                TextColor = UserData.Settings.Theme.TextColor,
                PlaceholderColor = UserData.Settings.Theme.PlaceholderColor,

                FontSize = UserData.Settings.Fonts.BASE_FONT_SIZE,
                Placeholder = placeholder ?? string.Empty,
                Text = text ?? string.Empty,

                AutoSize = EditorAutoSizeOption.TextChanges,
            };
            if (textChangedAction != null)
            {
                editor.TextChanged += (sender, e) => textChangedAction(e.NewTextValue);
            }

            return editor;
        }

        public static Label CreateLabel(string? text, int maxLength = -1)
        {
            if (text != null && maxLength > 0 && text.Length > maxLength)
            {
                text = text.Substring(0, maxLength) + " …";
            }

            var label = new Label
            {
                TextColor = UserData.Settings.Theme.TextColor,

                FontSize = UserData.Settings.Fonts.BASE_FONT_SIZE,
                Text = text ?? string.Empty,
            };

            return label;
        }

        public static Label CreateSearchPopupAsLabel(List<Item> itemList, Action<long> idChangedAction)
        {
            var searchLabel = CreateLabel("Поиск");
            searchLabel.BackgroundColor = UserData.Settings.Theme.AccentColorFields;
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (sender, e) =>
            {
                long id = -1;
                Action<long> idChangedActionLocal = newText => id = newText;

                var popup = new SearchPopup(itemList, idChangedActionLocal);

                popup.Closed += (sender, e) =>
                {
                    if (popup.AllItems.Count > 0 && id > -1)
                    {
                        if (idChangedAction != null) idChangedAction(id);
                        searchLabel.Text = popup.AllItems.FirstOrDefault(item => item.Id == id)?.Name ?? "Найти";
                    }
                    searchLabel.Focus();
                };

                var page = Application.Current?.Windows[0].Page;
                page?.ShowPopup(popup);
            };

            searchLabel.GestureRecognizers.Add(tapGesture);
            return searchLabel;
        }

        public static Picker CreatePicker(ObservableCollection<Item> itemList, Action<long> idChangedAction, long? baseSelectedId = null)
        {
            var picker = new Picker
            {
                BackgroundColor = UserData.Settings.Theme.AccentColorFields,
                TextColor = UserData.Settings.Theme.TextColor,

                FontSize = UserData.Settings.Fonts.BASE_FONT_SIZE,

                ItemsSource = itemList,
                ItemDisplayBinding = new Binding("Name"),
            };

            if (itemList != null && baseSelectedId != null)
            {
                var item = itemList.FirstOrDefault(i => i.Id == baseSelectedId);
                if (item != null)
                {
                    var selectedIndex = itemList.IndexOf(item);
                    picker.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
                }
            }

            if (idChangedAction != null)
            {
                picker.SelectedIndexChanged += (sender, e) =>
                {
                    if (picker.SelectedItem is Item selectedItem)
                    {
                        idChangedAction(selectedItem.Id);
                    }
                };
            }

            return picker;
        }



        public static Grid CreateGrid(int countColumns = 2, bool padding = true)
        {
            var grid = new Grid
            {
                ColumnSpacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,
                RowSpacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,

                BackgroundColor = UserData.Settings.Theme.BackgroundFillColor,
            };
            if (padding) { grid.Padding = UserData.Settings.Sizes.PADDING_ALL_PAGES; }

            for (int i = 0; i < countColumns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            return grid;
        }

        public static VerticalStackLayout CreateVerticalStackLayout()
        {
            var verticalStackLayout = new VerticalStackLayout
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,
                Padding = UserData.Settings.Sizes.PADDING_ALL_PAGES,
                Spacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,
            };

            return verticalStackLayout;
        }

        public static HorizontalStackLayout CreateHorizontalStackLayout()
        {
            var hStack = new HorizontalStackLayout()
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Fill,
            };

            return hStack;
        }

    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\Components\Elems\Item.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Pages.Components.Elems
{
    public class Item
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public Item(long id, string? name)
        {
            Id = id;
            Name = name ?? "ошибка";
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\Components\Elems\LineElemsCreator.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Pages.Components.Elems
{
    public static class LineElemsCreator
    {
        public class Data
        {
            public int CountJoinColumns { get; set; } = -1;
            public View? Elem { get; set; } = null;
        }

        public static void AddLineElems(Grid grid, int rowIndex, Data[] objectList)
        {
            Application.Current?.Dispatcher.Dispatch(() =>
            {
                var indexColumn = 0;

                foreach (var obj in objectList)
                {
                    if (obj.Elem is View view)
                    {
                        grid.Add(view, indexColumn++, rowIndex);
                        if (obj.CountJoinColumns > 0) Grid.SetColumnSpan(view, obj.CountJoinColumns);
                    }
                }
            });
        }

        private static void ClearGridRows(Grid grid, IView[] elementsToRemove)
        {
            Application.Current?.Dispatcher.Dispatch(() =>
            {
                foreach (var element in elementsToRemove)
                {
                    grid.Children.Remove(element);
                }
            });
        }

        public static void ClearGridRows(Grid grid, int index)
        {
            var elementsToRemove = grid.Children
                .Where(e => index == grid.GetRow(e))
                .ToArray();

            ClearGridRows(grid, elementsToRemove);
        }

        public static void ClearGridRows(Grid grid, int[] indexes)
        {
            var elementsToRemove = grid.Children
                .Where(e => indexes.Contains(grid.GetRow(e)))
                .ToArray();

            ClearGridRows(grid, elementsToRemove);
        }

        public static void ClearGridRows(Grid grid, int firstIndex, int lastIndex)
        {
            var elementsToRemove = grid.Children
                .Where(e =>
                {
                    int row = grid.GetRow(e);
                    return row >= firstIndex && row <= lastIndex;
                })
                .ToArray();

            ClearGridRows(grid, elementsToRemove);
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\Components\Elems\SearchPopup.cs
////////////////////////////////////////////////////////////////////////////////

using CommunityToolkit.Maui.Views;

using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.Others
{
    public partial class SearchPopup : Popup
    {
        public List<Item> AllItems { get; set; }
        private readonly ListView _listView;

        public SearchPopup(List<Item> items, Action<long> IdChangedAction)
        {
            AllItems = items;

            Color = UserData.Settings.Theme.BackgroundPageColor;

            var searchBar = new SearchBar
            {
                // Цвета
                BackgroundColor = UserData.Settings.Theme.AccentColorFields,
                TextColor = UserData.Settings.Theme.TextColor,
                PlaceholderColor = UserData.Settings.Theme.PlaceholderColor,

                // Текст
                FontSize = UserData.Settings.Fonts.BASE_FONT_SIZE,
                Placeholder = "Введите для поиска...",
            };

            searchBar.TextChanged += (sender, e) =>
            {
                FilterItems(e.NewTextValue);
            };

            _listView = new ListView
            {
                // Цвета
                BackgroundColor = UserData.Settings.Theme.BackgroundPageColor,

                ItemsSource = AllItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var label = new Label();
                    label.BackgroundColor = UserData.Settings.Theme.AccentColorFields;
                    label.TextColor = UserData.Settings.Theme.TextColor;
                    label.SetBinding(Label.TextProperty, "Name");
                    return new ViewCell { View = label };
                })
            };

            _listView.ItemTapped += (sender, e) =>
            {
                if (e.Item is Item selectedItem)
                {
                    HandleItemTapped(selectedItem, IdChangedAction);
                }
            };

            var stackLayout = new StackLayout
            {
                // Положение
                WidthRequest = (Application.Current?.Windows[0].Width ?? 0) / 1 * 0.90,
#if WINDOWS
                HeightRequest = 100,
#endif
                Padding = UserData.Settings.Sizes.PADDING_ALL_PAGES,
                Spacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,

                // Цвета
                BackgroundColor = UserData.Settings.Theme.BackgroundPageColor,

                Children =
                {
                    searchBar,
                    _listView,
                }
            };

            Content = stackLayout;
        }

        private void FilterItems(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _listView.ItemsSource = AllItems;
            }
            else
            {
                _listView.ItemsSource = AllItems.Where(item => (item.Name ?? string.Empty).Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        private void HandleItemTapped(Item selectedItem, Action<long> IdChangedAction)
        {
            IdChangedAction(selectedItem.Id);
            Close();
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\Components\Navigation\Navigator.cs
////////////////////////////////////////////////////////////////////////////////

using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.AdminPageComponents.ParentView;
using ElectronicDiary.Pages.AdminPageComponents.SchoolStudentView;
using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Pages.OtherPages;
using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.Components.Navigation
{
    public static class Navigator
    {
        public static void SetAsRoot(ContentPage page)
        {
            Application.Current?.Dispatcher.Dispatch(() =>
            {
                if (Application.Current?.Windows.Count > 0)
                {
                    Application.Current.Windows[0].Page = new ThemedNavigationPage(page);
                }
            });
        }

        public static async void ChooseRootPageByRole(UserInfo.RoleType role, long id)
        {
            ContentPage? page = null;
            switch (role)
            {
                case UserInfo.RoleType.MainAdmin:
                    page = new PreAdminPage();
                    break;

                default:
                    page = await GetProfileByRole(role, id);
                    break;
            }

            SetAsRoot(page ?? new LogPage());
        }

        public static async Task<ContentPage> GetProfileByRole(UserInfo.RoleType role, long id)
        {
            IController? controller = null;
            ScrollView? scrollView = null;
            string? response = null;
            switch (role)
            {
                case UserInfo.RoleType.LocalAdmin:
                    controller = new AdministratorController();
                    response = await controller.GetById(id);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var responseObject = JsonSerializer.Deserialize<UserResponse>(response, PageConstants.JsonSerializerOptions);
                        if (responseObject != null)
                        {
                            UserData.UserInfo.EducationId = responseObject.EducationalInstitution?.Id ?? - 1;
                            var creator = new UserViewObjectCreator<UserResponse, UserRequest, AdministratorController>();
                            scrollView = creator.Create(null, null, null, responseObject, responseObject.EducationalInstitution?.Id ?? -1);
                        }
                    }
                    break;

                case UserInfo.RoleType.Teacher:
                    controller = new TeacherController();
                    response = await controller.GetById(id);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var responseObject = JsonSerializer.Deserialize<UserResponse>(response, PageConstants.JsonSerializerOptions);
                        if (responseObject != null)
                        {
                            UserData.UserInfo.EducationId = responseObject.EducationalInstitution?.Id ?? -1;
                            var creator = new UserViewObjectCreator<UserResponse, UserRequest, TeacherController>();
                            scrollView = creator.Create(null, null, null, responseObject, responseObject.EducationalInstitution?.Id ?? -1);
                        }
                    }
                    break;

                case UserInfo.RoleType.SchoolStudent:
                    controller = new TeacherController();
                    response = await controller.GetById(id);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var responseObject = JsonSerializer.Deserialize<SchoolStudentResponse>(response, PageConstants.JsonSerializerOptions);
                        if (responseObject != null)
                        {
                            UserData.UserInfo.EducationId = responseObject.EducationalInstitution?.Id ?? -1;
                            var creator = new SchoolStudentViewObjectCreator();
                            scrollView = creator.Create(null, null, null, responseObject, responseObject.EducationalInstitution?.Id ?? -1);
                        }
                    }
                    break;

                case UserInfo.RoleType.Parent:
                    controller = new ParentController();
                    response = await controller.GetById(id);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var responseObject = JsonSerializer.Deserialize<UserResponse>(response, PageConstants.JsonSerializerOptions);
                        if (responseObject != null)
                        {
                            UserData.UserInfo.EducationId = responseObject.EducationalInstitution?.Id ?? - 1;
                            var creator = new ParentViewObjectCreator();
                            scrollView = creator.Create(null, null, null, responseObject, responseObject.EducationalInstitution?.Id ?? -1);
                        }
                    }
                    break;

                default:
                    scrollView = null;
                    break;

            }

            ContentPage page;
            if (scrollView != null)
            {
                Application.Current?.Dispatcher.Dispatch(() =>
                {
                    AdminPageStatic.CalcViewWidth(out var width, out _);
                    scrollView.MaximumWidthRequest = width;
                });
                page = new BaseUserUIPage(BaseElemsCreator.CreateHorizontalStackLayout(), [scrollView], BaseUserUIPage.PageType.Profile);
            }
            else
            {
                page = new EmptyPage();
            }

            return page;
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\Components\Navigation\ThemedNavigationPage.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.Others
{
    public partial class ThemedNavigationPage : NavigationPage
    {
        // Красит полосу навигации
        public ThemedNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = UserData.Settings.Theme.NavigationPageColor;
            BarTextColor = UserData.Settings.Theme.TextColor;
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\Components\Navigation\ToolbarItemsAdder.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.OtherPages;
using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Pages.Components.Navigation
{
    public static class ToolbarItemsAdder
    {
        private static void AddElem(IList<ToolbarItem> toolbarItems, string title, string imagePath, EventHandler eventHandler)
        {
            var item = new ToolbarItem();
#if WINDOWS
            item.Text = title;
#else
            item.IconImageSource = ImageSource.FromFile(imagePath);
#endif

            item.Clicked += eventHandler;
            toolbarItems.Add(item);
        }



        public static void AddSettings(IList<ToolbarItem> toolbarItems)
        {
            AddElem(toolbarItems, "Настройки", UserData.Settings.Theme.TextIsBlack ? "black_settings_icon.png" : "white_settings_icon.png", SettingsClicked);
        }
        private static void SettingsClicked(object? sender, EventArgs e)
        {
            var page = Application.Current?.Windows[0].Page;
            page?.Navigation.PushAsync(new SettignsPage());
        }



        public static void AddLogOut(IList<ToolbarItem> toolbarItems)
        {
            AddElem(toolbarItems, "Выход", UserData.Settings.Theme.TextIsBlack ? "black_log_out_icon.png" : "white_log_out_icon.png", LogOutClicked);
        }
        private async static void LogOutClicked(object? sender, EventArgs e)
        {
            var response = await AuthorizationСontroller.LogOut();
            if (!string.IsNullOrEmpty(response))
            {
                UserData.UserInfo = new UserInfo();
                UserData.SaveUserInfo();

                if (Application.Current?.Windows.Count > 0) Application.Current.Windows[0].Page = new ThemedNavigationPage(new LogPage());
            }
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\Components\Other\PageConstants.cs
////////////////////////////////////////////////////////////////////////////////

using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElectronicDiary.Pages.Components.Other
{
    public static class PageConstants
    {
        // Json
        public static JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new DateTimeConverter() }

        };

        public class DateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                => DateTime.Parse(reader.GetString()!);

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
                => writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss"));
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\OtherPages\BaseUserUIPage.cs
////////////////////////////////////////////////////////////////////////////////

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

////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\OtherPages\EmptyPage.cs
////////////////////////////////////////////////////////////////////////////////

using CommunityToolkit.Maui.Views;

using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class EmptyPage : ContentPage
    {
#if !WINDOWS
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
#else
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
#endif
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\OtherPages\LogPage.cs
////////////////////////////////////////////////////////////////////////////////

using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Navigation;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Pages
{
    public partial class LogPage : ContentPage
    {
        private string _login = string.Empty;
        private string _password = string.Empty;

        public LogPage()
        {
            Title = "Авторизация";
            ToolbarItemsAdder.AddSettings(ToolbarItems);
            BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;

            var loginEntry = BaseElemsCreator.CreateEditor(newText => _login = newText, "Логин");

            var passwordEntry = BaseElemsCreator.CreateEditor(newText => _password = newText, "Пароль");

            var toProfilePageButton = BaseElemsCreator.CreateButton("Вход", ToProfilePageButtonClicked);

            var vStack = BaseElemsCreator.CreateVerticalStackLayout();
            AdminPageStatic.CalcViewWidth(out double width, out _);
            vStack.MaximumWidthRequest = width;
            vStack.Add(loginEntry);
            vStack.Add(passwordEntry);
            vStack.Add(toProfilePageButton);
            Content = vStack;
        }

        private async void ToProfilePageButtonClicked(object? sender, EventArgs e)
        {
            await AuthorizationСontroller.LogIn(_login.Trim(), _password.Trim());
            var response = await AuthorizationСontroller.GetUserInfo();
            if (!string.IsNullOrEmpty(response))
            {
                var obj = JsonSerializer.Deserialize<AuthorizationUserResponse>(response, PageConstants.JsonSerializerOptions);
                if (obj != null)
                {
                    UserData.UserInfo = new UserInfo()
                    {
                        Id = obj.Id,
                        Role = UserInfo.ConverStringRoleToEnum(obj.Role),
                        Login = _login,
                        Password = _password
                    };
                    UserData.SaveUserInfo();
                    Navigator.ChooseRootPageByRole(UserData.UserInfo.Role, UserData.UserInfo.Id);
                }
            }
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\OtherPages\SettignsPage.cs
////////////////////////////////////////////////////////////////////////////////

using System.Collections.ObjectModel;

using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Navigation;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.SaveData.Themes;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class SettignsPage : ContentPage
    {
        private long _scaleIndex = -1;
        private long _themeIndex = -1;

        public SettignsPage()
        {
            Title = "Настройки";
            BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;

            var vStack = BaseElemsCreator.CreateVerticalStackLayout();
            AdminPageStatic.CalcViewWidth(out double width, out _);
            vStack.MaximumWidthRequest = width;

            var grid = BaseElemsCreator.CreateGrid();
            vStack.Children.Add(grid);

            var rowIndex = 0;
            LineElemsCreator.AddLineElems(
                grid,
                rowIndex++,
                [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Масштаб интерфейса")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreatePicker(GetScaleList(),
                                                selectedIndex => _scaleIndex = selectedIndex,
                                                (long)((UserData.Settings.UserSettings.ScaleFactor - START_SCALE_FACTOR) / SCALE_FACTOR))
                    }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid,
                rowIndex++,
                [
                new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Тема интерфейса")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreatePicker(GetThemeList(),
                                                selectedIndex => _themeIndex = selectedIndex,
                                                UserData.Settings.UserSettings.ThemeIndex)
                    }
                ]
            );


            vStack.Add(BaseElemsCreator.CreateButton("Сохранить", SaveButtonClicked));

            Content = vStack;
        }

        private const float START_SCALE_FACTOR = 0.5f;
        private const float SCALE_FACTOR = 0.25f;
        private const int SCALE_COUNT = 7;
        private static ObservableCollection<Item> GetScaleList()
        {
            var list = new ObservableCollection<Item>();

            for (var i = 0; i < SCALE_COUNT; i++)
            {
                list.Add(new Item(i, $"{(START_SCALE_FACTOR + i * SCALE_FACTOR) * 100}%"));
            }

            return list;
        }

        private static ObservableCollection<Item> GetThemeList()
        {
            var list = new ObservableCollection<Item>()
            {
                new(0, "Тёмная"),
                new(1, "Светлая"),
                new(2, "Красная"),
                new(3, "Зелённая"),
                new(4, "Синяя"),
                new(5, "Патриот"),
            };

            return list;
        }

        private void SaveButtonClicked(object? sender, EventArgs e)
        {
            if (_scaleIndex > -1)
            {
                var scaleFactor = START_SCALE_FACTOR + _scaleIndex * SCALE_FACTOR;
                UserData.Settings.UserSettings.ScaleFactor = scaleFactor;
                UserData.Settings.Sizes = new Settings.SizesClass(scaleFactor);
                UserData.Settings.Fonts = new Settings.FontsClass(scaleFactor);
            }

            if (_themeIndex > -1)
            {
                UserData.Settings.UserSettings.ThemeIndex = _themeIndex;
                UserData.Settings.Theme = ThemesMeneger.ChooseTheme(_themeIndex);
            }

            UserData.SaveUserSettings();
            Navigator.ChooseRootPageByRole(UserData.UserInfo.Role, UserData.UserInfo.Id);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\OtherViews\NewsView\NewsViewElemCreator.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.DTO.Requests.Social;
using ElectronicDiary.Web.DTO.Responses.Social;

namespace ElectronicDiary.Pages.OtherViews.NewsView
{
    public class NewsViewElemCreator : BaseViewElemCreator<NewsResponse, NewsRequest, NewsController, NewsViewObjectCreator>
    {
        protected override void CreateUI(ref int rowIndex)
        {
            base.CreateUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        CountJoinColumns = 2,
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Title),
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        CountJoinColumns = 2,
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Content, 100)
                    },
                ]
            );
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\OtherViews\NewsView\NewsViewListCreator.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.DTO.Requests.Social;
using ElectronicDiary.Web.DTO.Responses.Social;

namespace ElectronicDiary.Pages.OtherViews.NewsView
{
    public class NewsViewListCreator : BaseViewListCreator<NewsResponse, NewsRequest, NewsController, NewsViewElemCreator, NewsViewObjectCreator>
    {
        public NewsViewListCreator()
        {
            _maxCountViews = 3;
            _titleView = "Список новостей";
        }

        public NewsViewListCreator(int maxCountViews)
        {
            _maxCountViews = maxCountViews;
            _titleView = "Список новостей";
        }

        private string _titleFilter = string.Empty;
        protected override void CreateFilterUI(ref int rowIndex)
        {
            base.CreateFilterUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Название"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateEditor(newText => _titleFilter = newText, "Изменение порядка")
                    },
                ]
            );
        }

        protected override void FilterList()
        {
            bool titleFilter = string.IsNullOrEmpty(_titleFilter);

            _objectsArr = [.. _objectsArr
                .Where(e =>
                    (!titleFilter || (e.Title ?? string.Empty).Contains(_titleFilter!, StringComparison.OrdinalIgnoreCase)))];
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Pages\OtherViews\NewsView\NewsViewObjectCreator.cs
////////////////////////////////////////////////////////////////////////////////


using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.DTO.Requests.Social;
using ElectronicDiary.Web.DTO.Responses.Social;

namespace ElectronicDiary.Pages.OtherViews.NewsView
{
    public class NewsViewObjectCreator : BaseViewObjectCreator<NewsResponse, NewsRequest, NewsController>
    {
        protected override void CreateUI()
        {
            base.CreateUI();

            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                _baseRequest.OwnerUserId = _baseResponse.UserId;
                _baseRequest.Title = _baseResponse.Title;
                _baseRequest.Content = _baseResponse.Content;
                _baseRequest.DateTime = _baseResponse.DateTime;
            }
            else
            {
                _baseRequest.OwnerUserId = UserData.UserInfo.Id;
                _baseRequest.DateTime = DateTime.UtcNow;
            }

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            CountJoinColumns = 2,
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.Title),
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            CountJoinColumns = 2,
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.Title = newText, "Изменение порядка", _baseResponse.Title)
                        }
                ]
            );

            if(_componentState == AdminPageStatic.ComponentState.Read)
            {
                LineElemsCreator.AddLineElems(
                    grid: _baseInfoGrid,
                    rowIndex: _baseInfoGridRowIndex++,
                    objectList: [
                        new LineElemsCreator.Data
                        {
                            CountJoinColumns = 2,
                            Elem = BaseElemsCreator.CreateLabel( $"{_baseResponse.LastName} {_baseResponse.FirstName} {_baseResponse.Patronymic}" ),
                        }
                    ]
                );
            }

            if (_componentState == AdminPageStatic.ComponentState.Read)
            {
                LineElemsCreator.AddLineElems(
                    grid: _baseInfoGrid,
                    rowIndex: _baseInfoGridRowIndex++,
                    objectList: [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel( $"{_baseResponse.DateTime}" ),
                        }
                    ]
                );
            }

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            CountJoinColumns = 2,
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.Content),
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            CountJoinColumns = 2,
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.Content = newText, "Изменение порядка ...", _baseResponse.Content)
                        }
                ]
            );

            _baseRequest.EducationalInstitutionId = _educationalInstitutionId;
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Platforms\Android\MainActivity.cs
////////////////////////////////////////////////////////////////////////////////

using Android.App;
using Android.Content.PM;

namespace ElectronicDiary.Platforms.Android;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Platforms\Android\MainApplication.cs
////////////////////////////////////////////////////////////////////////////////

using Android.App;
using Android.Runtime;

using ElectronicDiary.Main;

namespace ElectronicDiary.Platforms.Android;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(nint handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Platforms\iOS\AppDelegate.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Main;

using Foundation;

namespace ElectronicDiary.Platforms.iOS;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Platforms\iOS\Program.cs
////////////////////////////////////////////////////////////////////////////////

using UIKit;

namespace ElectronicDiary.Platforms.iOS;

public class Program
{
    // This is the main entry point of the application.
    static void Main(string[] args)
    {
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Platforms\MacCatalyst\AppDelegate.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Main;

using Foundation;

namespace ElectronicDiary.Platforms.MacCatalyst;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Platforms\MacCatalyst\Program.cs
////////////////////////////////////////////////////////////////////////////////

using UIKit;

namespace ElectronicDiary.Platforms.MacCatalyst;

public class Program
{
    // This is the main entry point of the application.
    static void Main(string[] args)
    {
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Platforms\Tizen\Main.cs
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace ElectronicDiary;

class Program : MauiApplication
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	static void Main(string[] args)
	{
		var app = new Program();
		app.Run(args);
	}
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Platforms\Windows\App.xaml.cs
////////////////////////////////////////////////////////////////////////////////

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using ElectronicDiary.Main;

namespace ElectronicDiary.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}



////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Other\Settings.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.SaveData.Themes;

namespace ElectronicDiary.SaveData.Other
{
    public class Settings
    {
        public UserSettingsClass UserSettings { get; set; }
        public SizesClass Sizes { get; set; }
        public FontsClass Fonts { get; set; }
        public IAppTheme Theme { get; set; }

        public Settings()
        {
            UserSettings = new();
            Sizes = new(UserSettings.ScaleFactor);
            Fonts = new FontsClass(UserSettings.ScaleFactor);
            Theme = ThemesMeneger.ChooseTheme(UserSettings.ThemeIndex);
        }


        public class UserSettingsClass
        {
            public float ScaleFactor { get; set; } = 1f;
            public long ThemeIndex { get; set; } = 0;
        }

        public class SizesClass
        {
            // Размеры
            public Thickness PADDING_ALL_PAGES = 10;
            public int SPACING_ALL_PAGES = 10;
            public int IMAGE_SIZE = 100;
            public int IMAGE_BUTTON_SIZE = 50;

            public SizesClass(float scaleFactor)
            {
                PADDING_ALL_PAGES = (int)(scaleFactor * UserSettingConstants.SizesClass.PADDING_ALL_PAGES);
                SPACING_ALL_PAGES = (int)(scaleFactor * UserSettingConstants.SizesClass.SPACING_ALL_PAGES);
                IMAGE_SIZE = (int)(scaleFactor * UserSettingConstants.SizesClass.IMAGE_SIZE);
                IMAGE_BUTTON_SIZE = (int)(scaleFactor * UserSettingConstants.SizesClass.IMAGE_BUTTON_SIZE);
            }
        }

        public class FontsClass
        {
            public int TITLE_FONT_SIZE { get; set; } = 16;
            public int BASE_FONT_SIZE { get; set; } = 12;

            public FontsClass(float scaleFactor)
            {
                TITLE_FONT_SIZE = (int)(scaleFactor * UserSettingConstants.FontsClass.TITLE_FONT_SIZE);
                BASE_FONT_SIZE = (int)(scaleFactor * UserSettingConstants.FontsClass.BASE_FONT_SIZE);
            }
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Other\UserInfo.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.SaveData.Other
{
    public class UserInfo
    {
        public long Id { get; set; } = -1;
        public RoleType Role { get; set; } = RoleType.None;
        public long EducationId { get; set; } = -1;

        // Авторизация
        public string? Login { get; set; } = null;
        public string? Password { get; set; } = null;

        public enum RoleType
        {
            None, MainAdmin, LocalAdmin, Teacher, SchoolStudent, Parent
        }

        public static RoleType ConverStringRoleToEnum(string? role)
        {
            return role switch
            {
                "Main admin" => RoleType.MainAdmin,
                "Local admin" => RoleType.LocalAdmin,
                "Teacher" => RoleType.Teacher,
                "School student" => RoleType.SchoolStudent,
                "Parent" => RoleType.Parent,
                _ => RoleType.None,
            };
        }

        public static string ConvertEnumRoleToString(RoleType role)
        {
            return role switch
            {
                RoleType.MainAdmin => "Main admin",
                RoleType.LocalAdmin => "Local admin",
                RoleType.Teacher => "Teacher",
                RoleType.SchoolStudent => "School student",
                RoleType.Parent => "Parent",
                _ => "None"
            };
        }

        public static string ConvertEnumRoleToStringRus(RoleType role)
        {
            return role switch
            {
                RoleType.MainAdmin => "Главный администратор",
                RoleType.LocalAdmin => "Локальный администратор",
                RoleType.Teacher => "Учитель",
                RoleType.SchoolStudent => "Ученик школы",
                RoleType.Parent => "Родитель",
                _ => "Нет"
            };
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Other\UserSettingConstants.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.SaveData.Other
{
    public class UserSettingConstants
    {
        public SizesClass Sizes { get; set; } = new();

        public FontsClass Fonts { get; set; } = new();

        public class SizesClass
        {
            // Размеры
            public const int PADDING_ALL_PAGES = 10;
            public const int SPACING_ALL_PAGES = 10;
            public const int IMAGE_SIZE = 100;
            public const int IMAGE_BUTTON_SIZE = 50;
        }

        public class FontsClass
        {
            public const int TITLE_FONT_SIZE = 16;
            public const int BASE_FONT_SIZE = 12;
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Static\UserData.cs
////////////////////////////////////////////////////////////////////////////////

using System.Text.Json;

using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Themes;

namespace ElectronicDiary.SaveData.Static
{
    public static class UserData
    {
        //public static void SaveAll()
        //{
        //    SaveUserInfo();
        //    LoadUserSettings();
        //}

        public static void LoadAll()
        {
            LoadUserInfo();
            LoadUserSettings();
        }

        private static readonly string USER_INFO_PATH = Path.Combine(FileSystem.AppDataDirectory, "UserInfo.ed");
        public static UserInfo UserInfo { get; set; } = new();
        public static void SaveUserInfo()
        {
            var json = JsonSerializer.Serialize(UserInfo);
            File.WriteAllText(USER_INFO_PATH, json);
        }

        public static void LoadUserInfo()
        {
            if (File.Exists(USER_INFO_PATH))
            {
                try
                {
                    var json = File.ReadAllText(USER_INFO_PATH);
                    var obj = JsonSerializer.Deserialize<UserInfo>(json);
                    if (obj != null) UserInfo = obj;
                }
                catch
                {
                    UserInfo = new();
                }
            }
        }



        private static readonly string USER_SETTINGS_PATH = Path.Combine(FileSystem.AppDataDirectory, "UserSettings.ed");
        public static Settings Settings { get; set; } = new();

        public static void SaveUserSettings()
        {
            var json = JsonSerializer.Serialize(Settings.UserSettings);
            File.WriteAllText(USER_SETTINGS_PATH, json);
        }

        public static void LoadUserSettings()
        {
            if (File.Exists(USER_SETTINGS_PATH))
            {
                try
                {
                    var json = File.ReadAllText(USER_SETTINGS_PATH);
                    var obj = JsonSerializer.Deserialize<Settings.UserSettingsClass>(json);
                    if (obj != null)
                    {
                        Settings.UserSettings = obj;
                        Settings.Sizes = new(obj.ScaleFactor);
                        Settings.Fonts = new(obj.ScaleFactor);
                        Settings.Theme = ThemesMeneger.ChooseTheme(obj.ThemeIndex);
                    }
                }
                catch
                {
                    Settings.UserSettings = new();
                }
            }
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Themes\BlueTheme.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.SaveData.Themes
{
    public class BlueTheme : IAppTheme
    {
        public bool TextIsBlack => false;

        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
        public Color BackgroundPageColor => Color.FromRgb(20, 20, 40);
        public Color NavigationPageColor => Color.FromRgb(40, 40, 80);
        public Color BackgroundFillColor => Color.FromRgb(30, 30, 60);

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ
        public Color AccentColor => Color.FromRgb(33, 33, 150);
        public Color AccentColorFields => Color.FromRgb(50, 50, 100);

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
        public Color TextColor => Color.FromRgb(255, 255, 255);
        public Color PlaceholderColor => Color.FromRgb(120, 120, 120);
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Themes\DarkTheme.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.SaveData.Themes
{
    public class DarkTheme : IAppTheme
    {
        public bool TextIsBlack => false;

        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
        public Color BackgroundPageColor => Color.FromRgb(20, 20, 20);
        public Color NavigationPageColor => Color.FromRgb(40, 40, 40);
        public Color BackgroundFillColor => Color.FromRgb(30, 30, 30);

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ

        public Color AccentColor => Color.FromRgb(33, 33, 150);

        public Color AccentColorFields => Color.FromRgb(50, 50, 50);

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
        public Color TextColor => Color.FromRgb(255, 255, 255);
        public Color PlaceholderColor => Color.FromRgb(120, 120, 120);
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Themes\GreenTheme.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.SaveData.Themes
{
    public class GreenTheme : IAppTheme
    {
        public bool TextIsBlack => false;


        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
        public Color BackgroundPageColor => Color.FromRgb(20, 40, 20);
        public Color NavigationPageColor => Color.FromRgb(40, 80, 40);
        public Color BackgroundFillColor => Color.FromRgb(30, 60, 30);

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ
        public Color AccentColor => Color.FromRgb(33, 150, 33);
        public Color AccentColorFields => Color.FromRgb(50, 100, 50);

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
        public Color TextColor => Color.FromRgb(255, 255, 255);
        public Color PlaceholderColor => Color.FromRgb(120, 120, 120);
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Themes\IAppTheme.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.SaveData.Themes
{
    public interface IAppTheme
    {
        bool TextIsBlack { get; }

        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА

        /// <summary> Основной фон страниц приложения (самый темный слой) </summary>
        Color BackgroundPageColor { get; }

        /// <summary> Фон навигационой панели </summary>
        Color NavigationPageColor { get; }

        /// <summary> Фон для контейнеров, карточек и элементов интерфейса </summary>
        Color BackgroundFillColor { get; }

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ

        /// <summary> Основной акцентный цвет для кнопок, переключателей, выделения </summary>
        Color AccentColor { get; }

        Color AccentColorFields { get; }

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ

        /// <summary> Основной цвет текста (максимальная контрастность) </summary>
        Color TextColor { get; }

        /// <summary> Цвет плейсхолдеров в полях ввода (подсказки) </summary>
        Color PlaceholderColor { get; }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Themes\LightTheme.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.SaveData.Themes
{
    public class LightTheme : IAppTheme
    {
        public bool TextIsBlack => true;

        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
        public Color BackgroundPageColor => Color.FromRgb(250, 250, 250);
        public Color NavigationPageColor => Color.FromRgb(200, 200, 200);
        public Color BackgroundFillColor => Color.FromRgb(225, 225, 225);

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ
        public Color AccentColor => Color.FromRgb(100, 100, 255);

        public Color AccentColorFields => Color.FromRgb(175, 175, 175);

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
        public Color TextColor => Color.FromRgb(0, 0, 0);
        public Color PlaceholderColor => Color.FromRgb(120, 120, 120);
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Themes\PatriotTheme.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.SaveData.Themes
{
    public class PatriotTheme : IAppTheme
    {
        public bool TextIsBlack => true;

        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
        public Color BackgroundPageColor => Color.FromRgb(0, 124, 48);
        public Color NavigationPageColor => Color.FromRgb(207, 23, 33);
        public Color BackgroundFillColor => Color.FromRgb(150, 225, 150);

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ
        public Color AccentColor => Color.FromRgb(100, 255, 100);

        public Color AccentColorFields => Color.FromRgb(150, 200, 150);

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
        public Color TextColor => Color.FromRgb(0, 0, 0);
        public Color PlaceholderColor => Color.FromRgb(120, 120, 120);
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Themes\RedTheme.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.SaveData.Themes
{
    public class RedTheme : IAppTheme
    {
        public bool TextIsBlack => false;

        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
        public Color BackgroundPageColor => Color.FromRgb(40, 20, 20);
        public Color NavigationPageColor => Color.FromRgb(80, 40, 40);
        public Color BackgroundFillColor => Color.FromRgb(60, 30, 30);

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ

        public Color AccentColor => Color.FromRgb(150, 33, 33);

        public Color AccentColorFields => Color.FromRgb(100, 50, 50);

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
        public Color TextColor => Color.FromRgb(255, 255, 255);
        public Color PlaceholderColor => Color.FromRgb(120, 120, 120);
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\SaveData\Themes\ThemesMeneger.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.SaveData.Themes
{
    public class ThemesMeneger
    {
        public static IAppTheme ChooseTheme(long index)
        {
            switch (index)
            {
                case 0:
                    return new DarkTheme();
                case 1:
                    return new LightTheme();
                case 2:
                    return new RedTheme();
                case 3:
                    return new GreenTheme();
                case 4:
                    return new BlueTheme();
                case 5:
                    return new PatriotTheme();
            }

            return new LightTheme();
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Educations\AddressController.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public static class AddressСontroller
    {
        public static Task<string?> GetRegions()
        {
            const string url = "/getRegions";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> GetSettlements(long regionId)
        {
            string url = $"/getSettlements?region={regionId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Educations\ClassController.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class ClassController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getClasses?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findClassByTeacherId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addClass";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Edit(object request)
        {
            return Task.Run(() => { return "empty method"; });
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteClass?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            return Task.Run(() => { return "empty method"; });
        }

        // Не интерфейсные методы
        public Task<string?> GetClassByTeacher(long id)
        {
            string url = $"/findClassByTeacherId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetTeacherByClass(long id)
        {
            string url = $"/getTeacherOfClass?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Educations\EducationalInstitutionController.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class EducationalInstitutionСontroller : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            const string url = "/getSchools";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/getSchoolsById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
        public Task<string?> Edit(object request)
        {
            const string url = "/changeEducationalInstitution";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addEducationalInstitution";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            string url = $"/addImageEducational?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, image: image);
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteEducationalInstitution?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Other\AuthorizationController.cs
////////////////////////////////////////////////////////////////////////////////

using System.Net;

namespace ElectronicDiary.Web.Api.Other
{
    public static class AuthorizationСontroller
    {
        public static Task<string?> LogIn(string login, string password)
        {
            var url = $"/login?login={WebUtility.UrlEncode(login)}&password={WebUtility.UrlEncode(password)}";

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url);
        }

        public static Task<string?> LogOut()
        {
            const string url = $"/logout";

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
        public static Task<string?> GetUserInfo()
        {
            const string url = $"/getAuthorizationUserInfo";

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Other\HttpClientCustom.cs
////////////////////////////////////////////////////////////////////////////////

using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using System.Text.Json;

using ElectronicDiary.Pages;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Pages.Others;

namespace ElectronicDiary.Web.Api.Other
{
    public static class HttpClientCustom
    {
        private static readonly HttpClientHandler _handler = new()
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };

        private static readonly HttpClient _httpClient = new(_handler)
        {
            BaseAddress = new Uri(WebConstants.BASE_URL),
            Timeout = TimeSpan.FromSeconds(30)
        };

        // Добавляем кеш в память
        private static readonly MemoryCache _cache = MemoryCache.Default;
        private static readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public enum HttpTypes
        {
            GET, POST, PUT, DELETE
        }

        public static async Task<string?> CheckResponse(HttpTypes httpTypes, string url, object? request = null, FileResult? image = null)
        {
            HttpContent? content = null;
            if (request != null)
            {
                var json = JsonSerializer.Serialize(request, PageConstants.JsonSerializerOptions);
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            else
            {
                if (image != null)
                {
                    var contentTmp = new MultipartFormDataContent();
                    var fileContent = new StreamContent(image.OpenReadAsync().Result);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                    contentTmp.Add(fileContent, "image", image.FileName);
                    content = contentTmp;
                }
            }


            HttpResponseMessage response = null;
            try
            {
                response = httpTypes switch
                {
                    HttpTypes.GET => await _httpClient.GetAsync(url),
                    HttpTypes.POST => await _httpClient.PostAsync(url, content),
                    HttpTypes.PUT => await _httpClient.PutAsync(url, content),
                    HttpTypes.DELETE => await _httpClient.DeleteAsync(url),
                    _ => throw new ArgumentOutOfRangeException(nameof(httpTypes), httpTypes, null)
                };

                var str = await response.Content.ReadAsStringAsync();
                // Статус код 200-299
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                
                var message = "Что-то пошло не так. Если ошибка повторяется, сообщите в поддержку";
                if (ex is HttpRequestException httpEx)
                {
                    if (httpEx.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            var window = Application.Current?.Windows[0];
                            if (window != null) window.Page = new ThemedNavigationPage(new LogPage());
                        });
                    }

                    message = httpEx.StatusCode switch
                    {
                        HttpStatusCode.Unauthorized => "Войдите в систему для доступа.",
                        HttpStatusCode.Forbidden => "Доступ к этому разделу запрещён.",
                        HttpStatusCode.NotFound => "Информация не найдена.",
                        HttpStatusCode.InternalServerError => "Проблема на сервере. Попробуйте позже.",
                        HttpStatusCode.ServiceUnavailable => "Сервис временно не работает.",
                        HttpStatusCode.GatewayTimeout or HttpStatusCode.RequestTimeout => "Слишком долгий ответ. Проверьте интернет и повторите.",
                        _ => "Ошибка связи с сервером."
                    };
                    message += $" Код {response?.StatusCode}";
                }

                Application.Current?.Dispatcher.Dispatch(() =>
                {
                    var page = Application.Current?.Windows[0].Page;
                    page?.DisplayAlert("Ошибка", message, "OK");
                });
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        //public static string SerializeCookies()
        //{
        //    var cookiesList = _handler.CookieContainer.GetAllCookies();
        //    return JsonSerializer.Serialize(cookiesList);
        //}

        //public static void DeserializeCookies(object request)
        //{
        //    var cookiesList = JsonSerializer.Deserialize<CookieCollection>(json);
        //    _handler.CookieContainer.Add(cookiesList);
        //}
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Other\IController.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Web.Api.Other
{
    public interface IController
    {
        Task<string?> GetAll(long schoolId = -1);
        Task<string?> GetById(long id);
        Task<string?> Add(object request);
        Task<string?> Edit(object request);
        Task<string?> Delete(long id);

        Task<string?> AddImage(long id, FileResult image);
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Other\WebConstants.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Web.Api.Other
{
    public static class WebConstants
    {
        public const string BASE_URL = "http://77.222.37.9:8090";
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Social\NewsController.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Social
{
    public class NewsController : IController
    {
        public Task<string?> GetAll(long educationId)
        {
            string url = $"/findNewsByEducationId?id={educationId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findNewsById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addNews";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Edit(object request)
        {
            return Task.Run(() => "empty method");
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteNewsById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            return Task.Run(() => "empty method");
        }

        // Не интерфейсные методы
        public Task<string?> DeleteComment(long commentId)
        {
            string url = $"/deleteNewsCommentById?id={commentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Users\AdministrationController.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class AdministratorController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getAdministrators?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }


        public Task<string?> GetById(long id)
        {
            string url = $"/findAdministratorById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addAdministrator";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Edit(object request)
        {
            const string url = "/changeAdministrator";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteAdministrator?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            string url = $"/addImageAdministrator?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, image: image);
        }

        // Не интерфейсные методы
        public static Task<string?> GetSchoolByAdministratorId(long id)
        {
            string url = $"/findSchoolByAdministratorId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Users\ParentController.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class ParentController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getParentsByEducationId?id={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findParentById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addNewParent";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }
        public Task<string?> Edit(object request)
        {
            const string url = "/changeParent";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }
        public Task<string?> Delete(long id)
        {
            string url = $"/deleteParent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            string url = $"/addImageParent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, image: image);
        }

        // Не интерфейсные методы
        // Получить родителей исключив родителей по id ребёнка
        public static Task<string?> GetParentsWithoutSchoolStudent(long schoolStudentId)
        {
            string url = $"/getNewParents?id={schoolStudentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        // Добавить уже существующего родителя ребёнку
        public static Task<string?> AddParent(object request)
        {
            const string url = "/addParent";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        // Удалить связь StudentParent
        public static Task<string?> DeleteStudentParent(long id)
        {
            string url = $"/deleteStudentParent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        // Получить тип родителя
        public static Task<string?> GetParentType()
        {
            const string url = "/getParentType";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        // Получить StudentParent по id ученика
        public static Task<string?> GetStudentParents(long schoolStudentId)
        {
            string url = $"/getStudentParents?id={schoolStudentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        // Получить StudentParent по id родителя
        public static Task<string?> GetParentStudents(long parentId)
        {
            string url = $"/getStudentParentsByParentTd?id={parentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Users\SchoolStudentController.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class SchoolStudentController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getSchoolStudents?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findSchoolStudentById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addSchoolStudent";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }
        public Task<string?> Edit(object request)
        {
            const string url = "/changeSchoolStudent";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }
        public Task<string?> Delete(long id)
        {
            string url = $"/deleteSchoolStudent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
        public Task<string?> AddImage(long id, FileResult image)
        {
            string url = $"/addImageSchoolStudent?id={id}";

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, image: image);
        }

        // Не интерфейсные методы
        //Получить школу по id ученика
        public static Task<string?> GetSchoolBySchoolStudentId(long id)
        {
            string url = $"/findSchoolBySchoolStudentId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //Получить учеников по id класса
        public static Task<string?> GetStudentsOfClass(long objectId)
        {
            string url = $"/getStudentsOfClass?ObjectId={objectId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\Api\Users\TeacherController.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class TeacherController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getTeachers?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findTeacherById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addTeacher";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Edit(object request)
        {
            const string url = "/changeTeacher";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }
        public Task<string?> Delete(long id)
        {
            string url = $"/deleteTeacher?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            string url = $"/addImageTeacher?id={id}";

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, image: image);
        }

        // Не интерфейсные методы
        public static Task<string?> GetSchoolByTeacherId(long id)
        {
            string url = $"/findSchoolByTeacherId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> GetTeachersToClass(long schoolId)
        {
            string url = $"/getTeachersToClass?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Requests\Educations\EducationalInstitutionRequest.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.DTO.Requests.Other;

namespace ElectronicDiary.Web.DTO.Requests.Educations
{
    public class EducationalInstitutionRequest : BaseRequest
    {
        public string? Name { get; set; } = null;
        public string? Address { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public long RegionId { get; set; } = -1;
        public long SettlementId { get; set; } = -1;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Requests\Other\BaseRequest.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Web.DTO.Requests.Other
{
    public class BaseRequest
    {
        public long Id { get; set; } = -1;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Requests\Social\NewsRequest.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.DTO.Requests.Other;

namespace ElectronicDiary.Web.DTO.Requests.Social
{
    public class NewsRequest : BaseRequest
    {
        public long EducationalInstitutionId { get; set; } = -1;
        public long OwnerUserId { get; set; } = -1;
        public string? Title { get; set; } = null;
        public string? Content { get; set; } = null;
        public DateTime? DateTime { get; set; } = null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Requests\Users\ParentRequest.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class ParentRequest : UserRequest
    {
        public long ParentType { get; set; } = -1;
        public long SchoolStudentId { get; set; } = -1;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Requests\Users\SchoolStudentRequest.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class SchoolStudentRequest : UserRequest
    {
        public long ClassId { get; set; } = -1;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Requests\Users\StudentParentRequest.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class StudentParentRequest
    {
        public long SchoolStudentId { get; set; } = -1;
        public long ParentId { get; set; } = -1;
        public long ParentTypeId { get; set; } = -1;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Requests\Users\UserRequest.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.DTO.Requests.Other;

namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class UserRequest : BaseRequest
    {
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Patronymic { get; set; }
        public string? Login { get; set; } = null;
        public string? Password { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public long UniversityId { get; set; } = -1;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Responses\Educations\ClassResponse.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record ClassResponse : BaseResponse
    {
        public string? Name { get; init; } = null;
        public UserResponse? Teacher { get; init; } = null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Responses\Educations\EducationalInstitutionResponse.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record EducationalInstitutionResponse : BaseResponse
    {
        public string? Name { get; init; } = null;
        public string? Address { get; init; } = null;
        public string? PathImage { get; init; } = null;
        public string? Email { get; init; } = null;
        public string? PhoneNumber { get; init; } = null;
        public TypeResponse? EducationalInstitutionType { get; init; } = null;
        public SettlementResponse? Settlement { get; init; } = null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Responses\Educations\RegionResponse.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record RegionResponse : BaseResponse
    {
        public string? Name { get; init; } = null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Responses\Educations\SettlementResponse.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record SettlementResponse : BaseResponse
    {
        public string? Name { get; init; } = null;
        public RegionResponse? Region { get; init; } = null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Responses\Other\AuthorizationUserResponse.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Web.DTO.Responses.Other
{
    public record AuthorizationUserResponse : BaseResponse
    {
        public string? Role { get; set; } = null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Responses\Other\BaseResponse.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Web.DTO.Responses.Other
{
    public record BaseResponse
    {
        public long Id { get; init; } = -1;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Responses\Other\TypeResponse.cs
////////////////////////////////////////////////////////////////////////////////

namespace ElectronicDiary.Web.DTO.Responses.Other
{
    public record TypeResponse : BaseResponse
    {
        public string? Name { get; init; } = null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Responses\Social\NewsResponse.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Social
{
    public record NewsResponse : BaseResponse
    {
        public long UserId { get; init; } = -1;
        public long EducationId { get; init; } = -1;
        public string? FirstName { get; init; } = null;
        public string? LastName { get; init; } = null;
        public string? Patronymic { get; init; } = null;
        public string? Title { get; init; } = null;
        public string? Content { get; init; } = null;
        public DateTime? DateTime { get; init; } = null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Responses\Users\SchoolStudentResponse.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record SchoolStudentResponse : UserResponse
    {
        public ClassResponse? Class { get; init; } = null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Responses\Users\StudentParentResponse.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record StudentParentResponse : BaseResponse
    {
        public UserResponse? SchoolStudent { get; init; } = null;
        public UserResponse? Parent { get; init; } = null;
        public TypeResponse? ParentType { get; init; } = null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// FILE: D:\WPS\ElectronicDiary\ElectronicDiary\Web\DTO\Responses\Users\UserResponse.cs
////////////////////////////////////////////////////////////////////////////////

using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record UserResponse : BaseResponse
    {
        public EducationalInstitutionResponse? EducationalInstitution { get; init; } = null;
        public string? FirstName { get; init; } = null;
        public string? LastName { get; init; } = null;
        public string? Patronymic { get; init; }
        public string? PathImage { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
    }
}

// ==================================================
// Статистика:
// Всего файлов: 82
// Общий размер: 0.16 MB
// Исключено файлов: 27
// ==================================================
