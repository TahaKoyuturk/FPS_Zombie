using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables

    #region Public Variables

    public WeaponData weaponData;
    public float speed = 100f;

    #endregion

    #region Private Variables

    private Vector3 lastPosition;
    private Camera cam;
    private float lifetime = 1;
    private float age = 0;
    private float hitDamage = 0;

    RaycastHit[] hit = new RaycastHit[1];

    #endregion

    #endregion

    #region Methods

    #region Unity Callbacks

    private void OnEnable()
    {
        age = 0;
        cam = Camera.main;
    }
    void Update()
    {
        lastPosition = transform.position;
        transform.position += speed * Time.deltaTime * cam.transform.forward;
        hitDamage = weaponData.Damage;

        CheckHit();
        CheckAge();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyAI>().TakeDamage();

            if (!weaponData.isPierceShot) { }
                //Destroy(gameObject);
        }
    }

    #endregion

    #region Custom Methods

    void CheckHit()
    {
        Ray ray = new Ray(lastPosition, transform.forward);
        float dist = Vector3.Distance(lastPosition, transform.position);

        if (Physics.RaycastNonAlloc(ray, hit, dist) > 0)
        {
            //hit something
            //Debug.Log(hit[0].);
            //if i want to spawn something when bullet hit code is this
            //Instantiate(/*whatever i want to spawn*/, hit[0].point, Quaternion.LookRotation(hit[0].normal));
            //gameObject.SetActive(false);
        }
    }

    void CheckAge()
    {
        age += Time.deltaTime;
        if(age>lifetime)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion

    #endregion
}
