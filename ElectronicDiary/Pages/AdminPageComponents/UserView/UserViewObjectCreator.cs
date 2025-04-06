using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

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

            if (_componentState == AdminPageStatic.ComponentState.Edit)
            {
                _baseRequest = new()
                {
                    FirstName = _baseResponse.FirstName ?? string.Empty,
                    LastName = _baseResponse.LastName ?? string.Empty,
                    Patronymic = _baseResponse.Patronymic,
                    Email = _baseResponse.Email ?? string.Empty,
                    PhoneNumber = _baseResponse.PhoneNumber ?? string.Empty,
                    UniversityId = _baseResponse.EducationalInstitution?.Id ?? 0,

                    Login = string.Empty,
                    Password = string.Empty
                };

            }

            LineElemsCreator.AddLineElems(
                   grid: _baseInfoGrid,
                   rowIndex: _baseInfoGridRowIndex++,
                   objectList: [
                        new LineElemsCreator.Data
                        {
                            CountJoinColumns = 2,
                            Elem = BaseElemsCreator.CreateImage(_baseResponse.PathImage),
                        }
                   ]);

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Фамилия")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.LastName),
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEntry(newText => _baseRequest.LastName = newText, "Дубовский", _baseResponse.LastName)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Имя")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.FirstName)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEntry(newText => _baseRequest.FirstName = newText, "Алексей", _baseResponse.FirstName)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Отчество")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Patronymic)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEntry(newText => _baseRequest.Patronymic = newText, "Владимирович", _baseResponse.Patronymic)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Email")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.Email)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEntry(newText => _baseRequest.Email = newText, "sh4@edus.by", _baseResponse.Email)
                        }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Телефон")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read  ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel(_baseResponse.PhoneNumber)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEntry( newText => _baseRequest.PhoneNumber = newText, "+375 17 433-09-02", _baseResponse.PhoneNumber)
                        }
                ]
            );
            if (_componentState != AdminPageStatic.ComponentState.Read)
            {
                LineElemsCreator.AddLineElems(
                    grid: _baseInfoGrid,
                    rowIndex: _baseInfoGridRowIndex++,
                    objectList:
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel("Логин")
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEntry(newText => _baseRequest.Login = newText, "admin")
                        }
                    ]
                );
                LineElemsCreator.AddLineElems(
                    grid: _baseInfoGrid,
                    rowIndex: _baseInfoGridRowIndex++,
                    objectList:
                    [
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel("Пароль")
                        },
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateEntry(newText => _baseRequest.Password = newText, "4Af7@adf")
                        }
                    ]
                );


                _baseRequest.UniversityId = _educationalInstitutionId;
            }
        }
    }
}
