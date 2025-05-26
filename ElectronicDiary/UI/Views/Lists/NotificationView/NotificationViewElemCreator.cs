using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.DTO.Requests.Educations.Other;
using ElectronicDiary.Web.DTO.Responses.Social;

namespace ElectronicDiary.UI.Views.Lists.NotificationView
{
    public class NotificationViewElemCreator : BaseViewElemCreator<NotificationResponse, BaseRequest, NotificationController, NotificationViewObjectCreator>
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
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Content),
                        CountJoinColumns = 2
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.DateTime.ToString())
                    },
                ]
            );
        }
        protected override void GestureTapped(object? sender, EventArgs e)
        {
            Delete(_baseResponse.Id);
        }
    }
}
