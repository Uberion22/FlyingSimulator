using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 3;
    [SerializeField] private Vector3 _cameraBehindHelicopterPosition = new Vector3(0, 9, -15);
    [SerializeField] private Vector3 _cameraPilotPosition = new Vector3(0.45f, 3.11f, 0.94f);

    private const float MaxRotationX = 30;
    private const float MaxRotationY = 90;
    private bool _pilotLookEnabled = true;
    private Vector3 _rotationVector = new Vector3(0, 0, 0);

    void Update()
    {
        if(!GameManager.GameStarted || !Input.GetKey(KeyCode.Mouse1)) return;

        float mouseX = Input.GetAxis("Mouse X") * _rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * _rotationSpeed;
        RotateCamera(mouseX, mouseY);
    }

    private void OnEnable()
    {
        InterfaceManager.OnViewChanged += ChangeCameraView;
    }

    private void OnDisable()
    {
        InterfaceManager.OnViewChanged -= ChangeCameraView;
    }

    private void SetCameraPosition()
    {
        transform.localPosition = _pilotLookEnabled ? _cameraPilotPosition : _cameraBehindHelicopterPosition;
    }

    private void RotateCamera(float mouseX, float mouseY)
    {
        _rotationVector.x = _rotationVector.x - mouseY;
        _rotationVector.y = _rotationVector.y + mouseX;
        if (Math.Abs(_rotationVector.x) > MaxRotationX)
        {
            _rotationVector.x = Math.Sign(_rotationVector.x) * (MaxRotationX);
        }
        if (Math.Abs(_rotationVector.y) > MaxRotationY)
        {
            _rotationVector.y = Math.Sign(_rotationVector.y) * (MaxRotationY);
        }
        _rotationVector.z = 0;
        transform.localEulerAngles = _rotationVector;
        transform.localEulerAngles = new Vector3(_rotationVector.x, _rotationVector.y, 0);
    }

    private void ChangeCameraView()
    {
        _pilotLookEnabled = !_pilotLookEnabled;
        SetCameraPosition();
    }

    public Vector3 GetCameraPilotPosition()
    {
        return _cameraPilotPosition;
    }
}
