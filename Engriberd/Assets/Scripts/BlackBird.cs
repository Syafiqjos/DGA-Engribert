using UnityEngine;
using System.Collections;

public class BlackBird : Bird
{

    [SerializeField]
    public float _explodeRatio = 1.01f;
    public float _explodeTime = 2.0f;
    public bool _exploded = false;

    public Transform _explodeField;
    private Rigidbody2D _explodeFieldRb;
    private CircleCollider2D _explodeFieldCol;

    public void Explode()
    {
        if (!_exploded)
        {
            _exploded = true;
            GetComponent<SpriteRenderer>().color = new Color(1,0,0);

            _explodeField.parent = null;
            _explodeFieldCol = _explodeField.GetComponent<CircleCollider2D>();
            _explodeFieldCol.enabled = true;

            _explodeFieldRb = _explodeField.gameObject.AddComponent<Rigidbody2D>();
            _explodeFieldRb.bodyType = RigidbodyType2D.Static;

            Physics2D.IgnoreCollision(_explodeFieldCol, GetComponent<CircleCollider2D>());

            StartCoroutine(DestroyField(_explodeField.gameObject));
        }
    }

    private IEnumerator DestroyField(GameObject field)
    {
        yield return new WaitForSeconds(_explodeTime);
        Destroy(field);
    }

    private void Exploding()
    {
        if (_explodeField)
        {
            //_explodeField.transform.localScale *= _explodeRatio;
            _explodeFieldCol.radius *= _explodeRatio;
        }
    }

    public override void OnHit()
    {
        Explode();
    }

    public override void OnFixedUpdate()
    {
        if (_exploded)
        {
            Exploding();
        }
    }
}
