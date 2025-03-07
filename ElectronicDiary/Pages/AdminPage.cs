﻿using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO.Requests;
using ElectronicDiary.Web.DTO.Responses;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Text.Json;

namespace ElectronicDiary.Pages
{
    public class AdminPage : ContentPage
    {
        private readonly HorizontalStackLayout _mainStack;
        private readonly List<ScrollView> _viewList = [];
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AdminPage()
        {
            Title = "Панель администратора";
            ToolbarItemsAdder.AddSettings(ToolbarItems);
            ToolbarItemsAdder.AddLogOut(ToolbarItems);

            // Цвета
            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_PAGE_COLOR;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            _viewList.Add(CreateEducationalInstitutionListView());

            _mainStack = new HorizontalStackLayout
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Fill,
            };

            RepaintPage();

            Content = _mainStack;
            SizeChanged += WindowSizeChanged;

            GetEducationalInstitutionList();
        }

        // Обновление видов
        private void RepaintPage()
        {
            _mainStack.Clear();

            double dpi = DeviceDisplay.MainDisplayInfo.Density * 160;

#if WINDOWS
            double coeff = 1;
            double width = Width;
#else
            double coeff = 2;
            double width = DeviceDisplay.MainDisplayInfo.Width;
#endif

            var countColumn = int.Min((int)(width / dpi / 2), 3);

            for (var i = int.Max(_viewList.Count - countColumn, 0); i < _viewList.Count; i++)
            {
                _viewList[i].MaximumWidthRequest = (width / coeff / countColumn) * 0.90;
                _mainStack.Add(_viewList[i]);
            }
        }

        private void WindowSizeChanged(object? sender, EventArgs e)
        {
            RepaintPage();
        }

        // Переопределение возврата
        protected override bool OnBackButtonPressed()
        {
            if (_viewList.Count > 1)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
                RepaintPage();
            }

