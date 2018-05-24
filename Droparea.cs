using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droparea : MonoBehaviour
{
    public Animator studentAnimator;    // 2스테이지 학생의 애니메이터 변수
    public GameObject books , mainCamera , subCamera;   // 게임 오브젝트, 책 그리고 메인카메라와 서브카메라 변수
    private StudentManager studentManager;  // StudentManager 스크립트의 변수

    private void Awake()
    {
        studentManager = GameObject.FindGameObjectWithTag("Student").GetComponent<StudentManager>();    // StudentManager 스크립트 가져오기
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Student")   // 트리거 충돌한 것의 태그가 Student라면
        {
            studentAnimator.SetTrigger("Drop"); // Drop이라는 트리거를 실행시켜, 떨어지는 애니메이션을 실행시킨다.
            books.SetActive(false);     // books 변수에 담긴, 책의 오브젝트를 끈다.
            studentManager.studentMoveFlag = false; // 학생이 움직이는 것을 멈추게 한다.
            mainCamera.SetActive(false);    // 메인카메라를 끈다.
            subCamera.SetActive(true);      // 서브카메라를 킨다.
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;     // 학생의 BoxCollider의 Trigger를 false로 바꾸어, 트리거에서 콜라이더로 충돌하게 한다.
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;     // 학생의 리지드바디에 있는, isKinematic을 false로 바꾸어, 물리효과를 끈다.
        }
    }
}
