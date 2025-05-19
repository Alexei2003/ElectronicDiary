using System.Text.Json;

using CommunityToolkit.Maui.Views;

using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Social;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Social;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.ChatView
{
    public class ChatViewListCreator : BaseViewListCreator<ChatResponse, ChatRequest, MessageController, ChatViewElemCreator, ChatViewObjectCreator>
    {
        public ChatViewListCreator() : base()
        {
            _maxCountViews = 2;
            _titleView = "Список чатов";
        }

        private string _lastNameFilter = string.Empty;
        private string _firstNameFilter = string.Empty;
        private string _patronymicFilter = string.Empty;

        protected override void CreateFilterUI(ref int rowIndex)
        {
            base.CreateFilterUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Фамилия"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateEditor(newText => _lastNameFilter = newText, "Дубовский")
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Имя"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateEditor(newText => _firstNameFilter = newText, "Алексей")
                    },
                ]
            );


            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Отчество"),
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateEditor(newText => _patronymicFilter = newText,  "Владимирович")
                    },
                ]
            );
        }

        protected override async Task GetList()
        {
            var response = await MessageController.GetAllByGetterId(_objectParentId);
            var messageGetArr = new MessageResponse[0];
            if (!string.IsNullOrEmpty(response))
            {
                messageGetArr = JsonSerializer.Deserialize<MessageResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];
            }

            response = await MessageController.GetAllBySenderId(_objectParentId);
            var messageSetArr = new MessageResponse[0];
            if (!string.IsNullOrEmpty(response))
            {
                messageSetArr = JsonSerializer.Deserialize<MessageResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];
            }

            MessageResponse[] messageArr = [.. messageGetArr, .. messageSetArr];
            messageArr = [.. messageArr.OrderByDescending(m => m.DateTime)];

            var objectList = new List<ChatResponse>();

            foreach (var message in messageArr)
            {
                var chats = objectList.Where(ch => ch.User.Id == message.GetterUserId || ch.User.Id == message.SenderUserId);
                if (!chats.Any())
                {
                    objectList.Add(new ChatResponse()
                    {
                        Messages = [message],
                        User = message.GetterUserId != UserData.UserInfo.UserId ?
                        new UserResponse()
                        {
                            Id = message.GetterUserId,
                            LastName = message.GetterLastName,
                            FirstName = message.GetterFirstName,
                            Patronymic = message.GetterPatronymic,
                        } :
                        new UserResponse()
                        {
                            Id = message.SenderUserId,
                            LastName = message.SenderLastName,
                            FirstName = message.SenderFirstName,
                            Patronymic = message.SenderPatronymic,
                        },
                        Id = message.GetterUserId != UserData.UserInfo.UserId ?
                        message.GetterUserId :
                        message.SenderUserId

                    });
                }
                else
                {
                    chats.First().Messages.Add(message);
                }
            }

            _objectsArr = [.. objectList];

            _listUsers = GetUsers();
            FilterList();
        }

        protected override void FilterList()
        {
            bool lastNameFilter = string.IsNullOrEmpty(_lastNameFilter);
            bool firstNameFilter = string.IsNullOrEmpty(_firstNameFilter);
            bool patronymicFilter = string.IsNullOrEmpty(_patronymicFilter);

            _objectsArr = [.. _objectsArr
                .Where(e =>
                    (!lastNameFilter || (e.User.LastName ?? string.Empty).Contains(_lastNameFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (!firstNameFilter || (e.User.FirstName ?? string.Empty).Contains(_firstNameFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (!patronymicFilter || (e.User.Patronymic ?? string.Empty).Contains(_patronymicFilter!, StringComparison.OrdinalIgnoreCase)))];
        }

        private List<TypeResponse> _listUsers = new();
        protected override async void AddButtonClicked(object? sender, EventArgs e)
        {
            var studentsList = new TypeResponse();
            long id = -1;

            var popup = new SearchPopup(_listUsers, newText => id = newText);
            var page = Application.Current?.Windows[0].Page;
            page?.ShowPopup(popup);

            popup.Closed += (sender, e) =>
            {
                if (popup.AllItems.Count > 0 && id > -1)
                {
                    Task.Run(async () =>
                    {
                        await _controller.Add(new MessageRequest()
                        {
                            DateTime = DateTime.UtcNow,
                            Message = "Начало чата",
                            SenderUserId = UserData.UserInfo.UserId,
                            GetterUserId = id
                        });
                        ChangeList();
                    });
                }
                _grid.Focus();
            };
        }

        private static List<TypeResponse> GetUsers()
        {
            var list = new List<TypeResponse>();

            Task.Run(async () =>
            {
                await GetPartUsers(list, new TeacherController());
                await GetPartUsers(list, new AdministratorController());
                await GetPartUsers(list, new SchoolStudentController());
                await GetPartUsers(list, new ParentController());
            });

            return list;
        }

        private static async Task GetPartUsers(List<TypeResponse> list, IController controller)
        {
            UserResponse[]? arr = null;
            var response = await controller.GetAll(UserData.UserInfo.EducationId);
            if (!string.IsNullOrEmpty(response))
            {
                arr = JsonSerializer.Deserialize<UserResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new TypeResponse(elem.Userid, $"{elem.LastName} {elem.FirstName} {elem.Patronymic}"));
                }
            }
        }
    }
}
