using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static event Action<Vector3> OnGameStartPressed;
    public static event Action OnGameStopped;
    public static event Action OnViewChanged;

    [SerializeField] private GameObject _startMenuBlock;
    [SerializeField] private GameObject _controlsBlock;
    [SerializeField] private Dropdown _spawnDropDown;
    [SerializeField] private GameObject[] _spawnPoints;
    [SerializeField] private Text _currentPosText;

    private Dictionary<Dropdown.OptionData, Vector3> _spawnPointsWithNames;
    private Vector3 _currentSpawnPos;
    private int _lastOption;
    private const float SpawnYShift = 2;
    private const int DefaultSpawnValue = 0; 

    // Start is called before the first frame update
    void Start()
    {
        _controlsBlock.SetActive(false);
        CreateSpawnPointsDictionary();
        _spawnDropDown.AddOptions( new List<Dropdown.OptionData>(_spawnPointsWithNames.Keys));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !GameManager.GameStarted)
        {
            GetSpawnPosByMouse();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackPressed();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnViewChangePressed();
        }
    }

    private void CreateSpawnPointsDictionary()
    {
        _spawnPointsWithNames = new Dictionary<Dropdown.OptionData, Vector3>();
        foreach (var point in _spawnPoints)
        {
            _spawnPointsWithNames.TryAdd(new Dropdown.OptionData(point.name), point.transform.position);
        }
    }

    public void OnStartBtnClick()
    {
        OnGameStartPressed?.Invoke(_currentSpawnPos);
        EnableOrDisableSpawnPoints(false);
        _startMenuBlock.SetActive(false);
        _controlsBlock.SetActive(true);
    }

    private void EnableOrDisableSpawnPoints(bool activateObjects)
    {
        foreach (var point in _spawnPoints)
        {
            point.SetActive(activateObjects);
        }
    }

    public void OnStartLocationChanged()
    {
        var currentOption = _spawnDropDown.options[_spawnDropDown.value];
        if (_spawnPointsWithNames.TryGetValue(currentOption, out var posVector3))
        {
            _currentSpawnPos = posVector3;
            SetColorToSpawnPoint(Color.blue, _lastOption);
            SetColorToSpawnPoint(Color.green, _spawnDropDown.value);
            _lastOption = _spawnDropDown.value;
            SetCurrentPositionText();
        }
    }

    private void GetSpawnPosByMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out var hit);
        var spawnPoint = hit.point;
        spawnPoint.y = spawnPoint.y + SpawnYShift;
        _currentSpawnPos = spawnPoint;
        ClearSelectedSpawnPoint();
        _spawnPoints.FirstOrDefault().transform.position = _currentSpawnPos;
        Debug.Log(spawnPoint);
    }

    private void SetColorToSpawnPoint(Color color, int optionNumber)
    {
        _spawnPoints[optionNumber].gameObject.GetComponent<Renderer>().material.color = color;
    }

    private void ClearSelectedSpawnPoint()
    {
        SetColorToSpawnPoint(Color.blue, _lastOption);
        SetColorToSpawnPoint(Color.green, DefaultSpawnValue);
        _spawnDropDown.value = DefaultSpawnValue;
        SetCurrentPositionText();
    }

    private void SetCurrentPositionText()
    {
        _currentPosText.text = $"Spawn position:\n {_currentSpawnPos}";
    }

    public void OnBackPressed()
    {
        OnGameStopped?.Invoke();
        EnableOrDisableSpawnPoints(true);
        _startMenuBlock.SetActive(true);
        _controlsBlock.SetActive(false);
    }

    public void OnViewChangePressed()
    {
        OnViewChanged?.Invoke();
    }
}
