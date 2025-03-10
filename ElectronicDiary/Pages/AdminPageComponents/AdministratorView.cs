using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests;
using ElectronicDiary.Web.DTO.Responses;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class AdministratorView : BaseView<AdministratorResponse, AdministratorRequest>
    {
        public AdministratorView(
            HorizontalStackLayout mainStack,
            List<ScrollView> viewList,
            long educationalInstitutionId
        ) : base(mainStack, viewList)
        {
            _controller = new AdministratorController();
            _request = new();
            _maxCountViews = 3;
            _educationalInstitutionId = educationalInstitutionId;
        }

        // Фильтр объектов
        private string _lastNameFilter = "";
        private string _firstNameFilter = "";
        private string _patronymicFilter = "";

        protected override void CreateFilterView(VerticalStackLayout verticalStack, Grid grid, int rowIndex = 0)
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

            base.CreateFilterView(verticalStack, grid, rowIndex);
        }

        // Получение списка объектов
        protected override async Task CreateListView()
        {
            await base.CreateListView();

            _objectsList = _objectsList
                .Where(e =>
                    (_lastNameFilter?.Length == 0 || e.LastName.Contains(_lastNameFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_firstNameFilter?.Length == 0 || e.FirstName.Contains(_firstNameFilter ?? "", StringComparison.OrdinalIgnoreCase)) &&
                    (_patronymicFilter?.Length == 0 || e.PathImage.Contains(_patronymicFilter ?? "", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            _listVerticalStack.Clear();

            for (var i = 0; i < _objectsList.Count; i++)
            {
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += GestureTapped;
                var grid = new Grid
                {
                    // Положение
                    ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = GridLength.Auto },
                            new ColumnDefinition { Width = GridLength.Star }
                        },
                    Padding = PageConstants.PADDING_ALL_PAGES,
                    ColumnSpacing = PageConstants.SPACING_ALL_PAGES,
                    RowSpacing = PageConstants.SPACING_ALL_PAGES,

                    // Цвета
                    BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,

                    // Доп инфа
                    BindingContext = _objectsList[i].Id,
                };
                grid.GestureRecognizers.Add(tapGesture);

                var rowIndex = 0;

                AdminPageStatic.AddLineElems(
                    componentType: AdminPageStatic.ComponentType.Label,
                    grid: grid,
                    startColumn: 0,
                    startRow: rowIndex++,
                    title: "Фамилия",

                    value: _objectsList[i].FirstName
                );

                AdminPageStatic.AddLineElems(
                    componentType: AdminPageStatic.ComponentType.Label,
                    grid: grid,
                    startColumn: 0,
                    startRow: rowIndex++,
                    title: "Имя",

                    value: _objectsList[i].LastName
                );

                AdminPageStatic.AddLineElems(
                    componentType: AdminPageStatic.ComponentType.Label,
                    grid: grid,
                    startColumn: 0,
                    startRow: rowIndex++,
                    title: "Отчество",

                    value: _objectsList[i].Patronymic
                );

                _listVerticalStack.Add(grid);
            }
        }

        // Действия с отдельными объектами
        protected override void CreateObjectInfoView(VerticalStackLayout verticalStack, Grid grid, int rowIndex = 0, long id = -1, bool edit = false)
        {
            AdminPageStatic.ComponentType componentTypeEntity;

            _request = new();
            _response = new();

            if (id == -1)
            {

                componentTypeEntity = AdminPageStatic.ComponentType.Entity;
            }
            else
            {
                _response = _objectsList.FirstOrDefault(x => x.Id == id) ?? new();
                componentTypeEntity = AdminPageStatic.ComponentType.Label;
            }

            AdminPageStatic.AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Фамилия",

                value: _response.FirstName,

                placeholder: "Дубовский",
                textChangedAction: newText => _request.FirstName = newText
            );


            AdminPageStatic.AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Имя",

                value: _response.LastName,

                placeholder: "Алексей",
                textChangedAction: newText => _request.LastName = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Отчество",

                value: _response.Patronymic,

                placeholder: "Владимирович",
                textChangedAction: newText => _request.Patronymic = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Email",

                value: _response.Email,

                placeholder: "sh4@edus.by",
                textChangedAction: newText => _request.Email = newText
            );

            AdminPageStatic.AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Телефон",

                value: _response.PhoneNumber,

                placeholder: "+375 17 433-09-02",
                textChangedAction: newText => _request.PhoneNumber = newText
            );

            if(id == -1 || edit)
            {
                AdminPageStatic.AddLineElems(
                    componentType: componentTypeEntity,
                    grid: grid,
                    startColumn: 0,
                    startRow: rowIndex++,
                    title: "Логин",

                    placeholder: "admin",
                    textChangedAction: newText => _request.Login = newText
                );

                AdminPageStatic.AddLineElems(
                    componentType: componentTypeEntity,
                    grid: grid,
                    startColumn: 0,
                    startRow: rowIndex++,
                    title: "Пароль",

                    placeholder: "4Af7@adf",
                    textChangedAction: newText => _request.Password = newText
                );

                _request.UniversityId = _educationalInstitutionId;
            }



            base.CreateObjectInfoView(verticalStack, grid, rowIndex, id, edit);
        }

    }
}
