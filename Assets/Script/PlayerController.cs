using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    private PlayerStats playerStats;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private Vector2 lastMove;

    // Use this for initialization
    void Start () {
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Menu.menuIsActive) return;
        animator.SetBool("isMoving", false);
        rigidBody.velocity = new Vector2(0f,0f);

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f) {
            rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerStats.moveSpeed, rigidBody.velocity.y);
            animator.SetBool("isMoving", true);
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"),0f);
        }

        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f) {

            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Input.GetAxisRaw("Vertical") * playerStats.moveSpeed);
            animator.SetBool("isMoving", true);
            lastMove = new Vector2( 0f, Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f) {
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
        }
        if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f) {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
        }

        animator.SetFloat("xInput", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("yInput", Input.GetAxisRaw("Vertical"));
        animator.SetFloat("yLastInput", lastMove.y);
        animator.SetFloat("xLastInput", lastMove.x);

    }
    public void TransferPlayer(RoomTransfer.Directions direction) {
        CmdTransferPlayer(direction);
    }
    [Command]
    void CmdTransferPlayer(RoomTransfer.Directions direction) {
        RpcTransferPlayer(direction);
    }
    [ClientRpc]
    void RpcTransferPlayer(RoomTransfer.Directions direction) {
        var localPlayer = NetworkManager.singleton.client.connection.playerControllers[0].gameObject;

        switch (direction) {
            case RoomTransfer.Directions.Up: {
                    localPlayer.transform.position = new Vector2(transform.position.x, transform.position.y + 4);
                    Camera.main.transform.Translate(new Vector2(0, 12));
                    break;
                }
            case RoomTransfer.Directions.Down: {
                    localPlayer.transform.position = new Vector2(transform.position.x, transform.position.y - 4);
                    Camera.main.transform.Translate(new Vector2(0, -12));
                    break;
                }
            case RoomTransfer.Directions.Left: {
                    localPlayer.transform.position = new Vector2(transform.position.x - 5, transform.position.y);
                    Camera.main.transform.Translate(new Vector2(-21, 0));
                    break;
                }
            case RoomTransfer.Directions.Rigth: {
                    localPlayer.transform.position = new Vector2(transform.position.x + 5, transform.position.y);
                    Camera.main.transform.Translate(new Vector2(21, 0));
                    break;
                }
            default:
                break;
        }
    }
    public void ReLoadDungeon() {
        CmdReLoadDungeon();
    }
    [Command]
    public void CmdReLoadDungeon() {
        GameObject.Find("GameManager").GetComponent<GameManager>().RpcReLoadDungeon();
    }
}
