using UnityEngine;

public class NpcDialogueController : MonoBehaviour
{
	[SerializeField]
	private string _npcMainKnot;

	[field: SerializeField]
	public Transform CameraLookPoint { get; private set; }

	public void StartStory()
	{
		DialogueManager.Instance.DialogueState.StartStory(_npcMainKnot);
	}
}
