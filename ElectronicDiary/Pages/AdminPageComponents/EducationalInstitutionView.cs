using ElectronicDiary.Pages.AdminPageComponents.Base;
using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses;
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
            _baseResponse = new();
            _baseRequest = new();
            _controller = new();
            _maxCountViews = 2;
        }


        // Фильтр объектов
        protected string _regionFilter = "";
        protected string _settlementFilter = "";
        protected string _nameFilter = "";

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
        protected override void CreateObjectInfoView(ref int rowIndex)
        {
            base.CreateObjectInfoView(ref rowIndex);

            if (_componentState == ComponentState.Edit)
            {
                _baseRequest = new()
                {
                    Name = _baseResponse?.Name ?? "",
                    Address = _baseResponse?.Address ?? "",
                    Email = _baseResponse?.Email,
                    PhoneNumber = _baseResponse?.PhoneNumber,
                    RegionId = _baseResponse?.Settlement?.Region?.Id ?? 0,
                    SettlementId = _baseResponse?.Settlement?.Id ?? 0
                };
            }

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Название"
                    },
                    _componentState == ComponentState.Read ?
                        new LineElemsAdder.LabelData{
                            Title = _baseResponse?.Name
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse?.Name,
                            Placeholder = "ГУО ...",
                            TextChangedAction = newText => { if (_baseRequest != null) _baseRequest.Name = newText; }
                        }
                ]
            );

            object[]? settlementElems = null;
            var regionElems = LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Регион"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsAdder.LabelData{
                            Title = _baseResponse?.Settlement.Region.Name
                        }
                    :
                        new LineElemsAdder.SearchData{
                            BaseItem =  new TypeResponse(){
                                Id = _baseResponse?.Settlement?.Region?.Id ?? 0,
                                Name = _baseResponse?.Settlement?.Region?.Name ?? "Найти"
                            },
                            IdChangedAction = selectedIndex =>
                            {
                                if (_baseRequest != null) _baseRequest.RegionId = selectedIndex;
                                if(settlementElems != null &&
                                   settlementElems[^1] is TapGestureRecognizer searchSettlement &&
                                   _baseRequest?.RegionId >= 0)
                                {
                                    Task.Run(async () =>
                                    {
                                        var settlementList = await GetSettlements(_baseRequest.RegionId);

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
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Населённый пункт"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsAdder.LabelData{
                            Title = _baseResponse?.Settlement.Name
                        }
                    :
                        new LineElemsAdder.SearchData{
                            BaseItem =  new TypeResponse(){
                                Id = _baseResponse?.Settlement?.Id ?? 0,
                                Name = _baseResponse?.Settlement?.Name ?? "Найти"
                            },
                            IdChangedAction = selectedIndex => {if (_baseRequest != null)  _baseRequest.SettlementId = selectedIndex;}
                        }
                ]
            );

            if (settlementElems[^1] is TapGestureRecognizer searchSettlement)
            {
                Task.Run(async () =>
                {
                    var settlementList = await GetSettlements(_baseResponse?.Settlement?.Region?.Id ?? 0);

                    searchSettlement.BindingContext = settlementList;
                });
            }

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Адресс"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsAdder.LabelData{
                            Title = _baseResponse?.Address
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse?.Address,
                            Placeholder = "ул. Ленина, 12",
                            TextChangedAction = newText => { if(_baseRequest != null) _baseRequest.Address = newText; }
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Email"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsAdder.LabelData{
                            Title = _baseResponse?.Email
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse?.Email,
                            Placeholder = "sh4@edus.by",
                            TextChangedAction = newText => { if(_baseRequest != null) _baseRequest.Email = newText; }
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Телефон"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsAdder.LabelData{
                            Title = _baseResponse?.PhoneNumber
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse?.PhoneNumber,
                            Placeholder = "+375 17 433-09-02",
                            TextChangedAction = newText => { if(_baseRequest != null) _baseRequest.PhoneNumber = newText; }
                        }
                ]
            );
        }

        protected static async Task<List<TypeResponse>> GetRegion()
        {
            List<TypeResponse>? list = null;
            var response = await AddressСontroller.GetRegions();
            if (!string.IsNullOrEmpty(response)) list = JsonSerializer.Deserialize<List<TypeResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];
            return list ?? [];
        }
        protected static async Task<List<TypeResponse>> GetSettlements(long regionId)
        {
            List<TypeResponse>? list = null;
            var response = await AddressСontroller.GetSettlements(regionId);
            if (!string.IsNullOrEmpty(response)) list = JsonSerializer.Deserialize<List<TypeResponse>>(response, PageConstants.JsonSerializerOptions) ?? [];

            return list ?? [];
        }
    }
}
