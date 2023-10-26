﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DroneServiceApplication
{
    /*
    6.1 Ceate a separate class file to hold the data items of the Drone. Use separate getter and setter methods, 
        ensure the attributes are private and the accessor methods are public. Add a display method that returns a string for Client Name and Service Cost. 
        Add suitable code to the Client Name and Service Problem accessor methods so the data is formatted as Title case or Sentence case. 
        Save the class as “Drone.cs”.
     */

    internal class Drone
    {
        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

        private string clientName = string.Empty;
        private string droneModel = string.Empty;
        private string serviceProblem = string.Empty;
        private int serviceCost = 0;
        private string serviceTag = string.Empty;

        public Drone() { }

        public Drone(string _clientName, string _droneModel, string _serviceProblem, int _serviceCost, string _serviceTag)
        {
            clientName = _clientName;
            droneModel = _droneModel;
            serviceProblem = _serviceProblem;
            serviceCost = _serviceCost;
            serviceTag = _serviceTag;
        }

        // Getters and Setters
        public string GetClientName()
        {
            return clientName;
        }
        public void SetClientName(string _value)
        {
            clientName = myTI.ToTitleCase(_value);
        }
        public string GetDroneModel()
        {
            return droneModel;
        }
        public void SetDroneModel(string _value)
        {
            droneModel = _value;
        }
        public string GetServiceProblem()
        {
            return serviceProblem;
        }
        public void SetServiceProblem(string _value)
        {
            serviceProblem = myTI.ToTitleCase(_value);
        }
        public int GetServiceCost()
        {
            return serviceCost;
        }
        public void SetServiceCost(int _value)
        {
            serviceCost = _value;
        }
        public string GetServiceTag()
        {
            return serviceTag;
        }
        public void SetServiceTag(string _value)
        {
            serviceTag = _value;
        }

        // FUNCTIONS
        public string Display()
        {
            return "Client: " + clientName + ", Cost: " + serviceCost;
        }
    }
}
