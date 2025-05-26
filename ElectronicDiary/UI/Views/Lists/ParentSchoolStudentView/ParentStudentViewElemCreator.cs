using System.Collections.ObjectModel;
using System.Text.Json;

using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.UI.Views.Lists.ParentSchoolStudentView
{
    public class ParentStudentViewElemCreator<TController> : BaseViewElemCreator<StudentParentResponse, StudentParentRequest, TController, ParentStudentViewObjectCreator<TController>>
        where TController : IController, new()
    {
        protected override void GestureTapped(object? sender, EventArgs e)
        {
            Delete(_objectParentId);
        }


        private static ObservableCollection<TypeResponse> GetParentTypes()
        {
            var list = new ObservableCollection<TypeResponse>();

            Task.Run(async () =>
            {
                TypeResponse[]? arr = null;
                var response = await StudentParentController.GetParentType();
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

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

        private static List<TypeResponse> GetSchoolStudents(long _objectParentId)
        {
            var list = new List<TypeResponse>();

            Task.Run(async () =>
            {
                UserResponse[]? arr = null;
                var controller = new SchoolStudentController();
                var response = await controller.GetAll(_objectParentId);
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
                var response = await _controller.GetById(_objectParentId);
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