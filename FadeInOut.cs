using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField]    // [SerializeField]를 사용하여, private로 선언된 변수도 보이게 한다.
    private float minus = 0.02f;    // minus를 0.02로 float형 변수로 선언한다.
    // 기획자가 시간 조절을 할 수 있게 만듬.

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(FadeIn());   // FadeIn 코루틴 함수를 실행
	}

    IEnumerator FadeIn()
    {
        for(float i=1f; i >= 0; i-= minus)  // i가 0보다 크거나 같을 때까지 minus를 빼서, 쿼드의 알파값을 뺀 값만큼, 계속 업데이트를 시켜준다.
        {
            Color color = new Vector4(1, 1, 1, i);
            GetComponent<MeshRenderer>().material.color = color;
            yield return null;
        }
    }
}
