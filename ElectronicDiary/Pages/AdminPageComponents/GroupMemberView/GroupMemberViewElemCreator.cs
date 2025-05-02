using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Other;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Pages.AdminPageComponents.GroupMemberView
{
    public class GroupMemberViewElemCreator : BaseViewElemCreator<TypeResponse, BaseRequest, GroupInfoController, GroupMemberViewObjectCreator>
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
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse.Name),
                        CountJoinColumns = 2
                    },
                ]
            );
        }

        protected override async void GestureTapped(object? sender, EventArgs e)
        {
            Delete(_baseResponse.Id);
        }

        protected override async void Delete(long id)
        {
            var page = Application.Current?.Windows[0].Page;
            if (page != null)
            {
                var accept = await page.DisplayAlert("Подтверждение", "Удаление объекта", "Да", "Нет");

                if (accept && _controller != null)
                {
                    await _controller.Delete(id, _objetParentId);
                    ChangeList();
                }
            }
        }
    }
}
