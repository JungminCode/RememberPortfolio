using UnityEngine;
using System.Collections;

public class PlayerScirpt : MonoBehaviour
{
	public bool ground = false;    // 땅과 닿았는지 안닿았는지 체크
	public bool jump;              // 점프 제한 변수
	public Animator charAnim;  // 캐릭터 애니메이터 변수
	public bool moveFlag = true;

	private float key; // 수평부분, 즉 A,D또는 왼쪽,오른쪽 화살표로 이동 가능하게 할 변수
	private SpriteRenderer spriteRenderer;
	[SerializeField]
	private float moveForce = 125f; // 이동 힘
	[SerializeField]
	private float speed = 10.0f;    // 속도 변수
	[SerializeField]
	private float jumpForce = 500f; // 점프 힘 변수

	private void Start()
	{
		spriteRenderer = GameObject.Find("Character").GetComponent<SpriteRenderer>();
		charAnim = GameObject.Find("Character").GetComponent<Animator>();
	}

	void Update()
	{
		key = Input.GetAxisRaw("Horizontal"); // GetAxis함수의 Horizontal을 사용한다.
		CheckGround();  // 땅 밟았는 지 안밟았는지 체크하는 함수
		if (Input.GetButtonDown("Jump") && ground) // 점프버튼을 눌렀고, ground가 true라면,
		{
			jump = true;   // jump를 true로 바꾼다
			charAnim.SetTrigger("doJumping");
			charAnim.SetBool("isJumping", true);
		}
		if (moveFlag)
		{
			Move(); // 이동 함수
		}
		Jump();
	}

	void Move()
	{
		if (moveFlag)
		{
			if (key != 0)  // key가 0이 아니면, 즉 움직이면
			{
				if (key * GetComponent<Rigidbody>().velocity.x < speed)   // key * x값의 속도가 speed보다 작다면
				{
					GetComponent<Rigidbody>().AddForce(Vector2.right * key * moveForce);  // 힘을 Vector2.right * key * moveForce만큼 준다.
				}
			}

			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))     // A키 또는 왼쪽 화살표 키를 눌렀을 때는, 스프라이트를 반전시킨다.
			{
				spriteRenderer.flipX = true;
			}

			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))    // D키 또는 오른쪽 화살표를 눌렀을 땐, 스프라이트 반전을 하지 않는다.
			{
				spriteRenderer.flipX = false;
			}

			if (key == 0)   // 움직이지 않으면, Idle상태로 변경
			{
				charAnim.SetBool("isMoving", false);
			}

			else if (key < 0)
			{
				charAnim.SetBool("isMoving", true);
			}

			else if (key > 0)
			{
				charAnim.SetBool("isMoving", true);
			}

            // 왼쪽 또는 오른쪽으로 이동시엔, 애니메이터에 있는 isMoving을 true로 바꾸고, 이동하는 애니메이션을 실행 

			if (Mathf.Abs(GetComponent<Rigidbody>().velocity.x) > speed)   // x의 속도 절대값이 speed보다 크면 속도를 조절시킨다.
			{
				GetComponent<Rigidbody>().velocity = new Vector2
					(Mathf.Sign(GetComponent<Rigidbody>().velocity.x) * speed, GetComponent<Rigidbody>().velocity.y);
				//속도 조절
				//sign -> 값이 양이거나 0 일때 1반환 음의 값이면 -1 반환
			}

			else
			{
				GetComponent<Rigidbody>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody>().velocity.x)
					* 0, GetComponent<Rigidbody>().velocity.y);
			}
		}
	}

	void Jump()
	{
		if (jump)  // jump가 true라면
		{
			GetComponent<Rigidbody>().AddForce(new Vector3(0f, jumpForce, 0f));    // Rigidbody를 사용하고, y값의 힘을 jumpForce만큼 줘서 띄운다.
			SoundManager.instance.PlayJumpSound();
			jump = false;  // jump는 false가 된다.
		}
	}

	void CheckGround()  // 3D용 Raycast를 이용해서, 체크
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f) || 
			Physics.Raycast(transform.position - new Vector3(0.7f,0,0), Vector3.down, out hit, 1.5f)
			|| Physics.Raycast(transform.position + new Vector3(0.7f, 0, 0), Vector3.down, out hit, 1.5f))  // 캐릭터 밑에 Raycast를 3개 쏴서, 점프를 할 수 있는 상태인지 아닌 지를 확인
		{
			if (hit.transform.tag == "Ground")
			{
				ground = true;
				return;
			}
			else if (hit.transform.tag == "Key")
			{
				ground = true;
				return;
			}
			else if (hit.transform.tag == "Lock")
			{
				ground = true;
				return;
			}
			else if(hit.transform.tag == "LeftPortal")
			{
				ground = true;
				return;
			}
			else if(hit.transform.tag == "RightPortal")
			{
				ground = true;
				return;
			}
			else if(hit.transform.tag == "Student")
			{
				ground = true;
				return;
			}
            // 레이캐스트와 충돌 된 것의 태그가 Ground, Key , Lock , LeftPortal , RightPortal , Student라면, 점프를 뛸 수 있는 상태로 변경
		}
		charAnim.SetBool("isJumping", false);
		ground = false; // 평소에는 false인 상태로 만들어, 점프를 할 수 없게 만든다.
	}

}