            return true;
        }



        // Тип вида
        private enum ViewType
        {
            EducationalInstitutionList, EducationalInstitution
        }

        private enum ComponentType
        {
            Label, Entity, Picker
        }


        
        // Добавление линий элементов в таблицах
        public sealed class ItemPicker()
        {
            public int Id { get; set; } = -1;
            public string Name { get; set; } = "";
        }
        private static object AddLineElems(ComponentType componentType,
                                         Grid grid,
                                         int startColumn,
                                         int startRow,
                                         string title,
                                         string? value = null,
                                         string? placeholder = null,
                                         Action<string>? textChangedAction = null,
                                         List<ItemPicker>? items = null,
                                         Action<int>? idChangedAction = null
                                         )
        {
            var titleLabel = new Label
            {
                // Цвета
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = title,
            };
            grid.Add(titleLabel, startColumn, startRow);

            switch (componentType)
            {
                case ComponentType.Label:
                    var valueLabel = new Label
                    {
                        // Цвета
                        TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                        // Текст
                        FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                        Text = value ?? "",
                    };

                    grid.Add(valueLabel, startColumn + 1, startRow);
                    return (object)valueLabel;
                case ComponentType.Entity:
                    var entryEntry = new Entry
                    {
                        // Цвета
                        BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                        TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                        PlaceholderColor = UserData.UserSettings.Colors.PLACEHOLDER_COLOR,

                        // Текст
                        FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                        Placeholder = placeholder ?? ""
                    };
                    if (textChangedAction != null)
                    {
                        entryEntry.TextChanged += (sender, e) => textChangedAction(e.NewTextValue);
                    }

                    grid.Add(entryEntry, startColumn + 1, startRow);
                    return (object)entryEntry;
                case ComponentType.Picker:
                    var valuePicker = new Picker
                    {
                        // Цвета
                        TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                        // Текст
                        FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,

                        ItemsSource = items ?? new List<ItemPicker>(),
                        ItemDisplayBinding = new Binding("Name"),
                    };
                    if (idChangedAction != null)
                    {
                        valuePicker.SelectedIndexChanged += (sender, e) =>
                        {
                            if (valuePicker.SelectedItem is ItemPicker selectedItem)
                            {
                                idChangedAction(selectedItem.Id);
                            }
                        };
                    }

                    grid.Add(valuePicker, startColumn + 1, startRow);
                    return (object)valuePicker;
            }

            return new object();
        }


        // Список школ
        private string _educationalInstitutionRegion = "";
        private string _educationalInstitutionSettlement = "";
        private string _educationalInstitutionName = "";
        private VerticalStackLayout _educationalInstitutionVerticalStackLayout = new()
        {
            Spacing = PageConstants.SPACING_ALL_PAGES
        };
        private ScrollView CreateEducationalInstitutionListView()
        {
            var verticalStack = new VerticalStackLayout
            {
                // Положение
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
            };

            var scrollView = new ScrollView()
            {
                // Доп инфа
                BindingContext = ViewType.EducationalInstitutionList,

                Content = verticalStack
            };

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
            };

            var rowIndex = 0;

            AddLineElems(
                componentType: ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Область",

                placeholder: "Минская область",
                textChangedAction: newText => _educationalInstitutionRegion = newText
            );

            AddLineElems(
                componentType: ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Населённый пункт",

                placeholder: "г.Солигорск",
                textChangedAction: newText => _educationalInstitutionSettlement = newText
            );

            AddLineElems(
                componentType: ComponentType.Entity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Название",

                placeholder: "ГУО ...",
                textChangedAction: newText => _educationalInstitutionName = newText
            );

            var GetEducationalInstitutionButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = "Найти",
            };
            GetEducationalInstitutionButton.Clicked += GetEducationalInstitutionListButtonClicked;
            grid.Add(GetEducationalInstitutionButton, 0, 3);
            verticalStack.Add(grid);

            var addEducationalInstitutionButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = "Добавить",
            };
            addEducationalInstitutionButton.Clicked += AddEducationalInstitutionButtonClicked;
            verticalStack.Add(addEducationalInstitutionButton);

            verticalStack.Add(_educationalInstitutionVerticalStackLayout);

            return scrollView;
        }

        // Получение списка школ
        private List<EducationalInstitutionResponse> _educationalInstitutionList = new();
        private async Task GetEducationalInstitutionList()
        {
            var response = await EducationalInstitutionControl.GetSchools();
            if (response.Error)
            {
                await DisplayAlert("Ошибка", response.Message, "OK");
            }
            else
            {
                _educationalInstitutionList = JsonSerializer.Deserialize<List<EducationalInstitutionResponse>>(response.Message, _jsonSerializerOptions) ?? new List<EducationalInstitutionResponse>();
                _educationalInstitutionList = _educationalInstitutionList
                    .Where(e =>
                        (_educationalInstitutionRegion?.Length == 0 || e.Settlement.Region.Name.Contains(_educationalInstitutionRegion ?? "", StringComparison.OrdinalIgnoreCase)) &&
                        (_educationalInstitutionSettlement?.Length == 0 || e.Settlement.Name.Contains(_educationalInstitutionSettlement ?? "", StringComparison.OrdinalIgnoreCase)) &&
                        (_educationalInstitutionName?.Length == 0 || e.Name.Contains(_educationalInstitutionName ?? "", StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                _educationalInstitutionVerticalStackLayout.Clear();

                for (var i = 0; i < _educationalInstitutionList.Count; i++)
                {
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += GetEducationalInstitutionListGestureTapped;
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
                        BindingContext = _educationalInstitutionList[i].Id,
                    };
                    grid.GestureRecognizers.Add(tapGesture);

                    var rowIndex = 0;

                    AddLineElems(
                        componentType: ComponentType.Label,
                        grid: grid,
                        startColumn: 0,
                        startRow: rowIndex++,
                        title: "Название",

                        value: _educationalInstitutionList[i].Name
                    );

                    AddLineElems(
                        componentType: ComponentType.Label,
                        grid: grid,
                        startColumn: 0,
                        startRow: rowIndex++,
                        title: "Регион",

                        value: _educationalInstitutionList[i].Settlement.Region.Name
                    );

                    AddLineElems(
                        componentType: ComponentType.Label,
                        grid: grid,
                        startColumn: 0,
                        startRow: rowIndex++,
                        title: "Город",

                        value: _educationalInstitutionList[i].Settlement.Name
                    );

                    _educationalInstitutionVerticalStackLayout.Add(grid);
                }
            }
        }
        private async void GetEducationalInstitutionListButtonClicked(object? sender, EventArgs e)
        {
            await GetEducationalInstitutionList();
        }

        // Действия с отдельными школами
        private async void GetEducationalInstitutionListGestureTapped(object? sender, EventArgs e)
        {
            string action = await DisplayActionSheet(
                "Выберите действие",    // Заголовок
                "Отмена",               // Кнопка отмены
                null,                   // Кнопка деструктивного действия (например, удаление)
                "Описание",             // Остальные кнопки
                "Перейти",
                "Редактировать",
                "Удалить");

            if (sender is not Grid grid)
            {
                return;
            }
            var id = (int)(grid).BindingContext;


            ScrollView scrollView;
            switch (action)
            {
                case "Описание":
                    scrollView = CreateEducationalInstitutionView(id);
                    if ((ViewType)_viewList[^1].BindingContext == ViewType.EducationalInstitution)
                    {
                        _viewList.RemoveAt(_viewList.Count - 1);
                    }
                    _viewList.Add(scrollView);
                    break;
                case "Перейти":

                    break;
                case "Редактировать":
                    scrollView = CreateEducationalInstitutionView(id);
                    if ((ViewType)_viewList[^1].BindingContext == ViewType.EducationalInstitution)
                    {
                        _viewList.RemoveAt(_viewList.Count - 1);
                    }
                    _viewList.Add(scrollView);
                    break;
                case "Удалить":
                    var response = await EducationalInstitutionControl.DeleteEducationalInstitution(id);
                    if (response.Error)
                    {
                        await DisplayAlert("Ошибка", response.Message, "OK");
                    }
                    break;
                default:
                    return;
            }

            await GetEducationalInstitutionList();
            RepaintPage();
        }
        private void AddEducationalInstitutionButtonClicked(object? sender, EventArgs e)
        {
            var scrollView = CreateEducationalInstitutionView();
            if ((ViewType)_viewList[^1].BindingContext == ViewType.EducationalInstitution)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
            }
            _viewList.Add(scrollView);
            RepaintPage();
        }
        private EducationalInstitutionRequest _educationalInstitutionRequest = new();
        private ScrollView CreateEducationalInstitutionView(int id = -1, bool edit = false)
        {
            var verticalStack = new VerticalStackLayout
            {
                // Положение
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
            };

            var scrollView = new ScrollView()
            {
                // Доп инфа
                BindingContext = ViewType.EducationalInstitution,

                Content = verticalStack
            };

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
            };

            ComponentType componentTypeEntity;
            ComponentType componentTypePicker;
            EducationalInstitutionResponse? educationalInstitutionResponse = new();
            if (id == -1)
            {
                _educationalInstitutionRequest = new();
                componentTypeEntity = ComponentType.Entity;
                componentTypePicker = ComponentType.Picker;
            }
            else
            {
                educationalInstitutionResponse = _educationalInstitutionList.FirstOrDefault(x => x.Id == id);
                if (educationalInstitutionResponse == null)
                {
                    return scrollView;
                }
                componentTypeEntity = ComponentType.Label;
                componentTypePicker = ComponentType.Label;
            }

            var rowIndex = 0;

            AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Название",

                value: educationalInstitutionResponse.Name,

                placeholder: "ГУО ...",
                textChangedAction: newText => _educationalInstitutionRequest.Name = newText
            );

            var objRegion = AddLineElems(
                componentType: componentTypePicker,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Регион",

                value: educationalInstitutionResponse.Settlement.Region.Name,

                idChangedAction: selectedIndex => _educationalInstitutionRequest.RegionId = selectedIndex
            );
            Task.Run(async() =>
            {
                var regionList = await GetRegion();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var pickerRegion = (Picker)objRegion;
                    pickerRegion.ItemsSource = regionList;
                });
            });

            var objSettlement = AddLineElems(
                componentType: componentTypePicker,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Населённый пункт",

                value: educationalInstitutionResponse.Settlement.Name,

                idChangedAction: selectedwId => _educationalInstitutionRequest.SettlementId = selectedwId
            );
            if (componentTypePicker == ComponentType.Picker)
            {
                var pickerRegion = (Picker)objRegion;
                pickerRegion.SelectedIndexChanged += async (sender, e) =>
                {
                    if (pickerRegion.SelectedItem is ItemPicker selectedItem)
                    {
                        var pickerSettlement = (Picker)objSettlement;
                        var settlementList = await GetSettlements(selectedItem.Id);

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            pickerSettlement.ItemsSource = settlementList;
                        });
                    }
                };
            }

            AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Адресс",

                value: educationalInstitutionResponse.Address,

                placeholder: "ул. Ленина, 12",
                textChangedAction: newText => _educationalInstitutionRequest.Address = newText
            );

            AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Email",

                value: educationalInstitutionResponse.Email,

                placeholder: "sh4@edus.by",
                textChangedAction: newText => _educationalInstitutionRequest.Email = newText
            );

            AddLineElems(
                componentType: componentTypeEntity,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Телефон",

                value: educationalInstitutionResponse.PhoneNumber,

                placeholder: "+375 17 433-09-02",
                textChangedAction: newText => _educationalInstitutionRequest.PhoneNumber = newText
            );

            if (id == -1)
            {
                var saveEducationalInstitutionButton = new Button
                {
                    // Положение
                    HorizontalOptions = LayoutOptions.Center,

                    // Цвета
                    BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                    TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                    // Текст
                    FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                    Text = "Сохранить",
                };
                saveEducationalInstitutionButton.Clicked += SaveEducationalInstitutionButtonClicked;

                grid.Add(saveEducationalInstitutionButton, 0, rowIndex++);
            }

            verticalStack.Add(grid);
            return scrollView;
        }
        private async void SaveEducationalInstitutionButtonClicked(object? sender, EventArgs e)
        {
            if (_educationalInstitutionRequest == null)
            {
                return;
            }

            var response = await EducationalInstitutionControl.AddEducationalInstitution(_educationalInstitutionRequest);
            if (response.Error)
            {
                await DisplayAlert("Ошибка", response.Message, "OK");
            }
            else
            {
                await DisplayAlert("Успех", "Школа добавлена", "OK");
                await GetEducationalInstitutionList();
                OnBackButtonPressed();
            }
        }
        private async Task<List<ItemPicker>> GetRegion()
        {
            List<ItemPicker>? list = null;
            var response = await AddressControl.GetRegions();
            if (response.Error)
            {
                await DisplayAlert("Ошибка", response.Message, "OK");
            }
            else
            {
                list = JsonSerializer.Deserialize<List<ItemPicker>>(response.Message, _jsonSerializerOptions);
            }
            return list ?? [];
        }
        private async Task<List<ItemPicker>> GetSettlements(int regionId)
        {
            List<ItemPicker>? list = null;
            var response = await AddressControl.GetSettlements(regionId);
            if (response.Error)
            {
                await DisplayAlert("Ошибка", response.Message, "OK");
            }
            else
            {
                list = JsonSerializer.Deserialize<List<ItemPicker>>(response.Message, _jsonSerializerOptions);
            }
            return list ?? [];
        }
    }
}
