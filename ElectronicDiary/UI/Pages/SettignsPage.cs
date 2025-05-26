using System.Collections.ObjectModel;

using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.SaveData.Themes;
using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Components.Navigation;
using ElectronicDiary.UI.Views.Lists.General;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Pages.OtherPages
{
    public partial class SettignsPage : ContentPage
    {
        private long _scaleIndex = -1;
        private long _themeIndex = -1;

        public SettignsPage()
        {
            Title = "Настройки";
            BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;

            var vStack = BaseElemsCreator.CreateVerticalStackLayout();
            AdminPageStatic.CalcViewWidth(out double width, out _);
            vStack.MaximumWidthRequest = width;

            var grid = BaseElemsCreator.CreateGrid();
            vStack.Children.Add(grid);

            var rowIndex = 0;
            LineElemsCreator.AddLineElems(
                grid,
                rowIndex++,
                [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Масштаб интерфейса")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreatePicker(GetScaleList(),
                                                selectedIndex => _scaleIndex = selectedIndex,
                                                (long)((UserData.Settings.UserSettings.ScaleFactor - START_SCALE_FACTOR) / SCALE_FACTOR))
                    }
                ]
            );

            LineElemsCreator.AddLineElems(
                grid,
                rowIndex++,
                [
                new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel("Тема интерфейса")
                    },
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreatePicker(GetThemeList(),
                                                selectedIndex => _themeIndex = selectedIndex,
                                                UserData.Settings.UserSettings.ThemeIndex)
                    }
                ]
            );


            vStack.Add(BaseElemsCreator.CreateButton("Сохранить", SaveButtonClicked));

            Content = vStack;
        }

        private const float START_SCALE_FACTOR = 0.5f;
        private const float SCALE_FACTOR = 0.25f;
        private const int SCALE_COUNT = 7;
        private static ObservableCollection<TypeResponse> GetScaleList()
        {
            var list = new ObservableCollection<TypeResponse>();

            for (var i = 0; i < SCALE_COUNT; i++)
            {
                list.Add(new TypeResponse(i, $"{(START_SCALE_FACTOR + i * SCALE_FACTOR) * 100}%"));
            }

            return list;
        }

        private static ObservableCollection<TypeResponse> GetThemeList()
        {
            var list = new ObservableCollection<TypeResponse>()
            {
                new(0, "Тёмная"),
                new(1, "Светлая"),
                new(2, "Красная"),
                new(3, "Зелённая"),
                new(4, "Синяя"),
                new(5, "Патриот"),
            };

            return list;
        }

        private void SaveButtonClicked(object? sender, EventArgs e)
        {
            if (_scaleIndex > -1)
            {
                var scaleFactor = START_SCALE_FACTOR + _scaleIndex * SCALE_FACTOR;
                UserData.Settings.UserSettings.ScaleFactor = scaleFactor;
                UserData.Settings.Sizes = new Settings.SizesClass(scaleFactor);
                UserData.Settings.Fonts = new Settings.FontsClass(scaleFactor);
            }

            if (_themeIndex > -1)
            {
                UserData.Settings.UserSettings.ThemeIndex = _themeIndex;
                UserData.Settings.Theme = ThemesMeneger.ChooseTheme(_themeIndex);
            }

            UserData.SaveUserSettings();
            Navigator.ChooseRootPageByRole(UserData.UserInfo.Role, UserData.UserInfo.Id);
        }
    }
}
