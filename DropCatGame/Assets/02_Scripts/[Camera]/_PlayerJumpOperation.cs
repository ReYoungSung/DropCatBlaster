using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CharacterBehaviour.Move;

public class _PlayerJumpOperation : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject player;
    private GroundCheck groundCheck;
    private float initialSize = 536.5f;
    [SerializeField] private float resizeSpeed;
    private float maxSize = 550f;

    private Rigidbody2D playerRigidBody2D;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        groundCheck = player.GetComponent<GroundCheck>();
        playerRigidBody2D = player.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        virtualCamera.m_Lens.OrthographicSize = initialSize;
    }

    private void Update()
    {
        float distanceToGround = groundCheck.GetDistanceToGround();
        if(distanceToGround > 0)
        {
            if(20f < player.transform.position.y)
            {
                float camSize = virtualCamera.m_Lens.OrthographicSize;
                if (playerRigidBody2D.velocity.y > 0)
                {
                    camSize += resizeSpeed * Time.deltaTime;
                }
                else if (playerRigidBody2D.velocity.y < 0)
                {
                    camSize -= resizeSpeed * Time.deltaTime;
                }
                camSize = Mathf.Clamp(camSize, initialSize, maxSize);
                virtualCamera.m_Lens.OrthographicSize = camSize;
            }
        }
    }
}
