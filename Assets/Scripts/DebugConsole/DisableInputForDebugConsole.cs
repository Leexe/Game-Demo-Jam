using IngameDebugConsole;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Disables input while the debug console is open, so typing "addfish *M*innow"
/// doesn't accidentally return you to the map view. Component should be placed
/// in IngameDebugConsole prefab so it becomes dontdestroyonload.
/// </summary>
public class DisableInputForDebugConsole : MonoBehaviour
{
#if UNITY_EDITOR
	public bool AutoPause = true;

	/** Unity Messages **/

	private void OnEnable()
	{
		DebugLogManager.Instance.OnLogWindowShown += OnConsoleOpen;
		DebugLogManager.Instance.OnLogWindowHidden += OnConsoleClose;
	}

	private void OnDisable()
	{
		if (DebugLogManager.Instance != null)
		{
			DebugLogManager.Instance.OnLogWindowShown -= OnConsoleOpen;
			DebugLogManager.Instance.OnLogWindowHidden -= OnConsoleClose;
		}
	}

	private void Update()
	{
		if (Keyboard.current.escapeKey.wasPressedThisFrame && DebugLogManager.Instance.IsLogWindowVisible)
		{
			DebugLogManager.Instance.HideLogWindow();
		}
	}

	/** Event Handlers **/

	private void OnConsoleOpen()
	{
		Debug.Log("Player input is disabled while the console is open.");
		InputManager.Instance.DisablePlayerInput();
		InputManager.Instance.DisableUIInput();

		if (AutoPause)
		{
			GameManager.SetPaused(true);
		}
	}

	private void OnConsoleClose()
	{
		InputManager.Instance.EnablePlayerInput();
		InputManager.Instance.EnableUIInput();

		if (AutoPause)
		{
			GameManager.SetPaused(false);
		}
	}
#endif
}
