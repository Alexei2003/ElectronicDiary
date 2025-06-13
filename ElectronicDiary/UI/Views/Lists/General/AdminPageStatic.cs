namespace ElectronicDiary.UI.Views.Lists.General
{
    public static class AdminPageStatic
    {
        // Обновление видов
        public static void RepaintPage(HorizontalStackLayout mainStack, List<ScrollView> viewList, bool fullscreen = false)
        {
            Application.Current?.Dispatcher.Dispatch(() =>
            {
                mainStack.Clear();

                CalcViewWidth(out var width, out var countColumn);

                for (var i = int.Max(viewList.Count - countColumn, 0); i < viewList.Count; i++)
                {
                    if (!fullscreen)
                    {
                        viewList[i].MinimumWidthRequest = width;
                        viewList[i].MaximumWidthRequest = width;
                    }
                    else
                    {
#if WINDOWS
                        const double coeffFixAndroidWidth = 1;
#else
                        const double coeffFixAndroidWidth = 2;
#endif

                        if (Application.Current?.Windows.Count > 0)
                        {
                            var widthFix = 0.95 * coeffFixAndroidWidth * Application.Current?.Windows[0].Width ?? 0d;
                            viewList[index: i].MinimumWidthRequest = widthFix;
                            viewList[i].MaximumWidthRequest = widthFix;
                        }
                    }
                    mainStack.Add(viewList[i]);
                }
            });
        }

        public static void CalcViewWidth(out double width, out int countColumn)
        {
            double dpi = DeviceDisplay.MainDisplayInfo.Density * 640;
            var widthWindow = 0d;
            if (Application.Current?.Windows.Count > 0)
            {
                widthWindow = Application.Current?.Windows[0].Width ?? 0d;
            }

#if WINDOWS
            const double coeffFixAndroidWidth = 1;
#else
            const double coeffFixAndroidWidth = 3.7;
#endif
            countColumn = int.Max((int)(widthWindow * coeffFixAndroidWidth / dpi), 1);

            width = 0.9 * dpi / coeffFixAndroidWidth;
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
