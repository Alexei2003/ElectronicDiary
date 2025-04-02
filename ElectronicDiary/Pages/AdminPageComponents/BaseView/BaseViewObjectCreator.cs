using ElectronicDiary.Pages.Components;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Responses;

using System.Text.Json;

using static ElectronicDiary.Pages.AdminPageComponents.AdminPageStatic;

namespace ElectronicDiary.Pages.AdminPageComponents.BaseView
{
    public class BaseViewObjectCreator<TResponse, TRequest, TController>
        where TResponse : BaseResponse, new()
        where TRequest : new()
        where TController : IController, new()
    {
        protected TResponse _baseResponse = new();
        protected TRequest _baseRequest = new();
        protected TController _controller = new();

        protected HorizontalStackLayout _mainStack = [];
        protected List<ScrollView> _viewList = [];
        protected event Action ChageListAction;

        protected long _educationalInstitutionId;


        protected ComponentState _componentState;

        // Вид объекта
        protected VerticalStackLayout _infoStack = [];
        protected Grid _baseInfoGrid = [];
        protected int _baseInfoGridRowIndex = 0;

        public ScrollView Create(HorizontalStackLayout mainStack,
                                    List<ScrollView> viewList,
                                    Action chageListAction,
                                    BaseResponse? baseResponse,
                                    long educationalInstitutionId,
                                    bool edit = false)
        {

            _mainStack = mainStack;
            _viewList = viewList;
            ChageListAction = chageListAction;
            _baseResponse = (baseResponse as TResponse) ?? new();
            _educationalInstitutionId = educationalInstitutionId;

            if (edit)
            {
                _componentState = ComponentState.Edit;
            }
            else
            {
                if (_baseResponse.Id != null)
                {
                    _componentState = ComponentState.Read;
                }
                else
                {
                    _componentState = ComponentState.New;
                }
            }

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

            _infoStack = new VerticalStackLayout
            {
                // Положение
                Spacing = UserData.UserSettings.Sizes.SPACING_ALL_PAGES,
            };
            verticalStack.Add(_infoStack);

            _baseInfoGrid = BaseElemCreator.CreateGrid();
            _infoStack.Add(_baseInfoGrid);

            CreateUI();

            if (_componentState != ComponentState.Read)
            {
                var saveButton = BaseElemCreator.CreateButton("Сохранить", SaveButtonClicked); 
                verticalStack.Add(saveButton);
            }

            return scrollView;
        }

        // Пусто
        protected virtual void CreateUI()
        {
            _baseInfoGridRowIndex = 0;
        }

        protected virtual async void SaveButtonClicked(object? sender, EventArgs e)
        {
            var json = JsonSerializer.Serialize(_baseRequest, PageConstants.JsonSerializerOptions);
            var response = await _controller.Add(json);
            if (!string.IsNullOrEmpty(response))
            {
                ChageListAction.Invoke();
                AdminPageStatic.OnBackButtonPressed(_mainStack, _viewList);
            }
        }
    }
}
