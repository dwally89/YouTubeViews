namespace YouTube_Views_CSharp
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// The video class
    /// </summary>
    public class Video : INotifyPropertyChanged
    {
        /// <summary>
        /// The _date added of the video
        /// </summary>
        private DateTime _dateAdded;

        /// <summary>
        /// The _hidden numbers of the video
        /// </summary>
        private int _hiddenNumbers;

        /*
        /// <summary>
        /// The views per day of the video
        /// </summary>        
        private int _viewsPerDay;
        */

        /// <summary>
        /// The views of the video
        /// </summary>
        private int _views;

        /// <summary>
        /// The name of the video
        /// </summary>
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Video" /> class.
        /// </summary>
        /// <param name="name">The name of the video.</param>
        /// <param name="views">The views of the video.</param>
        /// <param name="dateAdded">The date added of the video.</param>
        /// <param name="hiddenNumbers">The hidden numbers of the video.</param>
        public Video(string name, int views, DateTime dateAdded, int hiddenNumbers)
        {
            _name = name;
            _views = views;
            _dateAdded = dateAdded;
            _hiddenNumbers = hiddenNumbers;
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name of the video
        /// </value>
        [DisplayName("Name")]
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets the total views.
        /// </summary>
        /// <value>
        /// The total views.
        /// </value>
        public int TotalViews
        {
            get
            {
                return _views + _hiddenNumbers;
            }
        }

        /// <summary>
        /// Gets or sets the views.
        /// </summary>
        /// <value>
        /// The views.
        /// </value>
        public int Views
        {
            get
            {
                return _views;
            }

            set
            {
                _views = value;
                OnPropertyChanged("Views");
                OnPropertyChanged("TotalViews");
            }
        }

        /// <summary>
        /// Gets the views per day.
        /// </summary>
        /// <value>
        /// The views per day.
        /// </value>
        public double ViewsPerDay
        {
            get
            {
                return (double)TotalViews / (double)Math.Abs((DateAdded - DateTime.Today).Days);            
            }

            /*
            set
            {
                _viewsPerDay = value;
            }
             */ 
        }

        /// <summary>
        /// Gets or sets the hidden numbers.
        /// </summary>
        /// <value>
        /// The hidden numbers.
        /// </value>       
        public int HiddenNumbers
        {
            get
            {
                return _hiddenNumbers;
            }

            set
            {
                _hiddenNumbers = value;
                OnPropertyChanged("HiddenNumbers");
                OnPropertyChanged("TotalViews");
            }
        }

        /// <summary>
        /// Gets or sets the date added.
        /// </summary>
        /// <value>
        /// The date added.
        /// </value>
        public DateTime DateAdded
        {
            get
            {
                return _dateAdded;
            }

            set
            {
                _dateAdded = value;
                OnPropertyChanged("DateAdded");
            }
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status
        {
            get
            {
                if (ViewsPerDay > Views)
                {
                    return "Increasing";
                }
                else if (ViewsPerDay == Views)
                {
                    return "Stable";
                }
                else
                {
                    return "Decreasing";
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name + " " + Views + " " + DateAdded.ToShortDateString() + " " + HiddenNumbers;
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
