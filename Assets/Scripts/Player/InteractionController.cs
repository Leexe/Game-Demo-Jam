using UnityEngine;

public class InteractionController : MonoBehaviour
{
	[Header("Data")]
	[SerializeField]
	private float _interactDistance = 5f;

	[Header("Layer Masks")]
	[SerializeField]
	private LayerMask _interactionLayerMask;

	[SerializeField]
	private LayerMask _npcLayerMask;

	private Camera _mainCamera;

	private void Start()
	{
		_mainCamera = Camera.main;
	}

	private void OnEnable()
	{
		InputManager.Instance.OnInteractPerformed += OnInteract;
		DialogueManager.Instance.OnDialogueEnded += OnDialogueEnded;
	}

	private void OnDisable()
	{
		if (InputManager.Instance != null)
		{
			InputManager.Instance.OnInteractPerformed -= OnInteract;
		}

		if (DialogueManager.Instance != null)
		{
			DialogueManager.Instance.OnDialogueEnded -= OnDialogueEnded;
		}
	}

	private void OnInteract()
	{
		if (DialogueManager.Instance == null)
		{
			return;
		}

		var ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
		if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance, _npcLayerMask))
		{
			NpcDialogueController npcDialogueController = hit.collider.GetComponent<NpcDialogueController>();
			if (npcDialogueController != null)
			{
				CameraManager.Instance.FocusOn(npcDialogueController.CameraLookPoint);

				npcDialogueController.StartStory();
			}
			else
			{
				Debug.LogWarning("Npc Dialogue Controller Not Found");
			}
		}
	}

	private void OnDialogueEnded()
	{
		CameraManager.Instance.ClearFocus();
	}
}
