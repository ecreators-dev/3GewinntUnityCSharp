using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class DragDrop
{
    private readonly Camera camera;
    private Vector3 offset;

    public static DragDrop ActiveDragCommand { get; private set; }

    protected DragDrop()
    {
        this.camera = Camera.main;
    }

    protected void BeginDrag(Vector3 objectWorldPosition)
    {
        offset = camera.WorldToScreenPoint(objectWorldPosition) - Input.mousePosition;
    }

    protected bool DragUpdate(float z, DragPositionUpdateHandler positionUpdateHandler = null)
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 position = new Vector3(mousePosition.x, mousePosition.y, z);
        Vector3 objectPosition = camera.ScreenToWorldPoint(position + offset);

        bool cancelDrag = false;
        objectPosition = positionUpdateHandler?.Invoke(objectPosition, out cancelDrag) ?? objectPosition;

        if (cancelDrag)
        {
            return false;
        }
        UpdatePositionPosition(objectPosition);
        return true;
    }

    protected abstract void UpdatePositionPosition(Vector3 objectPosition);

    protected void Drop(Vector3 objectWorldPosition)
    {

    }

    private class DragDropCommandIntern : DragDrop
    {
        private readonly IDragDropComponent owner;
        private readonly DragDropConfig config;

        public DragDropCommandIntern(IDragDropComponent owner, DragDropConfig config = null)
        {
            this.owner = owner;
            this.config = config ?? new DragDropConfig();
        }

        private bool IsMouseButtonDown => Input.GetMouseButton((int)config.ClickButton);

        private bool CanStartDragProcess => config.CheckDragStartAllowanceRule == null || config.CheckDragStartAllowanceRule.Invoke();

        public void Register()
        {
            Unregister();

            owner.DragStartEvent += OnObjectDragBegin;
            owner.DragMoveEvent += OnObjectDrag;
            owner.DragEndEvent += OnObjectDrop;
            owner.DestroyEvent += Unregister;
        }

        private void Unregister()
        {
            owner.DragStartEvent -= OnObjectDragBegin;
            owner.DragMoveEvent -= OnObjectDrag;
            owner.DragEndEvent -= OnObjectDrop;
            owner.DestroyEvent -= Unregister;
        }

        private void OnObjectDragBegin()
        {
            if (IsMouseButtonDown && CanStartDragProcess)
            {
                DragDrop.ActiveDragCommand = this;
                ActiveDragCommand.BeginDrag(owner.GetWorldPosition());
            }
        }

        private void OnObjectDrop()
        {
            DragDrop.ActiveDragCommand.Drop(owner.GetWorldPosition());
            config.DropHandler?.Invoke();
            ActiveDragCommand = null;
        }

        private void OnObjectDrag()
        {
            // drag was cancelled
            if(DragDrop.ActiveDragCommand is null)
            {
                return;
            }

            Vector3 position = owner.GetWorldPosition();
            float z = config.GetZ?.Invoke() ?? position.z;
            bool continueDrag = DragDrop.ActiveDragCommand.DragUpdate(z, config.DragPositionUpdateHandler);
            if(continueDrag is false)
            {
                OnObjectDrop();
            }
        }

        protected override void UpdatePositionPosition(Vector3 objectPosition)
        {
            owner.SetWorldPosition(objectPosition);
        }
    }

    private class DragDropUiCommand : DragDrop
    {
        private readonly IDragDropUIComponent owner;
        private readonly DragDropConfig config;

        public DragDropUiCommand(IDragDropUIComponent owner, DragDropConfig config = null)
        {
            this.owner = owner;
            this.config = config ?? new DragDropConfig();
        }

        public void Register()
        {
            Unregister();

            owner.DragStartEvent += OnObjectDragBegin;
            owner.DragMoveEvent += OnObjectDrag;
            owner.DragEndEvent += OnObjectDrop;
            owner.DestroyEvent += Unregister;
        }

        private void OnObjectDrop(PointerEventData obj)
        {
        }

        private void OnObjectDrag(PointerEventData obj)
        {
        }

        private void OnObjectDragBegin(PointerEventData obj)
        {
        }

        public void Unregister()
        {
            owner.DragStartEvent -= OnObjectDragBegin;
            owner.DragMoveEvent -= OnObjectDrag;
            owner.DragEndEvent -= OnObjectDrop;
            owner.DestroyEvent -= Unregister;
        }

        protected override void UpdatePositionPosition(Vector3 objectPosition)
        {
            owner.SetWorldPosition(objectPosition);
        }
    }

    public static void EnableDragDrop(IDragDropUIComponent owner, DragDropConfig config) => new DragDropUiCommand(owner, config).Register();

    public static void EnableDragDrop(IDragDropComponent owner, DragDropConfig config) => new DragDropCommandIntern(owner, config).Register();
}