using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkSyncronization : MonoBehaviourPun, IPunObservable
{
    double lastSerializeTime;
    double ping;

    Vector3 syncPos;
    Quaternion syncRot;

    Vector3 lastSyncPos;
    Quaternion lastSyncRot;

    void Update()
    {
        HandlePlayerNetworkTransformation();
    }

    void HandlePlayerNetworkTransformation()
    {
        if (photonView.IsMineOrOffline())
            return;

        Vector3 moveDelta = (syncPos - lastSyncPos) / (float)PhotonNetwork.SerializationRate * (1f / Time.deltaTime);
        moveDelta *= (float) (PhotonNetwork.Time - lastSerializeTime + ping - Time.deltaTime);

        Vector3 rotateDelta = (syncRot.eulerAngles - lastSyncRot.eulerAngles) / (float)PhotonNetwork.SerializationRate * (1f / Time.deltaTime);
        rotateDelta *= (float)(PhotonNetwork.Time - lastSerializeTime + ping - Time.deltaTime);

        moveDelta = Vector3.Lerp(moveDelta, Quaternion.Euler(rotateDelta) * moveDelta, 0.25f);

        transform.position = Vector3.Lerp(transform.position, syncPos + moveDelta, Time.deltaTime / (float) ping * 1.3f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(syncRot.eulerAngles + rotateDelta), Time.deltaTime / (float) ping);

        //Old
        //transform.position = Vector3.Lerp(transform.position, syncPos, Time.deltaTime * 100);
        transform.rotation = Quaternion.Lerp(transform.rotation, syncRot, Time.deltaTime * 20);
    }

    //Sent 10 times per second
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        lastSerializeTime = info.SentServerTime;
        ping = PhotonNetwork.Time - lastSerializeTime;

        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            lastSyncPos = syncPos;
            lastSyncRot = syncRot;

            syncPos = (Vector3) stream.ReceiveNext();
            syncRot = (Quaternion) stream.ReceiveNext();
        }
    }
}