using ElectronicDiary.Pages.Components;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Responses;

using System.Text.Json;

namespace ElectronicDiary.Pages.AdminPageComponents.BaseView
{
    public interface IBaseViewListCreator
    {
        ScrollView Create(HorizontalStackLayout mainStack, List<ScrollView> viewList, long educationalInstitutionId = -1);
    }

    public class BaseViewListCreator<TResponse, TRequest, TController, TViewElemCreator, TViewObjectCreator> : IBaseViewListCreator
        where TResponse : BaseResponse, new()
        where TRequest : new()
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
            Spacing = UserData.UserSettings.Sizes.SPACING_ALL_PAGES
        };
        protected long _educationalInstitutionId;

        public BaseViewListCreator()
        {
            _maxCountViews = 0;
            ChageListAction += ChageListHandler;
        }

        protected Grid _grid = [];
        public ScrollView Create(HorizontalStackLayout mainStack, List<ScrollView> viewList, long educationalInstitutionId = -1)
        {
            _mainStack = mainStack;
            _viewList = viewList;
            _educationalInstitutionId = educationalInstitutionId;


            var verticalStack = new VerticalStackLayout
            {
                // Положение
                Padding = UserData.UserSettings.Sizes.PADDING_ALL_PAGES,
                Spacing = UserData.UserSettings.Sizes.SPACING_ALL_PAGES,
            };

            var scrollView = new ScrollView()
            {
                Content = verticalStack
            };

            _grid = BaseElemCreator.CreateGrid();

            var rowIndex = 0;
            CreateFilterUI(ref rowIndex);
            verticalStack.Add(_grid);

            var getButton = BaseElemCreator.CreateButton("Найти", GetButtonClicked); 
            verticalStack.Add(getButton);

            var addButton = BaseElemCreator.CreateButton("Добавить", AddButtonClicked); 
            verticalStack.Add(addButton);

            verticalStack.Add(_listVerticalStack);

            var _ = CreateListUI();

            return scrollView;
        }

        // Пусто
        protected virtual void CreateFilterUI(ref int rowIndex)
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
        protected List<TResponse> _objectsList = [];
        protected virtual async Task CreateListUI()
        {
            await GetList();

            _listVerticalStack.Clear();

            for (var i = 0; i < _objectsList.Count; i++)
            {

                var baseViewElemCreator = new TViewElemCreator();
                var grid = baseViewElemCreator.Create(_mainStack, _viewList, ChageListAction, _objectsList[i], _maxCountViews, _educationalInstitutionId);
                _listVerticalStack.Add(grid);
            }
        }

        protected virtual void ChageListHandler()
        {
            var _ = CreateListUI();
        }

        protected virtual async Task GetList()
        {
            var response = await _controller.GetAll(_educationalInstitutionId);
            if (!string.IsNullOrEmpty(response)) _objectsList = JsonSerializer.Deserialize<List<TResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];
            FilterList();
        }

        // Пусто
        protected virtual void FilterList()
        {

        }
    }
}
