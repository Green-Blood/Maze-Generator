using System.Collections;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    [SerializeField] private GameObject camera1;
    [SerializeField] private GameObject player;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        camera1.SetActive(false);
        player.SetActive(true);
    }
}
