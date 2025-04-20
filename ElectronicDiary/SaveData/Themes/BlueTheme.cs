namespace ElectronicDiary.SaveData.Themes
{
    public class BlueTheme : IAppTheme
    {
        public bool TextIsBlack => false;

        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
        public Color BackgroundPageColor => Color.FromRgb(20, 20, 40);
        public Color NavigationPageColor => Color.FromRgb(40, 40, 80);
        public Color BackgroundFillColor => Color.FromRgb(30, 30, 60);

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ
        public Color AccentColor => Color.FromRgb(33, 33, 150);
        public Color AccentColorFields => Color.FromRgb(50, 50, 100);

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
        public Color TextColor => Color.FromRgb(255, 255, 255);
        public Color PlaceholderColor => Color.FromRgb(120, 120, 120);
    }
}
