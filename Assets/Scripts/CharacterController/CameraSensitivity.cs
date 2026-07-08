using System.Collections.Generic;
using System.Linq;
// using Micwu.Settings;
using Unity.Cinemachine;
using UnityEngine;

public class CameraSensitivity : MonoBehaviour
{
	[SerializeField]
	private CinemachineInputAxisController _cinemachineInputAxis;

	[SerializeField]
	private float _webglSens = 1f;

	// cinemachine
	private InputAxisControllerBase<CinemachineInputAxisController.Reader>.Controller _lookX;
	private InputAxisControllerBase<CinemachineInputAxisController.Reader>.Controller _lookY;

	private float _baseGainX;
	private float _baseGainY;
	private float _sens;
	private float _aimSens;
	private bool _isReducedSens;

	public bool IsReducedSens => _isReducedSens;

	private void Awake()
	{
		LocateAxisControllers();
		_baseGainX = _lookX.Input.Gain;
		_baseGainY = _lookY.Input.Gain;
		_isReducedSens = false;

		// ReadSettingsValues();
		UpdateLookSensitivity();
	}

	// private void OnEnable()
	// {
	// 	Settings.OnSettingsChanged += OnSettingsChanged;
	// }
	//
	// private void OnDisable()
	// {
	// 	Settings.OnSettingsChanged -= OnSettingsChanged;
	// }

	//

	public void SetReducedSens(bool value)
	{
		_isReducedSens = value;
		UpdateLookSensitivity();
	}

	private void OnSettingsChanged(IEnumerable<(string, string)> changes)
	{
		if (changes.Any(e => e.Item1 is "MouseSens" or "AimSens"))
		{
			// ReadSettingsValues();
			UpdateLookSensitivity();
		}
	}

	// private void ReadSettingsValues()
	// {
	// 	if (SliderSettingDef.TryParse(Settings.Values["MouseSens"], out float mouseSensPercent))
	// 	{
	// 		_sens = mouseSensPercent * 0.01f;
	// 	}
	// 	if (SliderSettingDef.TryParse(Settings.Values["AimSens"], out float aimSensPercent))
	// 	{
	// 		_aimSens = aimSensPercent * 0.01f;
	// 	}
	// }

	private void UpdateLookSensitivity()
	{
		float platformMult = 1f;
#if UNITY_WEBGL
		platformMult = _webglSens;
#endif

		float sens = _isReducedSens ? _aimSens : _sens;
		_lookX.Input.Gain = sens * _baseGainX * platformMult;
		_lookY.Input.Gain = sens * _baseGainY * platformMult;
	}

	private void LocateAxisControllers()
	{
		foreach (
			InputAxisControllerBase<CinemachineInputAxisController.Reader>.Controller c in _cinemachineInputAxis.Controllers
		)
		{
			if (c.Name == "Look X (Pan)")
			{
				_lookX = c;
			}
			if (c.Name == "Look Y (Tilt)")
			{
				_lookY = c;
			}
		}
	}
}
