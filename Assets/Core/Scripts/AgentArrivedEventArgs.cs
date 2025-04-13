using UnityEngine;
using System;

namespace Core
{
    public class AgentArrivedEventArgs : EventArgs
    {
        public string AgentGUID { get; private set; }
        public string WaypointName { get; private set; }
        public DateTime ArrivalTime { get; private set; }

        public AgentArrivedEventArgs(string agentGuid, string waypointName, DateTime arrivalTime)
        {
            AgentGUID = agentGuid;
            WaypointName = waypointName;
            ArrivalTime = arrivalTime;
        }
    }
}