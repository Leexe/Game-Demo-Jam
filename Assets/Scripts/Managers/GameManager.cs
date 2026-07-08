using PrimeTween;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static float _baseTimeScale = 1f; // set by console commands
	private static float _simulationTimeScale = 1f; // gameplay
	private static float _effectTimeScale = 1f; // temp effects
	private static bool _isPaused;

	public static float BaseTimeScale => _baseTimeScale;
	public static float SimulationTimeScale => _simulationTimeScale;
	public static bool IsPaused => _isPaused;

	public float RunTime;
	private Sequence _timeSlowSequence;

	private void Start()
	{
		LockCursor();
	}

	// Cursor

	public static void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public static void UnlockCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	// Time

	public void TimeSlow(float targetTimeSlow, float duration)
	{
		_timeSlowSequence.Stop();
		_timeSlowSequence = Sequence
			.Create(useUnscaledTime: true)
			.Chain(
				Tween.Custom(
					this,
					_effectTimeScale,
					targetTimeSlow,
					duration / 4,
					onValueChange: (_, timeScale) =>
					{
						_effectTimeScale = timeScale;
						ApplyTime();
					},
					Ease.OutQuad
				)
			)
			.Chain(
				Tween.Custom(
					this,
					targetTimeSlow,
					1f,
					duration * 3 / 4,
					onValueChange: (_, timeScale) =>
					{
						_effectTimeScale = timeScale;
						ApplyTime();
					},
					Ease.InQuad
				)
			);
	}

	public void CancelTimeSlow()
	{
		if (_timeSlowSequence.isAlive)
		{
			_timeSlowSequence.Stop();
			_effectTimeScale = 1f;
			ApplyTime();
		}
	}

	public static void SetPaused(bool val)
	{
		_isPaused = val;
		ApplyTime();
	}

	public static void SetBaseTimeScale(float val)
	{
		_baseTimeScale = val;
		ApplyTime();
	}

	public static void SetSimulationTimeScale(float val)
	{
		_simulationTimeScale = val;
		ApplyTime();
	}

	private static void ApplyTime()
	{
		Time.timeScale = _isPaused ? 0f : _baseTimeScale * _simulationTimeScale * _effectTimeScale;
	}
}
