namespace RS.WPF.CTextBox;

/// <summary>
/// Interaction logic for MultiLineTextBox.xaml
/// </summary>
public partial class MultiLineTextBox : UserControl, INotifyPropertyChanged
{
    public static readonly DependencyProperty KeyValueProperty =
        DependencyProperty.Register("KeyValue", typeof(string), typeof(MultiLineTextBox),
            new PropertyMetadata(""));

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register("Label", typeof(string), typeof(MultiLineTextBox),
            new PropertyMetadata(""));

    public static readonly DependencyProperty LabelWidthProperty =
       DependencyProperty.Register("LabelWidth", typeof(int), typeof(MultiLineTextBox),
           new PropertyMetadata(170));

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(object), typeof(MultiLineTextBox),
            new PropertyMetadata(""));

    public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.Register("MaxLength", typeof(object), typeof(MultiLineTextBox),
            new PropertyMetadata(0));

    public static readonly DependencyProperty TextBoxHeightProperty =
        DependencyProperty.Register("TextBoxHeight", typeof(double), typeof(MultiLineTextBox),
            new PropertyMetadata(100.0));

    public static readonly DependencyProperty TextRequiredMessageProperty =
        DependencyProperty.Register("TextRequiredMessage", typeof(string), typeof(MultiLineTextBox),
            new PropertyMetadata(""));

    public static readonly DependencyProperty ErrorMessageProperty =
        DependencyProperty.Register("ErrorMessage", typeof(string), typeof(MultiLineTextBox),
            new PropertyMetadata(""));

    public static new readonly DependencyProperty HorizontalContentAlignmentProperty =
        DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(MultiLineTextBox),
            new PropertyMetadata(HorizontalAlignment.Left));

    public static readonly DependencyProperty ReadOnlyProperty =
        DependencyProperty.Register("ReadOnly", typeof(bool), typeof(MultiLineTextBox),
            new PropertyMetadata(false));

    public static readonly DependencyProperty TextBoxBackgroundProperty =
        DependencyProperty.Register("TextBoxBackground", typeof(SolidColorBrush), typeof(MultiLineTextBox),
            new PropertyMetadata(Brushes.White));

    public static readonly DependencyProperty TextBoxFocusBackgroundProperty =
        DependencyProperty.Register("TextBoxFocusBackground", typeof(SolidColorBrush), typeof(MultiLineTextBox),
            new PropertyMetadata(Brushes.LightGoldenrodYellow));

    public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged",
        RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MultiLineTextBox));
    

    public event PropertyChangedEventHandler PropertyChanged;
    public MultiLineTextBox()
    {
        InitializeComponent();

        GrdCText.DataContext = this;
        this.Height=this.TxtTextBox.Height+6;
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
        set { SetValue(LabelWidthProperty, value); }
    }
    public string Value
    {
        get { return (string)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }
    public int MaxLength
    {
        get { return (int)GetValue(MaxLengthProperty); }
        set { SetValue(MaxLengthProperty, value); }
    }
    public double TextBoxHeight
    {
        get { return Convert.ToDouble(GetValue(TextBoxHeightProperty)); }
        set { SetValue(TextBoxHeightProperty, value); }
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
    public bool IsValidationOk { get { return this.Validate(); } }
    #endregion

    #region METHODS
    protected void NotifyPropertyChanged([CallerMemberName] string name = null)
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
        if (this.TxtTextBox.Text.Trim() == "")
        {
            this.DisplayErrorMessage(this.ErrorMessage);
            return false;
        }

        //If Everything is ok then reduce the height and remove error message
        this.LblErrorMessage.Text = "";
        this.Height = (double)(this.TextBoxHeight + 6);
        return true;
    }
    private void DisplayErrorMessage(string ErrorMessage)
    {
        if (ErrorMessage.Trim() != "")
        {
            this.LblErrorMessage.Text = ErrorMessage;
            this.Height = (double)(this.TxtTextBox.Height + this.LblErrorMessage.Height);
        }
        else
        {
            this.Height = (double)(this.TextBoxHeight+6);
        }
    }
    void RaiseTextChangeEvent()
    {
        RoutedEventArgs newEventArgs = new RoutedEventArgs(TextBoxWithLabel.TextChangedEvent);
        RaiseEvent(newEventArgs);
    }
    #endregion

    #region EVENTS
    private void MultiLineTextBox_Loaded(object sender, RoutedEventArgs e)
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
        this.TxtTextBox.Height= (double)this.TextBoxHeight;
        this.Height = (double)(this.TextBoxHeight + 6);
    }

    private void TxtTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (ReadOnly == false)
        {
            if (this.TxtTextBox.Text.Trim() == "0")
            {
                this.TxtTextBox.SelectAll();
            }
            TxtTextBox.Background = TextBoxFocusBackground;
        }
    }
    private void TxtTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        TxtTextBox.Background = TextBoxBackground;
        if (this.TextRequiredMessage.Trim() != "") { this.Validate(); }
        else { this.Height = (double)(this.TextBoxHeight + 6); }
    }
    private void TxtTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        this.RaiseTextChangeEvent();
    }
    #endregion
}
