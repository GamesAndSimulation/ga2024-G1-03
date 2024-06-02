using UnityEngine;

public class CrystalScript: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0, 1f, 0);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Magic"))
        {
            Destroy(collision.gameObject);
        }
    }

}
