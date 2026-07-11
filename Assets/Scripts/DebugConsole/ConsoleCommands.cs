using IngameDebugConsole;
using UnityEngine;

/// <summary>
/// Static class containing all console commands.
/// </summary>
public static class ConsoleCommands
{
#if UNITY_EDITOR

	[ConsoleMethod("autopause", "sets whether the game freezes on opening the console")]
	public static void AutoPause(bool enabled)
	{
		if (ComponentExists(out DisableInputForDebugConsole comp))
		{
			comp.AutoPause = enabled;
			Debug.Log($"auto pause set to {enabled}");
		}
	}

	// helper function that checks if there's a component in the scene.
	// useful if we want our command to call a function on a gameobject in the scene.
	private static bool ComponentExists<T>(out T component, bool optional = false)
		where T : Component
	{
		component = Object.FindAnyObjectByType<T>();
		if (component == null)
		{
			if (!optional)
			{
				Debug.LogWarning($"{typeof(T).Name} not found in scene");
			}
			return false;
		}
		return true;
	}
#endif
}
