using UnityEngine;
using System;

namespace Core
{
    public class AgentArrivedEventArgs : EventArgs
    {
        public string AgentGUID { get; private set; }
        public Vector3 Destination { get; private set; }
        public string WaypointName { get; private set; }
        public DateTime ArrivalTime { get; private set; }

        public AgentArrivedEventArgs(string agentGUID, Vector3 destination, string waypointName, DateTime arrivalTime)
        {
            AgentGUID = agentGUID;
            Destination = destination;
            WaypointName = waypointName;
            ArrivalTime = arrivalTime;
        }
    }
}