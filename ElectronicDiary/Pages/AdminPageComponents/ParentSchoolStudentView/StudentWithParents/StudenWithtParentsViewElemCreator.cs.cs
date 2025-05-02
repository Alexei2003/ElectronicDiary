using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.ParentSchoolStudentView.StudentWithParents
{
    public class StudenWithtParentsViewElemCreator : ParentStudentViewElemCreator<StudentWithParentsController>
    {
        protected override void CreateUI(ref int rowIndex)
        {
            base.CreateUI(ref rowIndex);

            var user = _baseResponse?.Parent;
            var label = BaseElemsCreator.CreateLabel($"{user?.LastName} {user?.FirstName} {user?.Patronymic}");

            LineElemsCreator.AddLineElems(
                _grid,
                rowIndex++,
                [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel(_baseResponse?.ParentType?.Name)
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = label
                    }
                ]
            );
        }
    }
}
