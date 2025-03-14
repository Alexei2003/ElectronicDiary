using ElectronicDiary.Pages.AdminPageComponents.Base;
using ElectronicDiary.Pages.Otherts;
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

        protected override void CreateFilterView(Grid grid, ref int rowIndex)
        {
            base.CreateFilterView(grid, ref rowIndex);

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Область",
                    },
                    new LineElemsAdder.EntryData{
                        Placeholder = "Минская область",
                        TextChangedAction = newText => _regionFilter = newText
                    },
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Населённый пункт",
                    },
                    new LineElemsAdder.EntryData{
                        Placeholder = "г.Солигорск",
                        TextChangedAction = newText => _settlementFilter = newText
                    },
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Название",
                    },
                    new LineElemsAdder.EntryData{
                        Placeholder = "ГУО ...",
                        TextChangedAction = newText => _nameFilter = newText
                    },
                ]
            );
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
        protected override void CreateListElemView(Grid grid, ref int rowIndex, int indexElem)
        {
            base.CreateListElemView(grid, ref rowIndex, indexElem);

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Название",
                    },
                    new LineElemsAdder.LabelData{
                        Title = _objectsList[indexElem].Name
                    },
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Регион",
                    },
                    new LineElemsAdder.LabelData{
                        Title = _objectsList[indexElem].Settlement.Region.Name
                    },
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Населённый пункт",
                    },
                    new LineElemsAdder.LabelData{
                        Title = _objectsList[indexElem].Settlement.Name
                    },
                ]
            );
        }

        // Действия с отдельными объектами
        protected override void CreateObjectInfoView(Grid grid, ref int rowIndex, bool edit = false)
        {
            base.CreateObjectInfoView(grid, ref rowIndex, edit);

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Название"
                    },
                    _componentConst ?
                        new LineElemsAdder.LabelData{
                            Title = _response?.Name
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _response?.Name,
                            Placeholder = "ГУО ...",
                            TextChangedAction = newText => _request.Name = newText
                        }
                ]
            );

            var regionElems = LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Регион"
                    },
                    _componentConst ?
                        new LineElemsAdder.LabelData{
                            Title = _response?.Settlement.Region.Name
                        }
                    :
                        new LineElemsAdder.PickerData{
                            BaseSelectedId =  _response?.Settlement.Region.Id,
                            IdChangedAction = selectedIndex => _request.RegionId = selectedIndex
                        }
                ]
            );

            var settlementElems = LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Населённый пункт"
                    },
                    _componentConst ?
                        new LineElemsAdder.LabelData{
                            Title = _response?.Settlement.Name
                        }
                    :
                        new LineElemsAdder.PickerData{
                            BaseSelectedId = _response?.Settlement.Id,
                            IdChangedAction = selectedIndex => _request.SettlementId = selectedIndex
                        }
                ]
            );

            if (regionElems[^1] is Picker pickerRegion)
            {
                Task.Run(async () =>
                {
                    var regionList = await GetRegion();
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        pickerRegion.ItemsSource = regionList;
                    });
                });

                pickerRegion.SelectedIndexChanged += async (sender, e) =>
                {
                    if (pickerRegion.SelectedItem is LineElemsAdder.ItemPicker selectedItem  &&
                        settlementElems[^1] is Picker pickerSettlement)
                    {
                        var settlementList = await GetSettlements(selectedItem.Id);

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            pickerSettlement.ItemsSource = settlementList;
                        });
                    }
                };
            }

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Адресс"
                    },
                    _componentConst ?
                        new LineElemsAdder.LabelData{
                            Title = _response?.Address
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _response?.Address,
                            Placeholder = "ул. Ленина, 12",
                            TextChangedAction = newText => _request.Address = newText
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Email"
                    },
                    _componentConst ?
                        new LineElemsAdder.LabelData{
                            Title = _response?.Email
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _response?.Email,
                            Placeholder = "sh4@edus.by",
                            TextChangedAction = newText => _request.Email = newText
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Телефон"
                    },
                    _componentConst ?
                        new LineElemsAdder.LabelData{
                            Title = _response?.PhoneNumber
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _response?.PhoneNumber,
                            Placeholder = "+375 17 433-09-02",
                            TextChangedAction = newText => _request.PhoneNumber = newText
                        }
                ]
            );
        }

        private static async Task<List<LineElemsAdder.ItemPicker>> GetRegion()
        {
            List<LineElemsAdder.ItemPicker>? list = null;
            var response = await AddressСontroller.GetRegions();
            if (response != null)
            {
                list = JsonSerializer.Deserialize<List<LineElemsAdder.ItemPicker>>(response, PageConstants.JsonSerializerOptions);
            }
            return list ?? [];
        }
        private static async Task<List<LineElemsAdder.ItemPicker>> GetSettlements(int regionId)
        {
            List<LineElemsAdder.ItemPicker>? list = null;
            var response = await AddressСontroller.GetSettlements(regionId);
            if (response != null)
            {
                list = JsonSerializer.Deserialize<List<LineElemsAdder.ItemPicker>>(response, PageConstants.JsonSerializerOptions);
            }
            return list ?? [];
        }
    }
}
