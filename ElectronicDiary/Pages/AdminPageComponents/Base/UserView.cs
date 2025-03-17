using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.Base
{
    public class UserView<TResponse, TRequest, TController>
        : BaseView<TResponse, TRequest, TController>, IBaseView
        where TResponse : BaseUserResponse, new()
        where TRequest : BaseUserRequest, new()
        where TController : IController, new()

    {
        public UserView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList,
            long educationalInstitutionId
        ) : base(mainStack, viewList)
        {
            _maxCountViews = 3;
            _educationalInstitutionId = educationalInstitutionId;
        }

        // Фильтр объектов
        private string _lastNameFilter = string.Empty;
        private string _firstNameFilter = string.Empty;
        private string _patronymicFilter = string.Empty;

        protected override void CreateFilterView(Grid grid, ref int rowIndex)
        {
            base.CreateFilterView(grid, ref rowIndex);

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Фамилия",
                    },
                    new LineElemsAdder.EntryData{
                        Placeholder = "Дубовский",
                        TextChangedAction = newText => _lastNameFilter = newText
                    },
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Имя",
                    },
                    new LineElemsAdder.EntryData{
                        Placeholder = "Алексей",
                        TextChangedAction = newText => _firstNameFilter = newText
                    },
                ]
            );


            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Отчество",
                    },
                    new LineElemsAdder.EntryData{
                        Placeholder = "Владимирович",
                        TextChangedAction = newText => _patronymicFilter = newText
                    },
                ]
            );
        }

        // Получение списка объектов
        protected override void FilterList()
        {
            _objectsList = _objectsList
                .Where(e =>
                    (_lastNameFilter?.Length == 0 || (e.LastName ?? string.Empty).Contains(_lastNameFilter ?? string.Empty, StringComparison.OrdinalIgnoreCase)) &&
                    (_firstNameFilter?.Length == 0 || (e.FirstName ?? string.Empty).Contains(_firstNameFilter ?? string.Empty, StringComparison.OrdinalIgnoreCase)) &&
                    (_patronymicFilter?.Length == 0 || (e.Patronymic ?? string.Empty).Contains(_patronymicFilter ?? string.Empty, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        protected override void CreateListElemView(Grid grid, ref int rowIndex, int indexElem)
        {
            base.CreateListElemView(grid, ref rowIndex, indexElem);

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Фамилия",
                    },
                    new LineElemsAdder.LabelData{
                        Title =  _objectsList[indexElem].LastName,
                    },
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Имя",
                    },
                    new LineElemsAdder.LabelData{
                        Title = _objectsList[indexElem].FirstName
                    },
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Отчество",
                    },
                    new LineElemsAdder.LabelData{
                        Title = _objectsList[indexElem].Patronymic
                    },
                ]
            );
        }

        // Действия с отдельными объектами
        protected override void CreateObjectInfoView(ref int rowIndex)
        {
            base.CreateObjectInfoView(ref rowIndex);

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

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Фамилия"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsAdder.LabelData{
                            Title = _baseResponse.LastName
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse.LastName,
                            Placeholder = "Дубовский",
                            TextChangedAction = newText => {if (_baseRequest != null)_baseRequest.LastName = newText; }
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Имя",
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsAdder.LabelData{
                            Title = _baseResponse.FirstName
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse.FirstName,
                            Placeholder = "Алексей",
                            TextChangedAction = newText => {if(_baseRequest!=null) _baseRequest.FirstName = newText; }
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Отчество"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsAdder.LabelData{
                            Title = _baseResponse.Patronymic
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse.Patronymic,
                            Placeholder = "Владимирович",
                            TextChangedAction = newText => _baseRequest.Patronymic = newText
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Email"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsAdder.LabelData{
                            Title = _baseResponse.Email
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse.Email,
                            Placeholder = "sh4@edus.by",
                            TextChangedAction = newText => _baseRequest.Email = newText
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: _objectGrid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Телефон"
                    },
                    _componentState == ComponentState.Read  ?
                        new LineElemsAdder.LabelData{
                            Title = _baseResponse.PhoneNumber
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse.PhoneNumber,
                            Placeholder = "+375 17 433-09-02",
                            TextChangedAction = newText => _baseRequest.PhoneNumber = newText
                        }
                ]
            );
            if (_componentState != ComponentState.Read)
            {
                LineElemsAdder.AddLineElems(
                    grid: _objectGrid,
                    rowIndex: rowIndex++,
                    objectList:
                    [
                        new LineElemsAdder.LabelData{
                            Title = "Логин"
                        },
                        new LineElemsAdder.EntryData{
                            Placeholder = "admin",
                            TextChangedAction = newText => _baseRequest.Login = newText
                        }
                    ]
                );
                LineElemsAdder.AddLineElems(
                    grid: _objectGrid,
                    rowIndex: rowIndex++,
                    objectList:
                    [
                        new LineElemsAdder.LabelData {
                            Title = "Пароль"
                        },
                        new LineElemsAdder.EntryData {
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
