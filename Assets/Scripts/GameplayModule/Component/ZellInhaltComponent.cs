using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ZellInhaltComponent : MonoBehaviour, IDragDropComponent, IContentComponent
{
    private const float MINIMUM_DISTANCE = 0.1f;
    public static bool DisableGravityAll { get; set; } = false;

    public EContentType contentType = EContentType.APPLE;

    public event Action DragStartEvent;
    public event Action DragMoveEvent;
    public event Action DragEndEvent;
    public event Action DestroyEvent;

    public ICellComponent Cell { get; set; }
    public bool NotMoving { get; set; }
    /// <summary>
    /// Unable to set: use <see cref="NotMoving"/> to set status
    /// </summary>
    public bool InMovement => NotMoving == false;
    private Rigidbody2D RigidBody { get; set; }
    private float GravityScale { get; set; }
    private bool IsAtCellPosition => Cell != null && (Cell.transform.position - transform.position).magnitude <= MINIMUM_DISTANCE;
    /// <summary>
    /// Tests Rigidbody.velocity
    /// </summary>
    private bool IsStillStanding => RigidBody.velocity == Vector2.zero;

    private Transform Transform { get; set; }

    private Vector3 Position
    {
        get => (Transform is null ? transform : Transform).position;
        set => (Transform is null ? transform : Transform).position = value;
    }

    public GameObject GameObject => gameObject;

    public EContentType ContentType => contentType;

    private void Awake()
    {
        Transform = transform;
    }

    private void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        GravityScale = RigidBody.gravityScale;
        DragDrop.EnableDragDrop(this, new DragDropConfig
        {
            ClickButton = MouseButton.LeftMouse,
            GetZ = DragDropConfig.Z_IS_ZERO,
            DragPositionUpdateHandler = OnDragSnapAxis,
            DropHandler = OnDrop,
            CheckDragStartAllowanceRule = DragDropConfig.ALLOW_DRAG_ALWAYS
        });
    }

    private void OnDrop()
    {
        // validate drop
    }

    private Vector3 OnDragSnapAxis(Vector3 objectDragPositionByMouse, out bool cancelDrag)
    {
        cancelDrag = false;



        return objectDragPositionByMouse;
    }

    private void Update()
    {
        var gravityScale = DisableGravityAll == false ? GravityScale : 0;
        if (gravityScale != RigidBody.gravityScale)
        {
            RigidBody.gravityScale = gravityScale;
        }

        UpdateMoveDetection();
    }

    private void UpdateMoveDetection()
    {
        if (DisableGravityAll == false)
        {
            if (InMovement && IsStillStanding)
            {
                NotMoving = IsAtCellPosition;

                // snap to cell position
                if (NotMoving)
                {
                    SnapToCellPosition();
                }
            }
            else if (NotMoving == true && RigidBody.velocity != Vector2.zero)
            {
                NotMoving = false;
            }
        }
    }

    public void SnapToCellPosition() => transform.position = Cell.transform.position;

    public void OnMouseDown() => DragStartEvent?.Invoke();

    public void OnMouseDrag() => DragMoveEvent?.Invoke();

    public void OnMouseUp() => DragEndEvent?.Invoke();

    private void OnDestroy() => DestroyEvent?.Invoke();

    public Vector3 GetWorldPosition() => Position;

    public void SetWorldPosition(Vector3 objectPosition) => Position = objectPosition;

    public Component GetSelfComponent() => this;
}