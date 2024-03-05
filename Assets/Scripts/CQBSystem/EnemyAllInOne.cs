using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAllInOne : MonoBehaviour
{
    [SerializeField] private int hp = 100;
    [SerializeField] private GameObject highlighterMesh;
    [SerializeField] private GameObject self;


    void Start()
    {
        highlighterMesh.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (hp<0)
        {
            self.SetActive(false);
        }
    }

    public void Highlight()
    {
        highlighterMesh.SetActive(true);
    }

    public void RemoveHighlight()
    {
        highlighterMesh.SetActive(false);
    }

    public void Damage(int damage)
    {
        hp -= damage;
    }
}
