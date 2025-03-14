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
            _response = new();
            _request = new();
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
                    (_lastNameFilter?.Length == 0 || e.LastName.Contains(_lastNameFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_firstNameFilter?.Length == 0 || e.FirstName.Contains(_firstNameFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_patronymicFilter?.Length == 0 || e.Patronymic.Contains(_patronymicFilter ?? "", StringComparison.OrdinalIgnoreCase)))
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
        protected override void CreateObjectInfoView(Grid grid, ref int rowIndex, bool edit = false)
        {
            base.CreateObjectInfoView(grid, ref rowIndex, edit);


            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Фамилия"
                    },
                    _componentConst ?
                        new LineElemsAdder.LabelData{
                            Title = _response?.FirstName
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _response?.FirstName,
                            Placeholder = "Дубовский",
                            TextChangedAction = newText => _request.FirstName = newText
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Имя",
                    },
                    _componentConst ?
                        new LineElemsAdder.LabelData{
                            Title = _response?.LastName
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _response?.LastName,
                            Placeholder = "Алексей",
                            TextChangedAction = newText => _request.LastName = newText
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Отчество"
                    },
                    _componentConst ?
                        new LineElemsAdder.LabelData{
                            Title = _response?.Patronymic
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _response?.Patronymic,
                            Placeholder = "Владимирович",
                            TextChangedAction = newText => _request.Patronymic = newText
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Email"
                    },
                    _componentConst ?
                        new LineElemsAdder.LabelData{
                            Title = _response?.Email
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _response?.Email,
                            Placeholder = "sh4@edus.by",
                            TextChangedAction = newText => _request.Email = newText
                        }
                ]
            );

            LineElemsAdder.AddLineElems(
                grid: grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsAdder.LabelData{
                        Title = "Телефон"
                    },
                    _componentConst ?
                        new LineElemsAdder.LabelData{
                            Title = _response?.PhoneNumber
                        }
                    :
                        new LineElemsAdder.EntryData{
                            BaseText = _response?.PhoneNumber,
                            Placeholder = "+375 17 433-09-02",
                            TextChangedAction = newText => _request.PhoneNumber = newText
                        }
                ]
            );
            if (_elemId == -1 || edit)
            {
                LineElemsAdder.AddLineElems(
                    grid: grid,
                    rowIndex: rowIndex++,
                    objectList:
                    _componentConst ?
                        [
                            new LineElemsAdder.LabelData{
                                Title = "Логин"
                            }
                        ]
                    :
                        [
                            new LineElemsAdder.LabelData{
                                Title = "Логин"
                            },
                            new LineElemsAdder.EntryData{
                                Placeholder = "admin",
                                TextChangedAction = newText => _request.Login = newText
                            }
                        ]
                );
                LineElemsAdder.AddLineElems(
                    grid: grid,
                    rowIndex: rowIndex++,
                    objectList:
                    _componentConst ?
                        [
                            new LineElemsAdder.LabelData {
                                Title = "Пароль"
                            }
                        ]
                    :
                        [
                            new LineElemsAdder.LabelData {
                                Title = "Пароль"
                            },
                            new LineElemsAdder.EntryData {
                                Placeholder = "4Af7@adf",
                                TextChangedAction = newText => _request.Password = newText
                            }
                        ]
                );


                _request.UniversityId = _educationalInstitutionId;
            }
        }
    }

}
