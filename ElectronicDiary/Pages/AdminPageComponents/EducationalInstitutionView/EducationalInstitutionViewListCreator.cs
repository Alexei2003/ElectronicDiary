using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Pages.AdminPageComponents.EducationalInstitutionView
{
    class EducationalInstitutionViewListCreator<TResponse, TRequest, TController, TViewElemCreator, TViewObjectCreator> : BaseViewListCreator<TResponse, TRequest, TController, TViewElemCreator, TViewObjectCreator>
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

        protected string _regionFilter = "";
        protected string _settlementFilter = "";
        protected string _nameFilter = "";

        protected override void CreateFilterUI(ref int rowIndex)
        {
            base.CreateFilterUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Область",
                    },
                    new LineElemsCreator.EntryData{
                        Placeholder = "Минская область",
                        TextChangedAction = newText => _regionFilter = newText
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Населённый пункт",
                    },
                    new LineElemsCreator.EntryData{
                        Placeholder = "г.Солигорск",
                        TextChangedAction = newText => _settlementFilter = newText
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Название",
                    },
                    new LineElemsCreator.EntryData{
                        Placeholder = "ГУО ...",
                        TextChangedAction = newText => _nameFilter = newText
                    },
                ]
            );
        }

        protected override void FilterList()
        {
            bool isRegionFilterEmpty = string.IsNullOrEmpty(_regionFilter);
            bool isSettlementFilterEmpty = string.IsNullOrEmpty(_settlementFilter);
            bool isNameFilterEmpty = string.IsNullOrEmpty(_nameFilter);

            _objectsList = [.. _objectsList
                .Where(e =>
                    (isRegionFilterEmpty || (e.Settlement?.Region?.Name ?? "").Contains(_regionFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (isSettlementFilterEmpty || (e.Settlement?.Name ?? "").Contains(_settlementFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (isNameFilterEmpty || (e.Name ?? "").Contains(_nameFilter!, StringComparison.OrdinalIgnoreCase)))];
        }
    }
}
