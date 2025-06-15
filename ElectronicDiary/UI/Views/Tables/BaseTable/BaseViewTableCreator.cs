using Android.Icu.Number;

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

        protected virtual Label CreateClickLabel(string text)
        {
            var elem = BaseElemsCreator.CreateLabel(text);
            elem.Padding = UserData.Settings.Sizes.Padding;
            elem.WidthRequest = UserData.Settings.Sizes.CellWidthScore;
            elem.HorizontalTextAlignment = TextAlignment.Center;
            elem.VerticalTextAlignment = TextAlignment.Center;
            elem.Background = UserData.Settings.Theme.BackgroundFillColor;
            return elem;
        }
        protected virtual Label CreateStaticLabelFirst(string text)
        {
            var elem = BaseElemsCreator.CreateLabel(text);
            elem.Padding = UserData.Settings.Sizes.Padding;
            elem.WidthRequest = UserData.Settings.Sizes.CellWidthText;
            elem.HorizontalTextAlignment = TextAlignment.Start;
            elem.VerticalTextAlignment = TextAlignment.Center;
            elem.Background = UserData.Settings.Theme.BackgroundPageColor;
            return elem;
        }

        protected virtual Label CreateStaticLabel(string text)
        {
            var elem = BaseElemsCreator.CreateLabel(text);
            elem.Padding = UserData.Settings.Sizes.Padding;
            elem.WidthRequest = UserData.Settings.Sizes.CellWidthScore;
            elem.HorizontalTextAlignment = TextAlignment.Center;
            elem.VerticalTextAlignment = TextAlignment.Center;
            elem.Background = UserData.Settings.Theme.BackgroundPageColor;
            return elem;
        }


        protected virtual async Task GetData()
        {

        }

        protected virtual async void CreateUI()
        {
            await GetData();
            var delta = (_headerStrColumnArr.Length + 3) - _grid.ColumnDefinitions.Count;
            LineElemsCreator.ClearGridRows(_grid);
            if (delta >= 0)
            {
                BaseElemsCreator.GridAddColumn(_grid, delta, GridLength.Auto);
            }
            CrateTableHeader();
            CrateTable();

            SetSize(this, EventArgs.Empty);
        }

        public void SetSize(object? sender, EventArgs e)
        {
            var orientation = DeviceDisplay.MainDisplayInfo.Orientation;
            double scale = orientation == DisplayOrientation.Portrait ? 1.37 : 1.93;
            _grid.WidthRequest = scale * ((UserData.Settings.Sizes.CellWidthText + _size) + (UserData.Settings.Sizes.CellWidthScore + _size) * (_headerStrColumnArr.Length + 3));
        }

        protected virtual View CreateHeaderUI()
        {
            return new Label();
        }

        protected virtual void CrateTableHeader()
        {
            var elemEmpty = CreateStaticLabelFirst($"");
            _grid.Add(elemEmpty, 0, 0);
            _grid.Add(CreateHeaderUI(), 0, 0);

            for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length;)
            {
                var elem = CreateStaticLabelFirst($"{_headerStrRowArr[rowIndex]}");
                _grid.Add(elem, 0, ++rowIndex);
            }

            for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length;)
            {
                var elem = CreateStaticLabel($"{_headerStrColumnArr[columnIndex]}");
                _grid.Add(elem, ++columnIndex, 0);
            }
        }

        protected virtual void CrateTable()
        {

            for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length;)
                {
                    var elem = CreateClickLabel($"{_dataTableArr[rowIndex, columnIndex]}");

                    if (!_readOnly)
                    {
                        elem.AutomationId = $"{rowIndex},{columnIndex}";
                        var tapGesture = new TapGestureRecognizer();
                        tapGesture.Tapped += LabelTapped;
                        elem.GestureRecognizers.Add(tapGesture);
                    }

                    _grid.Add(elem, ++columnIndex, rowIndex + 1);
                }
            }

            CalcAverange();
        }

        protected string[] _choseArrWithH = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Н"];
        protected string[] _choseArr = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"];

        protected bool _hFlag = false;
        protected async void LabelTapped(object? sender, EventArgs e)
        {
            if (sender is Label label && label.AutomationId is string id)
            {
                var parts = id.Split(',');
                var rowIndexElem = int.Parse(parts[0]);
                var columnIndexElem = int.Parse(parts[1]);

                string[] arr;
                if (_hFlag && columnIndexElem < (_headerStrColumnArr.Length - 1))
                {
                    arr = _choseArrWithH;
                }
                else
                {
                    arr = _choseArr;
                }
                var value = await BaseElemsCreator.CreateActionSheet(arr);
                if (_choseArrWithH.Contains(value))
                {
                    var oldValue = _dataTableArr[rowIndexElem, columnIndexElem];
                    _dataTableArr[rowIndexElem, columnIndexElem] = value;
                    var elem = CreateClickLabel(value);
                    elem.AutomationId = $"{rowIndexElem},{columnIndexElem}";
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += LabelTapped;
                    elem.GestureRecognizers.Add(tapGesture);
                    _grid.Add(elem, columnIndexElem + 1, rowIndexElem + 1);

                    Send(rowIndexElem, columnIndexElem, value, oldValue);

                    var sum = 0;
                    var countScore = 0;
                    var countAttendance = 0;
                    for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length - 1; columnIndex++)
                    {
                        if (_dataTableArr[rowIndexElem, columnIndex] == "Н")
                        {
                            countAttendance++;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_dataTableArr[rowIndexElem, columnIndex]))
                            {
                                sum += int.Parse(_dataTableArr[rowIndexElem, columnIndex]);
                                countScore++;
                            }
                        }
                    }
                    var averange = countScore != 0 ? (1.0 * sum) / countScore : 0;
                    var elemScore = CreateStaticLabel($"{averange:F1}");
                    _grid.Add(elemScore, _headerStrColumnArr.Length + 1, rowIndexElem + 1);

                    if (_attendance)
                    {
                        var elemAttendance = CreateStaticLabel($"{countAttendance}");
                        _grid.Add(elemAttendance, _headerStrColumnArr.Length + 2, rowIndexElem + 1);
                    }
                }
            }
        }

        protected virtual async void Send(int rowIndex, int columnIndex, string value, string oldValue)
        {

        }


        protected bool _attendance = true;
        protected virtual async void CalcAverange()
        {
            var elemScoreName = CreateStaticLabel($"Средняя");
            _grid.Add(elemScoreName, _headerStrColumnArr.Length + 1, 0);

            if (_attendance)
            {
                var elemAttendanceName = CreateStaticLabel($"Пропуски");
                _grid.Add(elemAttendanceName, _headerStrColumnArr.Length + 2, 0);
            }

            for (var rowIndex = 0; rowIndex < _headerStrRowArr.Length; rowIndex++)
            {
                var sum = 0;
                var countScore = 0;
                var countAttendance = 0;
                for (var columnIndex = 0; columnIndex < _headerStrColumnArr.Length - 1; columnIndex++)
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
                var elemScore = CreateStaticLabel($"{averange:F1}");
                _grid.Add(elemScore, _headerStrColumnArr.Length + 1, rowIndex + 1);

                if (_attendance)
                {
                    var elemAttendance = CreateStaticLabel($"{countAttendance}");
                    _grid.Add(elemAttendance, _headerStrColumnArr.Length + 2, rowIndex + 1);
                }
            }
        }
    }
}
