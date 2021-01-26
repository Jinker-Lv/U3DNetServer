using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    //人物模型预设
    public GameObject humanPrefab;
    //人物列表
    private Control myHuamn;
    public Dictionary<string, BaseHuman> otherHumans=new Dictionary<string, BaseHuman>();
    void Start()
    {
        //网络模块
        NetManager.Addlistener("Enter", OnEnter);
        NetManager.Addlistener("Move", OnMove);
        NetManager.Addlistener("Leave", OnLeave);
        NetManager.Connect("127.0.0.1", 8888);
        //添加一个角色
        GameObject obj = (GameObject)Instantiate(humanPrefab);
        float x = Random.Range(-5, 5);
        float z = Random.Range(-5, 5);
        obj.transform.position = new Vector3(x, 0, z);
        myHuamn = obj.AddComponent<Player>();
        myHuamn.desc = NetManager.GetDesc();
        //发送协议
        Vector3 pos = myHuamn.transform.position;
        Vector3 eul = myHuamn.transform.eulerAngles;
        string sendStr = "Enter|";
        sendStr += NetManager.GetDesc() + ",";
        sendStr += pos.x + ",";
        sendStr += pos.y + ",";
        sendStr += pos.z + ",";
        sendStr += eul.y;
        NetManager.send(sendStr);
    }
    private void Update()
    {
        NetManager.Update();
    }
    void OnEnter(string msg)
    {
        Debug.Log($"OnEnter {msg}");
        //解析参数
        string[] split = msg.Split(',');
        string desc = split[0];
        float x = float.Parse(split[1]);
        float y = float.Parse(split[2]);
        float z = float.Parse(split[3]);
        float eulY = float.Parse(split[4]);
        //是自己
        if (desc == NetManager.GetDesc()) return;
        //添加一个角色
        GameObject obj = Instantiate(humanPrefab);
        obj.transform.position = new Vector3(x, y, z);
        obj.transform.eulerAngles = new Vector3(0, eulY, 0);
        BaseHuman h = obj.AddComponent<SyncHuman>();
        h.desc = desc;
        otherHumans.Add(desc, h);
    }
    void OnMove(string msg) => Debug.Log($"OnMove {msg}");
    void OnLeave(string msg) => Debug.Log($"OnLeave {msg}");
}
