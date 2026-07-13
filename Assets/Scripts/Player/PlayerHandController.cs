using Animancer;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
	[Header("References")]
	[SerializeField]
	private AnimancerComponent _animancer;

	[SerializeField]
	private MyCharacterController _myCharacterController;

	[Header("Animations")]
	[SerializeField]
	private AnimationClip _idleClip;

	[SerializeField]
	private AnimationClip _walkClip;

	[Header("Settings")]
	[SerializeField]
	private float _crossFadeDuration = 0.25f;

	private AnimationClip _currentClip;

	private void Update()
	{
		bool isMoving = _myCharacterController.CurrentHorVelocity.sqrMagnitude > 0.01f;
		bool isGrounded = _myCharacterController.IsGrounded;

		AnimationClip targetClip = (isMoving && isGrounded) ? _walkClip : _idleClip;

		if (targetClip != null && _currentClip != targetClip)
		{
			_currentClip = targetClip;
			_animancer.Play(targetClip, _crossFadeDuration);
		}
	}
}
