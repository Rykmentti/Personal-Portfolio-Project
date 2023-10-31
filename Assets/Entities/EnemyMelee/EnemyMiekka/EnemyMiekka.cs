using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMiekka: MonoBehaviour
{
    public Vector3 startRotation;
    public Vector2 localPosition;
    public float threshold;

    public float rotation;
    public float speed = 10f;
    public float swingSpeed = 360f;

    // Start is called before the first frame update
    void OnGUI()
    {
        GUI.Label(new Rect(10, 60, 150, 30), "Rotation = " + rotation);
    }
    void Start()
    {
        threshold = GetComponentInParent<EnemyControllerMelee>().threshold;
        localPosition = GetComponentInParent<EnemyControllerMelee>().localPosition;
        startRotation = GetComponentInParent<EnemyControllerMelee>().startRotation;
        transform.localPosition = localPosition;
        transform.eulerAngles = startRotation;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.right);
        transform.Rotate(swingSpeed * Time.deltaTime * Vector3.back);

        rotation = transform.rotation.z;

        if (rotation < threshold)
        {
            Destroy(gameObject);
            /*
            Debug.Log("Threshold works");
            */
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (PlayerController.rollActive == false))
        {
            PlayerController.playerHealth -= 25;
        }
    }

    private void OnDestroy()
    {
        if(transform.parent != null)
        {
            Animator animator;
            animator = GetComponentInParent<Animator>();
            animator.SetBool("IsAttackingUp", false);
            animator.SetBool("IsAttackingRight", false);
            animator.SetBool("IsAttackingDown", false);
            animator.SetBool("IsAttackingLeft", false);
        }

    }
}