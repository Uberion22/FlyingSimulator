using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _helicopter;

    private Vector3 _cameraStartPosition;
    private Quaternion _cameraStartRotation;
    private Vector3 _cameraPilotPosition;

    public static bool GameStarted { get; private set; }

    void OnEnable()
    {
        InterfaceManager.OnGameStartPressed += SpawnOnPositionMouseDown;
        InterfaceManager.OnGameStopped += StopGameAndBackToSelectSpawnPos;
    }

    void OnDisable()
    {
        InterfaceManager.OnGameStartPressed -= SpawnOnPositionMouseDown;
        InterfaceManager.OnGameStopped -= StopGameAndBackToSelectSpawnPos;
    }

    void Start()
    {
        _cameraStartPosition = _camera.transform.position;
        _cameraStartRotation = _camera.transform.rotation;
        _cameraPilotPosition = _camera.GetComponent<CameraController>().GetCameraPilotPosition();
    }

    void SpawnOnPositionMouseDown(Vector3 spawnPos)
    {
        GameStarted = true;
        _helicopter.transform.position = spawnPos;
        _helicopter.SetActive(true);
        _camera.transform.SetParent(_helicopter.transform);
        _camera.transform.localPosition = _cameraPilotPosition;
        _camera.transform.localRotation = Quaternion.identity;
    }

    private void StopGameAndBackToSelectSpawnPos()
    {
        GameStarted = false;
        _camera.transform.SetParent(null);
        _helicopter.SetActive(false);
        _camera.transform.position = _cameraStartPosition;
        _camera.transform.rotation = _cameraStartRotation;
    }
}
