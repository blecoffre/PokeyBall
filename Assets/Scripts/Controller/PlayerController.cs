using PokeyBallTest.Manager;
using UnityEngine;

[RequireComponent(typeof(SpringJoint), typeof(Rigidbody))][DisallowMultipleComponent]//Prevent adding multiple PlayerController wich can lead to weird behaviour
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody m_tempRigidbody = default;
    [SerializeField] private LineRenderer m_jointRenderer = default;
    [SerializeField] private float m_maxSpeed = 20.0f;
    [SerializeField] private float m_minHeightOffsetToLaunch = 0.2f;

    private SpringJoint m_joint; //The joint used to propel the ball
    private Rigidbody m_rigidbody;
    private bool m_isAttachedToWall = true;
    private bool m_isDragging = false;
    private Vector3 m_offset = Vector3.zero;
    private float m_zOffset = 2.5f;
    private LayerMask m_mask; //used to raycast only against specific layers
    private Vector3 m_wallHitPoint = Vector3.zero;
    private bool m_isAlive = true;
    private float m_startHeight = 0.0f;


    void Start()
    {
        SetMask();
        m_joint = GetComponent<SpringJoint>();
        m_rigidbody = GetComponent<Rigidbody>();
        TryAttachToWall();

        m_zOffset = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    /// <summary>
    /// Set the Mask used on raycast
    /// </summary>
    private void SetMask()
    {
        LayerMask wallMask = LayerMask.NameToLayer(LayersName.Target);
        LayerMask obstacleMask = LayerMask.NameToLayer(LayersName.Obstacle);
        LayerMask instantDeathObstacle = LayerMask.NameToLayer(LayersName.InstantDeathObstacle);
        m_mask = wallMask | obstacleMask | instantDeathObstacle;
        m_mask = ~m_mask;
    }

    /// <summary>
    /// Update is the most appropriate place to check inputs
    /// </summary>
    private void Update()
    {
        if (m_isAlive)
        {
            ProcessLeftMouseClick();
            RenderJoint();
        }
    }

    private void FixedUpdate()
    {
        //Clamp velocity
        if (m_rigidbody.velocity.magnitude > m_maxSpeed)
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_maxSpeed;
        }
    }

    /// <summary>
    /// All actions using Left Mouse Click
    /// </summary>
    private void ProcessLeftMouseClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!m_isAttachedToWall)
            {
                TryAttachToWall();
            }
            else
            {               
                m_startHeight = transform.position.y;
                m_offset = transform.position - GetMousePosition();
                m_isDragging = true;   
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && m_isDragging)
        {
            DetachPlayerFromWall();
            m_isDragging = false;
        }

        if (m_isDragging)
            MouseDrag();
    }

    /// <summary>
    /// Try attach the ball on the wall, handle if target can kill the ball, if no valid target found, attach to substitute object
    /// </summary>
    private void TryAttachToWall()
    {
        RaycastHit hit = RaycastForward();

        if (hit.transform.gameObject.layer == LayerMask.NameToLayer(LayersName.Obstacle)) //If layer is "Obstacle", player can't attach to it
        {
            AttachBallToSubstituteBody();
        }
        else if (hit.transform.gameObject.layer == LayerMask.NameToLayer(LayersName.InstantDeathObstacle))
        {
            PlayerIsDiying();
            AttachBallToSubstituteBody();
        }
        else if (hit.transform.gameObject.layer == LayerMask.NameToLayer(LayersName.Target))
        {
            m_isAttachedToWall = true;
            m_wallHitPoint = hit.point;
            Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();
            if (rb) //If target is valid, check if it has a rigidbody to attach to
            {
                m_joint.connectedAnchor = m_wallHitPoint;
                m_joint.connectedBody = rb;
                SetJointRendererState(true);
            }
            else //If the target is valid but has no rigidbody to attach to, the ball will attach to the Substitute one
            {
                AttachBallToSubstituteBody();
            }
        }
        else //Default, attach to substitute rigidbody
        {
            AttachBallToSubstituteBody();
        }
    }

    private void AttachBallToSubstituteBody()
    {
        m_joint.connectedBody = m_tempRigidbody;
    }

    /// <summary>
    /// Raycast to get the object forward the ball and get the object where it should attach
    /// </summary>
    /// <returns></returns>
    private RaycastHit RaycastForward()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, Mathf.Infinity, m_mask))
        {
            return hit;
        }

        return new RaycastHit();
    }

    /// <summary>
    /// Apply the drag offset to the ball by setting it's position
    /// </summary>
    private void MouseDrag()
    {
        SetPlayerPositionWhileDragging();
    }

    /// <summary>
    /// Set the position of the player relative to the player dragging action
    /// </summary>
    private void SetPlayerPositionWhileDragging()
    {
        Vector3 mousePosition = GetMousePosition();
        if ((mousePosition + m_offset).y < transform.position.y) //Security check to lock drag to the top
        {
            transform.position = mousePosition + m_offset;
        }    
    }

    /// <summary>
    /// Get mouse position from screen space into World space
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = m_zOffset;
        mousePos.x = 0;

        return Camera.main.ScreenToViewportPoint(mousePos);
    }

    /// <summary>
    /// Detach the player from the wall and let him fly !!!
    /// </summary>
    private void DetachPlayerFromWall()
    {
        if (m_isAttachedToWall && IsHeightOffsetEnoughToLaunch())
        {
            m_joint.connectedBody = m_tempRigidbody;
            m_isAttachedToWall = false;
        }
    }

    /// <summary>
    /// Check the Start Height at the begining of drag, and the current height of the ball, and return if this offset is enough to
    /// allow the ball to detach from the wall and start moving
    /// </summary>
    /// <returns></returns>
    private bool IsHeightOffsetEnoughToLaunch()
    {
        return ((m_startHeight - transform.position.y) > m_minHeightOffsetToLaunch);
    }

    /// <summary>
    /// Called when player collider hit an other collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagsName.DeathZone))
        {
            PlayerIsDiying();
        }
    }

    /// <summary>
    /// Set both start and end position of the line renderer used to represent the joint between the ball and the wall
    /// </summary>
    private void RenderJoint()
    {
        if (m_isAttachedToWall)
        {
            m_jointRenderer.SetPosition(0, transform.position); //The first point of the line renderer is on the player himself
            m_jointRenderer.SetPosition(1, m_wallHitPoint); //The second point of the line renderer is on the wall
        }
        else
        {
            //If ball is not attached, hide the LineRenderer
            SetJointRendererState(false);
        }
    }

    /// <summary>
    /// Called when player is Diying (Fall on ground, Try to attach to a Death Block)
    /// </summary>
    private void PlayerIsDiying()
    {
        EventManager.TriggerEvent(EventsName.PlayerDie);
        m_isAlive = false;
        SetJointRendererState(false);
    }

    /// <summary>
    /// Enable / Disable JointRenderer according to passed value
    /// </summary>
    /// <param name="enabled"></param>
    private void SetJointRendererState(bool enabled)
    {
        if (m_jointRenderer)
            m_jointRenderer.enabled = enabled;
    }
}
