using System.Text.Json;

using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.UI.Views.Lists.General;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.UI.Views.Lists.EducationalInstitutionView
{
    public class EducationalInstitutionViewObjectCreator : BaseViewObjectCreator<EducationalInstitutionResponse, EducationalInstitutionRequest, EducationalInstitutionСontroller>
    {
        private VerticalStackLayout? _imageStack = null;
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

            _imageStack = BaseElemsCreator.CreateImageFromUrl(_baseResponse.PathImage,
                _componentState == AdminPageStatic.ComponentState.Edit ? AddImageTapped : null);
            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                     new LineElemsCreator.Data
                     {
                         CountJoinColumns = 2,
                         Elem = _imageStack
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
                            Elem = BaseElemsCreator.CreateEditor( newText =>  _baseRequest.Name = newText, "ГУО ...",_baseResponse.Name )
                        }
                ]
            );

            List<TypeResponse> settlementList = [];
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
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Settlement?.Region?.Name)
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
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.Address = newText, "ул. Ленина, 12",_baseResponse.Address )
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
                            Elem = BaseElemsCreator.CreateEditor( newText => _baseRequest.Email = newText, "sh4@edus.by", _baseResponse.Email)
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
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.PhoneNumber = newText, "+375 17 433-09-02", _baseResponse.PhoneNumber)
                        }
                ]
            );
        }

        private Stream? _image_stream = null;
        private FileResult? _imageFile = null;
        private async void AddImageTapped(object? sender, EventArgs e)
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите изображение",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null) return;
            _imageFile = result;

            if (_imageStack != null)
            {
                _image_stream?.Dispose();
                _image_stream = await result.OpenReadAsync();
                Image image = (Image)_imageStack[^1];
                image.Source = ImageSource.FromStream(() => _image_stream);
            }
        }

        protected override void SaveButtonClicked(object? sender, EventArgs e)
        {
            base.SaveButtonClicked(sender, e);

            if (_imageFile != null)
            {
                _controller.AddImage(_baseResponse.Id, _imageFile);
            }
        }

        private static List<TypeResponse> GetRegions()
        {
            var list = new List<TypeResponse>();

            Task.Run(async () =>
            {
                TypeResponse[]? arr = null;
                var response = await AddressСontroller.GetRegions();
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new TypeResponse(elem.Id, elem.Name));
                }
            });

            return list;
        }

        private static void GetSettlements(List<TypeResponse> list, long regionId)
        {
            list.Clear();
            Task.Run(async () =>
            {
                TypeResponse[]? arr = null;
                var response = await AddressСontroller.GetSettlements(regionId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<TypeResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new TypeResponse(elem.Id, elem.Name));
                }
            });
        }
    }
}
