using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LodingManager : MonoBehaviour
{
    public Text sliderText; // 0~100% 까지 업데이트 시킬 Text 변수
    bool IsDone = false;    // 비동기 로딩을 시킬 수 있게 하기 위한 bool형 변수
    float fTime = 0f;       // fTime은, 로딩이 다 되어도, 설정한 시간만큼은 다음 씬으로 넘어가지 않게하기 위한 시간 변수
    float per = 0f;         // per는, 0~100으로 환산시켜주는 변수
    [SerializeField]
    string sceneName;       // 다음씬으로 넘길 scene의 이름을 넣을 변수
    AsyncOperation async_operation; // 비동기적으로 코루틴을 사용하는 것의 변수

    void Start()
    {
        MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 0; // 마우스 커서의 mouseLeftNumber를 0으로 초기화
        MouseCursorManager.mouseCursorInstance.mouseRightNumber = 0;  // 마우스 커서의 mouseRightNumber를 0으로 초기화
        MouseCursorManager.mouseCursorInstance.mouseTwoNumber = 0;  // 마우스 커서의 mouseTwoNumber를 0으로 초기화
        Cursor.visible = false;     // 마우스 커서를 숨긴다.
        StartCoroutine(StartLoad(sceneName));   // StartLoad라는 코루틴 함수에 sceneName을 매개변수로 실행
    }

    void Update()
    {
        fTime += Time.deltaTime;       // fTime에 Time.deltaTime을 더해준다.
        slider.value = fTime;          // 슬라이더의 값에 fTime 대입한다.

        per = slider.value * 100f;  // per에 slider의 값의 100f 곱한 만큼을, 대입한다.

        sliderText.text = per.ToString("N0") + "%";     // Text변수에, 0~100%까지 출력시킨다.


        if (fTime >= 3.5f)  // fTime이 3.5f보다 크면
        {
            Cursor.visible = true;  // 마우스 커서를 보이게 한다.
            async_operation.allowSceneActivation = true;    // 다음 씬 로딩이 준비되면, 다음씬을 즉시 활성화 시킨다.
        }
    }

    public IEnumerator StartLoad(string strSceneName)
    {
        async_operation = SceneManager.LoadSceneAsync(strSceneName);    // strSceneName에 넣어져 있는 씬을 비동기 로딩 시킨다.
        async_operation.allowSceneActivation = false;   // 아직 준비가 되지 않았기 때문에, 다음씬을 즉시 활성화 시키지 않는다.

        if (IsDone == false)    // IsDone이 false라면,
        {
            IsDone = true;  // IsDone을 true로 만들고,

            while (async_operation.progress < 0.9f)     // 작업의 진행상태가, 0.9f보다 작을 때까지
            {
                slider.value = async_operation.progress;    // slider의 값에 작업의 진행상태을 대입

                yield return true;
            }
        }
    }
}
