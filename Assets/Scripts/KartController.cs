using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KartController : MonoBehaviour
{
    [SerializeField] Rigidbody _physBody;
    [SerializeField] Transform _visualBody;

    [SerializeField] float _forwardForce;
    [SerializeField] float _sidewaysForce;
    [SerializeField] AnimationCurve _turnPowerCurve;
    [SerializeField] float _boostFactor; float _currentBoost = 1;

    Vector3 _lookDirection;
    Vector2 _moveInput;
    float _boostInput;


    private void FixedUpdate()
    {
        ApplyMovementForces();
    }

    void ApplyMovementForces()
    {
        _physBody.AddForce(_visualBody.forward * _forwardForce * _moveInput.y * _currentBoost);

        _physBody.AddForce(_visualBody.right * _sidewaysForce * _moveInput.x * _turnPowerCurve.Evaluate(_physBody.velocity.magnitude));
    }

    private void LateUpdate()
    {
        MoveVisualBody(); //Late update to avoid jittering
    }

    void MoveVisualBody()
    {
        _visualBody.position = _physBody.position;

        _lookDirection = _physBody.velocity;
        _lookDirection.y = 0;

        if (_lookDirection != Vector3.zero)
        {
            _visualBody.rotation = Quaternion.LookRotation(_lookDirection, Vector3.up);
        }
    }

    //Getting input from InputSystem
    public void OnMoveInput(InputAction.CallbackContext _context)
    {
        _moveInput = _context.ReadValue<Vector2>();
    }

    public void OnBoostInput(InputAction.CallbackContext _context)
    {
        _boostInput = _context.ReadValue<float>();
        if (_boostInput > 0.1f)
        {
            _currentBoost = _boostFactor;
        } else
        {
            _currentBoost = 1;
        }
    }

}
