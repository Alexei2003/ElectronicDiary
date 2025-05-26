using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.UI.Views.Lists.GroupView
{
    public class GroupViewListCreator : BaseViewListCreator<GroupResponse, GroupRequest, GroupController, GroupViewElemCreator, GroupViewObjectCreator>
    {
        public GroupViewListCreator() : base()
        {
            _maxCountViews = 4;
            _titleView = "Список групп";
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
                        Elem  = BaseElemsCreator.CreateEditor(newText => _nameFilter = newText, "Английский 2")
                    },
                ]
            );
        }

        // Получение списка объектов
        protected override void FilterList()
        {
            bool nameFilter = string.IsNullOrEmpty(_nameFilter);

            _objectsArr = [.. _objectsArr
                .Where(e =>
                    !nameFilter || (e.GroupName ?? string.Empty).Contains(_nameFilter!, StringComparison.OrdinalIgnoreCase))];
        }
    }
}
