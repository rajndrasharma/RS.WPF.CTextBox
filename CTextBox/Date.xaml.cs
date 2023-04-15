namespace RS.WPF.CTextBox;
/// <summary>
/// Interaction logic for Date.xaml
/// </summary>
public partial class DateTextBox : UserControl
{
    public static readonly DependencyProperty KeyValueProperty =
        DependencyProperty.Register("KeyValue", typeof(string), typeof(DateTextBox),
            new PropertyMetadata(""));

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register("Label", typeof(string), typeof(DateTextBox),
            new PropertyMetadata(""));

    public static readonly DependencyProperty LabelWidthProperty =
       DependencyProperty.Register("LabelWidth", typeof(int), typeof(DateTextBox),
           new PropertyMetadata(170));

    public static readonly DependencyProperty DateFormatProperty =
        DependencyProperty.Register("DateFormat", typeof(object), typeof(DateTextBox),
            new PropertyMetadata(DatePickerFormat.Long));

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(object), typeof(DateTextBox),
            new PropertyMetadata(""));

    public static readonly DependencyProperty SelectedDateValueProperty =
        DependencyProperty.Register("SelectedDateValue", typeof(object), typeof(DateTextBox),
            new PropertyMetadata(DateTime.Now));

    public static readonly DependencyProperty TextRequiredMessageProperty =
        DependencyProperty.Register("TextRequiredMessage", typeof(string), typeof(DateTextBox),
            new PropertyMetadata(""));

    public static readonly DependencyProperty ErrorMessageProperty =
        DependencyProperty.Register("ErrorMessage", typeof(string), typeof(DateTextBox),
            new PropertyMetadata(""));


    public static new readonly DependencyProperty HorizontalContentAlignmentProperty =
        DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(DateTextBox),
            new PropertyMetadata(HorizontalAlignment.Left));


    public static readonly DependencyProperty TextBoxBackgroundProperty =
        DependencyProperty.Register("TextBoxBackground", typeof(SolidColorBrush), typeof(DateTextBox),
            new PropertyMetadata(Brushes.White));

    public static readonly DependencyProperty TextBoxFocusBackgroundProperty =
        DependencyProperty.Register("TextBoxFocusBackground", typeof(SolidColorBrush), typeof(DateTextBox),
            new PropertyMetadata(Brushes.LightGoldenrodYellow));

    public event PropertyChangedEventHandler PropertyChanged;
    public event SelectionChangedEventHandler SelectionChanged;
     
    const int ControlHeight = 36;

    public DateTextBox()
    {
        InitializeComponent();
        GrdCText.DataContext = this;
    }

    #region CUSTOM PROPERTIES
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
        set { SetValue(LabelWidthProperty, value); }
    }
    public DatePickerFormat DateFormat
    {
        get { return (DatePickerFormat)(int)GetValue(DateFormatProperty); }
        set { SetValue(DateFormatProperty, value); }
    }
    public string Value
    {
        get { return (string)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }
    public DateTime SelectedDateValue
    {
        get { return (DateTime)GetValue(SelectedDateValueProperty); }
        set { SetValue(SelectedDateValueProperty, value); }
    }
    public DateOnly DateOnlyValue
    {
        get { DateOnly.TryParse(SelectedDateValue.ToString(), out DateOnly xDate); return xDate; }
    }
    public string TextRequiredMessage
    {
        get { return (string)GetValue(TextRequiredMessageProperty); }
        set { SetValue(TextRequiredMessageProperty, value); this.NotifyPropertyChanged(); }
    }
    public string ErrorMessage
    {
        get { return (string)GetValue(ErrorMessageProperty); }
        set { SetValue(ErrorMessageProperty, value); }
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
    public bool IsValidationOk { get { return this.Validate(); } }
    #endregion

    #region METHODS
    protected void NotifyPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public void Clear()
    {
        this.TxtDate.Text=DateTime.Now.ToString();
    }
    public new void Focus()
    {
        this.TxtDate.Focus();
    }
    private void DisplayErrorMessage(string ErrorMessage)
    {
        if (ErrorMessage.Trim() != "")
        {
            this.LblErrorMessage.Text = ErrorMessage;
            this.Height = this.TxtDate.Height + this.LblErrorMessage.Height;
        }
        else
        {
            this.Height = ControlHeight;
        }
    }
    private bool Validate()
    {
        if (this.TxtDate.Text=="")
        {
            this.DisplayErrorMessage(this.ErrorMessage);
            return false;
        }

        //If Everything is ok then reduce the height and remove error message
        this.LblErrorMessage.Text = "";
        this.Height = ControlHeight;
        return true;
    }
    #endregion

    #region EVENTS
    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (!this.IsEnabled)
        {
            this.TextBoxBackground = Brushes.WhiteSmoke;
        }
        TxtDate.Background = TextBoxBackground;

        if (TxtDate.IsFocused && this.IsEnabled)
        {
            this.TxtDate_GotFocus(TxtDate, e);
        }
    }

    private void TxtDate_GotFocus(object sender, RoutedEventArgs e)
    {
        if (this.IsEnabled == true)
        {
            TxtDate.Background = TextBoxFocusBackground;
        }
    }
    private void TxtDate_LostFocus(object sender, RoutedEventArgs e)
    {
        TxtDate.Background = TextBoxBackground;
        if (this.TextRequiredMessage.Trim() != "") { this.Validate(); }
        else { this.Height = ControlHeight; }
    }
    private void TxtDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectionChanged != null)
        {
            SelectionChanged(TxtDate, e);
        }
    }
    #endregion
}
