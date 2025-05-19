using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.DTO.Requests.Social;
using ElectronicDiary.Web.DTO.Responses.Social;


namespace ElectronicDiary.Pages.AdminPageComponents.MessageView
{
    public class MessageViewElemCreator : BaseViewElemCreator<MessageResponse, MessageRequest, MessageController, MessageViewObjectCreator>
    {
        public MessageViewElemCreator()
        {
            _description = false;
            _edit = false;
            _moveTo = false;
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
                        Elem = BaseElemsCreator.CreateLabel($"{_baseResponse.SenderLastName} {_baseResponse.SenderFirstName} {_baseResponse.SenderPatronymic}"),
                        CountJoinColumns = 2,
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Message),
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
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.DateTime.ToString()),
                        CountJoinColumns = 2
                    },
                ]
            );
        }

        protected override async void GestureTapped(object? sender, EventArgs e)
        {
            Delete(_baseResponse.Id);
        }
    }
}
