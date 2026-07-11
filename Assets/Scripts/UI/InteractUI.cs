using PrimeTween;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
	[Header("References")]
	[SerializeField]
	private RectTransform _interactTransform;

	[Header("Animation Settings")]
	[SerializeField]
	[Tooltip("How far down the UI travels when disabled")]
	private float _lowerValue = 10f;

	[SerializeField]
	[Tooltip("How long the UI takes to move up")]
	private float _shiftUpDuration = 0.15f;

	[SerializeField]
	[Tooltip("How long the UI takes to move up after dialogue")]
	private float _shiftUpDialogueDuration = 0.4f;

	[SerializeField]
	[Tooltip("How long the UI takes to move down")]
	private float _shiftDownDuration = 0.5f;

	private Sequence _interactSequence;
	private float _enabledPositionY;
	private float _disabledPositionY;

	private float TweenProgress
	{
		get
		{
			float distance = _enabledPositionY - _disabledPositionY;
			if (Mathf.Approximately(distance, 0f))
			{
				return 0f;
			}
			return (_interactTransform.anchoredPosition.y - _disabledPositionY) / distance;
		}
	}

	private void Start()
	{
		_enabledPositionY = _interactTransform.anchoredPosition.y;
		_disabledPositionY = _enabledPositionY - _lowerValue;

		Vector2 pos = _interactTransform.anchoredPosition;
		pos.y = _disabledPositionY;
		_interactTransform.anchoredPosition = pos;
	}

	private void OnEnable()
	{
		GameManager.Instance.OnInteractableEnter += EnableUI;
		GameManager.Instance.OnInteractableExit += DisableUI;
	}

	private void OnDisable()
	{
		_interactSequence.Complete();

		if (GameManager.Instance != null)
		{
			GameManager.Instance.OnInteractableEnter -= EnableUI;
			GameManager.Instance.OnInteractableExit -= DisableUI;
		}
	}

	private void EnableUI(bool isAfterDialogue)
	{
		_interactSequence.Stop();
		_interactSequence = Sequence.Create();

		float duration = isAfterDialogue ? _shiftUpDialogueDuration : _shiftUpDuration;
		float upDuration = duration * (1 - TweenProgress);

		if (upDuration > 0.001f)
		{
			_interactSequence.Chain(Tween.UIAnchoredPositionY(_interactTransform, _enabledPositionY, upDuration));
		}
	}

	private void DisableUI()
	{
		_interactSequence.Stop();
		_interactSequence = Sequence.Create();

		float downDuration = _shiftDownDuration * TweenProgress;

		if (downDuration > 0.001f)
		{
			_interactSequence.Chain(Tween.UIAnchoredPositionY(_interactTransform, _disabledPositionY, downDuration));
		}
	}
}
