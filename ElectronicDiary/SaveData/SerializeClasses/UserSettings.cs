namespace ElectronicDiary.SaveData.SerializeClasses
{
    public class UserSettings
    {
        public SizesClass Sizes { get; set; } = new();

        public FontsClass Fonts { get; set; } = new();
        public ColorsClass Colors { get; set; } = new();


        public class SizesClass
        {
            // Размеры
            public Thickness PADDING_ALL_PAGES { get; set; } = new Thickness(10);
            public int SPACING_ALL_PAGES = 10;
            public int IMAGE_SIZE = 100;
        }

        public class FontsClass
        {
            public int TITLE_FONT_SIZE { get; set; } = 16;
            public int BASE_FONT_SIZE { get; set; } = 12;
        }

        public class ColorsClass
        {
            // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА
            /// <summary> Основной фон страниц приложения (самый темный слой) </summary>
            public Color BACKGROUND_PAGE_COLOR { get; set; } = Color.FromRgb(20, 20, 20);
            /// <summary> Фон навигационой панели </summary>
            public Color NAVIGATION_PAGE_COLOR { get; set; } = Color.FromRgb(40, 40, 40);
            /// <summary> Фон для контейнеров, карточек и элементов интерфейса </summary>
            public Color BACKGROUND_FILL_COLOR { get; set; } = Color.FromRgb(30, 30, 30);

            // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ
            /// <summary> Основной акцентный цвет для кнопок, переключателей, выделения </summary>
            public Color ACCENT_COLOR { get; set; } = Color.FromRgb(33, 33, 150);

            // ТЕКСТОВОЕ ОФОРМЛЕНИЕ
            /// <summary> Основной цвет текста (максимальная контрастность) </summary>
            public Color TEXT_COLOR { get; set; } = Color.FromRgb(255, 255, 255);
            /// <summary> Цвет плейсхолдеров в полях ввода (подсказки) </summary>
            public Color PLACEHOLDER_COLOR { get; set; } = Color.FromRgb(127, 127, 127);  
        }
    }
}
