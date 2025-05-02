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

        public BaseViewListCreator() : base()
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

            var vStack = new VerticalStackLayout
            {
                // Положение
                Padding = UserData.Settings.Sizes.PADDING_ALL_PAGES,
                Spacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,
            };

            var scrollView = new ScrollView()
            {
                Content = vStack
            };

            _grid = BaseElemsCreator.CreateGrid();

            var rowIndex = 0;
            CreateFilterUI(ref rowIndex);
            vStack.Add(_grid);

            CreateGetButton(vStack);
            CreateAddButton(vStack);
            CreateColumnTitle(vStack);

            vStack.Add(_listVerticalStack);

            return new ScrollView() { Content = vStack };
        }

        protected string _titleView = string.Empty;

        protected virtual void CreateTitile(ref int rowIndex)
        {
            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateTitleLabel(_titleView),
                        CountJoinColumns = 2
                    },
                ]
            );
        }

        protected virtual void CreateFilterUI(ref int rowIndex)
        {
            CreateTitile(ref rowIndex);

        }

        protected virtual void CreateGetButton(VerticalStackLayout vStack)
        {
            var getButton = BaseElemsCreator.CreateButton("Найти", GetButtonClicked);
            vStack.Add(getButton);
        }

        protected virtual void CreateAddButton(VerticalStackLayout vStack)
        {
            var addButton = BaseElemsCreator.CreateButton("Добавить", AddButtonClicked);
            vStack.Add(addButton);
        }

        protected virtual void CreateColumnTitle(VerticalStackLayout vStack)
        {

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
