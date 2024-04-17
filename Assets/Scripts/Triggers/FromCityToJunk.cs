using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromCityToJunk : MonoBehaviour
{
    [SerializeField] GameObject junkyardObject;

    [SerializeField] GameObject cityObject;

    [SerializeField] bool isTeleportToCity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isTeleportToCity)
            {
                if (GameManager.Instance.isTutorial)
                {
                    GameManager.Instance.isTutorial = false;

                    GameManager.Instance.DeactivateTutorial();
                }

                cityObject.SetActive(true);

                collision.transform.position = new Vector3(-147.94f, 3.61f, 0);

                junkyardObject.SetActive(false);
            }
            else
            {
                junkyardObject.SetActive(true);

                collision.transform.position = new Vector3(-31.26f, -21.06f, 0);

                cityObject.SetActive(false);
            }
        }
    }
}
