using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteIntensityAnimator : MonoBehaviour
{
    public Volume volume; // Volume 컴포넌트를 참조
    private Vignette vignette; // Vignette 효과를 위한 변수
    private float targetIntensity = 0f; // 목표 Intensity 값 (0)
    private float duration = 2f; // 2초 동안 변화
    private float startTime; // 애니메이션 시작 시간

    void Start()
    {
        // Volume에서 Vignette 효과를 찾기
        if (volume.profile.TryGet(out vignette))
        {
            // 초기 Intensity 값 설정
            vignette.intensity.Override(1f); // 처음엔 1로 설정
            startTime = Time.time; // 시작 시간을 기록
        }
    }

    void Update()
    {
        if (vignette != null)
        {
            // 현재 시간에 따른 Intensity 값 계산
            float elapsedTime = Time.time - startTime; // 경과 시간
            float progress = elapsedTime / duration; // 0에서 1로 변화

            // 2초가 지나면 Intensity를 0으로 설정
            vignette.intensity.Override(Mathf.Lerp(1f, targetIntensity, progress));

            // 2초가 지나면 애니메이션을 멈추고 Intensity를 정확히 0으로 설정
            if (progress >= 1f)
            {
                vignette.intensity.Override(targetIntensity);
            }
        }
    }
}