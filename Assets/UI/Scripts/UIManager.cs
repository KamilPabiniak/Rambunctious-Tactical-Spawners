using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Text Displays")]
        [SerializeField] private TextMeshProUGUI agentCountText;
        [SerializeField] private TextMeshProUGUI gameSpeedText;
        [SerializeField] private TextMeshProUGUI messagesText;
        
        [Header("Buttons")]
        [SerializeField] private Button addAgentButton;
        [SerializeField] private Button removeAgentButton;  
        [SerializeField] private Button clearAgentsButton;     
        [SerializeField] private Button speedUpButton;
        [SerializeField] private Button slowDownButton;
        [SerializeField] private Button pauseButton;
        
        //Services here
        private IAgentService _agentService;
        private ITickService _tickService;
        
        //and this text from button
        private TextMeshProUGUI _pauseText;
        
        private readonly List<TimedMessage> _messages = new();
        private const float MessageTime = 10f;


        private void Awake()
        {
            //Agent 47 is here
            _agentService = ServiceLocator.GetService<IAgentService>();
            _tickService = ServiceLocator.GetService<ITickService>();
        }

        private void OnEnable()
        {
            _agentService.AgentArrived += OnAgentArrived;
        }
        
        private void OnDisable()
        {
            _agentService.AgentArrived -= OnAgentArrived;
        }

        private void Start()
        {
            _pauseText = pauseButton.GetComponent<TextMeshProUGUI>();

            addAgentButton.onClick.AddListener(OnAddAgent);
            removeAgentButton.onClick.AddListener(OnRemoveAgent);
            clearAgentsButton.onClick.AddListener(OnClearAgents);
            
            speedUpButton.onClick.AddListener(() => OnSetTickRate(_tickService.TickRate + 0.5f));
            slowDownButton.onClick.AddListener(() => OnSetTickRate(Mathf.Max(0.1f, _tickService.TickRate - 0.5f)));
            pauseButton.onClick.AddListener(OnPauseGame);

            UpdateAgentCount();
            UpdateGameSpeed();
        }

        private void FixedUpdate()
        {
            if (_messages.Count == 0) return;
            _messages.RemoveAll(m => Time.time - m.TimeAdded > MessageTime);
            messagesText.text = string.Join("\n", _messages.Select(m => m.Text));
        }

        private void UpdateAgentCount() => agentCountText.text = $"{_agentService.GetAgentCount()}";

        private void UpdateGameSpeed() => gameSpeedText.text = $"{_tickService.TickRate:0.0}x";

        private void OnAddAgent()
        {
            _agentService.SpawnAgent();
            UpdateAgentCount();
        }

        private void OnRemoveAgent()
        {
            _agentService.RemoveRandomAgent();
            UpdateAgentCount();
        }

        private void OnClearAgents()
        {
            _agentService.ClearAgents();
            UpdateAgentCount();
        }

        private void OnSetTickRate(float newRate)
        {
            _tickService.SetTickRate(newRate);
            UpdatePauseText(newRate);
            UpdateGameSpeed();
        }

        private void OnPauseGame()
        {
            float newRate = _tickService.TickRate == 0f ? 1f : 0f;
            OnSetTickRate(newRate);
            UpdatePauseText(newRate);
        }

        private void UpdatePauseText(float newRate) => _pauseText.text = newRate == 0f ? "RESUME" : "PAUSE";

        private void OnAgentArrived(object sender, AgentArrivedEventArgs e)
        {
            string timeString = e.ArrivalTime.ToString("HH:mm:ss");
            string message = $"[{timeString}] Agent {e.AgentGUID.Substring(0, 8)} arrived at {e.WaypointName}";
            _messages.Insert(0, new TimedMessage(message));
            messagesText.text = string.Join("\n", _messages.Select(m => m.Text));
        }
    }
}