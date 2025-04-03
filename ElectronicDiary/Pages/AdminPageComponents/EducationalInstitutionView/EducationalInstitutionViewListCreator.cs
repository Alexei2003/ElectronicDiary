using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Pages.AdminPageComponents.EducationalInstitutionView
{
    public sealed class EducationalInstitutionViewListCreator<TResponse, TRequest, TController, TViewElemCreator, TViewObjectCreator> : BaseViewListCreator<TResponse, TRequest, TController, TViewElemCreator, TViewObjectCreator>
        where TResponse : EducationalInstitutionResponse, new()
        where TRequest : EducationalInstitutionRequest, new()
        where TController : IController, new()
        where TViewElemCreator : BaseViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator>, new()
        where TViewObjectCreator : BaseViewObjectCreator<TResponse, TRequest, TController>, new()
    {
        public EducationalInstitutionViewListCreator()
        {
            _maxCountViews = 2;
        }

        private string _regionFilter = string.Empty;
        private string _settlementFilter = string.Empty;
        private string _nameFilter = string.Empty;

        protected override void CreateFilterUI(ref int rowIndex)
        {
            base.CreateFilterUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Область")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem  = BaseElemsCreator.CreateEntry(newText => _regionFilter = newText, "Минская область")
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Населённый пункт")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem  = BaseElemsCreator.CreateEntry(newText => _settlementFilter = newText, "г.Солигорск")
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Название"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem  = BaseElemsCreator.CreateEntry(newText => _nameFilter = newText, "ГУО ...")
                    },
                ]
            );
        }

        // Получение списка объектов
        protected override void FilterList()
        {
            bool isRegionFilterEmpty = string.IsNullOrEmpty(_regionFilter);
            bool isSettlementFilterEmpty = string.IsNullOrEmpty(_settlementFilter);
            bool isNameFilterEmpty = string.IsNullOrEmpty(_nameFilter);

            _objectsArr = [.. _objectsArr
                .Where(e =>
                    (isRegionFilterEmpty || (e.Settlement?.Region?.Name ?? string.Empty).Contains(_regionFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (isSettlementFilterEmpty || (e.Settlement?.Name ?? string.Empty).Contains(_settlementFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (isNameFilterEmpty || (e.Name ?? string.Empty).Contains(_nameFilter!, StringComparison.OrdinalIgnoreCase)))];
        }
    }
}
