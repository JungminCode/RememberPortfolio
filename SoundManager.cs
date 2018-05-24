using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioClip jumpSound; // 점프할 때 사운드
    public AudioClip shotSound; // 포탈 부착 사운드
    public AudioClip portalMoveSound;   // 포탈 이동 사운드
    public AudioClip closeClosetSound;  // 옷장 닫는 사운드
    public AudioClip buttonClickSound;  // 버튼 클릭시 사운드
    public AudioClip bookFireBurnSound; // 책이 불타는 애니메이션 시 사운드
    AudioSource soundManager;   // AudioSource를 넣을 변수

    public static SoundManager instance;    // 현재 클래스를 static으로 하여금, 싱글톤 패턴으로 만듬

    private void Awake()
    {
        if(SoundManager.instance == null)
        {
            SoundManager.instance = this;   // 사운드 매니저 자신을 넣어준다.
        }
    }

    private void Start()
    {
        soundManager = GetComponent<AudioSource>(); // AudioSource 컴포넌트를 대입
    }

    public void PlayJumpSound()
    {
        soundManager.PlayOneShot(jumpSound);    // 점프 사운드를 실행
    }

    public void PlayShotSound()
    {
        soundManager.PlayOneShot(shotSound);    // 포탈 부착 사운드를 실행
    }

    public void PlayPortalMoveSound()
    {
        soundManager.PlayOneShot(portalMoveSound);  // 포탈이동 사운드를 실행
    }

    public void ClosetCloseSound()
    {
        soundManager.PlayOneShot(closeClosetSound); // 옷장 닫는 사운드를 실행
    }

    public void ButtonClickSound()
    {
        soundManager.PlayOneShot(buttonClickSound); // 버튼 클릭 사운드를 실행
    }

    public void BookFireBurnSound()
    {
        soundManager.PlayOneShot(bookFireBurnSound);    // 책이 불타는 사운드를 실행
    }
}
