using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Views.Lists.BaseView;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests.Educations.Other;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.UI.Views.Lists.GroupMemberView
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

        protected override void GestureTapped(object? sender, EventArgs e)
        {
            if (!_readOnly)
            {
                Delete(_baseResponse.Id);
            }
        }

        protected override async void Delete(long id)
        {
            var page = Application.Current?.Windows[0].Page;
            if (page != null)
            {
                var accept = await page.DisplayAlert("Подтверждение", "Удаление объекта", "Да", "Нет");

                if (accept && _controller != null)
                {
                    await _controller.Delete(id, _objectParentId);
                    ChangeList();
                }
            }
        }
    }
}
