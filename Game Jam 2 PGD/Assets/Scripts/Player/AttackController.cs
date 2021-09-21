using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public bool attack = false;
    public bool animationEnded = true;

    [SerializeField] private GameObject _arrow = null;
    [SerializeField] private GameObject _shootPoint = null;

    private PickUpManager _pm;

    private void Awake()
    {
        _pm = GetComponent<PickUpManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _pm.arrowCount > 0)
            Attack();   
    }

    private void FixedUpdate()
    {
        attack = false;
    }

    private void Attack()
    {
        attack = true;

        Quaternion rotation = Quaternion.identity;

        if (this.transform.localScale.x < 0)
            rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z - 180);

        GameObject newarrow = Instantiate(_arrow, _shootPoint.transform.position, rotation);

        newarrow.GetComponent<ArrowMovement>()._isSticky = false;

        _pm.arrowCount--;
    }

    public void EndAttackAnim() { animationEnded = true; }
    public void StartAttackAnim() { animationEnded = false; }
}
