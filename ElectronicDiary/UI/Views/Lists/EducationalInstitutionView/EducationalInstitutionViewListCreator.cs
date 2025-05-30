﻿using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.UI.Views.Lists.EducationalInstitutionView
{
    public sealed class EducationalInstitutionViewListCreator : BaseViewListCreator<EducationalInstitutionResponse, EducationalInstitutionRequest, EducationalInstitutionСontroller, EducationalInstitutionViewElemCreator, EducationalInstitutionViewObjectCreator>
    {
        public EducationalInstitutionViewListCreator() : base()
        {
            _maxCountViews = 2;
            _titleView = "Список учебных заведений";
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
                        Elem  = BaseElemsCreator.CreateEditor(newText => _regionFilter = newText, "Минская область")
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
                        Elem  = BaseElemsCreator.CreateEditor(newText => _settlementFilter = newText, "г.Солигорск")
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
                        Elem  = BaseElemsCreator.CreateEditor(newText => _nameFilter = newText, "ГУО ...")
                    },
                ]
            );
        }

        // Получение списка объектов
        protected override void FilterList()
        {
            if (_objectParentId == -1)
            {
                bool regionFilter = string.IsNullOrEmpty(_regionFilter);
                bool settlementFilter = string.IsNullOrEmpty(_settlementFilter);
                bool nameFilter = string.IsNullOrEmpty(_nameFilter);

                _objectsArr = [.. _objectsArr
                .Where(e =>
                    (!regionFilter || (e.Settlement?.Region?.Name ?? string.Empty).Contains(_regionFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (!settlementFilter || (e.Settlement?.Name ?? string.Empty).Contains(_settlementFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (!nameFilter || (e.Name ?? string.Empty).Contains(_nameFilter!, StringComparison.OrdinalIgnoreCase)))];
            }
            else
            {
                _objectsArr = [.. _objectsArr.Where(e => e.Id == _objectParentId)];
            }

        }
    }
}
