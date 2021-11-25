using Photon.Pun;

public static class PhotonExtensionMethods
{
    public static bool IsMineOrOffline(this PhotonView view) => 
        (PhotonNetwork.CurrentRoom != null && PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && view.IsMine) || PhotonNetwork.CurrentRoom == null;
}