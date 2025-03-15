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

            if (edit)
            {
                _request = new()
                {
                    Name = _response.Name,
                    Address = _response.Address,
                    Email = _response.Email,
                    PhoneNumber = _response.PhoneNumber,
                    RegionId = _response.Settlement.Region.Id,
                    SettlementId = _response.Settlement.Id
                };
            }

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

            object[]? settlementElems = null;
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
                        new LineElemsAdder.SearchData{
                            BaseItem =  _response?.Settlement.Region.Name,

                            IdChangedAction = selectedIndex => 
                            {
                                _request.RegionId = selectedIndex;

                                if(settlementElems != null &&
                                   settlementElems[^1] is TapGestureRecognizer searchSettlement &&
                                   _request.RegionId >= 0)
                                {
                                    Task.Run(async () =>
                                    {
                                        var settlementList = await GetSettlements(_request.RegionId);

                                        searchSettlement.BindingContext = settlementList;
                                    });

                                }
                            }
                        }
                ]
            );
            if (regionElems[^1] is TapGestureRecognizer searchRegion)
            {
                Task.Run(async () =>
                {
                    var regionList = await GetRegion();
                    searchRegion.BindingContext = regionList;
                });
            }

            settlementElems = LineElemsAdder.AddLineElems(
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
                        new LineElemsAdder.SearchData{
                            BaseItem =  _response?.Settlement.Name,
                            IdChangedAction = selectedIndex => _request.SettlementId = selectedIndex
                        }
                ]
            );

            if (settlementElems[^1] is TapGestureRecognizer searchSettlement)
            {
                Task.Run(async () =>
                {
                    var settlementList = await GetSettlements(_response.Settlement.Region.Id);

                    searchSettlement.BindingContext = settlementList;
                });
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
        private static async Task<List<LineElemsAdder.ItemPicker>> GetSettlements(long regionId)
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
