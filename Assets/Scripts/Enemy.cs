using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform pointOne;
    [SerializeField] private Transform pointTwo;
    [SerializeField] private float speed;

    private Transform _target;
    private EnemyAnimationController _animationController;

    private void Awake()
    {
        _animationController = GetComponent<EnemyAnimationController>();
    }

    private void Start()
    {
        _target = Random.Range(0, 2) == 0 ? pointOne : pointTwo;
    }

    private void Update()
    {
        var direction = (_target.position - transform.position).normalized;
        //var distanceToTargetMag = (_target.position - transform.position).magnitude;
        var distanceToTarget = Vector3.Distance(_target.position, transform.position);
        var moveDistance = speed * Time.deltaTime;

        if (moveDistance > distanceToTarget)
        {
            moveDistance = distanceToTarget;
            _target = _target == pointOne ? pointTwo : pointOne;
        }

        transform.Translate(direction * moveDistance);

        UpdateSide((int) Mathf.Sign(direction.x));
        _animationController.SetMove((int) Mathf.Sign(direction.x));
    }

    private void UpdateSide(int sign)
    {
        var localScale = transform.localScale;
        if ((int) Mathf.Sign(localScale.x) != sign)
        {
            localScale.x *= -1;
        }

        transform.localScale = localScale;
    }
}