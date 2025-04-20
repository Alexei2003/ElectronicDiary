namespace ElectronicDiary.Pages.Components.Elems
{
    public static class LineElemsCreator
    {
        public class Data
        {
            public int CountJoinColumns { get; set; } = -1;
            public View? Elem { get; set; } = null;
        }

        public static void AddLineElems(Grid grid, int rowIndex, Data[] objectList)
        {
            Application.Current?.Dispatcher.Dispatch(() =>
            {
                var indexColumn = 0;

                foreach (var obj in objectList)
                {
                    if (obj.Elem is View view)
                    {
                        grid.Add(view, indexColumn++, rowIndex);
                        if (obj.CountJoinColumns > 0) Grid.SetColumnSpan(view, obj.CountJoinColumns);
                    }
                }
            });
        }

        private static void ClearGridRows(Grid grid, IView[] elementsToRemove)
        {
            Application.Current?.Dispatcher.Dispatch(() =>
            {
                foreach (var element in elementsToRemove)
                {
                    grid.Children.Remove(element);
                }
            });
        }

        public static void ClearGridRows(Grid grid, int index)
        {
            var elementsToRemove = grid.Children
                .Where(e => index == grid.GetRow(e))
                .ToArray();

            ClearGridRows(grid, elementsToRemove);
        }

        public static void ClearGridRows(Grid grid, int[] indexes)
        {
            var elementsToRemove = grid.Children
                .Where(e => indexes.Contains(grid.GetRow(e)))
                .ToArray();

            ClearGridRows(grid, elementsToRemove);
        }

        public static void ClearGridRows(Grid grid, int firstIndex, int lastIndex)
        {
            var elementsToRemove = grid.Children
                .Where(e =>
                {
                    int row = grid.GetRow(e);
                    return row >= firstIndex && row <= lastIndex;
                })
                .ToArray();

            ClearGridRows(grid, elementsToRemove);
        }
    }
}