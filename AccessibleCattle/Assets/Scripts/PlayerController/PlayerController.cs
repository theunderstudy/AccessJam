using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private Camera m_Camera;
    private Ray m_Ray;
    private RaycastHit m_Hit;
    public PlayerAction CurrentAction;
    public PlayerAction[] PlayerActions;
    protected override void Awake()
    {
        base.Awake();
        m_Camera = Camera.main;
    }


    private void Update()
    {
        if (CurrentAction != null)
        {
            // Mouse
            if (Input.GetMouseButtonDown(0))
            {
                CurrentAction.InputDown();
            }
            if (Input.GetMouseButton(0))
            {
                CurrentAction.InputHeld();
            }
            if (Input.GetMouseButtonUp(0))
            {
                CurrentAction.InputUp();
            }

            // Touch
        
            if (Input.touchCount >0)
            {
                Touch FirstTouch = Input.GetTouch(0);
               
                if (FirstTouch.phase == TouchPhase.Began)
                {
                    CurrentAction.InputDown();
                }
                if (FirstTouch.phase == TouchPhase.Stationary || FirstTouch.phase == TouchPhase.Moved)
                {
                    CurrentAction.InputHeld();

                }
                if (FirstTouch.phase == TouchPhase.Ended)
                {
                    CurrentAction.InputUp();
                }
            }
        }
    }
    public Targetable GetTargetableAtMousePosition()
    {
        m_Ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(m_Ray, out m_Hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Targetable")))
        {
            Targetable targetable = m_Hit.collider.gameObject.GetComponentInParent<Targetable>();
            return targetable;

        }
        return null;
    }
}
