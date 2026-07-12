using Unity.Cinemachine;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField]
    private MyCharacterController _mc;

    [SerializeField]
    private CinemachineBasicMultiChannelPerlin _cmPerlin;

    [SerializeField]
    private CinemachineImpulseSource _cmImpulse;

    [Tooltip("Speed at which bob amplitude is 0/maxed out")]
    public Vector2 ReferenceSpeedRange;

    [Tooltip("Fall heights at which impulse strength is 0/maxed out")]
    public Vector2 ReferenceFallHeightRange = new(2f, 10f);

    public float FallImpulseMagnitude = 0.2f;

    private float _fallFromY;
    private float _baseAmpGain;
    private float _baseFreqGain;

    private void OnEnable()
    {
        _baseAmpGain = _cmPerlin.AmplitudeGain;
        _baseFreqGain = _cmPerlin.FrequencyGain;
        _mc.OnLanding.AddListener(OnLand);
    }

    private void OnDisable()
    {
        _cmPerlin.AmplitudeGain = _baseAmpGain;
        _cmPerlin.FrequencyGain = _baseFreqGain;
        if (_mc != null)
        {
            _mc.OnLanding.RemoveListener(OnLand);
        }
        
    }

    private void Update()
    {   
        _fallFromY = _mc.IsFalling ? _fallFromY : _mc.transform.position.y;

        float bobMagnitude = _mc.IsGrounded ? Mathf.InverseLerp(ReferenceSpeedRange[0], ReferenceSpeedRange[1], _mc.CurrentHorVelocity.magnitude) : 0;
        _cmPerlin.AmplitudeGain = _baseAmpGain * bobMagnitude;
        _cmPerlin.FrequencyGain = _baseFreqGain * bobMagnitude;
    }

    private void OnLand()
    {
        float fallDistance = Mathf.Max(0f, _fallFromY - _mc.transform.position.y);
        float normalizedFallHeight = Mathf.InverseLerp(ReferenceFallHeightRange[0], ReferenceFallHeightRange[1], fallDistance);
        float impulseMagnitude = FallImpulseMagnitude * normalizedFallHeight;
        _cmImpulse.GenerateImpulseWithVelocity(Vector2.down * impulseMagnitude);
    }
}
