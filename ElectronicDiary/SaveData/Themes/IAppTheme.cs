namespace ElectronicDiary.SaveData.Themes
{
    public interface IAppTheme
    {
        bool TextIsBlack { get; }

        // ОСНОВНЫЕ ФОНОВЫЕ ЦВЕТА

        /// <summary> Основной фон страниц приложения (самый темный слой) </summary>
        Color BackgroundPageColor { get; }

        /// <summary> Фон навигационой панели </summary>
        Color NavigationPageColor { get; }

        /// <summary> Фон для контейнеров, карточек и элементов интерфейса </summary>
        Color BackgroundFillColor { get; }

        // АКЦЕНТНЫЕ И ИНТЕРАКТИВНЫЕ ЭЛЕМЕНТЫ

        /// <summary> Основной акцентный цвет для кнопок, переключателей, выделения </summary>
        Color AccentColor { get; }

        Color AccentColorFields { get; }

        // ТЕКСТОВОЕ ОФОРМЛЕНИЕ

        /// <summary> Основной цвет текста (максимальная контрастность) </summary>
        Color TextColor { get; }

        /// <summary> Цвет плейсхолдеров в полях ввода (подсказки) </summary>
        Color PlaceholderColor { get; }
    }
}
