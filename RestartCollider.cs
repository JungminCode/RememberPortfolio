using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartCollider : MonoBehaviour
{
    public GameObject[] fadeOutQuadGroup;   // Player와 Student에 달려있는 페이드 아웃 시킬 Quad의 그룹 배열

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))   // 떨어진 오브젝트가 플레이어라면
        {
            StartCoroutine(PlayerFadeOutRestart(fadeOutQuadGroup[0]));  // 플레이어가 가지고 있는 Quad로, 페이드 아웃 시킨다.
        }
        else if(other.gameObject.CompareTag("Key")) // 떨어진 오브젝트가 키여도
        {
            StartCoroutine(PlayerFadeOutRestart(fadeOutQuadGroup[0]));  // 플레이어가 가지고 있는 Quad로, 페이드 아웃을 시킨다.
        }
        else if(other.gameObject.CompareTag("Student")) // 떨어진 오브젝트가 학생이라면
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;  // 리지드바디 물리를 사용하지 못하게 하고
            StartCoroutine(StudentFadeOutRestart(fadeOutQuadGroup[1])); // 학생 오브젝트가 가지고있는 Quad로 페이드 아웃을 시킨다.
        }
    }

    IEnumerator PlayerFadeOutRestart(GameObject fadeOutQuad)    // 플레이어가 가지고 있는 Quad의 페이드 아웃 코루틴 함수
    {
        yield return new WaitForSeconds(0.1f);  // 0.1 뒤에
        for (float i = 0f; i <= 1f; i += 0.05f)
        {
            Color color = new Vector4(1, 1, 1, i);
            fadeOutQuad.GetComponent<MeshRenderer>().material.color = color;
            yield return null;
        }   // 페이드 아웃 실행
        yield return new WaitForSeconds(0.5f);  // 페이드 아웃이 다 되고, 0.5초 뒤에
        MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 0;
        MouseCursorManager.mouseCursorInstance.mouseRightNumber = 0;
        MouseCursorManager.mouseCursorInstance.mouseTwoNumber = 0;
        // 마우스 커서에 관한 것들 초기화하고
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   // 현재 씬으로 다시 간다.
        StopCoroutine(PlayerFadeOutRestart(fadeOutQuad));   // 페이드 아웃 코루틴을 멈춤
    }

    IEnumerator StudentFadeOutRestart(GameObject fadeOutQuad)   // 학생이 가지고 있는 Quad의 페이드 아웃 코루틴 함수인데, 위와 같다. 방식은
    {
        yield return new WaitForSeconds(0.3f);
        for (float i = 0f; i <= 1f; i += 0.05f)
        {
            Color color = new Vector4(1, 1, 1, i);
            fadeOutQuad.GetComponent<MeshRenderer>().material.color = color;
            yield return null;
        }   // 페이드 아웃 실행
        yield return new WaitForSeconds(0.5f);
        MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 0;
        MouseCursorManager.mouseCursorInstance.mouseRightNumber = 0;
        MouseCursorManager.mouseCursorInstance.mouseTwoNumber = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StopCoroutine(PlayerFadeOutRestart(fadeOutQuad));   // 페이드 아웃 코루틴을 멈춤
    }
}
