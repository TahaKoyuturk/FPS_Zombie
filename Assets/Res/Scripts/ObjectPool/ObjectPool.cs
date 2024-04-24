using UnityEngine;

public class ObjectPool 
{
    GameObject[] gameObjects;
    int m_numOfObjects;
    public ObjectPool(GameObject go, int numOfObjects)
    {
        m_numOfObjects = numOfObjects;
        gameObjects = new GameObject[numOfObjects];

        for (int i = 0; i < numOfObjects; i++)
        {
            gameObjects[i]=GameObject.Instantiate(go);
            gameObjects[i].SetActive(false);
        }
    }

    public void ActivateNext(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < m_numOfObjects; i++)
        {
            if (!gameObjects[i].activeSelf)
            {
                gameObjects[i].SetActive(true);
                gameObjects[i].transform.position = position;
                gameObjects[i].transform.rotation = rotation;
                break;
            }
        }
    }
}
