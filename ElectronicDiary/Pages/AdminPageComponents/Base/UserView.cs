using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.Base
{
    public class UserView<TController>
        : BaseView<BaseUserResponse, BaseUserRequest, TController>,
        IBaseView where TController : IController
    {
        public UserView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList,
            long educationalInstitutionId
        ) : base(mainStack, viewList)
        {
            _baseResponse = new();
            _baseRequest = new();
            _maxCountViews = 3;
            _educationalInstitutionId = educationalInstitutionId;
        }

        // Фильтр объектов
        private string _lastNameFilter = "";
        private string _firstNameFilter = "";
        private string _patronymicFilter = "";

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
                    (_lastNameFilter?.Length == 0 || (e.LastName ?? "").Contains(_lastNameFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_firstNameFilter?.Length == 0 || (e.FirstName ?? "").Contains(_firstNameFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_patronymicFilter?.Length == 0 || (e.Patronymic ?? "").Contains(_patronymicFilter ?? "", StringComparison.OrdinalIgnoreCase)))
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
                    FirstName = _baseResponse?.FirstName ?? "",
                    LastName = _baseResponse?.LastName ?? "",
                    Patronymic = _baseResponse?.Patronymic,
                    Email = _baseResponse?.Email ?? "",
                    PhoneNumber = _baseResponse?.PhoneNumber ?? "",
                    UniversityId = _baseResponse?.EducationalInstitution.Id ?? 0,

                    Login = "",
                    Password = ""
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
                            Title = _baseResponse?.LastName
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse?.LastName,
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
                            Title = _baseResponse?.FirstName
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse?.FirstName,
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
                            Title = _baseResponse?.Patronymic
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse?.Patronymic,
                            Placeholder = "Владимирович",
                            TextChangedAction = newText => { if(_baseRequest != null) _baseRequest.Patronymic = newText; }
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
                            Title = _baseResponse?.Email
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse?.Email,
                            Placeholder = "sh4@edus.by",
                            TextChangedAction = newText => { if(_baseRequest != null) _baseRequest.Email = newText; }
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
                            Title = _baseResponse?.PhoneNumber
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _baseResponse?.PhoneNumber,
                            Placeholder = "+375 17 433-09-02",
                            TextChangedAction = newText => { if(_baseRequest != null) _baseRequest.PhoneNumber = newText; }
                        }
                ]
            );
            if (_componentState == ComponentState.New || _componentState == ComponentState.Edit)
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
                            TextChangedAction = newText => { if(_baseRequest != null) _baseRequest.Login = newText; }
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
                            TextChangedAction = newText => {if(_baseRequest != null)_baseRequest.Password = newText; }
                        }
                    ]
                );


                if (_baseRequest != null) _baseRequest.UniversityId = _educationalInstitutionId;
            }
        }
    }

}
