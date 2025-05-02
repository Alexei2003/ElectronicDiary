using System.Collections.ObjectModel;
using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
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
        protected virtual async void GestureTapped(object? sender, EventArgs e)
        {
            Delete(_educationalInstitutionId);
        }


        private static ObservableCollection<Item> GetParentTypes()
        {
            var list = new ObservableCollection<Item>();

            Task.Run(async () =>
            {
                TypeResponse[]? arr = null;
                var response = await StudentParentController.GetParentType();
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                Application.Current?.Dispatcher.Dispatch(() =>
                {
                    foreach (var elem in arr ?? [])
                    {
                        list.Add(item: new Item(elem.Id, elem.Name));
                    }
                });
            });

            return list;
        }

        private static List<Item> GetSchoolStudents(long _educationalInstitutionId)
        {
            var list = new List<Item>();

            Task.Run(async () =>
            {
                UserResponse[]? arr = null;
                var controller = new SchoolStudentController();
                var response = await controller.GetAll(_educationalInstitutionId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<UserResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, $"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}"));
                }
            });

            return list;
        }

        private List<Item> GetParents()
        {
            var list = new List<Item>();

            Task.Run(async () =>
            {
                UserResponse[]? arr = null;
                var response = await _controller.GetById(_educationalInstitutionId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<UserResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, $"{elem?.LastName} {elem?.FirstName} {elem?.Patronymic}"));
                }
            });

            return list;
        }
    }
}