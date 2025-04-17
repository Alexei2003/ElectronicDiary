namespace ElectronicDiary.SaveData.Themes
{
    public class RedTheme : IAppTheme
    {
        public bool TextIsBlack => false;

        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
        public Color BackgroundPageColor => Color.FromRgb(40, 20, 20);
        public Color NavigationPageColor => Color.FromRgb(80, 40, 40);
        public Color BackgroundFillColor => Color.FromRgb(60, 30, 30);

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ

        public Color AccentColor => Color.FromRgb(150, 33, 33);

        public Color AccentColorFields => Color.FromRgb(100, 50, 50);

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
        public Color TextColor => Color.FromRgb(255, 255, 255);
        public Color PlaceholderColor => Color.FromRgb(120, 120, 120);
    }
}
