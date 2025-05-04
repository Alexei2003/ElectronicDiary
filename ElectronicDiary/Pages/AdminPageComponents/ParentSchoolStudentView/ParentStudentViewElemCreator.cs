using System.Collections.ObjectModel;
using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.ParentSchoolStudentView
{
    public class ParentStudentViewElemCreator<TController> : BaseViewElemCreator<StudentParentResponse, StudentParentRequest, TController, ParentStudentViewObjectCreator<TController>>
        where TController : IController, new()
    {
        protected override void GestureTapped(object? sender, EventArgs e)
        {
            Delete(_objetParentId);
        }


        private static ObservableCollection<TypeResponse> GetParentTypes()
        {
            var list = new ObservableCollection<TypeResponse>();

            Task.Run(async () =>
            {
                Web.DTO.Responses.Other.TypeResponse[]? arr = null;
                var response = await StudentParentController.GetParentType();
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<Web.DTO.Responses.Other.TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                Application.Current?.Dispatcher.Dispatch(() =>
                {
                    foreach (var elem in arr ?? [])
                    {
                        list.Add(item: new TypeResponse(elem.Id, elem.Name));
                    }
                });
            });

            return list;
        }

        private static List<TypeResponse> GetSchoolStudents(long _objetParentId)
        {
            var list = new List<TypeResponse>();

            Task.Run(async () =>
            {
                UserResponse[]? arr = null;
                var controller = new SchoolStudentController();
                var response = await controller.GetAll(_objetParentId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<UserResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new TypeResponse(elem.Id, $"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}"));
                }
            });

            return list;
        }

        private List<TypeResponse> GetParents()
        {
            var list = new List<TypeResponse>();

            Task.Run(async () =>
            {
                UserResponse[]? arr = null;
                var response = await _controller.GetById(_objetParentId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<UserResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new TypeResponse(elem.Id, $"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}"));
                }
            });

            return list;
        }
    }
}