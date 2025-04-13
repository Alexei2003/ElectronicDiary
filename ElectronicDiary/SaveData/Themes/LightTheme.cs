using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicDiary.SaveData.Themes
{
    public class LightTheme : IAppTheme
    {
        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
        public Color BackgroundPageColor => Color.FromRgb(250, 250, 250);
        public Color NavigationPageColor => Color.FromRgb(200, 200, 200);
        public Color BackgroundFillColor => Color.FromRgb(225, 225, 225);

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ
        public Color AccentColor => Color.FromRgb(100, 100, 255);

        public Color AccentColorFields => Color.FromRgb(175, 175, 175);

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
        public Color TextColor => Color.FromRgb(0, 0, 0);
        public Color PlaceholderColor => Color.FromRgb(120, 120, 120);
    }
}
