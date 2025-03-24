using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    /*
     * when player first gets within a certain distance of the knight
     * start first dialogue
     * after first dialogue ends, fight
     * do second dialogue
     * after second dialogue ends
     * the knight can just stand there
     */
    [SerializeField] Canvas one;
    [SerializeField] Canvas two;
    private List<GameObject> dialogueOne;
    private List<GameObject> dialogueTwo;
    public GameObject player;
    public GameObject[] walls;
    public float dialogueStartRange = 5f;
    private int index;
    public bool firstStarted = false;
    public bool firstEnded = false;
    public bool secondStarted = false;
    public bool secondEnded = false;
    public bool fightEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Lantern") == 1) Destroy(gameObject);
        index = 0;
        RectTransform[] temp = one.GetComponentsInChildren<RectTransform>(true);
        dialogueOne = new List<GameObject>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gameObject.name.Contains("dialogue")) {
                dialogueOne.Add(temp[i].gameObject);
            }
        }
        temp = two.GetComponentsInChildren<RectTransform>(true);
        dialogueTwo = new List<GameObject>();
        for(int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gameObject.name.Contains("dialogue"))
            {
                dialogueTwo.Add(temp[i].gameObject);
            }
                
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!fightEnded)
        {
            if(!firstStarted && (player.transform.position - transform.position).magnitude <= dialogueStartRange)
            {
                firstStarted = true;
                player.GetComponent<PlayerMovement>().SetInDialogue(true);
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            if (firstStarted && !firstEnded)
            {
                Debug.Log("showing dialogue");
                dialogueOne[index].SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    dialogueOne[index].SetActive(false);
                    index++;
                    if (index == dialogueOne.Count)
                    {
                        firstEnded = true;
                        player.GetComponent<PlayerMovement>().SetInDialogue(false);
                        gameObject.GetComponent<KnightBoss>().enabled = true;
                        for(int i = 0; i < walls.Length; i++)
                        {
                            walls[i].SetActive(true);
                        }
                        index = 0;
                    }
                    else
                    {
                        dialogueOne[index].SetActive(true);
                    }
                }
            }
        }
        else
        {
            if (!secondStarted)
            {
                secondStarted = true;
                player.GetComponent<PlayerMovement>().SetInDialogue(true);
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                gameObject.GetComponent<KnightBoss>().enabled = false;
                for (int i = 0; i < walls.Length; i++)
                {
                    walls[i].SetActive(false);
                }
            }
            if (secondStarted && !secondEnded)
            {
                dialogueTwo[index].SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    dialogueTwo[index].SetActive(false);
                    index++;
                    if (index == dialogueTwo.Count)
                    {
                        secondEnded = true;
                        player.GetComponent<PlayerMovement>().SetInDialogue(false);
                        gameObject.GetComponent<KnightBoss>().enabled = false;
                        index = 0;
                        for (int i = 0; i < walls.Length; i++)
                        {
                            walls[i].SetActive(false);
                        }
                    }
                    else
                    {
                        dialogueTwo[index].SetActive(true);
                    }
                }
            }
        }
    }
    
}
