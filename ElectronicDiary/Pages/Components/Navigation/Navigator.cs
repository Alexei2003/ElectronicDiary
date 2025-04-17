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

        public static async void ChoosePageByRole(UserInfo.RoleType role, long id)
        {
            ContentPage? page = null;
            IController? controller = null;
            ScrollView? scrollView = null;
            string? response = null;
            switch (role)
            {
                case UserInfo.RoleType.MainAdmin:
                    page = new PreAdminPage();
                    break;

                case UserInfo.RoleType.LocalAdmin:
                    controller = new AdministratorController();
                    response = await controller.GetById(id);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var responseObject = JsonSerializer.Deserialize<UserResponse>(response, PageConstants.JsonSerializerOptions);
                        if (responseObject != null)
                        {
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
                            var creator = new SchoolStudentViewObjectCreator<SchoolStudentResponse, SchoolStudentRequest, SchoolStudentController>();
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
                            var creator = new ParentViewObjectCreator<UserResponse, ParentRequest, ParentController>();
                            scrollView = creator.Create(null, null, null, responseObject, responseObject.EducationalInstitution?.Id ?? -1);
                        }
                    }
                    break;

                default:
                    page = new EmptyPage();
                    break;
            }

            if (page == null && scrollView != null)
            {
                var hStack = BaseElemsCreator.CreateHorizontalStackLayout();
                Application.Current?.Dispatcher.Dispatch(() =>
                {
                    AdminPageStatic.CalcViewWidth(out double width, out _);
                    scrollView.MaximumWidthRequest = width;
                });
                hStack.Add(scrollView);
                page = new BaseUserUIPage(hStack, "Профиль", BaseUserUIPage.PageType.Profile);
            }

            SetAsRoot(page ?? new LogPage());
        }
    }
}
