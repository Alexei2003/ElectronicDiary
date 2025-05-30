﻿using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.DTO.Requests.Social;
using ElectronicDiary.Web.DTO.Responses.Social;

namespace ElectronicDiary.UI.Views.Lists.NewView
{
    public class NewsViewListCreator : BaseViewListCreator<NewsResponse, NewsRequest, NewsController, NewViewElemCreator, NewViewObjectCreator>
    {
        public NewsViewListCreator() : base()
        {
            _maxCountViews = 3;
            _titleView = "Список новостей";
        }

        public NewsViewListCreator(int maxCountViews)
        {
            _maxCountViews = maxCountViews;
            _titleView = "Список новостей";
        }

        private string _titleFilter = string.Empty;
        protected override void CreateFilterUI(ref int rowIndex)
        {
            base.CreateFilterUI(ref rowIndex);

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
                        Elem = BaseElemsCreator.CreateEditor(newText => _titleFilter = newText, "Изменение порядка")
                    },
                ]
            );
        }

        protected override void FilterList()
        {
            bool titleFilter = string.IsNullOrEmpty(_titleFilter);

            _objectsArr = [.. _objectsArr
                .Where(e =>
                    !titleFilter || (e.Title ?? string.Empty).Contains(_titleFilter!, StringComparison.OrdinalIgnoreCase))];
        }
    }
}
