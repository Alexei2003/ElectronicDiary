using System.Text.Json;

using ElectronicDiary.Pages;
using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.OtherPages;
using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Lists.General;
using ElectronicDiary.UI.Views.Lists.ParentView;
using ElectronicDiary.UI.Views.Lists.SchoolStudentView;
using ElectronicDiary.UI.Views.Lists.UserView;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.UI.Components.Navigation
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

        public static async void ChooseRootPageByRole(UserInfo.RoleType role, long id)
        {
            ContentPage? page = null;
            switch (role)
            {
                case UserInfo.RoleType.MainAdmin:
                    page = new PreAdminPage();
                    break;

                default:
                    page = await GetProfileByRole(role, id);
                    break;
            }

            SetAsRoot(page ?? new LogPage());
        }

        public static async Task<ContentPage> GetProfileByRole(UserInfo.RoleType role, long id)
        {
            IController? controller = null;
            ScrollView? scrollView = null;
            string? response = null;
            switch (role)
            {
                case UserInfo.RoleType.LocalAdmin:
                    controller = new AdministratorController();
                    response = await controller.GetAll(id);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var responseObject = JsonSerializer.Deserialize<UserResponse>(response, PageConstants.JsonSerializerOptions);
                        if (responseObject != null)
                        {
                            UserData.UserInfo.EducationId = responseObject.EducationalInstitution?.Id ?? -1;
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
                            UserData.UserInfo.EducationId = responseObject.EducationalInstitution?.Id ?? -1;
                            var creator = new UserViewObjectCreator<UserResponse, UserRequest, TeacherController>();
                            scrollView = creator.Create(null, null, null, responseObject, responseObject.EducationalInstitution?.Id ?? -1);
                        }
                    }
                    break;

                case UserInfo.RoleType.SchoolStudent:
                    controller = new SchoolStudentController();
                    response = await controller.GetById(id);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var responseObject = JsonSerializer.Deserialize<SchoolStudentResponse>(response, PageConstants.JsonSerializerOptions);
                        if (responseObject != null)
                        {
                            UserData.UserInfo.EducationId = responseObject.EducationalInstitution?.Id ?? -1;
                            var creator = new SchoolStudentViewObjectCreator();
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
                            UserData.UserInfo.EducationId = responseObject.EducationalInstitution?.Id ?? -1;
                            var creator = new ParentViewObjectCreator();
                            scrollView = creator.Create(null, null, null, responseObject, responseObject.EducationalInstitution?.Id ?? -1);
                        }
                    }
                    break;

                default:
                    scrollView = null;
                    break;

            }

            ContentPage page;
            if (scrollView != null)
            {
                Application.Current?.Dispatcher.Dispatch(() =>
                {
                    AdminPageStatic.CalcViewWidth(out var width, out _);
                    scrollView.MaximumWidthRequest = width;
                });
                page = new BaseUserUIPage(BaseElemsCreator.CreateHorizontalStackLayout(), [scrollView], BaseUserUIPage.PageType.Profile);
            }
            else
            {
                page = new LogPage();
            }

            return page;
        }
    }
}
