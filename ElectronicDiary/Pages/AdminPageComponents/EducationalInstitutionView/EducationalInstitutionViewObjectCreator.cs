using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;

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

            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                _baseRequest.Name = _baseResponse.Name;
                _baseRequest.Address = _baseResponse.Address;
                _baseRequest.Email = _baseResponse.Email;
                _baseRequest.PhoneNumber = _baseResponse.PhoneNumber;
                _baseRequest.RegionId = _baseResponse.Settlement?.Region?.Id ?? -1;
                _baseRequest.SettlementId = _baseResponse.Settlement?.Id ?? -1;
            }

            var image = BaseElemsCreator.CreateImage(_baseResponse.PathImage);
            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += AddImageTapped;
                image.GestureRecognizers.Add(tapGesture);
            }
            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                     new LineElemsCreator.Data
                     {
                         CountJoinColumns = 2,
                         Elem = image
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
                    _componentState == AdminPageStatic.ComponentState.Read ?
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
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Settlement ?.Region ?.Name)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateSearchPopupAsLabel(GetRegions(),
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
                    _componentState == AdminPageStatic.ComponentState.Read  ?
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
                    _componentState == AdminPageStatic.ComponentState.Read  ?
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
                    _componentState == AdminPageStatic.ComponentState.Read  ?
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
                    _componentState == AdminPageStatic.ComponentState.Read  ?
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

        private async void AddImageTapped(object? sender, EventArgs e)
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите изображение",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null) return;

            await _controller.AddImage(_baseResponse.Id, result);
        }

        private static List<Item> GetRegions()
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

        private static void GetSettlements(List<Item> list, long regionId)
        {
            list.Clear();
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
