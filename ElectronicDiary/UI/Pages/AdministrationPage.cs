using System.Text.Json;

using ElectronicDiary.SaveData.Static;
using ElectronicDiary.UI.Components.Elems;  // Здесь твои элементы
using ElectronicDiary.UI.Components.Navigation;
using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;

using Microsoft.Maui.Controls;

namespace ElectronicDiary.UI.Pages
{
    public class AdministrationPage : ContentPage
    {
        private long _id = -1;
        public AdministrationPage()
        {
            Task.Run(async () =>
            {
                var response = await ReportController.FindReportsByEducationalId(UserData.UserInfo.EducationId);
                if (!string.IsNullOrEmpty(response))
                {
                    var arr = JsonSerializer.Deserialize<BaseResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];
                    _id = arr[0].Id;
                }
            });

            Title = "Страница администрации";
            ToolbarItemsAdder.AddNotifications(ToolbarItems);
            ToolbarItemsAdder.AddSettings(ToolbarItems);
            ToolbarItemsAdder.AddLogOut(ToolbarItems);
            BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;

            var mainLayout = BaseElemsCreator.CreateVerticalStackLayout();

            var createReportButton = BaseElemsCreator.CreateButton("Создать отчет", async (s, e) =>
            {
                var result = await ReportController.CreateReport(_id);
                await DisplayAlert("Создание отчета", result ?? "Ошибка", "OK");
            });
            mainLayout.Add(createReportButton);

            var downloadReportButton = BaseElemsCreator.CreateButton("Скачать отчет", async (s, e) =>
            {
                var response = await ReportController.DownloadReport(_id);
                if (response.IsSuccessStatusCode)
                {
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    await DisplayAlert("Отчет", $"Получено {bytes.Length} байт", "OK");
                }
                else
                {
                    await DisplayAlert("Ошибка", "Не удалось скачать отчет", "OK");
                }
            });
            mainLayout.Add(downloadReportButton);

            var deleteReportButton = BaseElemsCreator.CreateButton("Удалить отчет", async (s, e) =>
            {
                var result = await ReportController.DeleteReportById(_id);
                await DisplayAlert("Удаление отчета", result ?? "Ошибка", "OK");
            });
            mainLayout.Add(deleteReportButton);

            Content = new ScrollView { Content = mainLayout };
        }
    }
}
