using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneServiceApplication
{
    internal class Drone
    {
        public string clientName = string.Empty;
        public string droneModel = string.Empty;
        public string serviceProblem = string.Empty;
        public int serviceCost = 0;
        public string serviceTag = string.Empty;

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
            clientName = _value;
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
            serviceProblem = _value;
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
    }
}
