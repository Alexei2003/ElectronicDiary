using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.UI.Views.Lists.General;
using ElectronicDiary.UI.Views.Lists.GroupView;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.UI.Views.Lists.ClassView
{
    public class ClassViewElemCreator : BaseViewElemCreator<ClassResponse, ClassRequest, ClassController, ClassViewObjectCreator>
    {
        public ClassViewElemCreator()
        {
            _moveTo = true;
            _moveToName = "Переход к группам";
        }

        protected override void CreateUI(ref int rowIndex)
        {
            base.CreateUI(ref rowIndex);

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
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Name)
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Классный руководитель")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel($"{_baseResponse.Teacher?.LastName} {_baseResponse.Teacher?.FirstName} {_baseResponse.Teacher?.Patronymic}")
                    },
                ]
            );
        }

        protected override void MoveTo(long id)
        {
            var viewCreator = new GroupViewListCreator();

            AdminPageStatic.DeleteLastView(_mainStack, _viewList, _maxCountViews);
            var scrollView = viewCreator.Create(_mainStack, _viewList, _baseResponse.Id);

            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }
    }
}
