﻿using System.Text.Json;

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
        VerticalStackLayout Create(HorizontalStackLayout mainStack, List<ScrollView> viewList, long educationalInstitutionId = -1);
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
        public VerticalStackLayout Create(HorizontalStackLayout mainStack, List<ScrollView> viewList, long educationalInstitutionId = -1)
        {
            _mainStack = mainStack;
            _viewList = viewList;
            _educationalInstitutionId = educationalInstitutionId;

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

            var addButton = BaseElemsCreator.CreateButton("Добавить", AddButtonClicked);
            verticalStack.Add(addButton);

            verticalStack.Add(_listVerticalStack);

            return verticalStack;
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
            var vStack = viewObjectCreator.Create(_mainStack, _viewList, ChageListAction, null, _educationalInstitutionId);
            AdminPageStatic.DeleteLastView(_mainStack, _viewList, _maxCountViews);
            var scrollView = new ScrollView()
            {
                Content = vStack
            };
            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }

        // Получение списка объектов
        protected TResponse[] _objectsArr = [];
        protected virtual async Task CreateListUI()
        {
            await GetList();

            _listVerticalStack.Clear();

            for (var i = 0; i < _objectsArr.Length; i++)
            {
                var baseViewElemCreator = new TViewElemCreator();
                var grid = baseViewElemCreator.Create(_mainStack, _viewList, ChageListAction, _objectsArr[i], _maxCountViews, _educationalInstitutionId);
                _listVerticalStack.Add(grid);
            }
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
