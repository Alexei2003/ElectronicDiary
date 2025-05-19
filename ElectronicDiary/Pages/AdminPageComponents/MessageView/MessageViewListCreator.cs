using System.Text.Json;


using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.DTO.Requests.Social;
using ElectronicDiary.Web.DTO.Responses.Social;

namespace ElectronicDiary.Pages.AdminPageComponents.MessageView
{
    public class MessageViewListCreator : BaseViewListCreator<MessageResponse, MessageRequest, MessageController, MessageViewElemCreator, MessageViewObjectCreator>
    {
        public MessageViewListCreator() : base()
        {
            _maxCountViews = 2;
            _titleView = "Список сообщений";
            _addButtonName = "Отправить";
        }

        protected MessageRequest _baseRequest = new();
        protected Editor _messageEditor;
        protected override void CreateFilterUI(ref int rowIndex)
        {
            base.CreateFilterUI(ref rowIndex);

            _baseRequest.SenderUserId = UserData.UserInfo.UserId;
            _baseRequest.GetterUserId = _objectParentId;

            _messageEditor = BaseElemsCreator.CreateEditor(newText => _baseRequest.Message = newText, "текст");
            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [

                    new LineElemsCreator.Data
                    {
                        Elem = _messageEditor,
                        CountJoinColumns = 9
                    }
                ]
            );

        }


        protected override async Task GetList()
        {
            var response = await MessageController.GetAllByGetterId(UserData.UserInfo.UserId);
            var messageGetArr = new MessageResponse[0];
            if (!string.IsNullOrEmpty(response))
            {
                messageGetArr = JsonSerializer.Deserialize<MessageResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];
            }

            response = await MessageController.GetAllBySenderId(UserData.UserInfo.UserId);
            var messageSetArr = new MessageResponse[0];
            if (!string.IsNullOrEmpty(response))
            {
                messageSetArr = JsonSerializer.Deserialize<MessageResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];
            }

            MessageResponse[] messageArr = [.. messageGetArr, .. messageSetArr];
            messageArr = [.. messageSetArr.Where(m => m.GetterUserId == _objectParentId || m.SenderUserId == _objectParentId)];
            messageArr = [.. messageArr.OrderByDescending(m => m.DateTime)];

            _objectsArr = messageArr;
            FilterList();
        }

        protected override async void AddButtonClicked(object? sender, EventArgs e)
        {
            _baseRequest.DateTime = DateTime.UtcNow;
            if (string.IsNullOrEmpty(_baseRequest.Message) || _baseRequest.DateTime == null || _baseRequest.GetterUserId == -1 || _baseRequest.SenderUserId == -1)
            {
                return;
            }

            var response = await _controller.Add(_baseRequest);
            if (response != null)
            {
                ChageListHandler();
                _messageEditor.Text = "";
            }
        }
    }
}
