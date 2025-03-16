using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Responses;
using System.Text.Json;

namespace ElectronicDiary.Pages.AdminPageComponents.Base
{
    public interface IBaseView
    {
        ScrollView CreateMainView();
    }

    public class BaseView<TResponse, TRequest, TController> : IBaseView
        where TController : IController
        where TResponse : BaseResponse
    {
        protected readonly HorizontalStackLayout _mainStack;
        protected readonly List<ScrollView> _viewList;
        protected VerticalStackLayout _listVerticalStack = new()
        {
            Spacing = PageConstants.SPACING_ALL_PAGES
        };
        protected TController? _controller;
        protected int _maxCountViews;
        protected long _educationalInstitutionId;
        protected long _elemId;

        public BaseView(HorizontalStackLayout mainStack, List<ScrollView> viewList)
        {
            _mainStack = mainStack;
            _viewList = viewList;
            _educationalInstitutionId = -1;
            _maxCountViews = 0;
        }

        protected virtual void DeleteView(int indexDel = 1)
        {
            while (_viewList.Count >= _maxCountViews)
            {
                _viewList.RemoveAt(_viewList.Count - indexDel);
            }
        }

        public ScrollView CreateMainView()
        {
            var verticalStack = new VerticalStackLayout
            {
                // Положение
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
            };

            var scrollView = new ScrollView()
            {
                Content = verticalStack
            };

            var grid = new Grid
            {
                // Положение
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                Padding = PageConstants.PADDING_ALL_PAGES,
                ColumnSpacing = PageConstants.SPACING_ALL_PAGES,
                RowSpacing = PageConstants.SPACING_ALL_PAGES,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
            };

            var rowIndex = 0;
            CreateFilterView(grid, ref rowIndex);
            verticalStack.Add(grid);

            var getButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = "Найти",
            };
            getButton.Clicked += GetButtonClicked;
            verticalStack.Add(getButton);

            var addButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = "Добавить",
            };
            addButton.Clicked += AddButtonClicked;
            verticalStack.Add(addButton);

            verticalStack.Add(_listVerticalStack);

            var _ = CreateListView();

