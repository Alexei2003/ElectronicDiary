using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

using static ElectronicDiary.Pages.AdminPageComponents.AdminPageStatic;

namespace ElectronicDiary.Pages.AdminPageComponents.UserView
{
    public class UserViewObjectCreator<TResponse, TRequest, TController> : BaseViewObjectCreator<TResponse, TRequest, TController>
        where TResponse : UserResponse, new()
        where TRequest : UserRequest, new()
        where TController : IController, new()
    {
        protected override void CreateUI()
        {
            base.CreateUI();

            if (_componentState == ComponentState.Edit)
            {
                _baseRequest = new()
                {
                    FirstName = _baseResponse.FirstName ?? string.Empty,
                    LastName = _baseResponse.LastName ?? string.Empty,
                    Patronymic = _baseResponse.Patronymic,
                    Email = _baseResponse.Email ?? string.Empty,
                    PhoneNumber = _baseResponse.PhoneNumber ?? string.Empty,
                    UniversityId = _baseResponse.EducationalInstitution.Id ?? 0,

                    Login = string.Empty,
                    Password = string.Empty
                };

            }

            LineElemsCreator.AddLineElems(
                   grid: _baseInfoGrid,
                   rowIndex: _baseInfoGridRowIndex++,
                   objectList: [
                        new LineElemsCreator.ImageData{
                            PathImage = _baseResponse.PathImage,
                        }
                   ]);

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Фамилия"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.LabelData{
                            Title = _baseResponse.LastName
                        }
                    :
                        new LineElemsCreator.EntryData{
                            BaseText = _baseResponse.LastName,
                            Placeholder = "Дубовский",
                            TextChangedAction = newText => {if (_baseRequest != null)_baseRequest.LastName = newText; }
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Имя",
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.LabelData{
                            Title = _baseResponse.FirstName
                        }
                    :
                        new LineElemsCreator.EntryData{
                            BaseText = _baseResponse.FirstName,
                            Placeholder = "Алексей",
                            TextChangedAction = newText => {if(_baseRequest!=null) _baseRequest.FirstName = newText; }
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Отчество"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.LabelData{
                            Title = _baseResponse.Patronymic
                        }
                    :
                        new LineElemsCreator.EntryData{
                            BaseText = _baseResponse.Patronymic,
                            Placeholder = "Владимирович",
                            TextChangedAction = newText => _baseRequest.Patronymic = newText
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Email"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.LabelData{
                            Title = _baseResponse.Email
                        }
                    :
                        new LineElemsCreator.EntryData{
                            BaseText = _baseResponse.Email,
                            Placeholder = "sh4@edus.by",
                            TextChangedAction = newText => _baseRequest.Email = newText
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Телефон"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsCreator.LabelData{
                            Title = _baseResponse.PhoneNumber
                        }
                    :
                        new LineElemsCreator.EntryData{
                            BaseText = _baseResponse.PhoneNumber,
                            Placeholder = "+375 17 433-09-02",
                            TextChangedAction = newText => _baseRequest.PhoneNumber = newText
                        }
                ]
            );
            if (_componentState != ComponentState.Read)
            {
                LineElemsCreator.AddLineElems(
                    grid: _baseInfoGrid,
                    rowIndex: _baseInfoGridRowIndex++,
                    objectList:
                    [
                        new LineElemsCreator.LabelData{
                            Title = "Логин"
                        },
                        new LineElemsCreator.EntryData{
                            Placeholder = "admin",
                            TextChangedAction = newText => _baseRequest.Login = newText
                        }
                    ]
                );
                LineElemsCreator.AddLineElems(
                    grid: _baseInfoGrid,
                    rowIndex: _baseInfoGridRowIndex++,
                    objectList:
                    [
                        new LineElemsCreator.LabelData {
                            Title = "Пароль"
                        },
                        new LineElemsCreator.EntryData {
                            Placeholder = "4Af7@adf",
                            TextChangedAction = newText => _baseRequest.Password = newText
                        }
                    ]
                );


                _baseRequest.UniversityId = _educationalInstitutionId;
            }
        }
    }
}
