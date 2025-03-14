using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
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

        protected override void CreateFilterView(Grid grid, int rowIndex = 0)
        {
            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Фамилия",

                placeholder: "Дубовский",
                textChangedAction: newText => _lastNameFilter = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Имя",

                placeholder: "Алексей",
                textChangedAction: newText => _firstNameFilter = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Отчество",

                placeholder: "Владимирович",
                textChangedAction: newText => _patronymicFilter = newText
            );
        }

        // Получение списка объектов
        protected override void FilterList()
        {
            _objectsList = _objectsList
                .Where(e =>
                    (_lastNameFilter?.Length == 0 || e.LastName.Contains(_lastNameFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_firstNameFilter?.Length == 0 || e.FirstName.Contains(_firstNameFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_patronymicFilter?.Length == 0 || e.PathImage.Contains(_patronymicFilter ?? "", StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        protected override void CreateListElemView(Grid grid, int indexElem, int rowIndex = 0)
        {
            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Label,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Фамилия",

                value: _objectsList[indexElem].FirstName
            );

            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Label,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Имя",

                value: _objectsList[indexElem].LastName
            );

            AdminPageStatic.AddLineElems(
                componentType: AdminPageStatic.ComponentType.Label,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Отчество",

                value: _objectsList[indexElem].Patronymic
            );
        }

        // Действия с отдельными объектами
        protected override void CreateObjectInfoView(Grid grid, int rowIndex = 0, bool edit = false)
        {
            base.CreateObjectInfoView(grid, edit: edit);

            AdminPageStatic.AddLineElems(
                    componentType: _componentTypeEntity,
                    grid: grid,
                    startColumn: 0,
                    startRow: rowIndex++,
                    title: "Фамилия",

                    value: _response.FirstName,

                    placeholder: "Дубовский",
                    textChangedAction: newText => _request.FirstName = newText
                );

            AdminPageStatic.AddLineElems(
                componentType: _componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Имя",

                value: _response.LastName,

                placeholder: "Алексей",
                textChangedAction: newText => _request.LastName = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: _componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Отчество",

                value: _response.Patronymic,

                placeholder: "Владимирович",
                textChangedAction: newText => _request.Patronymic = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: _componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Email",

                value: _response.Email,

                placeholder: "sh4@edus.by",
                textChangedAction: newText => _request.Email = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: _componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Телефон",

                value: _response.PhoneNumber,

                placeholder: "+375 17 433-09-02",
                textChangedAction: newText => _request.PhoneNumber = newText
            );

            if (_elemId == -1 || edit)
            {
                AdminPageStatic.AddLineElems(
                    componentType: _componentTypeEntity,
                    grid: grid,
                    startColumn: 0,
                    startRow: rowIndex++,
                    title: "Логин",

                    placeholder: "admin",
                    textChangedAction: newText => _request.Login = newText
                );

                AdminPageStatic.AddLineElems(
                    componentType: _componentTypeEntity,
                    grid: grid,
                    startColumn: 0,
                    startRow: rowIndex++,
                    title: "Пароль",

                    placeholder: "4Af7@adf",
                    textChangedAction: newText => _request.Password = newText
                );

                _request.UniversityId = _educationalInstitutionId;
            }

            base.CreateObjectInfoView(grid, rowIndex, edit);
        }
    }

}
