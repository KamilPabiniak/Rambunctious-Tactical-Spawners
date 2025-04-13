using System;
using UnityEngine;
using Core;
using DG.Tweening;

namespace Agents
{
    public class AgentManager : MonoBehaviour, IAgentManager
    {
        [SerializeField] private GameObject agentPrefab;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Transform agentParent;

        private void Awake()
        {
            DOTween.SetTweensCapacity(1000, 50);
        }

        public IAgent SpawnAgent()
        {
            if (agentPrefab == null || waypoints == null || waypoints.Length == 0) {return null;}
            GameObject agentObj = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentParent);
            Agent agent = agentObj.GetComponent<Agent>();
            if (agent != null) { agent.waypoints = waypoints; }
            return agent;
        }
    }
}