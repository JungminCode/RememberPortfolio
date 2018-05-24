using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StudentManager : MonoBehaviour
{
	public bool studentMoveFlag = true;     // 학생 이동 가능 상태 변수
	public GameObject[] onCandleObj;        // 촛불이 학생과 부딪혔을 때의 상태 게임 오브젝트 변수
	public GameObject[] offCandleObj;       // 아직 없어지지 않은 촛불의 UI 게임 오브젝트 변수
	public GameObject mainCamera , subCamera , successParticle , successParticle_1  , fadeOutImage , fireParticle , fireParticle_1 , fireParticle_2;
    // 메인카메라 , 서브카메라 , 클리어시 파티클 2개 , 흰색으로 되어 있는 페이드 아웃 이미지 변수 , 책이 불타는 파티클 3개의 게임오브젝트 변수
	public Transform[] wayPoint;    // 날아가게 할 수 있는 연출을 위한 wayPoint 배열
	[HideInInspector]
	public int count;   // 촛불과 학생이 부딪혔을 때의 count변수
	public bool countProcess;   // 애니메이션들이 시작되는 것을 막기위해 추가해놓은 countProcess라는 변수
	public GameObject[] portals;    // 클리어시 부착되어 있던 포탈들을 숨기기위한 포탈 배열

	private bool fly, waypoint , fadeOut , nextScene = false;  // 코루틴 사용 및 업데이트에 쓰일 상태 변수
	private Animator studentAnimator;   // 학생의 애니메이터 변수
	private GameObject books , player;  // 책과, 플레이어의 게임오브젝트 변수
	private float speed = 20f;  // WayPoint들을 갈 때의 스피드 변수
	private int cur = 0;    // WayPoint 배열에 쓰일 변수
	private float curtest = 0.1f;   // 벡터의 크기가 0.1보다 크거나 같은 지를 비교하기위한 float형 변수
	private float time = 0f;    // 애니메이션이 끝나고, 페이드 아웃을 시킬 때,원하는 시간에 시킬려고 만든 변수
	[SerializeField]
	private string nextSceneName = "Stage3";    // 페이드 아웃이 끝나고, 다음 씬으로 넘어갈 때 필요한, 다음 씬 이름 변수 , 인스펙터창에서 변경 가능
    [SerializeField]
    private float studentSpeed = 0.007f;    // 학생이 움직이는 속도 변수 , 이것 또한 인스펙터 창에서 변경 가능


	private void Awake()
	{
		count = 0;  // count를 0으로 초기화
		countProcess = false;   // countProcess를 false로 초기화
		studentAnimator = GameObject.Find("Student").GetComponent<Animator>();  // 학생의 애니메이터 컨포넌트를 대입
		books = GameObject.Find("Book");    // Book의 게임오브젝트를 books라는 게임 오브젝트에 대입
		player = GameObject.FindWithTag("Player");  // Player의 오브젝트를 player변수에 대입
        this.GetComponent<AudioSource>().Pause();   // 현재 이 스크립트를 가지고 있는 오브젝트에 있는 오디오 소스 컴포넌트를 일시정지시킨다.
	}

	// Update is called once per frame
	void Update()
	{
		if (studentMoveFlag)    // 학생이 움직일 수 있는 상태라면
		{
            this.transform.Translate(studentSpeed, 0f, 0f); // x를 studentSpeed만큼 이동시킨다.
		}

		if(count == 1 && countProcess == true)      // 촛불 하나가 부딪혔으면,
		{
            books.GetComponent<Animator>().SetBool("First", true);  // 첫번째 책을 태우는 애니메이션을 실행시키고
			onCandleObj [0].SetActive (true);   // 하나의 촛불이 사라졌다는 UI를 보이게 하고
			offCandleObj [0].SetActive (false); // 없어졌으니, offCandleObj 배열 0번째에 있는 UI를 숨긴다.
			fireParticle.SetActive(true);   // 책이 태워지는 파티클을 보이게 한다.
		}

		if(count == 2 && countProcess == true)  // 촛불이 총 합해서 두개가 부딪혔다면
		{
            books.GetComponent<Animator>().SetBool("Second", true); // 두번째 책더미가 태워지는 애니메이션을 실행시키고
			onCandleObj [1].SetActive (true);   // 두개의 촛불이 사라졌다는 UI를 보이게 하고
			offCandleObj [1].SetActive (false); // offCandleObj의 1번째 배열에 있는 UI를 숨긴다.
			fireParticle_1.SetActive(true); // 두번째 책더미가 태워지는 파티클을 보이게 한다.
		}

		if(count == 3 && countProcess == true)  // 촛불이 총 합해서 3개가 부딪혔다면
		{
            fly = true; // fly를 true로 해서, 날아갈 수 있다는 상태를 만들어주고
			onCandleObj [2].SetActive (true);   // 세개의 촛불이 사라졌단 UI를 보이게 하고
			offCandleObj [2].SetActive (false); // offCandleObj의 2번째 배열에 있는 UI를 숨긴다.
            fireParticle_2.SetActive(true); // 세번째 책더미가 태워지는 파티클을 보이게 한다. 
		}

		if(fly) // 날 수 있는 상태면
		{
			StartCoroutine(StudentFly());   // StudentFly라는 코루틴 함수를 실행
		}

		if(fadeOut) // 페이드 아웃을 시킬 수 있는 상태라면,
		{
			time += Time.deltaTime; // time이란 변수에 Time.deltaTime만큼 더해주고
			if (time >= 0.2f)   // time이란 변수가 0.2보다 같거나 크면
			{
				StartCoroutine(FadeOut());  // FadeOut 시킬 코루틴 함수 실행
			}
		}

		if(nextScene)   // 다음씬으로 넘어 갈 상태라면
        {
            StartCoroutine(NextScene());    // 다음씬으로 넘어 갈 코루틴 함수를 실행
        }
	}

	private void FixedUpdate()
	{
		if(waypoint)    // 날 준비가 완료되었다면,
		{
			if (Mathf.Abs((transform.position - wayPoint[cur].position).sqrMagnitude) >= curtest)
			{
				Vector3 p = Vector3.MoveTowards(transform.position, wayPoint[cur].position, speed * Time.deltaTime);    // 학생을 wayPoint의 배열만큼 speed * deltaTime값 만큼의 속도로 이동시키는 것을, Vector3형 p라는 변수에 대입
				GetComponent<Rigidbody>().MovePosition(p);  // 리지드바디에 있는 MovePosition이라는 함수를 이용하여, 위치를 이동시킨다. 웨이포인트 만큼
			}
			else if(cur >= 3)   // cur가 3보다 크거나 같으면
			{
				waypoint = false;   // 웨이포인트를 움직일 수 없게 하고,
				fadeOut = true;     // 페이드 아웃을 사용할 수 있게 한다.
			}
			else
			{
				cur = (cur + 1) % wayPoint.Length;  // cur를 다음 웨이포인트로 이동시키게 끔 대입을 시켜준다. 0에 도착하면, 1로 변경, 1에 도착하면 2로 변경 이런식으로
			}
		}
	}

	IEnumerator StudentFly()
	{
        books.GetComponent<Animator>().SetBool("Third", true);  // 세번째 책이 불타고
        yield return new WaitForSeconds(5.0f);  // 5초 뒤에
		portals [0].SetActive (false); 
		portals [1].SetActive (false);  // 왼,오른쪽 포탈을 숨기고
		Cursor.visible = false; // 마우스 커서를 숨기고
        this.GetComponent<AudioSource>().UnPause(); // 일시정지되어있던, AudioSource를 일시정지 풀어, 클리어 사운드를 실행시키고
        subCamera.GetComponent<AudioListener>().enabled = true; // 서브카메라에 있는 오디오 리스너를 활성화시키고
        player.SetActive(false);    // player를 숨긴다.
        successParticle.SetActive(true);    // 클리어시 파티클 2개를 활성화 시키고
        successParticle_1.SetActive(true);
        mainCamera.SetActive(false);    // 메인 카메라를 비활성화시키고
		subCamera.SetActive(true);      // 서브 카메라를 활성화 시키고
		studentAnimator.SetTrigger("Fly");  // Fly라는 트리거를 실행시켜, 하늘 나는 모션의 애니메이션을 실행시키게 한다.
		books.SetActive(false); // 책 오브젝트를 비활성화 시키고
		yield return new WaitForSeconds(5.0f);  // 5초 뒤에
		waypoint = true;    // waypoint를 true로 만들어, 하늘을 날게 만들고
		studentMoveFlag = false;    // 학생은 움직일 수 없게 한다.
		StopCoroutine(StudentFly());    // 현재 코루틴을 정지 시킨다.
	}

	IEnumerator FadeOut()
	{
		for (float i = 0f; i <= 1f; i += 0.02f)
		{
			Color color = new Vector4(1, 1, 1, i);
			fadeOutImage.GetComponent<Image>().color = color;
			yield return null;
		}   // 날아 갈 수 있는 WayPoint의 3번째 배열에 도착했다면, 페이드 아웃 실행
		nextScene = true;   // nextScene을 true로 바꾸고
		StopCoroutine(FadeOut());   // 페이드 아웃 코루틴을 멈춤
	}

	IEnumerator NextScene()
	{
		yield return new WaitForSeconds(2.0f);  // 2초 후에
        SceneManager.LoadScene(nextSceneName);  // 다음 씬으로 지정한 이름의 씬을 로딩한다.
	}

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Key")) // Key라는 태그를 가진 촛불과 부딪혔다면
        {
            SoundManager.instance.BookFireBurnSound();  // 불타는 사운드를 실행하고
            count += 1; // count를 1더해준다.
            countProcess = true;    // countProcess는 true로 바꾸고
            Destroy(other.gameObject);  // 촛불의 게임오브젝트를 없앤다.
            return;
        }
    }
}