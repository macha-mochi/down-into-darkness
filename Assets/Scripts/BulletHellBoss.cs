using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletHellBoss : MonoBehaviour
{
    [SerializeField] private int bulletAmount;
    [SerializeField] private GameObject homingMissile;
    [SerializeField] private GameObject player;
    [SerializeField] private float length;
    [SerializeField] private float startAngle = 90f, endAngle = 270f;
    private Vector2 bulletMoveDirection;

    private void Start()
    {
        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        //pick random attack
        int rand = Random.Range(0, 3);
        if(rand == 0)
        {
            float angleStep = (endAngle - startAngle) / bulletAmount;
            float angle = startAngle;

            for(int i = 0; i < bulletAmount + 1; i++)
            {
                float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 45f);
                float bulDirY = transform.position.x + Mathf.Cos((angle * Mathf.PI) / 45f);

                Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0);
                Vector2 bulDir = (bulMoveVector - transform.position).normalized;

                GameObject bul = BulletPool.bulletPoolInstance.GetBullet();
                bul.transform.position = transform.position;
                bul.transform.rotation = transform.rotation;
                bul.SetActive(true);
                bul.GetComponent<Bullet>().SetMoveDirection(bulDir);

                angle += angleStep;
            }
            yield return new WaitForSecondsRealtime(3);
        }
        else if (rand == 1)
        {
            float dist = length / bulletAmount;
            for(int i = 0; i < bulletAmount + 1; i++)
            {
                float xPos = transform.position.x + i * dist - length/2;
                float yPos = transform.position.y;
                Vector3 bulDir = Vector2.down;
                GameObject bul = BulletPool.bulletPoolInstance.GetBullet();
                bul.transform.position = new Vector3(xPos, yPos, 0);
                bul.transform.rotation = transform.rotation;
                bul.SetActive(true);
                bul.GetComponent<Bullet>().SetMoveDirection(bulDir);
            }
            yield return new WaitForSeconds(1);
            for (int i = 0; i < bulletAmount + 1; i++)
            {
                float xPos = transform.position.x + i * dist - length / 2 + dist/2;
                float yPos = transform.position.y;
                Vector3 bulDir = Vector2.down;
                GameObject bul = BulletPool.bulletPoolInstance.GetBullet();
                bul.transform.position = new Vector3(xPos, yPos, 0);
                bul.transform.rotation = transform.rotation;
                bul.SetActive(true);
                bul.GetComponent<Bullet>().SetMoveDirection(bulDir);
            }
            yield return new WaitForSecondsRealtime(3);
        }
        else if (rand == 2)
        {
            GameObject curr = Instantiate(homingMissile);
            curr.transform.position = new Vector3(transform.position.x-length / 4, transform.position.y);
            curr.transform.localEulerAngles = new Vector3(0, 0 - 90);
            curr.GetComponent<HomingMissile>().player = player;
            yield return new WaitForSeconds(2);
            curr = Instantiate(homingMissile);
            curr.transform.position = new Vector3(transform.position.x, transform.position.y);
            curr.GetComponent<HomingMissile>().player = player;
            curr.transform.localEulerAngles = new Vector3(0, 0 - 90);
            yield return new WaitForSeconds(2);
            curr = Instantiate(homingMissile);
            curr.transform.position = new Vector3(transform.position.x + length / 4, transform.position.y);
            curr.GetComponent<HomingMissile>().player = player;
            curr.transform.localEulerAngles = new Vector3(0, 0 - 90);
            yield return new WaitForSeconds(2);
        }
        yield return null;
        StartCoroutine(Fire());

    }

    public void Death()
    {
        SceneManager.LoadScene(2);
    }
}
