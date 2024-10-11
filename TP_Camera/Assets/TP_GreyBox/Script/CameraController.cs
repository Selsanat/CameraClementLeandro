using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Camera _camera;  
    private CameraConfiguration _targetConfiguration;
    private CameraConfiguration _currentConfiguration;
    public float InterpolationSpeed = 1;
    public float InterpolationSnappingFloatPoint = 0.00001f;
    private List<AView> _activeViews = new List<AView>();  

    private static CameraController _instance;

    public static CameraController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraController>();
            }
            return _instance;
        }
    }

    public void AddView(AView view)
    {
        if (!_activeViews.Contains(view))
        {
            _activeViews.Add(view);
        }
    }

    public void RemoveView(AView view)
    {
        if (_activeViews.Contains(view))
        {
            _activeViews.Remove(view);
        }
    }

    private void Start()
    {
        _targetConfiguration = ComputeAverage();
        _currentConfiguration = ComputeAverage();
    }

    private void Update()
    {
        _targetConfiguration = ComputeAverage();  
        ApplyConfiguration();  
    }

    private void ComputeCurrentConfiguration()
    {
        _currentConfiguration.pivot += (_targetConfiguration.pivot - _currentConfiguration.pivot) * Time.deltaTime * InterpolationSpeed;
        _currentConfiguration.pitch = getAverage(_currentConfiguration.pitch , _targetConfiguration.pitch);
        Vector2 currentYaw = new Vector2(Mathf.Cos(_currentConfiguration.yaw * Mathf.Deg2Rad), Mathf.Sin(_currentConfiguration.yaw * Mathf.Deg2Rad));
        Vector2 targetYaw = new Vector2(Mathf.Cos(_targetConfiguration.yaw * Mathf.Deg2Rad), Mathf.Sin(_targetConfiguration.yaw * Mathf.Deg2Rad));
        _currentConfiguration.yaw = getAverage(currentYaw, targetYaw);
        _currentConfiguration.roll = getAverage(_currentConfiguration.roll , _targetConfiguration.roll);
        _currentConfiguration.fieldOfView = getAverage(_currentConfiguration.fieldOfView, _targetConfiguration.fieldOfView);
    }
    private float getAverage(float current, float target)
    {
        float sum = (target - current) * Time.deltaTime * InterpolationSpeed;
        return (Math.Abs(sum) < InterpolationSnappingFloatPoint) ? target : current+sum;
    }

    private float getAverage(Vector2 current, Vector2 target)
    {
        float targetAngle = Vector2.SignedAngle(Vector2.right, target);
        float sumAngle = Vector2.SignedAngle(Vector2.right, current + (target * Time.deltaTime*InterpolationSpeed));
        return sumAngle;
    }

    private void ApplyConfiguration()
    {
        if (_camera == null || _targetConfiguration.Equals(null))
            return;
        ComputeCurrentConfiguration();
        _camera.transform.position = _currentConfiguration.GetPosition();
        _camera.transform.rotation = _currentConfiguration.GetRotation();
        _camera.fieldOfView = _currentConfiguration.fieldOfView;
    }

    public CameraConfiguration ComputeAverage()
    {
        float totalWeight = 0f;
        Vector3 summedPosition = Vector3.zero;
        Vector3 summedRotation = Vector3.zero;
        float summedFov = 0f;

        foreach (AView view in _activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            totalWeight += view.weight;

            summedPosition += config.GetPosition() * view.weight;

            summedRotation.x += config.pitch * view.weight;
            summedRotation.y += ComputeAverageYaw() * view.weight;
            summedRotation.z += config.roll * view.weight;

            summedFov += config.fieldOfView * view.weight;
        }

        return ComputeAvarageResult(summedPosition, totalWeight, summedRotation, summedFov);
    }

    private static CameraConfiguration ComputeAvarageResult(Vector3 summedPosition, float totalWeight,
	    Vector3 summedRotation, float summedFov)
    {
	    CameraConfiguration averageConfig = new CameraConfiguration();
	    averageConfig.pivot = summedPosition / totalWeight;
	    averageConfig.pitch = summedRotation.x / totalWeight;
	    averageConfig.yaw = summedRotation.y / totalWeight;
	    averageConfig.roll = summedRotation.z / totalWeight;
	    averageConfig.fieldOfView = summedFov / totalWeight;
	    return averageConfig;
    }

    public float ComputeAverageYaw()
    {
        Vector2 sum = Vector2.zero;
        foreach (AView view in _activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            sum += new Vector2(Mathf.Cos(config.yaw * Mathf.Deg2Rad),
            Mathf.Sin(config.yaw * Mathf.Deg2Rad)) * view.weight;
        }
        return Vector2.SignedAngle(Vector2.right, sum);
    }
}

