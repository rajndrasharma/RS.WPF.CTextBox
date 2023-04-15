namespace RS.WPF.CTextBox;

/// <summary>
/// Interaction logic for UserControl1.xaml
/// </summary>
public partial class TextBoxWithLabel : UserControl,INotifyPropertyChanged
{
    public static readonly DependencyProperty KeyValueProperty =
        DependencyProperty.Register("KeyValue", typeof(string), typeof(TextBoxWithLabel),
            new PropertyMetadata(""));

    public static readonly DependencyProperty LabelProperty = 
        DependencyProperty.Register("Label", typeof(string), typeof(TextBoxWithLabel),
            new PropertyMetadata(""));

    public static readonly DependencyProperty LabelWidthProperty =
       DependencyProperty.Register("LabelWidth", typeof(int), typeof(TextBoxWithLabel),
           new PropertyMetadata(170));

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(object), typeof(TextBoxWithLabel),
            new PropertyMetadata(""));

    public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.Register("MaxLength", typeof(object), typeof(TextBoxWithLabel),
            new PropertyMetadata(0));

    public static readonly DependencyProperty TextRequiredMessageProperty =
        DependencyProperty.Register("TextRequiredMessage", typeof(string), typeof(TextBoxWithLabel),
            new PropertyMetadata(""));

    public static readonly DependencyProperty ErrorMessageProperty =
        DependencyProperty.Register("ErrorMessage", typeof(string), typeof(TextBoxWithLabel),
            new PropertyMetadata(""));

    public static readonly DependencyProperty TextBoxTypeProperty =
        DependencyProperty.Register("TextBoxType", typeof(CTextBoxType), typeof(TextBoxWithLabel),
            new PropertyMetadata(CTextBoxType.AlphaNumeric));

    public static new readonly DependencyProperty HorizontalContentAlignmentProperty =
        DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(TextBoxWithLabel),
            new PropertyMetadata(HorizontalAlignment.Left));

    public static readonly DependencyProperty ReadOnlyProperty =
        DependencyProperty.Register("ReadOnly", typeof(bool), typeof(TextBoxWithLabel),
            new PropertyMetadata(false));

    public static readonly DependencyProperty TextBoxBackgroundProperty =
        DependencyProperty.Register("TextBoxBackground", typeof(SolidColorBrush), typeof(TextBoxWithLabel),
            new PropertyMetadata(Brushes.White));

    public static readonly DependencyProperty TextBoxFocusBackgroundProperty =
        DependencyProperty.Register("TextBoxFocusBackground", typeof(SolidColorBrush), typeof(TextBoxWithLabel),
            new PropertyMetadata(Brushes.LightGoldenrodYellow));

    public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", 
        RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBoxWithLabel));
    public enum CTextBoxType
    {
        AlphaNumeric=0,
        OnlyNumbers=1,
        FloatValue=2,
        IntegerValue=3,
        LongValue=4,
        Date=5,
        Time=6,
        NegativeValues=7
    }
    public event PropertyChangedEventHandler PropertyChanged;
    const int ControlHeight=36;
    public TextBoxWithLabel()
    {
        InitializeComponent();

        GrdCText.DataContext = this;
    }

    #region CUSTOM PROPERTIES
    public event RoutedEventHandler TextChanged
    {
        add { AddHandler(TextChangedEvent, value); }
        remove { RemoveHandler(TextChangedEvent, value); }
    }
    public string KeyValue
    {
        get { return (string)GetValue(KeyValueProperty); }
        set { SetValue(KeyValueProperty, value); }
    }
    public new HorizontalAlignment HorizontalContentAlignment
    {
        get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty); }
        set { SetValue(HorizontalContentAlignmentProperty, value); }
    }
    public string Label
    {
        get { return (string)GetValue(LabelProperty); }
        set { SetValue(LabelProperty, value); }
    }
    public int LabelWidth
    {
        get { return (int)GetValue(LabelWidthProperty); }
        set {SetValue(LabelWidthProperty, value);}
    }
    public string Value
    {
        get { return (string)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }
    public int IntValue
    {
        get { int.TryParse(Value, out int xInt);return xInt; }
    }
    public long LongValue
    {
        get { long.TryParse(Value, out long xLong); return xLong; }
    }
    public float FloatValue
    {
        get { float.TryParse(Value, out float xFloat); return xFloat; }
    }
    public decimal DecimalValue
    {
        get { decimal.TryParse(Value, out decimal xDecimal); return xDecimal; }
    }
    public DateTime DateValue
    {
        get { DateTime.TryParse(Value, out DateTime xDate); return xDate; }
    }
    public DateOnly DateOnlyValue
    {
        get { DateOnly.TryParse(Value, out DateOnly xDate); return xDate; }
    }
    public TimeOnly TimeOnlyValue
    {
        get 
        {
            string xValue = Value;
            if (Value.Contains("."))
            {
                string[] xTimePart = Value.Split(".");

                if (xTimePart.Length != 2)
                {
                    return TimeOnly.MinValue;
                }
                xValue = xTimePart[0] + ":" + xTimePart[1];
            }    
            TimeOnly.TryParse(xValue, out TimeOnly xTime); 
            return xTime; 
        }
    }
    public int MaxLength
    {
        get { return (int)GetValue(MaxLengthProperty); }
        set { SetValue(MaxLengthProperty, value); }
    }
    public string TextRequiredMessage
    {
        get { return (string)GetValue(TextRequiredMessageProperty); }
        set {SetValue(TextRequiredMessageProperty, value); this.NotifyPropertyChanged();}
    }
    public string ErrorMessage
    {
        get { return (string)GetValue(ErrorMessageProperty); }
        set { SetValue(ErrorMessageProperty, value); }
    }
    public CTextBoxType TextBoxType
    {
        get { return (CTextBoxType)GetValue(TextBoxTypeProperty); }
        set { SetValue(TextBoxTypeProperty, value);}
    }
    public bool ReadOnly
    {
        get { return (bool)GetValue(ReadOnlyProperty); }
        set { SetValue(ReadOnlyProperty, value); }
    }
    public SolidColorBrush TextBoxBackground
    {
        get { return (SolidColorBrush)GetValue(TextBoxBackgroundProperty); }
        set { SetValue(TextBoxBackgroundProperty, value); }
    }
    public SolidColorBrush TextBoxFocusBackground
    {
        get { return (SolidColorBrush)GetValue(TextBoxFocusBackgroundProperty); }
        set { SetValue(TextBoxFocusBackgroundProperty, value); }
    }
    public bool IsValidationOk {get {return this.Validate();}}
    #endregion

    #region METHODS
    protected void NotifyPropertyChanged([CallerMemberName] string name=null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public void Clear()
    {
        this.TxtTextBox.Clear();
    }
    public void SelectedAll()
    {
        this.TxtTextBox.SelectAll();
    }
    public new void Focus()
    {
        this.TxtTextBox.Focus();
    }
    private bool Validate()
    {
        switch (this.TextBoxType)
        {
            case CTextBoxType.OnlyNumbers:
                {
                    if (this.TxtTextBox.Text.Trim() == "")
                    {
                        this.DisplayErrorMessage(this.ErrorMessage);
                        return false;
                    }
                    break;
                }
            case CTextBoxType.IntegerValue:
                {
                    int.TryParse(this.TxtTextBox.Text, out int xValue);
                    if (xValue == 0)
                    {
                        this.DisplayErrorMessage(this.ErrorMessage);
                        return false;
                    }
                    break;
                }
            case CTextBoxType.LongValue:
                {
                    long.TryParse(this.TxtTextBox.Text, out long xValue);
                    if (xValue == 0)
                    {
                        this.DisplayErrorMessage(this.ErrorMessage);
                        return false;
                    }
                    break;
                }
            case CTextBoxType.FloatValue:
                {
                    float.TryParse(this.TxtTextBox.Text, out float xValue);
                    if (xValue == 0)
                    {
                        this.DisplayErrorMessage(this.ErrorMessage);
                        return false;
                    }
                    break;
                }
            case CTextBoxType.Date:
                {
                    if (this.TxtTextBox.Text.Trim() == "")
                    {
                        this.DisplayErrorMessage(this.ErrorMessage);
                        return false;
                    }
                    
                    if (GetDateFromText(this.TxtTextBox.Text) == DateTime.MinValue)
                    {
                        this.DisplayErrorMessage("Please enter Date in correct format");
                        return false;
                    }
                    break;
                }
            case CTextBoxType.Time:
                {
                    if (this.TxtTextBox.Text.Trim() == "")
                    {
                        this.DisplayErrorMessage(this.ErrorMessage);
                        return false;
                    }
                    return IsTimeCorrect(this.TxtTextBox.Text);
                }
            default:
                {
                    if (this.TxtTextBox.Text.Trim() == "")
                    {
                        this.DisplayErrorMessage(this.ErrorMessage);
                        return false;
                    }
                    break;
                }
        }

        //If Everything is ok then reduce the height and remove error message
        this.LblErrorMessage.Text = "";
        this.Height = ControlHeight;
        return true;
    }
    private static bool IsTimeCorrect(string TextValue)
    {
        string splitChar = "";
        if (TextValue.Contains(".")) { splitChar = "."; }
        if (TextValue.Contains(":")) { splitChar = ":"; }

        string[] xTimePart = TextValue.Split(splitChar);

        if (xTimePart.Length != 2)
        {
            return false;
        }
        int.TryParse(xTimePart[0], out int TimePart1);
        int.TryParse(xTimePart[1], out int TimePart2);

        if (!(TimePart1 >=0 && TimePart1 <=24))
        {
            return false;
        }
        if (!(TimePart2 >=0 && TimePart2 <=60))
        {
            return false;
        }

        return true;
    }

    private static DateTime GetDateFromText(string TextValue)
    {
        if (!IsDateValid(TextValue))
        {
            return DateTime.MinValue;
        }

        string splitChar = "";
        if (TextValue.Contains(".")) { splitChar = "."; }
        if (TextValue.Contains("/")) { splitChar = "/"; }
        if (TextValue.Contains("-")) { splitChar = "-"; }

        string[] xDatePart = TextValue.Split(splitChar);

        if (xDatePart.Length != 3)
        {
            return DateTime.MinValue;
        }

        int.TryParse(xDatePart[0], out int Day);
        int.TryParse(xDatePart[1], out int Month);
        int.TryParse(xDatePart[2], out int Year);

        string FormatedDate = Day.ToString("00") + "/" + Month.ToString("00") + "/" + Year.ToString("0000");

        DateTime.TryParseExact(FormatedDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateTime xDate);

        return xDate;
    }
    private void DisplayErrorMessage(string ErrorMessage)
    {
        if (ErrorMessage.Trim()!="")
        {
            this.LblErrorMessage.Text = ErrorMessage;
            this.Height = this.TxtTextBox.Height + this.LblErrorMessage.Height;
        }
        else
        {
            this.Height = ControlHeight;
        }
    }
    public static bool IsDateValid(string DateValue)
    {
        List<int> MonthWith30Days = new () { 4, 6, 9, 11 };
        List<int> MonthWith31Days = new () { 1, 3, 5, 7, 8, 10, 12 };

        string splitChar = "";
        if (DateValue.Contains(".")) { splitChar = "."; }
        if (DateValue.Contains("/")) { splitChar = "/"; }
        if (DateValue.Contains("-")) { splitChar = "-"; }

        string[] xDatePart = DateValue.Split(splitChar);

        if (xDatePart.Length != 3)
        {
            return false;
        }

        int.TryParse(xDatePart[0], out int Day);
        int.TryParse(xDatePart[1], out int Month);
        int.TryParse(xDatePart[2], out int Year);

        if (Month < 0 || Month > 12)
        {
            return false;
        }
        if (Year < 1901 || Year > 9999)
        {
            return false;
        }
        if (Day < 0)
        {
            return false;
        }
        if (Month == 2)
        {
            if (Year / 4 == 0)
            {
                //Leap year
                if (Day > 29)
                {
                    return false;
                }
                else if (Day > 28)
                {
                    return false;
                }
            }
        }
        if (MonthWith30Days.Contains(Month))
        {
            if (Day > 30)
            {
                return false;
            }
        }

        if (MonthWith31Days.Contains(Month))
        {
            if (Day > 31)
            {
                return false;
            }
        }
        return true;
    }
    void RaiseTextChangeEvent()
    {
        RoutedEventArgs newEventArgs = new RoutedEventArgs(TextBoxWithLabel.TextChangedEvent);
        RaiseEvent(newEventArgs);
    }
    #endregion

    #region EVENTS
    private void TxtTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (this.TextBoxType == CTextBoxType.FloatValue)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch(TxtTextBox.Text.Insert(TxtTextBox.SelectionStart, e.Text));
        }
        else if (this.TextBoxType == CTextBoxType.NegativeValues)
        {
            Regex reg = new Regex(@"^-?\d+[.]?\d*$");
            e.Handled = !reg.IsMatch(TxtTextBox.Text.Insert(TxtTextBox.SelectionStart, e.Text));
        }
        else if (this.TextBoxType == CTextBoxType.OnlyNumbers || this.TextBoxType==CTextBoxType.IntegerValue ||
            this.TextBoxType==CTextBoxType.LongValue)
        {
            Regex regex = new Regex("^[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

    private void TextBoxWithLabel_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.ReadOnly)
        {
            this.TextBoxBackground = Brushes.WhiteSmoke;
        }
        TxtTextBox.Background = TextBoxBackground;

        if (TxtTextBox.IsFocused && !this.ReadOnly)
        {
            this.TxtTextBox_GotFocus(TxtTextBox, e);
        }

        if (this.TextBoxType==CTextBoxType.AlphaNumeric)
        {
            this.TxtTextBox.HorizontalContentAlignment = HorizontalAlignment.Left;
        }
        else if(this.TextBoxType==CTextBoxType.Date)
        {
            this.TxtTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;
        }
        else
        {
            this.TxtTextBox.HorizontalContentAlignment = HorizontalAlignment.Right;
        }
    }

    private void TxtTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (ReadOnly==false)
        {
            if(this.TxtTextBox.Text.Length>0)
            {
                this.TxtTextBox.SelectAll();
            }
            TxtTextBox.Background = TextBoxFocusBackground;
        }
    }
    private void TxtTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        TxtTextBox.Background = TextBoxBackground;
        if (this.TextRequiredMessage.Trim()!="") { this.Validate(); }
        else { this.Height = ControlHeight; }
    }
    private void TxtTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        this.RaiseTextChangeEvent();
    }
    #endregion
}