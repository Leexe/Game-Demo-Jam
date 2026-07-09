using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
	[Header("Cameras")]
	[SerializeField]
	private CinemachineCamera _cinemachineCamera;

	public void FocusOn(Transform target)
	{
		_cinemachineCamera.LookAt = target;
		_cinemachineCamera.Priority = 100;
	}

	public void ClearFocus()
	{
		_cinemachineCamera.Priority = 0;
	}
}
