using System;
using UnityEngine;
using UnityEngine.UI;

public class SCR_UI_Timer : MonoBehaviour {

    [SerializeField] private Image _treeImage;

    private SCR_DayNightCycle _dayNightCycle;
    private Vector3 _difference;
    private Vector3 _start;
    private Vector3 _target;
    private float _percent;
    private float _timer;
    private float _seconds;

    private void Awake() {
        _dayNightCycle = FindFirstObjectByType<SCR_DayNightCycle>();
    }

    private void Start() {
        _seconds = _dayNightCycle.LenghtOfDay;
        _start = transform.position;
        _target = _treeImage.transform.position;
        _difference = _target - _start;
    }


    public void UpdateUITimer() {
        if (_timer < _seconds) {
            _timer += Time.deltaTime;
            _percent = _timer / _seconds;
            transform.position = _start + _difference * _percent;
        }
    }
}
