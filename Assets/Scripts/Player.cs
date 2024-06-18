using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject thanks;
    private Rigidbody2D Rigidbody;
    private Animator animator;
    private Vector2 velocity;
    public float speed = 3.0f;
    private Transform chk;
    public float jumpForce = 5.0f; // 점프 힘 추가
    private bool isGrounded;
    private bool isJumping;
    private bool isWalked = true;
    public bool waiting = false;
    public bool isJumped = false;
    [SerializeField] private GameObject bodyObject;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        animator = bodyObject.GetComponent<Animator>();
    }
    void Start()
    {
        chk = transform.Find("Check");
    }
    void Update()
    {
        Move();
        Jump();
        CheckGround();
    }
    void FixedUpdate()
    {
        Rigidbody.velocity = new Vector2(velocity.x, Rigidbody.velocity.y);
    }
    void Move()
    {
        if (isWalked){
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            velocity = new Vector2(horizontalInput, 0).normalized * speed;
            if (velocity.x != 0)
            {
                animator.SetBool("isWalk", true);
            }
            else
            {
                animator.SetBool("isWalk", false);
            }

            if (horizontalInput > 0)
            {
                bodyObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (horizontalInput < 0)
            {
                bodyObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

    }
    void Jump()
    {
        jumpForce = 5.0f;
        if (isGrounded && !isJumping)
        {
            if (Input.GetButton("Jump")){
                isWalked = false;
                isJumped = true;
                animator.SetBool("isReady", true);
            }
            if (Input.GetButtonUp("Jump")){
                SpriteRenderer spriteRenderer = bodyObject.GetComponent<SpriteRenderer>();
                string spriteName = spriteRenderer.sprite.name;

                if (spriteName == "Ready_0"){
                    jumpForce = 5.0f;
                    animator.SetBool("W_Jump", true);
                }
                else if (spriteName == "Ready_1"){
                    jumpForce = 8.0f;
                    animator.SetBool("N_Jump", true);
                }
                else if (spriteName == "Ready_2"){
                    jumpForce = 12.0f;
                    animator.SetBool("S_Jump", true);
                }

                animator.SetBool("isReady", false);
                isWalked = true;
                isJumping = true;
                Rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }
    void CheckGround(){
        RaycastHit2D hit = Physics2D.Raycast(chk.position, Vector2.down, 0.2f);
        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            if (!waiting){
                StartCoroutine(ResetJump());

                SpriteRenderer spriteRenderer = bodyObject.GetComponent<SpriteRenderer>();
                string spriteName = spriteRenderer.sprite.name;

                if (spriteName == "W_Down_0"|| spriteName == "W_Down_1"|| spriteName == "W_Down_2"){
                    animator.SetBool("W_Down", false);
                }
                else if (spriteName == "N_Down_0"|| spriteName == "N_Down_1"|| spriteName == "N_Down_2"){
                    animator.SetBool("N_Down", false);
                }
                else if (spriteName == "S_Down_0"|| spriteName == "S_Down_1"|| spriteName == "S_Down_2"){
                    animator.SetBool("S_Down", false);
                }

                animator.SetBool("isDown", false);
            }
        }
        else{
            isGrounded = false;
            SpriteRenderer spriteRenderer = bodyObject.GetComponent<SpriteRenderer>();
            string spriteName = spriteRenderer.sprite.name;

            if (isJumped){
                if (spriteName == "W_Jump_0" || spriteName == "W_Jump_1" || spriteName == "W_Jump_2"){
                    animator.SetBool("W_Down", true);
                    animator.SetBool("W_Jump", false);
                }
                else if (spriteName == "N_Jump_0" || spriteName == "N_Jump_1" || spriteName == "N_Jump_2"){
                    animator.SetBool("N_Down", true);
                    animator.SetBool("N_Jump", false);
                }
                else if (spriteName == "S_Jump_0" || spriteName == "S_Jump_1" || spriteName == "S_Jump_2"){
                    animator.SetBool("S_Down", true);
                    animator.SetBool("S_Jump", false);
                }
                isJumped = false;
            }
            else{
                animator.SetBool("isDown", true);
            }
        }
    }
    IEnumerator ResetJump()
    {
        waiting = true;
        yield return new WaitForSeconds(0.5f); // 짧은 대기 시간으로 점프 상태 리셋
        isGrounded = true;
        isJumping = false;
        waiting = false;
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        Transform other = coll.transform;

        switch (other.tag)
        {
            case "Enemy":
                SceneManager.LoadScene(1); break;
            case "Goal":
                thanks.SetActive(true); break;
        }
    }
    public void TitleGame()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        StopAllCoroutines();
        Application.Quit();
    }
    public void Off()
    {
        thanks.SetActive(false);
    }
}
