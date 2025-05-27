using ElectronicDiary.SaveData.Static;
using ElectronicDiary.UI.Components.Elems;

using Microsoft.Maui.Controls;

namespace ElectronicDiary.UI.Views.Tables.BaseTable
{
    public class BaseViewTableCreator<THeaderRow, THeaderColumn>
    {
        protected Grid _grid = [];

        protected THeaderRow[] _headerRowArr = [];
        protected THeaderColumn[] _headerColumnArr = [];

        protected string[] _headerStrRowArr = [];
        protected string[] _headerStrColumnArr = [];
        protected string[,] _dataTableArr = new string[0, 0];

        protected long _id1;
        protected long _id2;
        protected bool _readOnly = false;

        public ScrollView Create(long id1,
                                 long id2,
                                 bool readOnly = false)
        {
            _id1 = id1;
            _id2 = id2;
            _readOnly = readOnly;

            CreateHeaderUI();

            _grid = BaseElemsCreator.CreateGrid(0);
            var size = UserData.Settings.Sizes.SPACING_ALL_PAGES / 10;
            _grid.ColumnSpacing = size;
            _grid.RowSpacing = size;
            _grid.Padding = size;
            _grid.BackgroundColor = UserData.Settings.Theme.TextColor;

            var scrollView = new ScrollView()
            {
                Content = _grid,
                Orientation = ScrollOrientation.Both
            };

            CreateUI();

            return scrollView;
        }

        protected virtual async Task GetData()
        {

        }

        protected virtual async void CreateUI()
        {
            LineElemsCreator.ClearGridRows(_grid);
            await GetData();

            CrateTableHeader();
            CrateTable();
        }

        protected virtual View CreateHeaderUI()
        {
            return new Label();
        }

        protected virtual void CrateTableHeader()
        {
            BaseElemsCreator.GridAddColumn(_grid, _headerStrRowArr.Length + 1, GridLength.Auto);

            var elemEmpty = BaseElemsCreator.CreateLabel($"");
            elemEmpty.Background = UserData.Settings.Theme.BackgroundPageColor;
            _grid.Add(elemEmpty, 0, 0);
            _grid.Add(CreateHeaderUI(), 0, 0);

            for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; )
            {
                var elem = BaseElemsCreator.CreateLabel($"{_headerStrRowArr[rowIndex]}");
                elem.VerticalTextAlignment = TextAlignment.Center;
                elem.Background = UserData.Settings.Theme.BackgroundPageColor;
                _grid.Add(elem, 0, ++rowIndex);
            }

            for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length; )
            {
                var elemBack = BaseElemsCreator.CreateLabel($"");
                elemBack.Background = UserData.Settings.Theme.BackgroundPageColor;

                var elem = BaseElemsCreator.CreateLabel($"{_headerStrColumnArr[columnIndex]}");
                elem.Rotation = - 90;
                elem.VerticalTextAlignment = TextAlignment.Center;
                elem.Background = UserData.Settings.Theme.BackgroundPageColor;

                _grid.Add(elemBack, ++columnIndex, 0);
                _grid.Add(elem, columnIndex, 0);
            }
        }

        protected virtual void CrateTable()
        {
            for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length; )
                {
                    var elem = BaseElemsCreator.CreateLabel($"{_dataTableArr[rowIndex, columnIndex]}");
                    elem.HorizontalTextAlignment = TextAlignment.Center;
                    elem.VerticalTextAlignment = TextAlignment.Center;
                    elem.Background = UserData.Settings.Theme.BackgroundFillColor;

                    _grid.Add(elem, ++columnIndex, rowIndex + 1);
                }
            }

            var elemName = BaseElemsCreator.CreateLabel($"Средняя оценка");
            elemName.HorizontalTextAlignment = TextAlignment.Center;
            elemName.VerticalTextAlignment = TextAlignment.Center;
            elemName.Background = UserData.Settings.Theme.BackgroundFillColor;
            _grid.Add(elemName, _headerStrColumnArr.Length + 2, 0);

            CalcAverange();
        }

        protected virtual async void CalcAverange()
        {
            for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; rowIndex++)
            {
                var sum = 0;
                var count = 0;
                for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length; columnIndex++)
                {
                    if (_dataTableArr[rowIndex, columnIndex] != "" && _dataTableArr[rowIndex, columnIndex] != "Н")
                    {
                        sum += int.Parse(_dataTableArr[rowIndex, columnIndex]);
                        count++;
                    }
                }
                var averange = count != 0 ? (1.0 * sum) / count : 0;
                var elem = BaseElemsCreator.CreateLabel($"{averange:F2}");
                elem.HorizontalTextAlignment = TextAlignment.Center;
                elem.VerticalTextAlignment = TextAlignment.Center;
                elem.Background = UserData.Settings.Theme.BackgroundFillColor;
                _grid.Add(elem, _headerStrColumnArr.Length + 2, rowIndex + 1);
            }
        }
    }
}