            return scrollView;
        }

        // Пусто
        protected virtual void CreateFilterView(Grid grid, ref int rowIndex)
        {
        }

        protected virtual async void GetButtonClicked(object? sender, EventArgs e)
        {
            await CreateListView();
        }

        protected virtual void AddButtonClicked(object? sender, EventArgs e)
        {
            _elemId = -1;
            var scrollView = CreateObjectView();
            DeleteView();
            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }



        // Получение списка объектов
        protected List<TResponse> _objectsList = [];
        protected virtual async Task GetList()
        {
            if (_controller != null)
            {
                var response = await _controller.GetAll(_educationalInstitutionId);
                if(!string.IsNullOrEmpty(response)) _objectsList = JsonSerializer.Deserialize<List<TResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];
                FilterList();
            }
        }

        // Пусто
        protected virtual void FilterList()
        {

        }

        // Пусто
        protected virtual void CreateListElemView(Grid grid, ref int rowIndex, int indexElem)
        {

        }

        protected virtual async Task CreateListView()
        {
            await GetList();

            _listVerticalStack.Clear();

            for (var i = 0; i < _objectsList.Count; i++)
            {
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += GestureTapped;
                var grid = new Grid
                {
                    // Положение
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Star }
                    },
                    Padding = PageConstants.PADDING_ALL_PAGES,
                    ColumnSpacing = PageConstants.SPACING_ALL_PAGES,
                    RowSpacing = PageConstants.SPACING_ALL_PAGES,

                    // Цвета
                    BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,

                    // Доп инфа
                    BindingContext = _objectsList[i].Id,
                };
                grid.GestureRecognizers.Add(tapGesture);

                var rowIndex = 0;
                CreateListElemView(grid, ref rowIndex, i);

                _listVerticalStack.Add(grid);
            }
        }




        // Действия с отдельными объектами
        protected virtual async void GestureTapped(object? sender, EventArgs e)
        {
            string action = "";
            var page = Application.Current?.Windows[0].Page;
            if (_maxCountViews == 2)
            {
                if (page != null) action = await page.DisplayActionSheet(
                    "Выберите действие",    // Заголовок
                    "Отмена",               // Кнопка отмены
                    null,                   // Кнопка деструктивного действия (например, удаление)
                    "Описание",             // Остальные кнопки
                    "Перейти",
                    "Редактировать",
                    "Удалить");

            }
            else
            {
                if (page != null) action = await page.DisplayActionSheet(
                    "Выберите действие",    // Заголовок
                    "Отмена",               // Кнопка отмены
                    null,                   // Кнопка деструктивного действия (например, удаление)
                    "Описание",             // Остальные кнопки
                    "Редактировать",
                    "Удалить");
            }

            if (sender is not Grid grid)
            {
                return;
            }
            var id = (long)grid.BindingContext;

            switch (action)
            {
                case "Описание":
                    ShowInfo(id);
                    break;
                case "Перейти":
                    await MoveTo(id);
                    break;
                case "Редактировать":
                    Edit(id);
                    break;
                case "Удалить":
                    Delete(id);
                    await CreateListView();
                    break;
                default:
                    return;
            }

            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }

        protected virtual void ShowInfo(long id)
        {
            _elemId = id;
            var scrollView = CreateObjectView();
            DeleteView();
            _viewList.Add(scrollView);
        }

        protected virtual async Task MoveTo(long id)
        {
            string action = "";
            var page = Application.Current?.Windows[0].Page;
            if (page != null) action = await page.DisplayActionSheet(
                "Выберите список",      // Заголовок
                "Отмена",               // Кнопка отмены
                null,                   // Кнопка деструктивного действия (например, удаление)
                "Администраторы",       // Остальные кнопки
                "Учителя",
                "Классы",
                "Ученики",
                "Родители");

            IBaseView view;
            switch (action)
            {
                case "Администраторы":
                    view = new AdministratorView(_mainStack, _viewList, id);
                    break;
                case "Учителя":
                    view = new TeacherView(_mainStack, _viewList, id);
                    break;
                case "Классы":
                    view = new AdministratorView(_mainStack, _viewList, id);
                    break;
                case "Ученики":
                    view = new SchoolStudentView(_mainStack, _viewList, id);
                    break;
                case "Родители":
                    view = new ParentView(_mainStack, _viewList, id);
                    break;
                default:
                    return;
            }

            DeleteView();
            _viewList.Add(view.CreateMainView());
        }

        protected virtual void Edit(long id)
        {
            _elemId = id;
            var scrollView = CreateObjectView(true);
            while (_viewList.Count >= _maxCountViews)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
            }
            _viewList.Add(scrollView);
        }

        protected virtual void Delete(long id)
        {

            _controller?.Delete(id);
        }



        // Вид объекта
        protected TResponse? _baseResponse;
        protected Grid _objectGrid = new();
        protected virtual ScrollView CreateObjectView(bool edit = false)
        {
            var verticalStack = new VerticalStackLayout
            {
                // Положение
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
            };

            var scrollView = new ScrollView()
            {
                Content = verticalStack
            };

            var rowIndex = 0;
            _objectGrid = new Grid
            {
                // Положение
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                Padding = PageConstants.PADDING_ALL_PAGES,
                ColumnSpacing = PageConstants.SPACING_ALL_PAGES,
                RowSpacing = PageConstants.SPACING_ALL_PAGES,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
            };
            InitializeObjectInfo();
            CreateObjectInfoView(ref rowIndex);
            verticalStack.Add(_objectGrid);

            if (_elemId == -1 || edit)
            {
                var saveButton = new Button
                {
                    // Положение
                    HorizontalOptions = LayoutOptions.Fill,

                    // Цвета
                    BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                    TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                    // Текст
                    FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                    Text = "Сохранить",
                };
                saveButton.Clicked += SaveButtonClicked;

                verticalStack.Add(saveButton);
            }

            return scrollView;
        }

        protected enum ComponentState 
        { 
            Read, New, Edit
        }
        protected ComponentState _componentState;

        protected virtual void InitializeObjectInfo(bool edit = false)
        {
            if (edit)
            {
                _componentState = ComponentState.Edit;
            }
            else
            {
                if (_elemId > 0)
                {
                    _componentState = ComponentState.Read;
                }
                else
                {
                    _componentState = ComponentState.Edit;
                }

            }
        }
        protected virtual void CreateObjectInfoView(ref int rowIndex)
        {
            if (_componentState == ComponentState.Read || _componentState == ComponentState.Edit)
            {
                _baseResponse = _objectsList.FirstOrDefault(x => x.Id == _elemId);
            }
        }

        protected TRequest? _baseRequest;
        protected virtual async void SaveButtonClicked(object? sender, EventArgs e)
        {
            if (_controller != null)
            {
                var json = JsonSerializer.Serialize(_baseRequest, PageConstants.JsonSerializerOptions);
                var response = await _controller.Add(json);
                if (response != null)
                {
                    await CreateListView();
                    AdminPageStatic.OnBackButtonPressed(_mainStack, _viewList);
                }
            }
        }
    }
}
