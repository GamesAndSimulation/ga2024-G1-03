using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningSwords : MonoBehaviour
{
    [SerializeField] private GameObject swordsPrefab;
    [SerializeField] private Transform swordsSpawn;
    [SerializeField] private Transform sword;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(SpinSwords());
        }
    }

    private IEnumerator SpinSwords()
    {

        GetComponent<Animator>().SetTrigger("SpinSwords");
        yield return new WaitForSeconds(3.4f);

        GameObject swords = Instantiate(swordsPrefab, swordsSpawn.position, swordsSpawn.rotation);
        Destroy(swords, 15f);
        StartCoroutine(RotateSwordCenter(swords));
    }

    private IEnumerator RotateSwordCenter(GameObject swords)
    {
        float rotationSpeed = 180f; 
        float rotationAmount = 0f;

        float elapsedTime = 1f;
        while (elapsedTime < 6f)
        {
            rotationAmount = rotationSpeed/(elapsedTime/2) * Time.deltaTime;
            swords.transform.Rotate(0f, rotationAmount, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 1f;
        while (elapsedTime < 4f)
        {
            rotationAmount = elapsedTime*5000/rotationSpeed * Time.deltaTime;
            swords.transform.Rotate(0f, -rotationAmount, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 1f;
        while (elapsedTime < 3f)
        {
            rotationAmount -= 1f * Time.deltaTime;
            swords.transform.Rotate(0f, -rotationAmount, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        ThrowSwords(swords);
    }

    private void ThrowSwords(GameObject swords)
    {
        foreach(Projectile sword in swords.GetComponentsInChildren<Projectile>())
        {
            sword.enabled = true;
        }
    }


}
