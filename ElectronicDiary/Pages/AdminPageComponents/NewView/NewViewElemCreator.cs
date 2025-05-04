using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.DTO.Requests.Social;
using ElectronicDiary.Web.DTO.Responses.Social;

namespace ElectronicDiary.Pages.AdminPageComponents.NewsView
{
    public class NewViewElemCreator : BaseViewElemCreator<NewsResponse, NewsRequest, NewsController, NewViewObjectCreator>
    {
        protected override void CreateUI(ref int rowIndex)
        {
            base.CreateUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        CountJoinColumns = 2,
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Title),
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        CountJoinColumns = 2,
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Content, 100)
                    },
                ]
            );
        }
    }
}
