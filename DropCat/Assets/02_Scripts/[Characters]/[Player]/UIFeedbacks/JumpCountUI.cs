using UnityEngine;
using UnityEngine.UI;
using CharacterManager;

public class JumpCountUI : MonoBehaviour
{
    [SerializeField] private Image[] JumpCountImages;
    private PlayerManager playerManager = null;
    private int jumpCountCache = 0;
    private int maxJumpCount = 3;

    private void Awake()
    {
        playerManager = GameObject.Find("[Player]").GetComponent<PlayerManager>();
        jumpCountCache = playerManager.AvailableJump;
    }

    private void Update()
    {
        UpdateJumpCountUI();
    }

    private void UpdateJumpCountUI()
    {
        if (jumpCountCache != playerManager.AvailableJump)
        {
            for (int i = 0; i < maxJumpCount; i++)
            {
                if (i < playerManager.AvailableJump)
                {
                    JumpCountImages[i].enabled = true;
                    LeanTween.scale(JumpCountImages[i].gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutBounce();
                }
                else
                {
                    JumpCountImages[i].enabled = false;
                    LeanTween.scale(JumpCountImages[i].gameObject, new Vector3(0, 0, 0), 0.2f).setEaseInBounce();
                }
            }
            jumpCountCache = playerManager.AvailableJump;
        }
    }
    
}
