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
        protected string[,] _dataTableArr = new string[0, 0];

        protected string[] _headerStrRowArr = [];
        protected string[] _headerStrColumnArr = [];

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
            await GetData();

            CrateTableHeader();
            CrateTable();
        }

        protected virtual void CrateTableHeader()
        {
            BaseElemsCreator.GridAddColumn(_grid, _headerStrRowArr.Length + 1, GridLength.Auto);

            for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; )
            {
                var elem = BaseElemsCreator.CreateLabel($"{_headerStrRowArr[rowIndex]}");
                elem.Background = UserData.Settings.Theme.BackgroundPageColor;
                _grid.Add(elem, 0, ++rowIndex);
            }


            for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length; )
            {
                var elem = BaseElemsCreator.CreateLabel($"{_headerStrColumnArr[columnIndex]}");
                elem.Rotation = - 90;
                elem.VerticalTextAlignment = TextAlignment.Center;
                elem.Background = UserData.Settings.Theme.BackgroundPageColor;
                _grid.Add(elem, ++columnIndex, 0);
            }
        }

        protected virtual void CrateTable()
        {
            for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; rowIndex++)
            {
                var tpmRowIndex = rowIndex + 1;
                for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length; )
                {
                    var elem = BaseElemsCreator.CreateLabel($"{_dataTableArr[rowIndex, columnIndex]}");
                    elem.Background = UserData.Settings.Theme.BackgroundFillColor;

                    _grid.Add(elem, ++columnIndex, tpmRowIndex);
                }
            }
        }
    }
}
