using UnityEngine;
using Spine.Unity;

public class CatSpaceshipHatchClosing : MonoBehaviour
{
    [SerializeField] private float maxDistanceToPlayer;
    [SerializeField] private BoxCollider2D playerCollider;
    private CircleCollider2D hatchCollider;
    private bool hatchIsClosed = false;
    public bool HatchIsClosed { get { return hatchIsClosed; } }

    private Animator animator;
    private const string OPENINGHATCH_Anim = "CatSpaceship_OpeningTheHatch";
    private const string CLOSINGHATCH_Anim = "CatSpaceship_ClosingTheHatch";
    private string currentAnimation = null;
    private ObjectSpawnManager objectSpawnManager;
    private SkeletonAnimation skeletonAnimation = null;
    private Spine.TrackEntry trackEntry = null;
    private GameObject electricEffectObj = null;

    private void Awake()
    {
        playerCollider = GameObject.Find("[Player]").GetComponent<BoxCollider2D>();
        hatchCollider = this.transform.GetChild(0).gameObject.GetComponent<CircleCollider2D>();
        objectSpawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
        skeletonAnimation = this.GetComponent<SkeletonAnimation>();
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, "BaseAnimation", true);
        if (objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.DjClimbing)
            electricEffectObj = this.transform.GetChild(0).GetChild(0).gameObject;

        skeletonAnimation.AnimationState.End += delegate {
            if(skeletonAnimation.AnimationName == "OpeningTheHatch")
            {
                trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, "BaseAnimation", true);
            }
        };
    }

    private void Update()
    {
        OperateHatch();
    }

    private void OperateHatch()
    {
        if (PlayerIsNearToSpaceship())
        {
            if (hatchIsClosed == false)
            {
                trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, "ClosingTheHatch", false);
                if (objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.DjClimbing)
                {
                    electricEffectObj.SetActive(false);
                }
            }
            else
            {
                return;
            }
            hatchIsClosed = true;
        }
        else
        {
            if(objectSpawnManager.GetModeName != ObjectSpawnManager.ModeName.DjClimbing)
            {
                if (hatchIsClosed == true)
                {
                    trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, "OpeningTheHatch", false);
                }
                else
                {
                    return;
                }
                hatchIsClosed = false;
            }
        }
    }

    private bool PlayerIsNearToSpaceship()
    {
        return hatchCollider.IsTouching(playerCollider);
    }
}
