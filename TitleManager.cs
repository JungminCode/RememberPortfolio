using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Animator[] animators; // 버튼을 페이드 인 한 애니메이션들을 넣을 배열
    public Button[] buttons;    // 버튼 UI의 배열
    public GameObject creditPanel , titleStart, titleLoop, titlelogoLoop;   // 크레딧 패널과 타이틀을 애니메이션으로 만든 것들의 변수들
    [SerializeField]
    private string loadSceneName = "Tutorial";  // Start버튼을 눌렀을 때 시작할 씬의 이름 변수
    [SerializeField]
    private float buttonOnTime = 7.9f;  // 버튼이 페이드 인 되는 시간의 float형 변수, 코루틴 함수에서 사용
    float time = 0f;    // 사운드를 시작하고, 1초 후에 들릴 수 있게 하기 위한 변수
    AudioSource audio;  // 현재 씬의 AudioSource를 넣을 변수

    private void Awake()
    {
        StartCoroutine(ButtonOn());     // 버튼 페이드 인 코루틴 함수를 실행
        StartCoroutine(TitleStart());   // 타이틀 애니메이션을 교체해 줄 코루틴 함수
        audio = this.GetComponent<AudioSource>();   // audio라는 변수에 AudioSource 컴포넌트를 대입
    }

    private void Update()
    {
        time += Time.deltaTime; // time을 Time.deltaTime만큼 더해준다.
        if (time >= 1.0f)   // time이 1초보다 같거나 크거면
        {
            audio.UnPause();    // 일시정지를 풀어 실행한다.
        }
        else
        {
            audio.Pause();  // 아니면, 일시정지 시킨다.
        }
    }

    public void StartButtonClicked()    // 스타트 버튼 클릭 버튼 함수
    {
        SoundManager.instance.ButtonClickSound();   // 버튼 클릭 사운드 실행
        SceneManager.LoadSceneAsync(loadSceneName); // loadSceneName에 설정되어 있는 씬의 이름으로 로딩한다.
    }

    public void CreditButtonClicked()   // 크레딧 버튼 클릭시
    {
        creditPanel.SetActive(true);    // 크레딧 패널을 활성화 시켜준다.
        SoundManager.instance.ButtonClickSound();   // 버튼 클릭 사운드 실행
    }

    public void ExitButtonClicked() // 종료 버튼 클릭 함수
    {
        SoundManager.instance.ButtonClickSound();   // 버튼 클릭 사운드 실행
        Application.Quit(); // 빌드 된 파일을 종료한다.
    }

    public void CreditExitButtonClicked()   // 크레딧 패널에 있는, Exit 버튼을 눌렀을 때
    {
        creditPanel.SetActive(false);   // 크레딧 패널을 비활성화 시킨다.
    }

    IEnumerator ButtonOn()  // 버튼 페이드 인 코루틴 함수
    {
        yield return new WaitForSeconds(buttonOnTime);  // 설정해놓은, 시간만큼 기다리고, 시간이 다 되면
        for (int i = 0; i < 3; i++)
        {
            animators[i].SetTrigger("FadeIn");
        }   // 페이드 인 애니메이션을 실행
        yield return new WaitForSeconds(2.2f);  // 2.2초 후에
        for(int i=0; i<3; i++)
        {
            buttons[i].interactable = true;
        }   // 버튼이 클릭될 수 있게 한다.
    }

    IEnumerator TitleStart()
    {
        yield return new WaitForSeconds(buttonOnTime);  // 설정해놓은 시간만큼 기다리고, 시간이 다 되면
        titleStart.SetActive(false);    // titleStart라는 게임 오브젝트를 비활성화하고
        titleLoop.SetActive(true);      // titleLoop와
        titlelogoLoop.SetActive(true);  // titlelogoLoop를 활성화 시킨다.
    }
}
