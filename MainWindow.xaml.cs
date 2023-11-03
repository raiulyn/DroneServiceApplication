﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
            ClientName_TextBox.PreviewGotKeyboardFocus += Focus_Text;
            ClientName_TextBox.PreviewLostKeyboardFocus += Unfocus_Text;
            DroneModel_TextBox.PreviewGotKeyboardFocus += Focus_Text;
            DroneModel_TextBox.PreviewLostKeyboardFocus += Unfocus_Text;
            ServiceCost_TextBox.PreviewGotKeyboardFocus += Focus_Text;
            ServiceCost_TextBox.PreviewLostKeyboardFocus += Unfocus_Text;
            ServiceTag_TextBox.PreviewGotKeyboardFocus += Focus_Text;
            ServiceTag_TextBox.PreviewLostKeyboardFocus += Unfocus_Text;
            ServiceProblem_TextBox.PreviewGotKeyboardFocus += Focus_Text;
            ServiceProblem_TextBox.PreviewLostKeyboardFocus += Unfocus_Text;
            ServiceTag_UpDown.ValueChanged += IncrementServiceTagControl;
            ServiceCost_TextBox.PreviewTextInput += EnsureServiceCostIsNumeric;
            Regular_ListView.SelectionChanged += DisplayRegularServicesIntoTextBoxes;
            Regular_ListView.LostFocus += ListView_UnSelect;
            Express_ListView.SelectionChanged += DisplayExpressServicesIntoTextBoxes;
            Express_ListView.LostFocus += ListView_UnSelect;
            Completed_ListView.MouseDoubleClick += DoubleClickFinishedListItem;
            FinishRegular_Button.Click += FinishRegularService;
            FinishExpress_Button.Click += FinishExpressService;
            PrefillHints();
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem();
        }
        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }
        private void Focus_Text(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Foreground = Brushes.Black;
            if (textBox.Text == GetHintTexts(textBox))
            {
                textBox.Text = string.Empty;
            }
        }
        private void Unfocus_Text(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == string.Empty)
            {
                ShowHintTexts(textBox);
            }
        }
        private void ListView_UnSelect(object sender, RoutedEventArgs e)
        {
            ListView view = (ListView)sender;
            view.UnselectAll();
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
            if (CheckIfTextboxesAreInvalid())
            {
                return;
            }

            int priority = GetServicePriority();
            float cost = float.Parse(ServiceCost_TextBox.Text);
            Drone drone = new Drone(ClientName_TextBox.Text, DroneModel_TextBox.Text, ServiceProblem_TextBox.Text, CalculatePricing(cost, priority), ServiceTag_TextBox.Text);
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
            PrefillHints();
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
            return MathF.Round(_value, 2, MidpointRounding.ToZero);
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
            Regular_ListView.Items.Clear();
            foreach (var item in RegularService)
            {
                Regular_ListView.Items.Add(new
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
            Express_ListView.Items.Clear();
            foreach (var item in ExpressService)
            {
                Express_ListView.Items.Add(new
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
                var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                e.Handled = !regex.IsMatch(e.Text);
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
            if (newVal < 0) { updown.Value = maxVal; }
            if (newVal > maxVal) { updown.Value = 0; }

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
            ServiceTag_TextBox.Foreground = Brushes.Black;
        }

        /// <summary>
        /// 6.12 Create a mouse click method for the regular service ListView that will display the Client Name and Service Problem in the related textboxes.
        /// </summary>
        public void DisplayRegularServicesIntoTextBoxes(object sender, SelectionChangedEventArgs e)
        {
            if (Regular_ListView.SelectedIndex > -1)
            {
                Drone drone = RegularService.ElementAt(Regular_ListView.SelectedIndex);
                ClientName_TextBox.Text = drone.GetClientName();
                ClientName_TextBox.Foreground = Brushes.Black;
                DroneModel_TextBox.Text = drone.GetDroneModel();
                DroneModel_TextBox.Foreground = Brushes.Black;
                ServiceProblem_TextBox.Text = drone.GetServiceProblem();
                ServiceProblem_TextBox.Foreground = Brushes.Black;
                ServiceCost_TextBox.Text = drone.GetServiceCost().ToString();
                ServiceCost_TextBox.Foreground = Brushes.Black;
                ServiceTag_TextBox.Text = drone.GetServiceTag();
                ServiceTag_TextBox.Foreground = Brushes.Black;
            }
            else
            {
                Regular_ListView.UnselectAll();
            }
        }

        /// <summary>
        /// 6.13 Create a mouse click method for the express service ListView that will display the Client Name and Service Problem in the related textboxes.
        /// </summary>
        public void DisplayExpressServicesIntoTextBoxes(object sender, SelectionChangedEventArgs e)
        {
            if (Express_ListView.SelectedIndex > -1)
            {
                Drone drone = ExpressService.ElementAt(Express_ListView.SelectedIndex);
                ClientName_TextBox.Text = drone.GetClientName();
                ClientName_TextBox.Foreground = Brushes.Black;
                DroneModel_TextBox.Text = drone.GetDroneModel();
                DroneModel_TextBox.Foreground = Brushes.Black;
                ServiceProblem_TextBox.Text = drone.GetServiceProblem();
                ServiceProblem_TextBox.Foreground = Brushes.Black;
                ServiceCost_TextBox.Text = drone.GetServiceCost().ToString();
                ServiceCost_TextBox.Foreground = Brushes.Black;
                ServiceTag_TextBox.Text = drone.GetServiceTag();
                ServiceTag_TextBox.Foreground = Brushes.Black;
            }
            else
            {
                Regular_ListView.UnselectAll();
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
            if (RegularService.Count <= 0)
            {
                SetStatusMessage("No available Regular Services", true);
                return;
            }
            Drone drone = RegularService.Dequeue();
            FinishedList.Add(drone);

            DisplayFinishedServices();
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
            if (ExpressService.Count <= 0)
            {
                SetStatusMessage("No available Express Services", true);
                return;
            }
            Drone drone = ExpressService.Dequeue();
            FinishedList.Add(drone);

            DisplayFinishedServices();
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
            ListView lv = (ListView)sender;
            FinishedList.RemoveAt(lv.SelectedIndex);
            DisplayFinishedServices();
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
            PrefillHints();
        }

        // 6.18 All code is required to be adequately commented. Map the programming criteria and features to your code/methods by adding comments above the method signatures.
        // Ensure your code is compliant with the CITEMS coding standards (refer http://www.citems.com.au/).

        private void SetStatusMessage(string _msg, bool _showWindow = false, string _windowTitle = "Message")
        {
            StatusMessage_TextBox.Text = _msg;
            if (_showWindow)
            {
                System.Windows.MessageBox.Show(_msg, _windowTitle);
            }
        }
        private bool CheckIfTextboxesAreInvalid()
        {
            int invalidInputs = 0;
            string msg = string.Empty;

            if (GetServicePriority() < 0)
            {
                msg += "Please select priority level." + Environment.NewLine;
                invalidInputs++;
            }
            if (string.IsNullOrEmpty(ClientName_TextBox.Text) || ClientName_TextBox.Text == GetHintTexts(ClientName_TextBox))
            {
                msg += "Please input Client Name." + Environment.NewLine;
                invalidInputs++;
            }
            if (string.IsNullOrEmpty(DroneModel_TextBox.Text) || DroneModel_TextBox.Text == GetHintTexts(DroneModel_TextBox))
            {
                msg += "Please input Drone Model." + Environment.NewLine;
                invalidInputs++;
            }
            if (string.IsNullOrEmpty(ServiceProblem_TextBox.Text) || ServiceProblem_TextBox.Text == GetHintTexts(ServiceProblem_TextBox))
            {
                msg += "Please input Service Problem." + Environment.NewLine;
                invalidInputs++;
            }
            if (string.IsNullOrEmpty(ServiceCost_TextBox.Text) || ServiceCost_TextBox.Text == GetHintTexts(ServiceCost_TextBox))
            {
                msg += "Please input Service Cost." + Environment.NewLine;
                invalidInputs++;
            }
            if (string.IsNullOrEmpty(ServiceTag_TextBox.Text) || ServiceTag_TextBox.Text == GetHintTexts(ServiceTag_TextBox))
            {
                msg += "Please input Service Tag." + Environment.NewLine;
                invalidInputs++;
            }

            if (invalidInputs > 0)
            {
                System.Windows.MessageBox.Show(msg, "Message");
                return true;
            }
            else
            {

                return false;
            }
        }
        private void DisplayFinishedServices()
        {
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
        }

        private string GetHintTexts(TextBox textbox)
        {
            switch (textbox.Name)
            {
                case "ClientName_TextBox":
                    return "Enter Client Name...";
                case "DroneModel_TextBox":
                    return "Enter Drone Model...";
                case "ServiceCost_TextBox":
                    return "Enter Service Cost...";
                case "ServiceTag_TextBox":
                    return "Enter Service Tag...";
                case "ServiceProblem_TextBox":
                    return "Enter Service Problem...";
                default:
                    return "Error";
            }
        }
        private void ShowHintTexts(TextBox textbox)
        {
            textbox.Foreground = Brushes.Gray;
            switch (textbox.Name)
            {
                case "ClientName_TextBox":
                    textbox.Text = "Enter Client Name...";
                    break;
                case "DroneModel_TextBox":
                    textbox.Text = "Enter Drone Model...";
                    break;
                case "ServiceCost_TextBox":
                    textbox.Text = "Enter Service Cost...";
                    break;
                case "ServiceTag_TextBox":
                    textbox.Text = "Enter Service Tag...";
                    break;
                case "ServiceProblem_TextBox":
                    textbox.Text = "Enter Service Problem...";
                    break;
                default:
                    break;
            }
        }
        private void PrefillHints()
        {
            ClientName_TextBox.Text = "Enter Client Name...";
            ClientName_TextBox.Foreground = Brushes.Gray;
            DroneModel_TextBox.Text = "Enter Drone Model...";
            DroneModel_TextBox.Foreground = Brushes.Gray;
            ServiceCost_TextBox.Text = "Enter Service Cost...";
            ServiceCost_TextBox.Foreground = Brushes.Gray;
            ServiceTag_TextBox.Text = "Enter Service Tag...";
            ServiceTag_TextBox.Foreground = Brushes.Gray;
            ServiceProblem_TextBox.Text = "Enter Service Problem...";
            ServiceProblem_TextBox.Foreground = Brushes.Gray;
        }

        private bool isPrefillingTestingData = false;
        private void PrefillTestingData()
        {
            ClientName_TextBox.Text = "James";
            DroneModel_TextBox.Text = "Model K";
            ServiceCost_TextBox.Text = "220";
            ServiceTag_TextBox.Text = "Tag 2";
            ServiceProblem_TextBox.Text = "Drone ain't working. Needs full service!";

            ClientName_TextBox.Text = "Ash";
            DroneModel_TextBox.Text = "Model L";
            ServiceCost_TextBox.Text = "150";
            ServiceTag_TextBox.Text = "Tag 1";
            ServiceProblem_TextBox.Text = "Drone misbehaving. Need parts inspection.";

            ClientName_TextBox.Text = "Steve";
            DroneModel_TextBox.Text = "Model P";
            ServiceCost_TextBox.Text = "50.50";
            ServiceTag_TextBox.Text = "Tag 4";
            ServiceProblem_TextBox.Text = "Drone miscolor. Needs respray.";
        }


    }
}
