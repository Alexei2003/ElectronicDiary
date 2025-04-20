namespace ElectronicDiary.SaveData.Themes
{
    public class PatriotTheme : IAppTheme
    {
        public bool TextIsBlack => true;

        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
        public Color BackgroundPageColor => Color.FromRgb(0, 124, 48);
        public Color NavigationPageColor => Color.FromRgb(207, 23, 33);
        public Color BackgroundFillColor => Color.FromRgb(150, 225, 150);

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ
        public Color AccentColor => Color.FromRgb(100, 255, 100);

        public Color AccentColorFields => Color.FromRgb(150, 200, 150);

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
        public Color TextColor => Color.FromRgb(0, 0, 0);
        public Color PlaceholderColor => Color.FromRgb(120, 120, 120);
    }
}
