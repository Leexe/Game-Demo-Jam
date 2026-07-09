using System;
using UnityEngine;

public class NpcDialogueController : MonoBehaviour
{
	[SerializeField]
	private string _npcMainKnot;

	private void OnEnable()
	{
		InputManager.Instance.OnInteractPerformed += OnInteract;
	}

	private void OnDisable()
	{
		if (InputManager.Instance != null)
		{
			InputManager.Instance.OnInteractPerformed -= OnInteract;
		}
	}

	private void OnInteract() { }
}
