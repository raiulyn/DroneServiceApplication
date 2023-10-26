using System;
using System.Collections.Generic;
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
        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {

        }


        // 6.2 Create a global List<T> of type Drone called “FinishedList”. 
        private List<Drone> FinishedList = new List<Drone>();

        // 6.3 Create a global Queue<T> of type Drone called “RegularService”.
        private Queue<Drone> RegularService = new Queue<Drone>();

        // 6.4 Create a global Queue<T> of type Drone called “ExpressService”.
        private Queue<Drone> ExpressService = new Queue<Drone>();

        // 6.5 Create a button method called “AddNewItem” that will add a new service item to a Queue<> based on the priority.
        // Use TextBoxes for the Client Name, Drone Model, Service Problem and Service Cost. Use a numeric control for the Service Tag.
        // The new service item will be added to the appropriate Queue based on the Priority radio button.
        public void AddNewItem()
        {
            // TODO
            /*
            switch (switch_on)
            {
                case 0:
                    break;
                default:
                    break;
            }
            GetServicePriority();
            IncrementServiceTagControl();
            */
        }

        // 6.6 Before a new service item is added to the Express Queue the service cost must be increased by 15%.
        public int CheckPriorityPricing(int _value)
        {

            /*
            if ()
            {

            }
            */
            return _value;
        }

        // 6.7 Create a custom method called “GetServicePriority” which returns the value of the priority radio group.
        // This method must be called inside the “AddNewItem” method before the new service item is added to a queue.
        public int GetServicePriority()
        {
            return 0;
        }

        // 6.8 Create a custom method that will display all the elements in the RegularService queue.
        // The display must use a List View and with appropriate column headers.
        public void DisplayRegularService()
        {
            // TODO
        }

        // 6.9 Create a custom method that will display all the elements in the ExpressService queue.
        // The display must use a List View and with appropriate column headers.
        public void DisplayExpressService()
        {
            // TODO
        }

        // 6.10 Create a custom method to ensure the Service Cost textbox can only accept a double value with two decimal point.
        public bool EnsureServiceCost()
        {
            return false;
        }

        // 6.11 Create a custom method to increment the service tag control,
        // this method must be called inside the “AddNewItem” method before the new service item is added to a queue.
        public void IncrementServiceTagControl()
        {

        }

        // 6.12 Create a mouse click method for the regular service ListView that will display the Client Name and Service Problem in the related textboxes.
        public void ClickRegularService()
        {

        }

        // 6.13 Create a mouse click method for the express service ListView that will display the Client Name and Service Problem in the related textboxes.
        public void ClickExpressService()
        {

        }

        // 6.14 Create a button click method that will remove a service item from the regular ListView and dequeue the regular service Queue<T> data structure.
        // The dequeued item must be added to the List<T> and displayed in the ListBox for finished service items.
        public void ClickRegularServiceRemove()
        {

        }

        // 6.15 Create a button click method that will remove a service item from the express ListView and dequeue the express service Queue<T> data structure.
        // The dequeued item must be added to the List<T> and displayed in the ListBox for finished service items.
        public void ClickExpressServiceRemove()
        {

        }

        // 6.16 Create a double mouse click method that will delete a service item from the finished listbox and remove the same item from the List<T>.
        public void DoubleClickFinishedListItem()
        {

        }

        // 6.17 Create a custom method that will clear all the textboxes after each service item has been added.
        public void Clear()
        {

        }

        // 6.18 All code is required to be adequately commented. Map the programming criteria and features to your code/methods by adding comments above the method signatures.
        // Ensure your code is compliant with the CITEMS coding standards (refer http://www.citems.com.au/).


    }
}
