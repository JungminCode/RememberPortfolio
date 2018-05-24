using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneStageClear : MonoBehaviour
{
    public Animator[] clearAnims;   // 0 : 카메라 , 1: 옷장 , 2: 스팟라이트 , 3: 인형
    public GameObject[] clearGameobjs;  // 0: 카메라 , 1: 클리어 이펙트 , 2: 페이드 아웃 쿼드 , 3: 플레이어
	public GameObject[] uiObj;
    private GameObject generator;   // 비 X자 게임 오브젝트
    private GameObject xBlackHole;  // 블랙홀 이펙트 오브젝트
    [SerializeField]
    private string nextSceneName = "";  // 다음으로 넘어 갈 scene의 네임
    private bool fadeOut = false;   // 페이드 아웃 코루틴을 사용하기 위한 bool형 변수
    private bool nextScene = false; // 넥스트 씬 코루틴을 사용하기 위한 bool형 변수
    private bool dollMove = false;  // 인형이 떠오르는 것을 막기 위한 bool형 변수
    float moveSpeed = 0.4f; // 위로 올라가는 것의 스피드값

    private void Awake()
    {
        generator = GameObject.FindWithTag("Enemy");    // 비 X자 게임 오브젝트 찾기
        xBlackHole = GameObject.Find("Blackhole");  // Blackhole 게임 오브젝트 찾기
        this.GetComponent<AudioSource>().Pause();
    
    }

    private void Update()
    {
        float dollY = moveSpeed * Time.deltaTime;   //  인형의 y값의 이동 좌표를 속도 * Time.deltaTime으로 대입
        if (dollMove)   // dollMove가 true일 때
        {
            this.transform.Translate(0f, dollY, 0f);    // 인형에 스크립트가 부착되어 있기 때문에, this를 썼고, 그 인형의 y값 좌표를 dollY만큼, true일 때까지 계속 올린다.
            if (this.transform.position.y >= 12.0f) // 인형의 y값이 12이상이면
            {
                dollMove = false;   // dollMove를 false로 바꿔주어, 더 이상 이동하지 못하게 한다.
            }
        }
        if (fadeOut)
        {
            StartCoroutine(FadeOut());  // fadeOut이 참이면, 페이드아웃 코루틴 실행
        }
        if (nextScene)   // nextScene이 참이면
        {
            StartCoroutine(NextScene());    // NextScene 코루틴 실행
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))    // 인형이 부딪힌 것의 Tag가 Key와 같다면
        {
            Destroy(other.gameObject);  // 촛불을 없애고,
            StartCoroutine(FinishAnimation());  // FinishAnimation이라는 코루틴을 실행
            this.GetComponent<AudioSource>().UnPause();
        }
    }

    IEnumerator FinishAnimation()
    {
		uiObj [0].SetActive (false);
		uiObj [1].SetActive (true);
        clearGameobjs[3].SetActive(false);  // 플레이어 감추기
        Cursor.visible = false;
        clearGameobjs[0].SetActive(true);   // 애니메이션 카메라 보이기
        clearGameobjs[0].GetComponent<Camera>().depth = 0;  // 애니메이션 카메라가 주 카메라로 되게 depth를 0으로 설정
        Destroy(generator); // 비 X자 없애기
        Destroy(xBlackHole);    // 블랙홀 이펙트 없애기
        clearGameobjs[1].SetActive(true);   // 이펙트 보이기
        dollMove = true;
        clearAnims[3].SetTrigger("DollHit");    // 인형 애니메이션 시작
        yield return new WaitForSeconds(5.2f);  // 5.2초후에
        this.transform.position = new Vector3(-39.13f, 12.15f, 0f); // 인형을 -39.13 , 12.15 , 0의 위치로 옮겨놓는다.
        yield return new WaitForSeconds(1.0f);
        clearAnims[0].SetTrigger("FinishCamera");   // 카메라를 점점 줌 아웃한다.
        yield return new WaitForSeconds(2.0f);  // 2초 후에
        clearAnims[2].SetTrigger("SpotLight");  // 스팟라이트를 점점 키운다.
        yield return new WaitForSeconds(7.0f);  // 7초 후에
        clearAnims[1].SetTrigger("CloseDoor");  // 옷장의 문을 닫는다.
        SoundManager.instance.ClosetCloseSound();
        yield return new WaitForSeconds(2.2f);  // 2.2초후에
        clearAnims[0].SetTrigger("FinishCamera2");  // 카메라를 옷장에 있는 아이의 얼굴로 줌인을 한다.
        yield return new WaitForSeconds(4f);  // 4초 후에
        fadeOut = true; // fadeOut을 true로 바꾸고
        StopCoroutine(FinishAnimation());   // 현재 코루틴을 멈춤
    }

    IEnumerator FadeOut()
    {
        for (float i = 0f; i <= 1f; i += 0.02f)
        {
            Color color = new Vector4(1, 1, 1, i);
            clearGameobjs[2].GetComponent<MeshRenderer>().material.color = color;
            yield return null;
        }   // 페이드 아웃 실행
        nextScene = true;   // nextScene을 true로 바꾸고
        StopCoroutine(FadeOut());   // 페이드 아웃 코루틴을 멈춤
    }

    IEnumerator NextScene()
    {
        GameManger.GM.NextStage();
        yield return new WaitForSeconds(2.0f);  // 2초 후에
        SceneManager.LoadScene(nextSceneName); // Stage2로 넘어간다.
    }
}
