using ElectronicDiary.SaveData.Static;
using ElectronicDiary.UI.Components.Elems;

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

        private int _size = 0;
        private ScrollView _scrollView = new();
        public ScrollView Create(long id1,
                                 long id2,
                                 bool readOnly = false)
        {
            _id1 = id1;
            _id2 = id2;
            _readOnly = readOnly;

            CreateHeaderUI();

            _grid = BaseElemsCreator.CreateGrid(0);
            _size = UserData.Settings.Sizes.Spacing / 10;
            _grid.ColumnSpacing = _size;
            _grid.RowSpacing = _size;
            _grid.Padding = _size;
            _grid.Margin = UserData.Settings.Sizes.Padding;
            _grid.BackgroundColor = UserData.Settings.Theme.TextColor;

            _scrollView = new ScrollView()
            {
                Content = _grid,
                Orientation = ScrollOrientation.Both,
                HorizontalOptions = LayoutOptions.Start,
                Background = UserData.Settings.Theme.BackgroundPageColor
            };

            CreateUI();

            return _scrollView;
        }

        protected virtual async Task GetData()
        {

        }

        protected virtual async void CreateUI()
        {
            await GetData();
            var add = _attendance ? 2 : 1;
            var delta = (_headerStrColumnArr.Length + add) - _grid.ColumnDefinitions.Count;
            LineElemsCreator.ClearGridRows(_grid);
            if (delta >= 0)
            {
                BaseElemsCreator.GridAddColumn(_grid, delta, GridLength.Auto);
            }
            else
            {
                BaseElemsCreator.GridRemoveColumn(_grid, -delta);
            }
            CrateTableHeader();
            CrateTable();
            _scrollView.MinimumWidthRequest = 2 * ((UserData.Settings.Sizes.CellWidthText + _size) + (UserData.Settings.Sizes.CellWidthScore + _size) * (_grid.ColumnDefinitions.Count - 1));
        }

        protected virtual View CreateHeaderUI()
        {
            return new Label();
        }

        protected virtual void CrateTableHeader()
        {
            var elemEmpty = BaseElemsCreator.CreateLabel($"");
            elemEmpty.Padding = UserData.Settings.Sizes.Padding;
            elemEmpty.WidthRequest = UserData.Settings.Sizes.CellWidthText;
            elemEmpty.Background = UserData.Settings.Theme.BackgroundPageColor;
            _grid.Add(elemEmpty, 0, 0);
            _grid.Add(CreateHeaderUI(), 0, 0);

            var elemColor = BaseElemsCreator.CreateLabel("");
            elemColor.Margin = new Thickness(0, -_size, -_size, -_size);
            elemColor.Background = UserData.Settings.Theme.BackgroundPageColor;
            _grid.Add(elemColor, _headerStrColumnArr.Length + 3, 0);

            for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length;)
            {
                var elem = BaseElemsCreator.CreateLabel($"{_headerStrRowArr[rowIndex]}");
                elem.Padding = UserData.Settings.Sizes.Padding;
                elem.WidthRequest = UserData.Settings.Sizes.CellWidthText;
                elem.VerticalTextAlignment = TextAlignment.Center;
                elem.Background = UserData.Settings.Theme.BackgroundPageColor;
                _grid.Add(elem, 0, ++rowIndex);

                elemColor = BaseElemsCreator.CreateLabel("");
                elemColor.Background = UserData.Settings.Theme.BackgroundPageColor;
                elemColor.Margin = new Thickness(0, -_size, -_size, -_size);
                _grid.Add(elemColor, _headerStrColumnArr.Length + 3, rowIndex);
            }

            for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length;)
            {
                var elem = BaseElemsCreator.CreateLabel($"{_headerStrColumnArr[columnIndex]}");
                elem.Padding = UserData.Settings.Sizes.Padding;
                elem.WidthRequest = UserData.Settings.Sizes.CellWidthScore;
                elem.VerticalTextAlignment = TextAlignment.Center;
                elem.HorizontalTextAlignment = TextAlignment.Center;
                elem.Background = UserData.Settings.Theme.BackgroundPageColor;
                _grid.Add(elem, ++columnIndex, 0);
            }
        }

        protected virtual void CrateTable()
        {
            for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length;)
                {
                    var elem = BaseElemsCreator.CreateLabel($"{_dataTableArr[rowIndex, columnIndex]}");
                    elem.Padding = UserData.Settings.Sizes.Padding;
                    elem.WidthRequest = UserData.Settings.Sizes.CellWidthScore;
                    elem.HorizontalTextAlignment = TextAlignment.Center;
                    elem.VerticalTextAlignment = TextAlignment.Center;
                    elem.Background = UserData.Settings.Theme.BackgroundFillColor;

                    _grid.Add(elem, ++columnIndex, rowIndex + 1);
                }
            }

            CalcAverange();
        }

        protected bool _attendance = true;

        protected virtual async void CalcAverange()
        {
            var elemScoreName = BaseElemsCreator.CreateLabel($"Средняя");
            elemScoreName.WidthRequest = UserData.Settings.Sizes.CellWidthScore;
            elemScoreName.HorizontalTextAlignment = TextAlignment.Center;
            elemScoreName.VerticalTextAlignment = TextAlignment.Center;
            elemScoreName.Background = UserData.Settings.Theme.BackgroundPageColor;
            _grid.Add(elemScoreName, _headerStrColumnArr.Length + 1, 0);

            if (_attendance)
            {
                var elemAttendanceName = BaseElemsCreator.CreateLabel($"Пропуски");
                elemAttendanceName.WidthRequest = UserData.Settings.Sizes.CellWidthScore;
                elemAttendanceName.HorizontalTextAlignment = TextAlignment.Center;
                elemAttendanceName.VerticalTextAlignment = TextAlignment.Center;
                elemAttendanceName.Background = UserData.Settings.Theme.BackgroundPageColor;
                _grid.Add(elemAttendanceName, _headerStrColumnArr.Length + 2, 0);
            }

            for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; rowIndex++)
            {
                var sum = 0;
                var countScore = 0;
                var countAttendance = 0;
                for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length; columnIndex++)
                {
                    if (_dataTableArr[rowIndex, columnIndex] == "Н")
                    {
                        countAttendance++;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(_dataTableArr[rowIndex, columnIndex]))
                        {
                            sum += int.Parse(_dataTableArr[rowIndex, columnIndex]);
                            countScore++;
                        }
                    }
                }
                var averange = countScore != 0 ? (1.0 * sum) / countScore : 0;
                var elemScore = BaseElemsCreator.CreateLabel($"{averange:F1}");
                elemScore.Padding = UserData.Settings.Sizes.Padding;
                elemScore.WidthRequest = UserData.Settings.Sizes.CellWidthScore;
                elemScore.HorizontalTextAlignment = TextAlignment.Center;
                elemScore.VerticalTextAlignment = TextAlignment.Center;
                elemScore.Background = UserData.Settings.Theme.BackgroundPageColor;
                _grid.Add(elemScore, _headerStrColumnArr.Length + 1, rowIndex + 1);

                if (_attendance)
                {
                    var elemAttendance = BaseElemsCreator.CreateLabel($"{countAttendance}");
                    elemAttendance.Padding = UserData.Settings.Sizes.Padding;
                    elemAttendance.WidthRequest = UserData.Settings.Sizes.CellWidthScore;
                    elemAttendance.HorizontalTextAlignment = TextAlignment.Center;
                    elemAttendance.VerticalTextAlignment = TextAlignment.Center;
                    elemAttendance.Background = UserData.Settings.Theme.BackgroundPageColor;
                    _grid.Add(elemAttendance, _headerStrColumnArr.Length + 2, rowIndex + 1);
                }
            }
        }
    }
}
