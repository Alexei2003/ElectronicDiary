namespace ElectronicDiary.SaveData.Themes
{
    public class GreenTheme : IAppTheme
    {
        public bool TextIsBlack => false;


        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
        public Color BackgroundPageColor => Color.FromRgb(20, 40, 20);
        public Color NavigationPageColor => Color.FromRgb(40, 80, 40);
        public Color BackgroundFillColor => Color.FromRgb(30, 60, 30);

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ
        public Color AccentColor => Color.FromRgb(33, 150, 33);
        public Color AccentColorFields => Color.FromRgb(50, 100, 50);

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
        public Color TextColor => Color.FromRgb(255, 255, 255);
        public Color PlaceholderColor => Color.FromRgb(120, 120, 120);
    }
}
