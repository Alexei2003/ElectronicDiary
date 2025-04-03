using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses;
using ElectronicDiary.Web.DTO.Responses.Educations;

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
                     new LineElemsCreator.Data
                     {
                         CountJoinColumns = 2,
                         Elem = BaseElemsCreator.CreateImage(_baseResponse.PathImage)
                     }
                ]);

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel( "Название")
                    },
                    _componentState == ComponentState.Read ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.Name)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEntry( newText =>  _baseRequest.Name = newText, "ГУО ...",_baseResponse.Name )
                        }
                ]
            );

            List<Item> settlementList = [];
            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Регион")
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Settlement ?.Region ?.Name)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateSearchPopupAsLabel(GetRegion(),
                                selectedIndex =>
                                {
                                    _baseRequest.RegionId = selectedIndex;
                                    if(_baseRequest.RegionId > -1)
                                    {
                                        GetSettlements(settlementList, _baseRequest.RegionId);
                                    }
                                }
                            )
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Населённый пункт")
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Settlement ?.Name)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateSearchPopupAsLabel(settlementList, selectedIndex => _baseRequest.SettlementId = selectedIndex)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Адресс")
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Address)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEntry(newText => _baseRequest.Address = newText, "ул. Ленина, 12",_baseResponse.Address )
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Email")
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Email)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEntry( newText => _baseRequest.Email = newText, "sh4@edus.by", _baseResponse.Email)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Телефон")
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.PhoneNumber)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEntry(newText => _baseRequest.PhoneNumber = newText, "+375 17 433-09-02", _baseResponse.PhoneNumber)
                        }
                ]
            );
        }

        protected static List<Item> GetRegion()
        {
            var list = new List<Item>();

            Task.Run(async () =>
            {
                TypeResponse[]? arr = null;
                var response = await AddressСontroller.GetRegions();
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, elem.Name));
                }
            });

            return list;
        }

        protected static void GetSettlements(List<Item> list, long regionId)
        {
            Task.Run(async () =>
            {
                TypeResponse[]? arr = null;
                var response = await AddressСontroller.GetSettlements(regionId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new Item(elem.Id, elem.Name));
                }
            });
        }
    }
}
