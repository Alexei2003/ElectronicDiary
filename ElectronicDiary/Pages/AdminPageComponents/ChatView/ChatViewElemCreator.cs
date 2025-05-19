using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.AdminPageComponents.MessageView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.DTO.Requests.Social;
using ElectronicDiary.Web.DTO.Responses.Social;

namespace ElectronicDiary.Pages.AdminPageComponents.ChatView
{
    public class ChatViewElemCreator : BaseViewElemCreator<ChatResponse, ChatRequest, MessageController, ChatViewObjectCreator>
    {
        public ChatViewElemCreator() : base()
        {
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
                        Elem = BaseElemsCreator.CreateLabel($"{_baseResponse.User.LastName} {_baseResponse.User.FirstName} {_baseResponse.User.Patronymic}"),
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
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Messages[0].Message),
                        CountJoinColumns = 2
                    },
                ]
            );
        }

        protected override void ShowInfo(long id)
        {
            var baseViewObjectCreator = new MessageViewListCreator();
            var scrollView = baseViewObjectCreator.Create(_mainStack, _viewList, _baseResponse.Id);
            AdminPageStatic.DeleteLastView(_mainStack, _viewList, _maxCountViews);
            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }

        protected override async void Delete(long id)
        {
            foreach(var message in _baseResponse.Messages)
            {
                await _controller.Delete(message.Id);
            }
            ChangeList();
        }
    }
}
