namespace ElectronicDiary.SaveData.Themes
{
    public class ThemesMeneger
    {
        public static IAppTheme ChooseTheme(long index)
        {
            switch (index)
            {
                case 0:
                    return new DarkTheme();
                case 1:
                    return new LightTheme();
                case 2:
                    return new RedTheme();
                case 3:
                    return new GreenTheme();
                case 4:
                    return new BlueTheme();
                case 5:
                    return new PatriotTheme();
            }

            return new LightTheme();
        }
    }
}
