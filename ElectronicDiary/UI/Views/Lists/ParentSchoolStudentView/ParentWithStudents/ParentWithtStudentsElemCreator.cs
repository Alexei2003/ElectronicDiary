using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.Web.Api.Users;

namespace ElectronicDiary.UI.Views.Lists.ParentSchoolStudentView.ParentWithStudents
{
    public class ParentWithtStudentsElemCreator : ParentStudentViewElemCreator<ParentWithStudentsController>
    {
        protected override void CreateUI(ref int rowIndex)
        {
            base.CreateUI(ref rowIndex);

            var user = _baseResponse?.SchoolStudent;
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
