using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
	[Header("Cameras")]
	[SerializeField]
	private CinemachineCamera _playerCamera;

	[SerializeField]
	private CinemachinePanTilt _panTilt;

	[SerializeField]
	private CinemachineInputAxisController _inputAxisController;

	[Header("Settings")]
	[SerializeField]
	private float _focusTweenDuration = 0.5f;

	private Sequence _focusSequence;

	private void OnDisable()
	{
		ClearFocus();
	}

	public void FocusOn(Transform target)
	{
		// Disable Camera Movement
		_inputAxisController.enabled = false;

		Vector3 direction = target.position - _playerCamera.transform.position;
		if (direction == Vector3.zero)
		{
			return;
		}

		var targetRotation = Quaternion.LookRotation(direction);
		Vector3 targetEuler = targetRotation.eulerAngles;

		float targetPan = targetEuler.y;
		float targetTilt = targetEuler.x;
		if (targetTilt > 180f)
		{
			targetTilt -= 360f;
		}

		// Calculate shortest path for pan
		float startPan = _panTilt.PanAxis.Value;
		float deltaPan = Mathf.DeltaAngle(startPan, targetPan);
		float finalTargetPan = startPan + deltaPan;

		_focusSequence.Stop();
		_focusSequence = Sequence.Create();

		_focusSequence.Group(
			Tween.Custom(
				startPan,
				finalTargetPan,
				_focusTweenDuration,
				onValueChange: newVal => _panTilt.PanAxis.Value = newVal,
				ease: Ease.InOutSine
			)
		);

		_focusSequence.Group(
			Tween.Custom(
				_panTilt.TiltAxis.Value,
				targetTilt,
				_focusTweenDuration,
				onValueChange: newVal => _panTilt.TiltAxis.Value = newVal,
				ease: Ease.InOutSine
			)
		);
	}

	public void ClearFocus()
	{
		_focusSequence.Stop();
		_inputAxisController.enabled = true;
	}
}
