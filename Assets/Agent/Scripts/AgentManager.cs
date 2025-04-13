using System.Collections;
using UnityEngine;
using Core;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Agents
{
    public class AgentManager : MonoBehaviour, IAgentManager
    {
        [Header("Reference")]
        [SerializeField] private GameObject agentPrefab;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Transform agentParent;
        [SerializeField] private string uiSceneName;

        private void Awake()
        {
            DOTween.SetTweensCapacity(1000, 50);
            StartCoroutine(LoadUISceneAsync());
        }
        
        private IEnumerator LoadUISceneAsync()
        {
            if (string.IsNullOrEmpty(uiSceneName))
            {
                Debug.LogError("UI scene name is not set!");
                yield break;
            }

            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);
            asyncOp.allowSceneActivation = true;

            while (!asyncOp.isDone) { yield return null; }
        }

        public IAgent SpawnAgent()
        {
            if (agentPrefab == null || waypoints == null || waypoints.Length == 0) {return null;}
            GameObject agentObj = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentParent);
            Agent agent = agentObj.GetComponent<Agent>();
            agent.SetSpeed(4f);
            if (agent != null) { agent.waypoints = waypoints; }
            return agent;
        }
    }
}