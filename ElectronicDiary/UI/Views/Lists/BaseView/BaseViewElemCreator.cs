using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Views.Lists.ClassView;
using ElectronicDiary.UI.Views.Lists.General;
using ElectronicDiary.UI.Views.Lists.NewView;
using ElectronicDiary.UI.Views.Lists.ParentView;
using ElectronicDiary.UI.Views.Lists.SchoolStudentView;
using ElectronicDiary.UI.Views.Lists.UserView;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Educations.Other;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.UI.Views.Lists.BaseView
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
        protected long _objectParentId;
        protected bool _readOnly = false;

        protected Grid _grid = [];
        public Grid Create(HorizontalStackLayout mainStack,
                           List<ScrollView> viewList,
                           Action chageListAction,
                           BaseResponse? baseResponse,
                           int maxCountViews,
                           long objetParentId,
                           bool readOnly = false)
        {
            _mainStack = mainStack;
            _viewList = viewList;
            ChageListAction = chageListAction;
            _baseResponse = baseResponse as TResponse ?? new();
            _objectParentId = objetParentId;
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

        protected bool _description = true;
        protected bool _moveTo = false;
        protected bool _edit = true;
        protected string _moveToName = "Перейти к ";
        protected virtual async void GestureTapped(object? sender, EventArgs e)
        {
            if (!_readOnly)
            {
                if (_baseResponse.Id > -1)
                {
                    var action = string.Empty;

                    var strList = new List<string>();
                    if (_description) strList.Add("Описание");
                    if (_moveTo) strList.Add(_moveToName);
                    if (_edit) strList.Add("Редактирование");
                    strList.Add("Удаление");

                    action = await BaseElemsCreator.CreateActionSheet([.. strList]);

                    switch (action)
                    {
                        case "Описание":
                            ShowInfo(_baseResponse.Id);
                            break;
                        case "Редактирование":
                            Edit(_baseResponse.Id);
                            break;
                        case "Удаление":
                            Delete(_baseResponse.Id);
                            break;
                        default:
                            if (action == _moveToName)
                            {
                                MoveTo(_baseResponse.Id);
                            }
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
            var scrollView = baseViewObjectCreator.Create(_mainStack, _viewList, ChageListAction, _baseResponse, _objectParentId);
            AdminPageStatic.DeleteLastView(_mainStack, _viewList, _maxCountViews);
            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }


        protected virtual async void MoveTo(long id)
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
                    viewCreator = new ClassViewListCreator();
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
                var scrollView = viewCreator.Create(_mainStack, _viewList, _baseResponse.Id);

                _viewList.Add(scrollView);
                AdminPageStatic.RepaintPage(_mainStack, _viewList);
            }
        }

        protected virtual void Edit(long id)
        {
            var baseViewObjectCreator = new TViewObjectCreator();
            var scrollView = baseViewObjectCreator.Create(_mainStack, _viewList, ChageListAction, _baseResponse, _objectParentId, true);
            AdminPageStatic.DeleteLastView(_mainStack, _viewList, _maxCountViews);
            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }

        protected virtual async void Delete(long id)
        {
            var page = Application.Current?.Windows[0].Page;
            if (page != null)
            {
                var accept = await page.DisplayAlert("Подтверждение", "Удаление объекта", "Да", "Нет");

                if (accept && _controller != null)
                {
                    await _controller.Delete(id);
                    ChangeList();
                }
            }
        }

        protected virtual void ChangeList()
        {
            ChageListAction.Invoke();
        }
    }
}
