using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Requests.Educations;
using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Pages.AdminPageComponents.EducationalInstitutionView
{
    public class EducationalInstitutionViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator> : BaseViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator>
        where TResponse : EducationalInstitutionResponse, new()
        where TRequest : EducationalInstitutionRequest, new()
        where TController : IController, new()
        where TViewObjectCreator : BaseViewObjectCreator<TResponse, TRequest, TController>, new()
    {

        protected override void CreateUI(ref int rowIndex)
        {
            base.CreateUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Название",
                    },
                    new LineElemsCreator.LabelData{
                        Title = _baseResponse.Name
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Регион",
                    },
                    new LineElemsCreator.LabelData{
                        Title = _baseResponse.Settlement?.Region?.Name
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Населённый пункт",
                    },
                    new LineElemsCreator.LabelData{
                        Title = _baseResponse.Settlement?.Name
                    },
                ]
            );
        }
    }
}
