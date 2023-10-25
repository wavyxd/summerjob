using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook3 : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _hook;
    [SerializeField] private Transform _handPos;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private LayerMask _hookLayer;
    [SerializeField] private float _maxHookDist;
    [SerializeField] private float _hookSpeed;
    [SerializeField] private Vector3 _offset;


    private bool isShooting, isGrappling;
    private Vector3 _hookPoint;

    private void Start()
    {
        isShooting = false;
        isGrappling = false;
    }

    private void Update()
    {
        _controller.enabled = true;

        if (Input.GetMouseButtonDown(0))
        {
            ShootHook();
        }

        if (isGrappling)
        {
            _hook.position = Vector3.Lerp(_hook.position, _hookPoint, _hookSpeed * Time.deltaTime);
            if (Vector3.Distance(_hook.position, _hookPoint) < 0.5f)
            {
                _controller.enabled = true;
                _playerBody.position = Vector3.Lerp(_playerBody.position, _hookPoint - _offset, _hookSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(_playerBody.position, _hookPoint - _offset) < 0.5f)
            {
                _controller.enabled = true;
                isGrappling = false;
                _hook.SetParent(_handPos);
            }
        }
    }

    private void ShootHook()
    {
        if (isShooting || isGrappling) return;

        isShooting = true;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, _maxHookDist, _hookLayer))
        {
            _hookPoint = hit.point;
            isGrappling = true;
            _hook.parent = null;
            _hook.LookAt(_hookPoint);
            Debug.Log("Hit");
        }
    }
}
