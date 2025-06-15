using System.Text.Json;

using CommunityToolkit.Maui.Extensions;

using ElectronicDiary.Pages.Others;
using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Educations.Other;
using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.UI.Views.Lists.GroupMemberView
{
    public class GroupMemberViewListCreator : BaseViewListCreator<TypeResponse, BaseRequest, GroupInfoController, GroupMemberViewElemCreator, GroupMemberViewObjectCreator>
    {
        public GroupMemberViewListCreator() : base()
        {
            _maxCountViews = 4;
            _titleView = "Список учеников";
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
                        Elem  = BaseElemsCreator.CreateEditor(newText => _nameFilter = newText, "ФИО")
                    },
                ]
            );
        }

        protected override async Task GetList()
        {
            if (_objectParentId != -1)
            {
                var response = await _controller.GetAll(_objectParentId);
                if (!string.IsNullOrEmpty(response))
                {
                    var tmp = JsonSerializer.Deserialize<GroupInfoResponse>(response, PageConstants.JsonSerializerOptions);
                    _objectsArr = tmp?.GroupMembers?.Select(m => new TypeResponse(
                        m.SchoolStudent?.Id ?? -1,
                        $"{m.SchoolStudent?.LastName} {m.SchoolStudent?.FirstName} {m.SchoolStudent?.Patronymic}"
                    )).ToArray();
                }
                FilterList();
            }
        }

        // Получение списка объектов
        protected override void FilterList()
        {
            bool nameFilter = string.IsNullOrEmpty(_nameFilter);

            _objectsArr = [.. _objectsArr
                .Where(e =>
                    !nameFilter || (e.Name ?? string.Empty).Contains(_nameFilter!, StringComparison.OrdinalIgnoreCase))];
        }

        protected override async void AddButtonClicked(object? sender, EventArgs e)
        {
            var studentsList = new TypeResponse();
            long id = -1;
            var popup = new SearchPopup(await GetStudentsFromClass(), newText => id = newText);
            var page = Application.Current?.Windows[0].Page;
            page?.ShowPopup(popup);

            popup.Closed += (sender, e) =>
            {
                if (popup.AllItems.Count > 0 && id > -1)
                {
                    Task.Run(async () =>
                    {
                        await _controller.Add(id, _objectParentId);
                        ChangeList();
                    });
                }
                _grid.Focus();
            };
        }

        private async Task<List<TypeResponse>> GetStudentsFromClass()
        {
            var list = new List<TypeResponse>();

            var response = await SchoolStudentController.GetStudentsOfClass(_objectPreParentId);
            if (!string.IsNullOrEmpty(response))
            {
                var studentArr = JsonSerializer.Deserialize<SchoolStudentResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var student in studentArr)
                {
                    list.Add(new TypeResponse(student.Id, $"{student?.LastName} {student?.FirstName} {student?.Patronymic}"));
                }
            }

            return list;
        }
    }
}
