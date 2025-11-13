using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            ProcessTouch(worldPos, true);
        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    Vector2 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //    ProcessTouch(worldPos, false);
        //}
    }

    private void HandleTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
                ProcessTouch(worldPos, true);

            if (touch.phase == TouchPhase.Ended)
                ProcessTouch(worldPos, false);
        }
    }

    private void ProcessTouch(Vector2 worldPos, bool isDown)
    {
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        if (hit == null) return;

        if (hit.TryGetComponent(out normalTile normalTile) && isDown)
        {
            normalTile.OnHit();
        }

        //if (hit.TryGetComponent(out HoldTile holdTile))
        //{
        //    if (isDown)
        //        holdTile.OnHoldStart();
        //    else
        //        holdTile.OnHoldEnd();
        //}
    }
}