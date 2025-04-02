using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses;
using ElectronicDiary.Web.DTO.Responses.Educations;

using System.Text.Json;

using static ElectronicDiary.Pages.AdminPageComponents.AdminPageStatic;

namespace ElectronicDiary.Pages.AdminPageComponents.EducationalInstitutionView
{
    public class EducationalInstitutionViewObjectCreator<TResponse, TRequest, TController> : BaseViewObjectCreator<TResponse, TRequest, TController>
        where TResponse : EducationalInstitutionResponse, new()
        where TRequest : EducationalInstitutionRequest, new()
        where TController : IController, new()
    {

        protected override void CreateUI()
        {
            base.CreateUI();

            if (_componentState == ComponentState.Edit)
            {
                _baseRequest = new()
                {
                    Name = _baseResponse.Name ?? string.Empty,
                    Address = _baseResponse.Address ?? string.Empty,
                    Email = _baseResponse.Email,
                    PhoneNumber = _baseResponse.PhoneNumber,
                    RegionId = _baseResponse.Settlement?.Region?.Id ?? 0,
                    SettlementId = _baseResponse.Settlement?.Id ?? 0
                };
            }

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                     new LineElemsCreator.ImageData{
                         PathImage = _baseResponse.PathImage,
                     }
                ]);

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Название"
                    },
                    _componentState == ComponentState.Read ?
                        new LineElemsCreator.LabelData{
                            Title = _baseResponse.Name
                        }
                    :
                        new LineElemsCreator.EntryData{
                            BaseText = _baseResponse.Name,
                            Placeholder = "ГУО ...",
                            TextChangedAction = newText => { _baseRequest.Name = newText; }
                        }
                ]
            );

            object[]? settlementElems = null;
            var regionElems = LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Регион"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.LabelData{
                            Title = _baseResponse.Settlement?.Region?.Name
                        }
                    :
                        new LineElemsCreator.SearchData{
                            BaseItem =  new TypeResponse(){
                                Id = _baseResponse.Settlement?.Region?.Id ?? 0,
                                Name = _baseResponse.Settlement?.Region?.Name ?? "Найти"
                            },
                            IdChangedAction = selectedIndex =>
                            {
                                _baseRequest.RegionId = selectedIndex;
                                if(settlementElems != null &&
                                   settlementElems[^1] is TapGestureRecognizer searchSettlement &&
                                   _baseRequest.RegionId != null)
                                {
                                    Task.Run(async () =>
                                    {
                                        var settlementList = await GetSettlements(_baseRequest.RegionId.Value);

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

            settlementElems = LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Населённый пункт"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.LabelData{
                            Title = _baseResponse.Settlement?.Name
                        }
                    :
                        new LineElemsCreator.SearchData{
                            BaseItem =  new TypeResponse(){
                                Id = _baseResponse.Settlement?.Id ?? 0,
                                Name = _baseResponse.Settlement?.Name ?? "Найти"
                            },
                            IdChangedAction = selectedIndex => { _baseRequest.SettlementId = selectedIndex;}
                        }
                ]
            );

            if (settlementElems[^1] is TapGestureRecognizer searchSettlement)
            {
                Task.Run(async () =>
                {
                    var settlementList = await GetSettlements(_baseResponse.Settlement?.Region?.Id ?? 0);

                    searchSettlement.BindingContext = settlementList;
                });
            }

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Адресс"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.LabelData{
                            Title = _baseResponse.Address
                        }
                    :
                        new LineElemsCreator.EntryData{
                            BaseText = _baseResponse.Address,
                            Placeholder = "ул. Ленина, 12",
                            TextChangedAction = newText => _baseRequest.Address = newText
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Email"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.LabelData{
                            Title = _baseResponse.Email
                        }
                    :
                        new LineElemsCreator.EntryData{
                            BaseText = _baseResponse.Email,
                            Placeholder = "sh4@edus.by",
                            TextChangedAction = newText => _baseRequest.Email = newText
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Телефон"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.LabelData{
                            Title = _baseResponse.PhoneNumber
                        }
                    :
                        new LineElemsCreator.EntryData{
                            BaseText = _baseResponse.PhoneNumber,
                            Placeholder = "+375 17 433-09-02",
                            TextChangedAction = newText => _baseRequest.PhoneNumber = newText
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
