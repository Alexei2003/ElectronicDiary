namespace ElectronicDiary.Pages.AdminPageComponents.General
{
    public static class AdminPageStatic
    {
        // Обновление видов
        public static void RepaintPage(HorizontalStackLayout mainStack, List<ScrollView> viewList)
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                mainStack.Clear();

                CalcViewWidth(out var width,out var countColumn);

                for (var i = int.Max(viewList.Count - countColumn, 0); i < viewList.Count; i++)
                {
                    viewList[i].MaximumWidthRequest = width;
                    mainStack.Add(viewList[i]);
                }
            });
        }

        public static void CalcViewWidth(out double width, out int countColumn)
        {

            double dpi = DeviceDisplay.MainDisplayInfo.Density * 160;
            var widthLocal = 0d;
            if (Application.Current?.Windows.Count > 0)
            {
                widthLocal = Application.Current?.Windows[0].Width ?? 0;
            }

#if WINDOWS
            const double coeff = 1;
#else
            const double coeff = 2;
#endif
            countColumn = int.Min((int)(widthLocal * coeff / dpi / 2), 3);

            width = widthLocal / countColumn * 0.90;
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

        public static void DeleteLastView(HorizontalStackLayout mainStack, List<ScrollView> viewList, int maxCountViews, int indexDel = 1)
        {
            while (viewList.Count >= maxCountViews)
            {
                viewList.RemoveAt(viewList.Count - indexDel);
            }
        }
        public enum ComponentState
        {
            Read, New, Edit
        }
    }
}
