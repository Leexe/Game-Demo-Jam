using UnityEngine;

public class NpcDialogueController : MonoBehaviour
{
	[SerializeField]
	private string _npcMainKnot;

	[SerializeField]
	private Renderer _renderer;

	[field: SerializeField]
	public Transform CameraLookPoint { get; private set; }

	private static readonly int ObjectRotation = Shader.PropertyToID("_ObjectRotation");
	private MaterialPropertyBlock _mpb;
	private Camera _mainCamera;

	private void Start()
	{
		_mpb = new MaterialPropertyBlock();
		_mainCamera = Camera.main;
	}

	public void StartStory()
	{
		UpdateMaterialRotation();
		DialogueManager.Instance.DialogueState.StartStory(_npcMainKnot);
	}

	private void UpdateMaterialRotation()
	{
		_renderer.GetPropertyBlock(_mpb);
		Vector3 dirToCamera = _mainCamera.transform.position - transform.position;
		dirToCamera.y = 0;
		float angleTowardsCamera = Mathf.Atan2(dirToCamera.x, dirToCamera.z) * Mathf.Rad2Deg;
		_mpb.SetFloat(ObjectRotation, angleTowardsCamera);
		Debug.Log(angleTowardsCamera);
		_renderer.SetPropertyBlock(_mpb);
	}
}
