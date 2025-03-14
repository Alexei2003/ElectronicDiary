namespace ElectronicDiary.Pages.AdminPageComponents.Base
{
    public static class AdminPageStatic
    {
        // Обновление видов
        public static void RepaintPage(HorizontalStackLayout mainStack, List<ScrollView> viewList)
        {
            mainStack.Clear();

            double dpi = DeviceDisplay.MainDisplayInfo.Density * 160;

            double widthLocal = 0;
            if (Application.Current.Windows.Count > 0)
            {
                widthLocal = Application.Current.Windows[0].Width;
            }

#if WINDOWS
            double coeff = 1;
#else
            double coeff = 2;
#endif
            var countColumn = int.Min((int)(widthLocal * coeff / dpi / 2), 3);

            for (var i = int.Max(viewList.Count - countColumn, 0); i < viewList.Count; i++)
            {
                viewList[i].MaximumWidthRequest = widthLocal / countColumn * 0.90;
                mainStack.Add(viewList[i]);
            }
        }

        public static bool OnBackButtonPressed(HorizontalStackLayout mainStack, List<ScrollView> viewList)
        {
            if (viewList.Count > 1)
            {
                viewList.RemoveAt(viewList.Count - 1);
                RepaintPage(mainStack, viewList);
            }

            return true;
        }
    }
}
