void ReconstructWorldPos_float(float2 screenUV, float rawDepth, out float3 worldPos)
{
    worldPos = ComputeWorldSpacePosition(screenUV, rawDepth, UNITY_MATRIX_I_VP);
}