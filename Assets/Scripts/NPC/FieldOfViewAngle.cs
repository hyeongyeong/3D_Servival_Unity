using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle; // 시야각
    [SerializeField] private float viewDistance; // 시야 거리
    [SerializeField] private LayerMask targetMask;

    private PlayerController thePlayer;

    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
    }

    public Vector3 GetTargetPos()
    {
        return thePlayer.transform.position;
    }

    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    public  bool View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle *0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);
        
        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);

                if(_angle<viewAngle * 0.5f)
                {
                    RaycastHit _hit;
                    if(Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if (_hit.transform.name == "Player")
                        {
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
