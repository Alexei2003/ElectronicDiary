using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.AdminPageComponents.ParentView;
using ElectronicDiary.Pages.AdminPageComponents.SchoolStudentView;
using ElectronicDiary.Pages.AdminPageComponents.UserView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.BaseView
{
    public class BaseViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator>
        where TResponse : BaseResponse, new()
        where TRequest : new()
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



        protected Grid _grid = [];
        public Grid Create(HorizontalStackLayout mainStack,
                                   List<ScrollView> viewList,
                                   Action chageListAction,
                                   BaseResponse? baseResponse,
                                   int maxCountViews,
                                   long educationalInstitutionId)
        {
            _mainStack = mainStack;
            _viewList = viewList;
            ChageListAction = chageListAction;
            _baseResponse = baseResponse as TResponse ?? new();
            _educationalInstitutionId = educationalInstitutionId;
            _maxCountViews = maxCountViews;



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
            if (_baseResponse.Id > -1)
            {
                var action = string.Empty;
                if (_maxCountViews == 2)
                {
                    action = await BaseElemsCreator.CreateActionSheetCreator(
                       [
                        "Описание",
                        "Перейти",
                        "Редактировать",
                        "Удалить"]);

                }
                else
                {
                    action = await BaseElemsCreator.CreateActionSheetCreator(
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
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
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
            action = await BaseElemsCreator.CreateActionSheetCreator(
               [
                "Администраторы",
                "Учителя",
                "Классы",
                "Ученики",
                "Родители"]);

            IBaseViewListCreator? view = null;
            switch (action)
            {
                case "Администраторы":
                    view = new UserViewListCreator<UserResponse, UserRequest, AdministratorController,
                        UserViewElemCreator<UserResponse, UserRequest, AdministratorController,
                        UserViewObjectCreator<UserResponse, UserRequest, AdministratorController>>,
                        UserViewObjectCreator<UserResponse, UserRequest, AdministratorController>>();
                    break;
                case "Учителя":
                    view = new UserViewListCreator<UserResponse, UserRequest, TeacherController,
                        UserViewElemCreator<UserResponse, UserRequest, TeacherController,
                        UserViewObjectCreator<UserResponse, UserRequest, TeacherController>>,
                        UserViewObjectCreator<UserResponse, UserRequest, TeacherController>>();
                    break;
                case "Классы":
                    //view = new AdministratorView(_mainStack, _viewList, id);
                    break;
                case "Ученики":
                    view = new UserViewListCreator<SchoolStudentResponse, SchoolStudentRequest, SchoolStudentController,
                        UserViewElemCreator<SchoolStudentResponse, SchoolStudentRequest, SchoolStudentController,
                        SchoolStudentViewObjectCreator<SchoolStudentResponse, SchoolStudentRequest, SchoolStudentController>>,
                        SchoolStudentViewObjectCreator<SchoolStudentResponse, SchoolStudentRequest, SchoolStudentController>>();
                    break;
                case "Родители":
                    view = new UserViewListCreator<UserResponse, ParentRequest, ParentController,
                        UserViewElemCreator<UserResponse, ParentRequest, ParentController,
                        ParentViewObjectCreator<UserResponse, ParentRequest, ParentController>>,
                        ParentViewObjectCreator<UserResponse, ParentRequest, ParentController>>();
                    break;
                default:
                    return;
            }

            if (view != null)
            {
                AdminPageStatic.DeleteLastView(_mainStack, _viewList, _maxCountViews);
                _viewList.Add(view.Create(_mainStack, _viewList, _baseResponse.Id));
                AdminPageStatic.RepaintPage(_mainStack, _viewList);
            }
        }

        protected virtual async void Edit(long id)
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
