using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.UI.Views.Lists.ClassView
{
    public class ClassViewListCreator : BaseViewListCreator<ClassResponse, ClassRequest, ClassController, ClassViewElemCreator, ClassViewObjectCreator>
    {
        public ClassViewListCreator() : base()
        {
            _maxCountViews = 3;
            _titleView = "Список классов";
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
                        Elem = BaseElemsCreator.CreateLabel("Название")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem  = BaseElemsCreator.CreateEditor(newText => _nameFilter = newText, "7А")
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
                    !nameFilter || (e.Name ?? string.Empty).Contains(_nameFilter!, StringComparison.OrdinalIgnoreCase))];
        }
    }
}
