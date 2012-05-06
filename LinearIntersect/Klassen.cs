using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;

using System.Windows.Forms;
using System.ComponentModel;

namespace LinearIntersect
{
    public enum GridOrientation
    {
        Vertical,
        Horizontal
    }

    public class Overlay : INotifyPropertyChanged
    {
        public List<Point> Points = new List<Point>();
        public List<int> GridPosition = new List<int>();
        private int GridStart = 30;
        private int GridDistance = 100;
        public float zoom = 1f;
        private BindableKeyValuePair<string, float> _calibration;

        public BindableKeyValuePair<string, float> Calibration
        {
            get
            {
                if (_calibration == null)
                {
                    BindableKeyValuePair<string, float> tmp = new BindableKeyValuePair<string, float>();
                    tmp.Key = "Default";
                    tmp.Value = 1f;
                    _calibration = tmp;
                }

                return _calibration;
            }
            set
            {
                _calibration = value;
                Debug.WriteLine("Calibration changed to: " + value.ToString());
                NotifyPropertyChanged("Calibration");
            }
        }

        public string StartString
        {
            get
            {
                return GridStart.ToString();
            }
            set
            {
                try
                {
                    GridStart = int.Parse(value);
                    //clear();
                }
                catch
                {

                }
            }
        }
        public string DSTString
        {
            get
            {
                return GridDistance.ToString();
            }
            set
            {
                try
                {
                    GridDistance = int.Parse(value);
                    //clear();
                }
                catch
                {

                }
            }
        }
        private GridOrientation _Orientation = GridOrientation.Horizontal;
        public GridOrientation Orientation
        {
            get
            {
                return _Orientation;
            }
            set
            {
                Debug.WriteLine("Richtung geändert");
                _Orientation = value;
                //clear();
            }
        }
        public int clickOffset = 5;
        public Size ImageSize;
        public Point previewPoint;

        public Overlay()
        {
        }

        public Overlay(Size size)
        {
            ImageSize = size;
        }

        public void clear()
        {
            GridPosition.Clear();
            //Points.Clear();
            createGrid();
            NotifyPropertyChanged("Grid Changed");
        }

        public bool getPointAt(Point Coordinate, out Point POut, int offSet)
        {
            foreach (Point p in Points)
            {
                if (isBetween(p.X, Coordinate.X, offSet) && isBetween(p.Y, Coordinate.Y, offSet))
                {
                    POut = p;
                    return true;
                }
            }
            POut = Point.Empty;
            return false;
        }

        public void AddPoint(Point p)
        {
            Point newPoint = getSnapPoint(p);
            Point oldPoint;

            if (getPointAt(newPoint, out oldPoint, clickOffset))
            {
                //Points.Remove(oldPoint);
                return;
            }

            Points.Add(newPoint);
        }

        public void removePointAt(Point p)
        {
            Point newPoint = getSnapPoint(p);
            Point oldPoint;

            if (getPointAt(newPoint, out oldPoint, clickOffset * 2))
            {
                Points.Remove(oldPoint);
                return;
            }
        }

        public void PreviewPoint(Point p)
        {
            previewPoint = getSnapPoint(p);
        }

        public Point getSnapPoint(Point p)
        {
            Point res = new Point();
            int dst = int.MaxValue;

            if (Orientation == GridOrientation.Horizontal)
            {
                int y = 0;
                foreach (int g in GridPosition)
                {
                    int tmp = Math.Abs(g - p.Y);
                    if (tmp < dst)
                    {
                        dst = tmp;
                        y = g;
                    }
                }
                res.X = p.X;
                res.Y = y;
            }
            else if (Orientation == GridOrientation.Vertical)
            {
                int x = 0;
                foreach (int g in GridPosition)
                {
                    int tmp = Math.Abs(g - p.X);
                    if (tmp < dst)
                    {
                        dst = tmp;
                        x = g;
                    }
                }
                res.X = x;
                res.Y = p.Y;
            }

            return res;
        }

        public void createGrid()
        {
            GridPosition.Clear();
            int upper = 0;
            if (Orientation == GridOrientation.Horizontal) upper = ImageSize.Height;
            if (Orientation == GridOrientation.Vertical) upper = ImageSize.Width;

            for (int pos = GridStart; pos < upper; pos += GridDistance)
            {
                GridPosition.Add(pos);
            }
        }

        private bool isBetween(int value, int related, int offset)
        {
            if (value <= related + offset && value >= related - offset)
            {
                return true;
            }
            return false;
        }

        public Dictionary<int, List<int>> getLineInfo()
        {
            Dictionary<int, List<int>> res = new Dictionary<int, List<int>>();

            foreach (int g in GridPosition)
            {
                res.Add(g, new List<int>());
            }

            foreach (Point p in Points)
            {
                int line, value;
                if (Orientation == GridOrientation.Horizontal)
                {
                    line = p.Y;
                    value = p.X;
                }
                else
                {
                    line = p.X;
                    value = p.Y;
                }

                res[line].Add(value);
            }
            Debug.WriteLine("LineInfo angefordert");
            return res;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    [Serializable]
    public class SettingsData : INotifyPropertyChanged
    {
        private Color _gridcolor = Color.LightGreen;
        private Color _pointColor = Color.OrangeRed;
        private Color _cursorColor = Color.Red;
        private int _DefaultDistance = 100;
        private int _DefaultStart = 30;
        private GridOrientation _DefaultDir = GridOrientation.Horizontal;
        //public Dictionary<string, float> Calibration = new Dictionary<string, float>();
        public CalibrationList Calibration = new CalibrationList();

        public Color GridColor
        {
            get
            {
                return _gridcolor;
            }
            set
            {
                _gridcolor = value;
                NotifyPropertyChanged("Gridcolor");
            }
        }

        public Color PointColor
        {
            get
            {
                return _pointColor;
            }
            set
            {
                _pointColor = value;
                NotifyPropertyChanged("_pointColor");
            }
        }

        public Color CursorColor
        {
            get
            {
                return _cursorColor;
            }
            set
            {
                _cursorColor = value;
                NotifyPropertyChanged("_cursorColor");
            }
        }

        public string DefaultDistance
        {
            get
            {
                return _DefaultDistance.ToString(); ;
            }
            set
            {
                try
                {
                    _DefaultDistance = int.Parse(value);
                    NotifyPropertyChanged("_DefaultDistance");
                }
                catch
                {

                }
            }
        }

        public string DefaultStart
        {
            get
            {
                return _DefaultStart.ToString(); ;
            }
            set
            {
                _DefaultStart = int.Parse(value);
                NotifyPropertyChanged("_DefaultStart");
            }
        }

        public GridOrientation DefaultDir
        {
            get
            {
                return _DefaultDir;
            }
            set
            {
                _DefaultDir = value;
                NotifyPropertyChanged("DefaultDir");
            }
        }

        public SettingsData()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    [Serializable]
    public class BindableKeyValuePair<T1, T2>
    {
        private T1 _key = default(T1);
        public T1 Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
                NotifyPropertyChanged("Key Changed");
            }
        }

        private T2 _value = default(T2);
        public T2 Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                NotifyPropertyChanged("Value Changed");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    [Serializable]
    public class CalibrationList : BindingList<BindableKeyValuePair<string, float>>
    {

    }
}
