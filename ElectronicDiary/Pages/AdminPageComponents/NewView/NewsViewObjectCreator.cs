using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Social;
using ElectronicDiary.Web.DTO.Requests.Social;
using ElectronicDiary.Web.DTO.Responses.Social;

namespace ElectronicDiary.Pages.AdminPageComponents.NewsView
{
    public class NewsViewObjectCreator : BaseViewObjectCreator<NewsResponse, NewsRequest, NewsController>
    {
        protected override void CreateUI()
        {
            base.CreateUI();

            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                _baseRequest.OwnerUserId = _baseResponse.UserId;
                _baseRequest.Title = _baseResponse.Title;
                _baseRequest.Content = _baseResponse.Content;
                _baseRequest.DateTime = _baseResponse.DateTime;
            }
            else
            {
                _baseRequest.OwnerUserId = UserData.UserInfo.Id;
                _baseRequest.DateTime = DateTime.UtcNow;
            }

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            CountJoinColumns = 2,
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.Title),
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            CountJoinColumns = 2,
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.Title = newText, "Изменение порядка", _baseResponse.Title)
                        }
                ]
            );

            if (_componentState == AdminPageStatic.ComponentState.Read)
            {
                LineElemsCreator.AddLineElems(
                    grid: _baseInfoGrid,
                    rowIndex: _baseInfoGridRowIndex++,
                    objectList: [
                        new LineElemsCreator.Data
                        {
                            CountJoinColumns = 2,
                            Elem = BaseElemsCreator.CreateLabel( $"{_baseResponse.LastName} {_baseResponse.FirstName} {_baseResponse.Patronymic}" ),
                        }
                    ]
                );
            }

            if (_componentState == AdminPageStatic.ComponentState.Read)
            {
                LineElemsCreator.AddLineElems(
                    grid: _baseInfoGrid,
                    rowIndex: _baseInfoGridRowIndex++,
                    objectList: [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel( $"{_baseResponse.DateTime}" ),
                        }
                    ]
                );
            }

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            CountJoinColumns = 2,
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.Content),
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            CountJoinColumns = 2,
                            Elem = BaseElemsCreator.CreateEditor(newText => _baseRequest.Content = newText, "Изменение порядка ...", _baseResponse.Content)
                        }
                ]
            );

            _baseRequest.EducationalInstitutionId = _educationalInstitutionId;
        }
    }
}
