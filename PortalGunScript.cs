using UnityEngine;
using System.Collections;

public class PortalGunScript : MonoBehaviour
{
    public GameObject leftPortal, rightPortal, wisp;
    public LayerMask notToHit;
    // 포탈 게임오브젝트와, 포탈을 쏠 때 캐릭터 주변에서 빛이나는 위습 오브젝트, 그리고 레이어마스크를 통해, 캐릭터를 충돌하지 못하게 하기위한 변수들
    public GameObject leftPortalPoint, rightPortalPoint;
    public GameObject leftParticleSystem, leftSideParticleSystem;
    public GameObject rightParticleSystem, rightSideParticleSystem;
    public GameObject leftHitParticleSystem, leftHitSideParticleSystem;
    public GameObject rightHitParticleSystem, rightHitSideParticleSystem;
    public bool portalShootFlag;    // 포탈을 사용할 수 있는 지 없는 지를 확인 시켜주는 변수
    public Color[] colors = new Color[4];   // 라인 렌더러의 칼라를 변경해 줄 Color값 배열

    private Camera subCamera;  // 서브카메라를 오소그래픽으로 맞추고,그 서브카메라로 레이캐스트를 확인
    private Transform firePoint;
    [SerializeField]    // 인스펙터창에 보여진다. private도!
    private float rayLength = 10f;  // 기본 레이캐스트의 선의 길이 값
    private bool portalNumber = false;
    private LineRenderer lineRenderer;  // 라인렌더러 변수

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");  // 첫 지점을 대입
        subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();   // 서브카메라를 변수에 대입
        portalShootFlag = true; // 기본으로는 포탈을 사용할 수 있는 상태를 만든다.
        lineRenderer = GetComponent<LineRenderer>();    // 라인렌더러 컴포넌트 대입
        lineRenderer.enabled = false;   // 라인렌더러를 처음엔 보이지 않게 한다. 쏠 때만 볼 수 있게 하기 위해서
    }

    private void Update()
    {
        if (portalShootFlag)    // 포탈을 쏠 수 있는 상태라면
        {
            if (Input.GetMouseButton(0))    // 왼쪽 마우스를 누르고 있을 상태
            {
                Vector2 mousePos = new Vector2(subCamera.ScreenToWorldPoint(Input.mousePosition).x, subCamera.ScreenToWorldPoint(Input.mousePosition).y);   // 마우스의 포지션값을 가져온다.
                Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y); // 레이가 나올 첫 지점의 포지션값을 가져온다.
                RaycastHit2D hit = Physics2D.Raycast(firePointPos, (mousePos - firePointPos).normalized, rayLength, notToHit);  // 2D Raycast를 사용하고, 최대 10.0f의 길이로, 레이어 마스크가 설정한 곳만을 충돌할 수 있게 하고, 첫 지점에서,
                // 마우스 포지션 - 첫지점을 노말라이즈 한 값에 레이를 쏠 수 있게 한다.
                leftPortal.SetActive(false);    // 왼쪽 포탈은 숨기고,
                lineRenderer.SetColors(colors[0], colors[1]);   // 라인렌더러의 칼라는, 왼쪽 포탈에 있는 파티클과 같은 색깔로, 그라데이션을 시킨다.
                lineRenderer.SetPosition(0, firePoint.transform.position);  // 첫 지점에서
                lineRenderer.SetPosition(1, hit.point); // 충돌 된 지점까지 선을 그릴 수 있게 한다.

                if (hit.collider == null)   // 충돌 된 콜라이더가 없다면
                {
                    leftPortal.GetComponent<PortalMove>().portalFlag = false;   // 포탈을 이동할 수 없게 하고
                    leftPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0);  // 포탈의 위치는 -100.01f , 100.5f , 0으로 설정해서, 포탈 부착 지점을 생성하지 않고
                    MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 0; // 왼쪽 포탈이 부착되지 않았기 때문에, mouseCursorManager에 있는 mouseLeftNumber의 값은 0이 되고
                    lineRenderer.enabled = false;   // 라인렌더러는 그리지 않게 된다.
                }

                else if (hit.collider.tag == "Door")    // 충돌 된 것의 태그가 Door라면,
                {
                    leftPortal.GetComponent<PortalMove>().portalFlag = false;   // 문에도 포탈을 부착할 수 없기 때문에, 포탈을 이동할 수 없게 하고
                    leftPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0);  // 포탈의 위치는 충돌된 콜라이더가 없는 것과 같이 변경하며
                    MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 0; // MouseCursorManager에 있는 mouseLeftNumber의 값은 0이 되고,
                    lineRenderer.enabled = false;   // 라인렌더러는 그리지 않는다.
                }

				else if (hit.collider.tag == "NoPortal")    // NoPortal도 마찬가지이다.
				{
					leftPortal.GetComponent<PortalMove>().portalFlag = false;
					leftPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0);
					MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 0;
					lineRenderer.enabled = false;
				}

                else if (hit.collider.tag == "Ground")  // 땅과 충돌 되었다면
                {
                    leftPortalPoint.transform.position = hit.point;  // portal의 위치를 맞은 위치에 대입
                    leftPortalPoint.transform.rotation = Quaternion.LookRotation(hit.normal);    // 회전값도 맞은곳을 보게 하고
                    leftHitParticleSystem.SetActive(true);      // 바닥에 부착되는 파티클을 보이게 하고
                    leftHitSideParticleSystem.SetActive(false); // 옆면에 부착되는 파티클을 숨긴다.
                    lineRenderer.enabled = true;    // 라인렌더러를 킨다.
                }

                else if (hit.collider.tag == "SideGround")  // 옆면과 충돌 되었다면
                {
                    leftPortalPoint.transform.position = hit.point;  // portal의 위치를 맞은 위치에 대입
                    leftPortalPoint.transform.rotation = Quaternion.LookRotation(hit.normal);    // 회전값도 맞은곳을 보게 하고
                    leftHitParticleSystem.SetActive(false);     // 바닥에 부착되는 파티클은 숨기고
                    leftHitSideParticleSystem.SetActive(true);  // 옆면에 부착되는 파티클을 보이게 한다.
                    lineRenderer.enabled = true;    // 라인렌더러를 킨다.
                }
            }

            if (Input.GetMouseButton(1))    // 오른쪽 마우스를 누르고 있으면, 왼쪽 마우스를 누르고 있는 것과 같지만, 오른쪽 포탈에 대한 값만 변경한다. 오른쪽 포탈에 대한 값은 ★로 대신 처리
            {
                Vector2 mousePos = new Vector2(subCamera.ScreenToWorldPoint(Input.mousePosition).x, subCamera.ScreenToWorldPoint(Input.mousePosition).y);
                Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);
                RaycastHit2D hit = Physics2D.Raycast(firePointPos, (mousePos - firePointPos).normalized, rayLength, notToHit);
                rightPortal.SetActive(false);   // ★
                lineRenderer.SetColors(colors[2], colors[3]);   // ★
                lineRenderer.SetPosition(0, firePoint.transform.position);
                lineRenderer.SetPosition(1, hit.point);

                if (hit.collider == null)
                {
                    rightPortal.GetComponent<PortalMove>().portalFlag = false;   // ★
                    rightPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0);   // ★
                    MouseCursorManager.mouseCursorInstance.mouseRightNumber = 0;   // ★
                    lineRenderer.enabled = false;
                }

                else if (hit.collider.tag == "Door")
                {
                    rightPortal.GetComponent<PortalMove>().portalFlag = false;   // ★
                    rightPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0);   // ★
                    MouseCursorManager.mouseCursorInstance.mouseRightNumber = 0;   // ★
                    lineRenderer.enabled = false;
                }

				else if (hit.collider.tag == "NoPortal")
				{
                    rightPortal.GetComponent<PortalMove>().portalFlag = false;   // ★
                    rightPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0);   // ★
                    MouseCursorManager.mouseCursorInstance.mouseRightNumber = 0;   // ★
					lineRenderer.enabled = false;
				}

                else if (hit.collider.tag == "Ground")   // ★
                {
                    rightPortalPoint.transform.position = hit.point;  // portal의 위치를 맞은 위치에 대입
                    rightPortalPoint.transform.rotation = Quaternion.LookRotation(hit.normal);    // 회전값도 맞은곳을 보게 하고
                    rightHitParticleSystem.SetActive(true);
                    rightHitSideParticleSystem.SetActive(false);
                    lineRenderer.enabled = true;
                }

                else if (hit.collider.tag == "SideGround")   // ★
                {
                    rightPortalPoint.transform.position = hit.point;  // portal의 위치를 맞은 위치에 대입
                    rightPortalPoint.transform.rotation = Quaternion.LookRotation(hit.normal);    // 회전값도 맞은곳을 보게 하고
                    rightHitParticleSystem.SetActive(false);
                    rightHitSideParticleSystem.SetActive(true);
                    lineRenderer.enabled = true;
                }
            }

            if (Input.GetMouseButtonUp(0))  // 왼쪽 포탈 부착 지점을 생성하고 왼쪽 마우스를 뗐을때
            {
                lineRenderer.enabled = false;   // 라인렌더러를 끄고
                LeftThrowPortal(leftPortal);    // LeftThrowPortal함수를 실행, 매개 변수는 leftPortal 게임오브젝트를 사용
            }

            if (Input.GetMouseButtonUp(1))  // 오른쪽 포탈 부착 지점을 생성하고 오른쪽 마우스를 뗐을 때
            {
                lineRenderer.enabled = false;   // 라인렌더러를 끄고
                RightThrowPortal(rightPortal);  // RightThrowPortal함수를 실행, 매개 변수는 rightPortal 게임오브젝트를 사용
            }
        }
    }

    void LeftThrowPortal(GameObject portal) // 왼쪽 포탈에 대한 함수
    {
        portal.GetComponent<PortalMove>().portalFlag = true;    // 포탈에 있는 PortalMove라는 스크립트에 있는 portalFlag를 true로 만들어, 포탈이 부착되었다는 것을 알려준다.

        Vector2 mousePos = new Vector2(subCamera.ScreenToWorldPoint(Input.mousePosition).x, subCamera.ScreenToWorldPoint(Input.mousePosition).y);   // 마우스의 위치를 대입
        Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);     // 레이가 쏘여질 위치를 대입
        RaycastHit2D hit = Physics2D.Raycast(firePointPos, mousePos - firePointPos, rayLength, notToHit);   // 최대 위치 rayLength만큼, 첫 지점에서, 마우스 위치 - 첫 지점만큼 , 레이어 충돌 지정한 곳에 2D Raycast를 사용할 수 있게 한다.

        if (hit.collider == null)
        {
            leftPortal.GetComponent<PortalMove>().portalFlag = false;
            MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 0;
            leftPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0);
            leftPortal.SetActive(false);
        }

		if (hit.collider != null && hit.collider.tag == "NoPortal")
		{
			leftPortal.GetComponent<PortalMove>().portalFlag = false;
			MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 0;
			leftPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0);
			leftPortal.SetActive(false);
		}
        // 포탈이 부착되지 말아야되는 곳이라면,
        // 포탈을 이동하지 못하게 만들고, 부착되지 않았기 때문에, MouseCursorManager의 mouseLeftNumber의 값은 0으로, 
        // 왼쪽 포탈 부착 지점의 위치는 처음 위치로 보내고, 왼쪽 포탈은 보이지 않게 한다.

        if (hit.collider != null && hit.collider.tag == "Ground")   // 부딪힌게 널값이 아니고, 태그가 Ground라는 땅이라면
        {
            SoundManager.instance.PlayShotSound();  // SoundManager에 있는 PlayShotSound함수를 실행시켜, 포탈을 쐈을 때 소리를 실행시키고
            portal.transform.position = hit.point;  // portal의 위치를 맞은 위치에 대입
            portal.transform.rotation = Quaternion.LookRotation(hit.normal);    // 회전값도 맞은곳을 보게 하고
            leftPortal.SetActive(true); // leftPortal은 보이게 하고
            leftParticleSystem.SetActive(true); // 땅에 부딪힐 때 쓰이는 파티클을 보이게 하고,
            leftSideParticleSystem.SetActive(false);    // 옆면에 부딪힐 때 쓰이는 파티클은 숨기고
            leftPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0);  // 포탈 부착 지점의 오브젝트는 원래 위치로 보내고
            MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 1; // 부착되었기 때문에, mouseLeftNumber는 1로
            StartCoroutine(PortalCoroutine(wisp));  // 그리고, wisp의 게임 오브젝트를 매개변수로 사용한, PortalCoroutine이라는 코루틴 함수를 실행
        }

        if (hit.collider != null && hit.collider.tag == "SideGround")   // 부딪힌 곳이 옆면이라면
        {
            SoundManager.instance.PlayShotSound();  // SoundManager에 있는 PlayShotSound함수를 실행시켜, 포탈을 쐈을 때 소리를 실행시키고
            portal.transform.position = hit.point;  // portal의 위치를 맞은 위치에 대입
            portal.transform.rotation = Quaternion.LookRotation(hit.normal);    // 회전값도 맞은곳을 보게 하고
            leftPortal.SetActive(true); // leftPortal은 보이게 하고
            leftParticleSystem.SetActive(false); // 땅에 부딪힐 때 쓰이는 파티클을 숨기고,
            leftSideParticleSystem.SetActive(true);    // 옆면에 부딪힐 때 쓰이는 파티클은 보이게 하고
            leftPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0);  // 왼쪽 포탈 부착 지점 오브젝트는 원래 위치로 보내고
            MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 1; // 이것 또한 부착되었기 때문에, mouseLeftNumber의 값을 1로 변경
            StartCoroutine(PortalCoroutine(wisp));  // wisp의 게임 오브젝트를 매개변수로 사용한, PortalCoroutine이라는 코루틴 함수를 실행
        }
    }

    void RightThrowPortal(GameObject portal)    // 왼쪽 포탈 함수와 같은 방식이다. 이것 또한, 오른쪽에 관한 것은 ★로 주석처리를 대신
    {
        portal.GetComponent<PortalMove>().portalFlag = true;

        Vector2 mousePos = new Vector2(subCamera.ScreenToWorldPoint(Input.mousePosition).x, subCamera.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPos, mousePos - firePointPos, rayLength, notToHit);

        if (hit.collider == null)
        {
            rightPortal.GetComponent<PortalMove>().portalFlag = false;  // ★
            MouseCursorManager.mouseCursorInstance.mouseRightNumber = 0;// ★
            rightPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0); // ★
            rightPortal.SetActive(false); // ★
        }

		if (hit.collider != null && hit.collider.tag == "NoPortal")
		{
            rightPortal.GetComponent<PortalMove>().portalFlag = false; // ★
            MouseCursorManager.mouseCursorInstance.mouseRightNumber = 0; // ★
            rightPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0); // ★
            rightPortal.SetActive(false); // ★
		}

        if (hit.collider != null && hit.collider.tag == "Ground")   // 부딪힌게 널값이 아니고, 태그가 Ground라는 땅 또는 벽이라면
        {
            SoundManager.instance.PlayShotSound(); 
            portal.transform.position = hit.point;  // portal의 위치를 맞은 위치에 대입
            portal.transform.rotation = Quaternion.LookRotation(hit.normal);    // 회전값도 맞은곳을 보게 하고
            rightPortal.SetActive(true); // ★
            rightParticleSystem.SetActive(true); // ★
            rightSideParticleSystem.SetActive(false); // ★
            rightPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0); // ★
            MouseCursorManager.mouseCursorInstance.mouseRightNumber = 2; // ★
            StartCoroutine(PortalCoroutine(wisp));
        }

        if (hit.collider != null && hit.collider.tag == "SideGround")
        {
            SoundManager.instance.PlayShotSound();
            portal.transform.position = hit.point;  // portal의 위치를 맞은 위치에 대입
            portal.transform.rotation = Quaternion.LookRotation(hit.normal);    // 회전값도 맞은곳을 보게 하고
            rightPortal.SetActive(true); // ★
            rightParticleSystem.SetActive(false); // ★
            rightSideParticleSystem.SetActive(true); // ★
            rightPortalPoint.transform.position = new Vector3(-100.01f, 100.5f, 0); // ★
            MouseCursorManager.mouseCursorInstance.mouseRightNumber = 2; // ★
            StartCoroutine(PortalCoroutine(wisp));
        }
    }

    IEnumerator PortalCoroutine(GameObject wispObj)
    {
        wispObj.SetActive(true);    // 위습을 보이게 하고,
        yield return new WaitForSeconds(0.5f);  // 0.5초 뒤에
        wispObj.SetActive(false);   // 위습을 숨긴다.
        StopCoroutine(PortalCoroutine(wispObj));    // 그리고 코루틴을 정지시킨다.
    }
}
