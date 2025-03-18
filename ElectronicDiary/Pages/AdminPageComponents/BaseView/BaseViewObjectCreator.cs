using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Responses;
using System.Text.Json;

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

        protected enum ComponentState
        {
            Read, New, Edit
        }
        protected ComponentState _componentState;

        // Вид объекта
        protected Grid _grid = [];
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
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
            };

            var scrollView = new ScrollView()
            {
                Content = verticalStack
            };

            _grid = new Grid
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
            CreateUI(ref rowIndex);

            verticalStack.Add(_grid);

            if (_componentState != ComponentState.Read)
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

        // Пусто
        protected virtual void CreateUI(ref int rowIndex)
        {

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
