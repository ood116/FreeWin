using UnityEngine;

public class RoomList : MonoBehaviour
{
    /*
    RoomManager RM;
    private Dictionary<string, GameObject> roomDictionary = new Dictionary<string, GameObject>();
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList) => StartCoroutine(RoomListUpdateCoroutine(roomList));

    IEnumerator RoomListUpdateCoroutine(List<RoomInfo> roomList)
    {
        GameObject roomObj = null;

        foreach (var room in roomList)
        {
            // 룸 삭제
            if (room.RemovedFromList == true) {
                roomDictionary.TryGetValue(room.Name, out roomObj);
                Destroy(roomObj);
                roomDictionary.Remove(room.Name);
            }
            // 룸 추가
            else {
                if (roomDictionary.ContainsKey(room.Name) == false) {
                    GameObject _room = Instantiate(RM.roomObj);
                    _room.GetComponent<RoomData>().RoomInfo = room;
                    roomDictionary.Add(room.Name, _room);
                }
                // 룸 갱신
                else {
                    roomDictionary.TryGetValue(room.Name, out roomObj);
                    roomObj.GetComponent<RoomData>().RoomInfo = room;
                }
            }
            yield return null;
        }
    }
    */
}
