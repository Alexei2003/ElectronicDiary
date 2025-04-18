﻿using ElectronicDiary.Pages.AdminPageComponents.General;
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

        public VerticalStackLayout Create(HorizontalStackLayout? mainStack,
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
                var saveButton = BaseElemsCreator.CreateButton("Сохранить", SaveButtonClicked);
                verticalStack.Add(saveButton);
            }

            return verticalStack;
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
