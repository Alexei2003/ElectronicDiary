using ElectronicDiary.Pages.AdminPageComponents.Base;
using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;
using System.Text.Json;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class EducationalInstitutionView
        : BaseView<EducationalInstitutionResponse, EducationalInstitutionRequest, EducationalInstitutionСontroller>
    {
        public EducationalInstitutionView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList
        ) : base(mainStack, viewList)
        {
            _response = new();
            _request = new();
            _controller = new();
            _maxCountViews = 2;
        }


        // Фильтр объектов
        private string _regionFilter = "";
        private string _settlementFilter = "";
        private string _nameFilter = "";

        protected override void CreateFilterView(Grid grid, int rowIndex = 0)
        {
            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Область",

                placeholder: "Минская область",
                textChangedAction: newText => _regionFilter = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Населённый пункт",

                placeholder: "г.Солигорск",
                textChangedAction: newText => _settlementFilter = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Название",

                placeholder: "ГУО ...",
                textChangedAction: newText => _nameFilter = newText
            );

            base.CreateFilterView(grid, rowIndex);
        }

        // Получение списка объектов
        protected override void FilterList()
        {
            _objectsList = _objectsList
                .Where(e =>
                    (_regionFilter?.Length == 0 || e.Settlement.Region.Name.Contains(_regionFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_settlementFilter?.Length == 0 || e.Settlement.Name.Contains(_settlementFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_nameFilter?.Length == 0 || e.Name.Contains(_nameFilter ?? "", StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        protected override void CreateListElemView(Grid grid, int indexElem, int rowIndex = 0)
        {
            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Label,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Название",

                value: _objectsList[indexElem].Name
            );

            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Label,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Регион",

                value: _objectsList[indexElem].Settlement.Region.Name
            );

            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Label,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Город",

                value: _objectsList[indexElem].Settlement.Name
            );
        }

        // Действия с отдельными объектами
        protected override void CreateObjectInfoView(Grid grid, int rowIndex = 0, bool edit = false)
        {
            base.CreateObjectInfoView(grid, edit: edit);

            AdminPageStatic.AddLineElems(
                componentType: _componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Название",

                value: _response.Name,

                placeholder: "ГУО ...",
                textChangedAction: newText => _request.Name = newText
            );

            var objRegion = AdminPageStatic.AddLineElems(
                componentType: _componentTypePicker,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Регион",

                value: _response.Settlement.Region.Name,

                idChangedAction: selectedIndex => _request.RegionId = selectedIndex
            );
            if (_componentTypePicker == AdminPageStatic.ComponentType.Picker)
            {
                Task.Run(async() =>
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
                componentType: _componentTypePicker,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Населённый пункт",

                value: _response.Settlement.Name,

                idChangedAction: selectedwId => _request.SettlementId = selectedwId
            );
            if (_componentTypePicker == AdminPageStatic.ComponentType.Picker)
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
                componentType: _componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Адресс",

                value: _response.Address,

                placeholder: "ул. Ленина, 12",
                textChangedAction: newText => _request.Address = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: _componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Email",

                value: _response.Email,

                placeholder: "sh4@edus.by",
                textChangedAction: newText => _request.Email = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: _componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Телефон",

                value: _response.PhoneNumber,

                placeholder: "+375 17 433-09-02",
                textChangedAction: newText => _request.PhoneNumber = newText
            );

            base.CreateObjectInfoView(grid, rowIndex, edit);
        }

        private static async Task<List<AdminPageStatic.ItemPicker>> GetRegion()
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
