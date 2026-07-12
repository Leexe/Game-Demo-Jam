using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public class NpcDialogueController : MonoBehaviour
{
	[Header("References")]
	[SerializeField]
	private string _npcMainKnot;

	[SerializeField]
	private Renderer _renderer;

	[field: SerializeField]
	public Transform CameraLookPoint { get; private set; }

	[Header("Animations")]
	[SerializeField]
	private bool _enableAnimations;

	[ShowIf(nameof(_enableAnimations))]
	[SerializeField]
	private AnimancerComponent _animancer;

	[ShowIf(nameof(_enableAnimations))]
	[SerializeField]
	private AnimationClip _idleClip;

	[ShowIf(nameof(_enableAnimations))]
	[SerializeField]
	private AnimationClip _talkingClip;

	[ShowIf(nameof(_enableAnimations))]
	[SerializeField]
	private AnimationClip _returnClip;

	[SerializeField]
	private int _columnOffset;

	[SerializeField]
	private int _rowOffset;

	private static readonly int ObjectRotation = Shader.PropertyToID("_ObjectRotation");
	private static readonly int ColOffsetProp = Shader.PropertyToID("_ColOffset");
	private static readonly int RowOffsetProp = Shader.PropertyToID("_RowOffset");

	private AnimancerState _currentState;
	private int _lastColumnOffset = -1;
	private int _lastRowOffset = -1;
	private MaterialPropertyBlock _mpb;
	private Camera _mainCamera;
	private bool _isInteracting;

	private void Start()
	{
		_mpb = new MaterialPropertyBlock();
		_mainCamera = Camera.main;
	}

	private void OnEnable()
	{
		DialogueManager.Instance.OnDialogueEnded += OnDialogueEnded;
	}

	private void OnDisable()
	{
		if (DialogueManager.Instance != null)
		{
			DialogueManager.Instance.OnDialogueEnded -= OnDialogueEnded;
		}
	}

	public void StartStory()
	{
		UpdateMaterialRotation();
		_isInteracting = true;

		if (_enableAnimations && _animancer != null)
		{
			if (_talkingClip != null)
			{
				_currentState = _animancer.Play(_talkingClip);
			}
		}

		DialogueManager.Instance.DialogueState.StartStory(_npcMainKnot);
	}

	private void OnDialogueEnded()
	{
		if (!_isInteracting)
		{
			return;
		}
		_isInteracting = false;

		if (_enableAnimations && _animancer != null)
		{
			if (_returnClip != null)
			{
				_currentState = _animancer.Play(_returnClip);
				_currentState.Events(this).OnEnd = () =>
				{
					if (_idleClip != null)
					{
						_currentState = _animancer.Play(_idleClip);
					}
				};
			}
			else if (_idleClip != null)
			{
				_currentState = _animancer.Play(_idleClip);
			}
		}
	}

	private void LateUpdate()
	{
		if (_columnOffset == _lastColumnOffset && _rowOffset == _lastRowOffset)
		{
			return;
		}

		_lastColumnOffset = _columnOffset;
		_lastRowOffset = _rowOffset;

		_renderer.GetPropertyBlock(_mpb);
		_mpb.SetInt(ColOffsetProp, _columnOffset);
		_mpb.SetInt(RowOffsetProp, _rowOffset);
		_renderer.SetPropertyBlock(_mpb);
	}

	private void UpdateMaterialRotation()
	{
		_renderer.GetPropertyBlock(_mpb);
		Vector3 dirToCamera = _mainCamera.transform.position - transform.position;
		dirToCamera.y = 0;
		float angleTowardsCamera = Mathf.Atan2(dirToCamera.x, dirToCamera.z) * Mathf.Rad2Deg;
		_mpb.SetFloat(ObjectRotation, angleTowardsCamera);
		_renderer.SetPropertyBlock(_mpb);
	}
}
