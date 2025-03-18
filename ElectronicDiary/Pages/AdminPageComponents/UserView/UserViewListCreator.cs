using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.UserView
{
    public class UserViewListCreator<TResponse, TRequest, TController, TViewElemCreator, TViewObjectCreator> : BaseViewListCreator<TResponse, TRequest, TController, TViewElemCreator, TViewObjectCreator>
        where TResponse : UserResponse, new()
        where TRequest : UserRequest, new()
        where TController : IController, new()
        where TViewElemCreator : BaseViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator>, new()
        where TViewObjectCreator : BaseViewObjectCreator<TResponse, TRequest, TController>, new()
    {
        public UserViewListCreator()
        {
            _maxCountViews = 3;
        }

        private string _lastNameFilter = string.Empty;
        private string _firstNameFilter = string.Empty;
        private string _patronymicFilter = string.Empty;

        protected override void CreateFilterUI(ref int rowIndex)
        {
            base.CreateFilterUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Фамилия",
                    },
                    new LineElemsCreator.EntryData{
                        Placeholder = "Дубовский",
                        TextChangedAction = newText => _lastNameFilter = newText
                    },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Имя",
                    },
                    new LineElemsCreator.EntryData{
                        Placeholder = "Алексей",
                        TextChangedAction = newText => _firstNameFilter = newText
                    },
                ]
            );


            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                        Title = "Отчество",
                    },
                    new LineElemsCreator.EntryData{
                        Placeholder = "Владимирович",
                        TextChangedAction = newText => _patronymicFilter = newText
                    },
                ]
            );
        }

        // Получение списка объектов
        protected override void FilterList()
        {
            bool filterByLastName = string.IsNullOrEmpty(_lastNameFilter);
            bool filterByFirstName = string.IsNullOrEmpty(_firstNameFilter);
            bool filterByPatronymic = string.IsNullOrEmpty(_patronymicFilter);

            _objectsList = [.. _objectsList
                .Where(e =>
                    (!filterByLastName || (e.LastName ?? string.Empty).Contains(_lastNameFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (!filterByFirstName || (e.FirstName ?? string.Empty).Contains(_firstNameFilter!, StringComparison.OrdinalIgnoreCase)) &&
                    (!filterByPatronymic || (e.Patronymic ?? string.Empty).Contains(_patronymicFilter!, StringComparison.OrdinalIgnoreCase)))];
        }
    }
}
