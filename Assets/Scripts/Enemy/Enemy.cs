using UnityEngine;
using Random = UnityEngine.Random;


namespace DefaultNamespace
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;

        private Transform _target;
        private EnemyAnimationController _animationController;
        private Transform _pointOne;
        private Transform _pointTwo;
        private void Awake()
        {
            _animationController = GetComponent<EnemyAnimationController>();
        }

        private void Start()
        {
            _target = Random.Range(0, 2) == 0 ? _pointOne : _pointTwo;
        }

        private void Update()
        {
            var direction = (_target.position - transform.position).normalized; 
            
            //var distanceToTarget = Vector3.Distance(_target.position, transform.position);
            var distanceToTarget = (_target.position - transform.position).magnitude;
            
            var moveDistance = speed * Time.deltaTime; // проходит один кадр 

            if (moveDistance > distanceToTarget) // чтоб шаг не оказался дальше таргета за один кадр
            {
                moveDistance = distanceToTarget;
                _target = _target == _pointOne ? _pointTwo : _pointOne;
            }
            
            _animationController.SetMove((int) Mathf.Sign(direction.x));
            
            transform.Translate(direction * moveDistance);
            UpdateSize((int) Mathf.Sign(direction.x));

        }

        private void UpdateSize(int sign)
        {
            var localScale = transform.localScale;
            if ((int)Mathf.Sign(localScale.x) != sign)
                localScale.x *= -1;
            
            transform.localScale = localScale;
        }

        public void SetPoints(Transform one, Transform two)
        {
            _pointOne = one;
            _pointTwo = two;
        }
    }
}