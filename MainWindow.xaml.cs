using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace DroneServiceApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Add_Button.Click += Add_Button_Click;
            Clear_Button.Click += Clear_Button_Click;
            ServiceTag_UpDown.ValueChanged += IncrementServiceTagControl;
            ServiceCost_TextBox.PreviewTextInput += EnsureServiceCostIsNumeric;
            Normal_ListView.SelectionChanged += DisplayRegularServicesIntoTextBoxes;
            Priority_ListView.SelectionChanged += DisplayExpressServicesIntoTextBoxes;
            Completed_ListView.PreviewMouseDoubleClick += DoubleClickFinishedListItem;
            FinishRegular_Button.Click += FinishRegularService;
            FinishExpress_Button.Click += FinishExpressService;
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem();
        }
        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }


        /// <summary>
        /// 6.2 Create a global List<T> of type Drone called “FinishedList”. 
        /// </summary>
        private List<Drone> FinishedList = new List<Drone>();

        /// <summary>
        /// 6.3 Create a global Queue<T> of type Drone called “RegularService”.
        /// </summary>
        private Queue<Drone> RegularService = new Queue<Drone>();

        /// <summary>
        /// 6.4 Create a global Queue<T> of type Drone called “ExpressService”.
        /// </summary>
        private Queue<Drone> ExpressService = new Queue<Drone>();

        /// <summary>
        /// 6.5 Create a button method called “AddNewItem” that will add a new service item to a Queue<> based on the priority.
        /// Use TextBoxes for the Client Name, Drone Model, Service Problem and Service Cost. Use a numeric control for the Service Tag.
        /// The new service item will be added to the appropriate Queue based on the Priority radio button.
        /// </summary>
        public void AddNewItem()
        {
            // TODO
            if(CheckIfTextboxesAreInvalid())
            {
                return;
            }

            int priority = GetServicePriority();
            Drone drone = new Drone(ClientName_TextBox.Text, DroneModel_TextBox.Text, ServiceProblem_TextBox.Text, CalculatePricing(100, priority), ServiceTag_TextBox.Text);
            Clear();
            switch (priority)
            {
                case 0: // Normal
                    RegularService.Enqueue(drone);
                    DisplayRegularService();
                    SetStatusMessage("Added new entry to Regular Service");
                    break;
                case 1: // Express
                    ExpressService.Enqueue(drone);
                    DisplayExpressService();
                    SetStatusMessage("Added new entry to Express Service");
                    break;
                default:
                    break;
            }
            
        }

        /// <summary>
        /// 6.6 Before a new service item is added to the Express Queue the service cost must be increased by 15%.
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public float CalculatePricing(float _value, int _priority)
        {
            switch (_priority)
            {
                case 0: // Normal
                    break;
                case 1: // Express
                    _value = _value * 1.15f;
                    break;
                default:
                    break;
            }
            return _value;
        }

        /// <summary>
        /// 6.7 Create a custom method called “GetServicePriority” which returns the value of the priority radio group.
        /// This method must be called inside the “AddNewItem” method before the new service item is added to a queue.
        /// </summary>
        /// <returns></returns>
        public int GetServicePriority()
        {
            if (Radio_Regular.IsChecked.GetValueOrDefault(false))
            {
                return 0;
            }
            if (Radio_Express.IsChecked.GetValueOrDefault(false))
            {
                return 1;
            }
            return -1;
        }

        /// <summary>
        /// 6.8 Create a custom method that will display all the elements in the RegularService queue.
        /// The display must use a List View and with appropriate column headers.
        /// </summary>
        public void DisplayRegularService()
        {
            Normal_ListView.Items.Clear();
            foreach (var item in RegularService)
            {
                Normal_ListView.Items.Add(new
                {
                    Name = item.GetClientName(),
                    Model = item.GetDroneModel(),
                    Problem = item.GetServiceProblem(),
                    Cost = item.GetServiceCost(),
                    Tag = item.GetServiceTag()
                });
            }
        }

        /// <summary>
        /// 6.9 Create a custom method that will display all the elements in the ExpressService queue.
        /// The display must use a List View and with appropriate column headers.
        /// </summary>
        public void DisplayExpressService()
        {
            Priority_ListView.Items.Clear();
            foreach (var item in ExpressService)
            {
                Priority_ListView.Items.Add(new
                {
                    Name = item.GetClientName(),
                    Model = item.GetDroneModel(),
                    Problem = item.GetServiceProblem(),
                    Cost = item.GetServiceCost(),
                    Tag = item.GetServiceTag()
                });
            }
        }

        /// <summary>
        /// 6.10 Create a custom method to ensure the Service Cost textbox can only accept a double value with two decimal point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EnsureServiceCostIsNumeric(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
                double val;
                e.Handled = !double.TryParse(fullText, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out val);
            }
        }

        /// <summary>
        /// 6.11 Create a custom method to increment the service tag control,
        /// this method must be called inside the “AddNewItem” method before the new service item is added to a queue.
        /// </summary>
        /// <param name="_value"></param>
        public void IncrementServiceTagControl(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            IntegerUpDown updown = (IntegerUpDown)sender;
            int newVal = (int)e.NewValue;
            int maxVal = 4;
            if(newVal < 0) { updown.Value = maxVal; }
            if(newVal > maxVal) { updown.Value = 0; }

            switch (newVal)
            {
                case 0:
                    ServiceTag_TextBox.Text = "Tag 1";
                    break;
                case 1:
                    ServiceTag_TextBox.Text = "Tag 2";
                    break;
                case 2:
                    ServiceTag_TextBox.Text = "Tag 3";
                    break;
                case 3:
                    ServiceTag_TextBox.Text = "Tag 4";
                    break;
                case 4:
                    ServiceTag_TextBox.Text = "Tag 5";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 6.12 Create a mouse click method for the regular service ListView that will display the Client Name and Service Problem in the related textboxes.
        /// </summary>
        public void DisplayRegularServicesIntoTextBoxes(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                string[] values = OutputDroneData(e.AddedItems[0].ToString());
                ClientName_TextBox.Text = values[0];
                DroneModel_TextBox.Text = values[1];
                ServiceProblem_TextBox.Text = values[2];
                ServiceCost_TextBox.Text = values[3];
                ServiceTag_TextBox.Text = values[4];
            }
        }

        /// <summary>
        /// 6.13 Create a mouse click method for the express service ListView that will display the Client Name and Service Problem in the related textboxes.
        /// </summary>
        public void DisplayExpressServicesIntoTextBoxes(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string[] values = OutputDroneData(e.AddedItems[0].ToString());
                ClientName_TextBox.Text = values[0];
                DroneModel_TextBox.Text = values[1];
                ServiceProblem_TextBox.Text = values[2];
                ServiceCost_TextBox.Text = values[3];
                ServiceTag_TextBox.Text = values[4];
            }
        }

        /// <summary>
        /// 6.14 Create a button click method that will remove a service item from the regular ListView and dequeue the regular service Queue<T> data structure.
        /// The dequeued item must be added to the List<T> and displayed in the ListBox for finished service items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FinishRegularService(object sender, RoutedEventArgs e)
        {
            Drone drone = RegularService.Dequeue();
            FinishedList.Add(drone);

            Completed_ListView.Items.Clear();
            foreach (var item in FinishedList)
            {
                Completed_ListView.Items.Add(new
                {
                    Name = item.GetClientName(),
                    Model = item.GetDroneModel(),
                    Problem = item.GetServiceProblem(),
                    Cost = item.GetServiceCost(),
                    Tag = item.GetServiceTag()
                });
            }

            DisplayRegularService();
            Clear();
        }

        /// <summary>
        /// 6.15 Create a button click method that will remove a service item from the express ListView and dequeue the express service Queue<T> data structure.
        /// The dequeued item must be added to the List<T> and displayed in the ListBox for finished service items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FinishExpressService(object sender, RoutedEventArgs e)
        {
            Drone drone = ExpressService.Dequeue();
            FinishedList.Add(drone);

            Completed_ListView.Items.Clear();
            foreach (var item in FinishedList)
            {
                Completed_ListView.Items.Add(new
                {
                    Name = item.GetClientName(),
                    Model = item.GetDroneModel(),
                    Problem = item.GetServiceProblem(),
                    Cost = item.GetServiceCost(),
                    Tag = item.GetServiceTag()
                });
            }

            DisplayExpressService();
            Clear();
        }

        /// <summary>
        /// 6.16 Create a double mouse click method that will delete a service item from the finished listbox and remove the same item from the List<T>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DoubleClickFinishedListItem(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// 6.17 Create a custom method that will clear all the textboxes after each service item has been added.
        /// </summary>
        public void Clear()
        {
            ClientName_TextBox.Text = string.Empty;
            DroneModel_TextBox.Text = string.Empty;
            ServiceProblem_TextBox.Text = string.Empty;
            ServiceCost_TextBox.Text = string.Empty;
            ServiceTag_TextBox.Text = string.Empty;
        }

        // 6.18 All code is required to be adequately commented. Map the programming criteria and features to your code/methods by adding comments above the method signatures.
        // Ensure your code is compliant with the CITEMS coding standards (refer http://www.citems.com.au/).

        private void SetStatusMessage(string _msg, bool _showWindow = false, string _windowTitle = "Message")
        {
            StatusMessage_TextBox.Text = _msg;
            if(_showWindow)
            {
                Window win = new Window();
                win.Title = _windowTitle;
                win.Show();
            }
        }
        private bool CheckIfTextboxesAreInvalid()
        {
            int invalidInputs = 0;
            string msg = string.Empty;

            if(GetServicePriority() < 0)
            {
                msg += "Please select priority level." + Environment.NewLine;
                invalidInputs++;
            }
            if(string.IsNullOrEmpty(ClientName_TextBox.Text))
            {
                msg += "Please input Client Name." + Environment.NewLine;
                invalidInputs++;
            }
            if (string.IsNullOrEmpty(DroneModel_TextBox.Text))
            {
                msg += "Please input Drone Model." + Environment.NewLine;
                invalidInputs++;
            }
            if (string.IsNullOrEmpty(ServiceProblem_TextBox.Text))
            {
                msg += "Please input Service Problem." + Environment.NewLine;
                invalidInputs++;
            }
            if (string.IsNullOrEmpty(ServiceCost_TextBox.Text))
            {
                msg += "Please input Service Cost." + Environment.NewLine;
                invalidInputs++;
            }
            if (string.IsNullOrEmpty(ServiceTag_TextBox.Text))
            {
                msg += "Please input Service Tag." + Environment.NewLine;
                invalidInputs++;
            }

            if(invalidInputs > 0)
            {
                System.Windows.MessageBox.Show(msg);
                return true;
            }
            else
            {

                return false;
            }
        }

        private string[] OutputDroneData(string _value)
        {
            // { Name = scac, Model = ascasc, Problem = fasc, Cost = 100, Tag = Tag 1 }

            string[] values = _value.Split(",");
            values[0] = values[0].Replace("{ Name = ", "");
            values[1] = values[1].Replace(" Model = ", "");
            values[2] = values[2].Replace(" Problem = ", "");
            values[3] = values[3].Replace(" Cost = ", "");
            values[4] = values[4].Replace(" Tag = ", "");
            values[4] = values[4].Replace(" }", "");

            return values;
        }
    }
}
