using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// draw a line between the player's gun barrel and the presumed target, 
/// also highlight the enemy if apply
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class AimLine : MonoBehaviour
{
    public Transform player; // The starting point of the raycast
    public float maxRange = 20f; // Maximum range of the weapon
    public LayerMask hitLayers;
    private LineRenderer lineRenderer;
    private GameObject lastHighlighted = null;
    private Vector3 endPosition;
    private bool isAiming = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        // Optionally, configure LineRenderer appearance here
    }

    void Update()
    {
        isAiming = GetComponent<PlayerAllInOne>().isAiming;
        if (isAiming)
        {
            Vector3 direction = player.forward;
            RaycastHit hit;

            if (Physics.Raycast(player.position, direction, out hit, maxRange, hitLayers))
            {
                endPosition = hit.point;
                GameObject hitTarget = hit.collider.gameObject;

                if (hitTarget.layer == 7) // enemy layer
                {
                    if (lastHighlighted != hitTarget)
                    {
                        // Hide Last highlighted enemy
                        if (lastHighlighted != null)
                            lastHighlighted.GetComponent<EnemyAllInOne>().RemoveHighlight();

                        // high the new enemy
                        hit.collider.GetComponent<EnemyAllInOne>().Highlight();
                        lastHighlighted = hit.collider.gameObject;
                    }
                }
                else // hit non-enemy obstacle
                {
                    RemoveHighLight();
                }
            }
            else
            {
                RemoveHighLight();
                endPosition = player.position + player.forward.normalized * maxRange;
            }

            // draw aim line
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, player.position);
            lineRenderer.SetPosition(1, endPosition);
        }
        else
        {
            lineRenderer.enabled = false;
            RemoveHighLight();
        }
    }

    private void RemoveHighLight()
    {
        if (lastHighlighted != null)
        {
            lastHighlighted.GetComponent<EnemyAllInOne>().RemoveHighlight();
            lastHighlighted = null;
        }
    }
}
