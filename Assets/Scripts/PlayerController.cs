using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks, IDamagable
{
    [SerializeField] GameObject ui;
    [SerializeField] GameObject glases;
    [SerializeField] UIPlayer uiPlayer;

    [SerializeField] GameObject cameraHolder;
    [SerializeField] float  sprintSpeed, walkSpeed, jumpForce, smoothTime;
    public float mouseSensitivity;
    [SerializeField] Item[] items;

    bool isFire;
    int itemIndex;
    int previousItemIndex = -1;

    //ammunation
    int ammunation;

    const float maxHealth = 100f;
    float curretHealth = maxHealth;

    float verticalLookRotation;
    bool grounded;
    Vector3 smothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody playerRigidbody;
    PhotonView PV;
    PlayerManager playerManager;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }
    private void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);
            Cursor.lockState = CursorLockMode.Locked;
            mouseSensitivity = LobbyManager.manager.sensetivety;
            Destroy(glases);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(playerRigidbody);
            Destroy(ui);
            for (int i = 0; i < items.Length; i++)
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;
        Look();
        Move();
        Jump();
        UpdateUI(itemIndex);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);
            }
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if(itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            isFire = true;
            items[itemIndex].Use();
        }
        /*else if (Input.GetMouseButtonUp(0))
        {
            isFire = false;
            items[itemIndex]?.GetComponent<AutoShotGun>().Use(isFire);
        }*/
        if (Input.GetKeyDown(KeyCode.R))
        {
           items[itemIndex]?.GetComponent<Gun>().Reload();

        }

        if (transform.position.y < -50)
        {
            Die();
        }
    }
    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        playerRigidbody.MovePosition(playerRigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 85.5f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smothMoveVelocity, smoothTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            playerRigidbody.AddForce(transform.up * jumpForce);
        }
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;

        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);
        UpdateUI(itemIndex);
        


        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    public void SetGroundedSate(bool _grounded)
    {
        grounded = _grounded;
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    void Die()
    {
        playerManager.Die();
    }

    public void UpdateUI(int index)
    {
        uiPlayer.ammunation.text = items[index]?.GetComponent<Gun>().ammunation.ToString();
        uiPlayer.maxAmmunation.text = items[index]?.GetComponent<Gun>().maxAmmunation.ToString();
        uiPlayer.itemName.text = items[index].itemName;
    }
    IEnumerator TimerUpdateUI(float timer)
    {
        yield return new WaitForSeconds(timer);
        UpdateUI(itemIndex);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine)
        {
            return;
        }

        curretHealth -= damage;
        uiPlayer.HealthBarImage.fillAmount = curretHealth / maxHealth;

        if (curretHealth <= 0)
        {
            Die();
        }
    }
}