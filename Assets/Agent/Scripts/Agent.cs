using System;
using UnityEngine;
using DG.Tweening;
using Pathfinding;
using Core;

namespace Agents
{
    public class Agent : MonoBehaviour, IAgent
    {
        [Header("Reference")]
        [SerializeField] private Animator animator;
        [SerializeField] private Seeker seeker;
        
        [Header("Data")]
        public Transform[] waypoints;
        public GameObject AgentGameObject => gameObject;
        public event EventHandler<AgentArrivedEventArgs> AgentArrived;

        private float _movementSpeed;
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private string _agentGuid;
        private Path _currentPath;
        private int _currentPathIndex;
        private int _currentWaypointIndex;
        
        //For DoTween
        private Sequence _movementSequence;
        
        private void Awake()
        {
            _agentGuid = Guid.NewGuid().ToString();
        }

        private void Start()
        {
            if (waypoints is { Length: > 0 })
            {
                MoveToRandomWaypoint();
            }
        }

        private void MoveToRandomWaypoint()
        {
            if (waypoints == null || waypoints.Length == 0) return;

            int newIndex;
            do
            {
                newIndex = UnityEngine.Random.Range(0, waypoints.Length);
            } while (waypoints.Length > 1 && newIndex == _currentWaypointIndex);

            _currentWaypointIndex = newIndex;
            RequestPathToWaypoint(waypoints[_currentWaypointIndex].position);
        }

        private void RequestPathToWaypoint(Vector3 targetPosition)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(transform.position, targetPosition, OnPathComplete);
            }
        }

        private void OnPathComplete(Path p)
        {
            if (p.error)
            {
                MoveToRandomWaypoint();
                return;
            }
            _currentPath = p;
            _currentPathIndex = 0;
            FollowPath();
        }

        private void FollowPath()
        {
            if (_currentPath == null || _currentPath.vectorPath.Count == 0) return;
            
            _movementSequence?.Kill();
            _movementSequence = DOTween.Sequence().SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    
            Vector3 previousPosition = transform.position;

            for (int i = _currentPathIndex; i < _currentPath.vectorPath.Count; i++)
            {
                Vector3 nextPoint = _currentPath.vectorPath[i];
                float segmentDistance = Vector3.Distance(previousPosition, nextPoint);
                float duration = segmentDistance / _movementSpeed;
                
                Vector3 direction = (nextPoint - previousPosition).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                //Move
                Tween moveTween = transform.DOMove(nextPoint, duration)
                    .SetEase(Ease.Linear)
                    .OnUpdate(() => UpdateAnimation(true));
                //Here is rotate
                Tween rotateTween = transform.DORotateQuaternion(targetRotation, duration)
                    .SetEase(Ease.Linear);
                
                float tweenStartTime = _movementSequence.Duration();
                
                _movementSequence.Append(moveTween);
                _movementSequence.Insert(tweenStartTime, rotateTween);
        
                previousPosition = nextPoint;
            }

            //Upon arrival, inform and move on, boy
            _movementSequence.OnComplete(() =>
            {
                UpdateAnimation(false);
                AgentArrived?.Invoke(this, new AgentArrivedEventArgs(
                    _agentGuid,
                    waypoints[_currentWaypointIndex].name,
                    DateTime.Now));
                MoveToRandomWaypoint();
            });
        }

        public void SetSpeed(float speed)
        {
            _movementSpeed = speed;
        }


        private void UpdateAnimation(bool isMoving)
        {
            if (animator != null)
            {
                animator.SetFloat(SpeedHash, isMoving ? _movementSpeed : 0f);
            }
        }
        
        private void OnDestroy()
        {
            _movementSequence?.Kill();
        }
        
    }
}
