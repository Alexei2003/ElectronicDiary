using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.ParentSchoolStudentView
{
    public class ParentStudentViewListCreator<TController, TViewElemCreator> : BaseViewListCreator<StudentParentResponse, StudentParentRequest, TController, TViewElemCreator, ParentStudentViewObjectCreator<TController>>
        where TController : IController, new()
        where TViewElemCreator : ParentStudentViewElemCreator<TController>, new()
    {
        public ParentStudentViewListCreator()
        {
            _maxCountViews = 2;
            _titleView = "Список ";
        }

        protected override void CreateFilterUI(ref int rowIndex)
        {
            CreateTitile(ref rowIndex);
        }

        protected override void CreateGetButton(VerticalStackLayout vStack)
        {

        }

        protected override void CreateColumnTitle(VerticalStackLayout vStack)
        {
            var grid = BaseElemsCreator.CreateGrid();

            LineElemsCreator.AddLineElems(
                grid,
                0,
                [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Тип")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("ФИО")
                    }
                ]
            );
            vStack.Add(grid);
        }

        protected virtual void AddButtonClicked(object? sender, EventArgs e)
        {

        }
    }
}
