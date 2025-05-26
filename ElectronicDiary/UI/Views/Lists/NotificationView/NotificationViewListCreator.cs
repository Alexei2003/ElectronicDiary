using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.DTO.Requests.Educations.Other;
using ElectronicDiary.Web.DTO.Responses.Social;

namespace ElectronicDiary.UI.Views.Lists.NotificationView
{
    public class NotificationViewListCreator : BaseViewListCreator<NotificationResponse, BaseRequest, NotificationController, NotificationViewElemCreator, NotificationViewObjectCreator>
    {
        public NotificationViewListCreator() : base()
        {
            _titleView = "Список уведомлений";
            _maxCountViews = 1;
        }

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
                        Elem = BaseElemsCreator.CreateLabel("Название"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateEditor(newText => _nameFilter = newText, "Выставлена оценка"),
                    },
                ]
            );
        }


        protected override void FilterList()
        {
            bool nameFilter = string.IsNullOrEmpty(_nameFilter);

            _objectsArr = [.. _objectsArr
                .Where(e =>
                    !nameFilter || (e.Title ?? string.Empty).Contains(_nameFilter!, StringComparison.OrdinalIgnoreCase))];
        }
    }
}
