using ElectronicDiary.Web.DTO.Responses;

namespace ElectronicDiary.Pages.Components.Elems
{
    public static class LineElemsCreator
    {
        public class LabelData
        {
            public string? Title { get; set; } = null;
        }

        public class EntryData
        {
            public string? BaseText { get; set; } = null;
            public string? Placeholder { get; set; } = null;
            public Action<string>? TextChangedAction { get; set; } = null;
        }


        public class SearchData
        {
            public TypeResponse? BaseItem { get; set; } = null;
            public Action<long>? IdChangedAction { get; set; } = null;
        }

        public class PickerData
        {
            public long? BaseSelectedId { get; set; } = null;
            public List<Item>? Items { get; set; } = null;
            public Action<long>? IdChangedAction { get; set; } = null;
        }

        public class ImageData
        {
            public string? PathImage { get; set; } = null;
        }

        public static object[] AddLineElems(Grid grid, int rowIndex, object[] objectList)
        {
            var resultList = new List<object>();

            var indexColumn = 0;

            foreach (var obj in objectList)
            {
                switch (obj)
                {
                    case LabelData labelData:
                        var label = BaseElemCreator.CreateLabel(labelData.Title);
                        grid.Add(label, indexColumn++, rowIndex);
                        resultList.Add(label);
                        break;

                    case EntryData entryData:
                        var entry = BaseElemCreator.CreateEntry(entryData.TextChangedAction, entryData.Placeholder, entryData.BaseText);
                        grid.Add(entry, indexColumn++, rowIndex);
                        resultList.Add(entry);
                        break;

                    case SearchData searchData:
                        var searchLabel = BaseElemCreator.CreateLabel(searchData?.BaseItem?.Name ?? "Найти");

                        var tapGesture = new TapGestureRecognizer();
                        tapGesture.Tapped += (sender, e) =>
                        {
                            if (tapGesture.BindingContext is List<Item> items)
                            {
                                var id = searchData?.BaseItem?.Id;
                                Action<long> idChangedActionLocal = newText => id = newText;

                                var popup = BaseElemCreator.CreateSearchPopup(items, idChangedActionLocal);
                                popup.Closed += (sender, e) =>
                                {
                                    if (popup.AllItems.Count > 0 && id >= 0)
                                    {
                                        if (searchData?.IdChangedAction != null) searchData.IdChangedAction(id ?? 0);
                                        searchLabel.Text = popup.AllItems.FirstOrDefault(item => item.Id == id)?.Name ?? "Найти";
                                    }
                                    searchLabel.Focus();
                                };
                            }
                        };
                        searchLabel.GestureRecognizers.Add(tapGesture);

                        grid.Add(searchLabel, indexColumn++, rowIndex);
                        resultList.Add(tapGesture);
                        break;

                    case PickerData pickerData:
                        var picker = BaseElemCreator.CreatePicker(pickerData.Items, pickerData.IdChangedAction, pickerData.BaseSelectedId);

                        grid.Add(picker, indexColumn++, rowIndex);
                        resultList.Add(picker);
                        break;

                    case ImageData imageData:
                        var image = BaseElemCreator.CreateImage(imageData.PathImage);
                        grid.Add(image, 0, indexColumn);
                        Grid.SetColumnSpan(image, 2);
                        resultList.Add(image);
                        break;
                }
            }

            return [.. resultList];
        }

        private static void ClearGridRows(Grid grid, IView[] elementsToRemove)
        {
            foreach (var element in elementsToRemove)
            {
                grid.Children.Remove(element);
            }
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