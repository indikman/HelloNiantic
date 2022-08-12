using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.Networking.HLAPI.Object.Unity;

public class PaintBall : MonoBehaviour
{
    public GameObject explodeParticle;

    private PaintBallGenerator paintBallGenerator;

    public void SetGenerator(PaintBallGenerator gen)
    {
        paintBallGenerator = gen;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            if(paintBallGenerator!=null)
            {
                // Instantiate explode particle and Play sound (instaltiate a sound object)
                Instantiate(explodeParticle, transform.position, Quaternion.identity);

                //Add score
                GameNetworkManager.Instance.AddScore();

                // Delete the paintball from all peers
                paintBallGenerator.DestroyObjectFromAllPeers(GetComponent<NetworkedUnityObject>(), .5f);

            }
        }
    }
}
