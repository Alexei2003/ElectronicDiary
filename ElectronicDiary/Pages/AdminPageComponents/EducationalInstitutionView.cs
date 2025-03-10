using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests;
using ElectronicDiary.Web.DTO.Responses;
using System.Text.Json;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class EducationalInstitutionView : BaseView<EducationalInstitutionResponse, EducationalInstitutionRequest>
    {
        // Список школ
        private string _educationalInstitutionRegionFilter = "";
        private string _educationalInstitutionSettlementFilter = "";
        private string _educationalInstitutionNameFilter = "";

        public EducationalInstitutionView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList
        ) : base(mainStack, viewList)
        {
            _controller = new EducationalInstitutionСontroller();
            _request = new();
            _objectViewType = AdminPageStatic.ViewType.EducationalInstitution;
        }

        protected override void CreateFilterView(VerticalStackLayout verticalStack, Grid grid, int rowIndex = 0)
        {
            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Область",

                placeholder: "Минская область",
                textChangedAction: newText => _educationalInstitutionRegionFilter = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Населённый пункт",

                placeholder: "г.Солигорск",
                textChangedAction: newText => _educationalInstitutionSettlementFilter = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Название",

                placeholder: "ГУО ...",
                textChangedAction: newText => _educationalInstitutionNameFilter = newText
            );

            base.CreateFilterView(verticalStack, grid, rowIndex);
        }

        // Получение списка объектов
        protected override async Task CreateListView()
        {
            await base.CreateListView();

            _objectsList = _objectsList
                .Where(e =>
                    (_educationalInstitutionRegionFilter?.Length == 0 || e.Settlement.Region.Name.Contains(_educationalInstitutionRegionFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_educationalInstitutionSettlementFilter?.Length == 0 || e.Settlement.Name.Contains(_educationalInstitutionSettlementFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_educationalInstitutionNameFilter?.Length == 0 || e.Name.Contains(_educationalInstitutionNameFilter ?? "", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            _listVerticalStack.Clear();

            for (var i = 0; i < _objectsList.Count; i++)
            {
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += GestureTapped;
                var grid = new Grid
                {
                    // Положение
                    ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = GridLength.Auto },
                            new ColumnDefinition { Width = GridLength.Star }
                        },
                    Padding = PageConstants.PADDING_ALL_PAGES,
                    ColumnSpacing = PageConstants.SPACING_ALL_PAGES,
                    RowSpacing = PageConstants.SPACING_ALL_PAGES,

                    // Цвета
                    BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,

                    // Доп инфа
                    BindingContext = _objectsList[i].Id,
                };
                grid.GestureRecognizers.Add(tapGesture);

                var rowIndex = 0;

                AdminPageStatic.AddLineElems(
                    componentType: AdminPageStatic.ComponentType.Label,
                    grid: grid,
                    startColumn: 0,
                    startRow: rowIndex++,
                    title: "Название",

                    value: _objectsList[i].Name
                );

                AdminPageStatic.AddLineElems(
                    componentType: AdminPageStatic.ComponentType.Label,
                    grid: grid,
                    startColumn: 0,
                    startRow: rowIndex++,
                    title: "Регион",

                    value: _objectsList[i].Settlement.Region.Name
                );

                AdminPageStatic.AddLineElems(
                    componentType: AdminPageStatic.ComponentType.Label,
                    grid: grid,
                    startColumn: 0,
                    startRow: rowIndex++,
                    title: "Город",

                    value: _objectsList[i].Settlement.Name
                );

                _listVerticalStack.Add(grid);
            }
        }

        // Действия с отдельными объектами
        protected override void CreateObjectInfoView(VerticalStackLayout verticalStack, Grid grid, int rowIndex = 0, long id = -1, bool edit = false)
        {
            AdminPageStatic.ComponentType componentTypeEntity;
            AdminPageStatic.ComponentType componentTypePicker;
            EducationalInstitutionResponse? educationalInstitutionResponse = new();
            if (id == -1)
            {
                _request = new();
                componentTypeEntity = AdminPageStatic.ComponentType.Entity;
                componentTypePicker = AdminPageStatic.ComponentType.Picker;
            }
            else
            {
                educationalInstitutionResponse = _objectsList.FirstOrDefault(x => x.Id == id);
                componentTypeEntity = AdminPageStatic.ComponentType.Label;
                componentTypePicker = AdminPageStatic.ComponentType.Label;
            }

            AdminPageStatic.AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Название",

                value: educationalInstitutionResponse.Name,

                placeholder: "ГУО ...",
                textChangedAction: newText => _request.Name = newText
            );

            var objRegion = AdminPageStatic.AddLineElems(
                componentType: componentTypePicker,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Регион",

                value: educationalInstitutionResponse.Settlement.Region.Name,

                idChangedAction: selectedIndex => _request.RegionId = selectedIndex
            );
            if (componentTypePicker == AdminPageStatic.ComponentType.Picker)
            {
                Task.Run(async () =>
                {
                    var regionList = await GetRegion();
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        var pickerRegion = (Picker)objRegion;
                        pickerRegion.ItemsSource = regionList;
                    });
                });
            }

            var objSettlement = AdminPageStatic.AddLineElems(
                componentType: componentTypePicker,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Населённый пункт",

                value: educationalInstitutionResponse.Settlement.Name,

                idChangedAction: selectedwId => _request.SettlementId = selectedwId
            );
            if (componentTypePicker == AdminPageStatic.ComponentType.Picker)
            {
                var pickerRegion = (Picker)objRegion;
                pickerRegion.SelectedIndexChanged += async (sender, e) =>
                {
                    if (pickerRegion.SelectedItem is AdminPageStatic.ItemPicker selectedItem)
                    {
                        var pickerSettlement = (Picker)objSettlement;
                        var settlementList = await GetSettlements(selectedItem.Id);

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            pickerSettlement.ItemsSource = settlementList;
                        });
                    }
                };
            }

            AdminPageStatic.AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Адресс",

                value: educationalInstitutionResponse.Address,

                placeholder: "ул. Ленина, 12",
                textChangedAction: newText => _request.Address = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Email",

                value: educationalInstitutionResponse.Email,

                placeholder: "sh4@edus.by",
                textChangedAction: newText => _request.Email = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Телефон",

                value: educationalInstitutionResponse.PhoneNumber,

                placeholder: "+375 17 433-09-02",
                textChangedAction: newText => _request.PhoneNumber = newText
            );

            base.CreateObjectInfoView(verticalStack, grid, rowIndex, id, edit);
        }

        private async Task<List<AdminPageStatic.ItemPicker>> GetRegion()
        {
            List<AdminPageStatic.ItemPicker>? list = null;
            var response = await AddressСontroller.GetRegions();
            if (response != null)
            {
                list = JsonSerializer.Deserialize<List<AdminPageStatic.ItemPicker>>(response, PageConstants.JsonSerializerOptions);
            }
            return list ?? [];
        }
        private static async Task<List<AdminPageStatic.ItemPicker>> GetSettlements(int regionId)
        {
            List<AdminPageStatic.ItemPicker>? list = null;
            var response = await AddressСontroller.GetSettlements(regionId);
            if (response != null)
            {
                list = JsonSerializer.Deserialize<List<AdminPageStatic.ItemPicker>>(response, PageConstants.JsonSerializerOptions);
            }
            return list ?? [];
        }
    }
}
