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
            ServiceCost_TextBox.PreviewTextInput += EnsureServiceCostIsNumeric;
            Normal_ListView.SelectionChanged += ClickRegularService;
            Completed_ListView.PreviewMouseDoubleClick += DoubleClickFinishedListItem;
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
            Drone drone = new Drone(ClientName_TextBox.Text, DroneModel_TextBox.Text, ServiceProblem_TextBox.Text, 100, "Tag");
            Clear();
            switch (GetServicePriority())
            {
                case 0:
                    RegularService.Enqueue(drone);
                    DisplayRegularService();
                    SetStatusMessage("Added new entry to Regular Service");
                    break;
                case 1:
                    ExpressService.Enqueue(drone);
                    DisplayExpressService();
                    SetStatusMessage("Added new entry to Express Service");
                    break;
                default:
                    break;
            }
            
            
            
            // IncrementServiceTagControl();
        }

        /// <summary>
        /// 6.6 Before a new service item is added to the Express Queue the service cost must be increased by 15%.
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public float AddPriorityPricing(float _value)
        {
            return _value * 1.15f;
        }

        /// <summary>
        /// 6.7 Create a custom method called “GetServicePriority” which returns the value of the priority radio group.
        /// This method must be called inside the “AddNewItem” method before the new service item is added to a queue.
        /// </summary>
        /// <returns></returns>
        public int GetServicePriority()
        {
            if (Radio_Normal.IsChecked.GetValueOrDefault(false))
            {
                return 0;
            }
            if (Radio_Priority.IsChecked.GetValueOrDefault(false))
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
            // TODO
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
            // TODO
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
        public void IncrementServiceTagControl(int _value)
        {

        }

        /// <summary>
        /// 6.12 Create a mouse click method for the regular service ListView that will display the Client Name and Service Problem in the related textboxes.
        /// </summary>
        public void ClickRegularService(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                //ClientName_TextBox.Text = e.AddedItems[0].ToString();
            }
        }

        /// <summary>
        /// 6.13 Create a mouse click method for the express service ListView that will display the Client Name and Service Problem in the related textboxes.
        /// </summary>
        public void ClickExpressService(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// 6.14 Create a button click method that will remove a service item from the regular ListView and dequeue the regular service Queue<T> data structure.
        /// The dequeued item must be added to the List<T> and displayed in the ListBox for finished service items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClickRegularServiceRemove(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// 6.15 Create a button click method that will remove a service item from the express ListView and dequeue the express service Queue<T> data structure.
        /// The dequeued item must be added to the List<T> and displayed in the ListBox for finished service items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClickExpressServiceRemove(object sender, MouseButtonEventArgs e)
        {

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
            //ServiceTag_Dropdown.Text = string.Empty;
        }

        // 6.18 All code is required to be adequately commented. Map the programming criteria and features to your code/methods by adding comments above the method signatures.
        // Ensure your code is compliant with the CITEMS coding standards (refer http://www.citems.com.au/).

        public void SetStatusMessage(string _msg, bool _showWindow = false, string _windowTitle = "Message")
        {
            StatusMessage_TextBox.Text = _msg;
            if(_showWindow)
            {
                Window win = new Window();
                win.Title = _windowTitle;
                win.Show();
            }
        }
    }
}
