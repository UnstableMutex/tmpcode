  public class Period
    {
        public Period()
        {
           
        }
        static Period()
        {
            now = new Period(DateTime.Today.Month,DateTime.Today.Year);
        }
        private static Period now;
        public static Period Now
        { get { return now; } }
        public override bool Equals(object obj)
        {
            var a = obj as Period;

            if (a == null)
            {
                return false;
            }
            else
            {
                return a.Year == Year & a.Month == Month;
            }

        }
        public override int GetHashCode()
        {
            return Month * Year;
        }
        public Period(int month, int year)
        {
            Month = month;
            Year = year;
        }
        public int Month { get; set; }
        public int Year { get; set; }
    }
    
      public partial class MonthYear : UserControl
    {
        static MonthYear()
        {
            
        }
        public MonthYear()
        {
            InitializeComponent();
            FillMonthes();
        }

        private void FillMonthes()
        {
            for (int i = 0; i < 12; i++)
            {
                m.Items.Add(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i]);
            }
            for (int i = DateTime.Today.Year-2; i < DateTime.Today.Year+3; i++)
            {
                y.Items.Add(i);

            }
           
          
        }


        public Period Period
        {
            get { return (Period)GetValue(PeriodProperty); }
            set { SetValue(PeriodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Period.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PeriodProperty =
            DependencyProperty.Register("Period", typeof(Period), typeof(MonthYear), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,PropertyChanged));
        static void PropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {            var s = sender as MonthYear;
           
             
            var nv = e.NewValue as Period;
            s.y.SelectedValue = nv.Year;
            s.m.SelectedIndex = nv.Month - 1;   
            

        }

        private void M_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                Period=new Period(m.SelectedIndex+1,(int)y.SelectedValue);
            }
            catch (NullReferenceException exception)
            {
               
            }
        }
    }
    
    <UserControl x:Class="WpfApplication1.MonthYear"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             mc:Ignorable="d" 
            >
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="167*"/>
            <ColumnDefinition Width="133*"/>
        </Grid.ColumnDefinitions>
        <ComboBox Name="m" SelectionChanged="M_OnSelectionChanged" />
        <ComboBox Name="y" SelectionChanged="M_OnSelectionChanged" Grid.Column="1"/>

    </Grid>
</UserControl>
