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
            VerticalStackLayout? vStack = null;
            string? response = null;
            switch (role)
            {
                case UserInfo.RoleType.LocalAdmin:
                    controller = new AdministratorController();
                    response = await controller.GetById(id);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var responseObject = JsonSerializer.Deserialize<UserResponse>(response, PageConstants.JsonSerializerOptions);
                        if (responseObject != null)
                        {
                            var creator = new UserViewObjectCreator<UserResponse, UserRequest, AdministratorController>();
                            vStack = creator.Create(null, null, null, responseObject, responseObject.EducationalInstitution?.Id ?? -1);
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
                            vStack = creator.Create(null, null, null, responseObject, responseObject.EducationalInstitution?.Id ?? -1);
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
                            vStack = creator.Create(null, null, null, responseObject, responseObject.EducationalInstitution?.Id ?? -1);
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
                            vStack = creator.Create(null, null, null, responseObject, responseObject.EducationalInstitution?.Id ?? -1);
                        }
                    }
                    break;

                default:
                    vStack = null;
                    break;

            }

            ContentPage page;
            if (vStack != null)
            {
                var hStack = BaseElemsCreator.CreateHorizontalStackLayout();
                Application.Current?.Dispatcher.Dispatch(() =>
                {
                    AdminPageStatic.CalcViewWidth(out var width, out _);
                    vStack.MaximumWidthRequest = width;
                });
                page = new BaseUserUIPage([vStack], BaseUserUIPage.PageType.Profile);
            }
            else
            {
                page = new EmptyPage();
            }

            return page;
        }
    }
}
