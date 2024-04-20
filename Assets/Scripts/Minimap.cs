using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public GameObject Player;
    public MinimapData[] maps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var v in maps)
        {
            if (!v.Go) continue;
            var ePos = v.Go.transform.position;
            var pPos = Player.transform.position;
            var qrot = Quaternion.LookRotation(ePos - pPos);
            var inRange = IsRange(pPos, ePos, 4.5f);

            v.IconTri.gameObject.SetActive(!inRange);
            v.IconCube.SetActive(inRange);
            v.IconTri.transform.rotation = Quaternion.Euler(new Vector3(0,0,-qrot.eulerAngles.y));
        } 
    }

    bool IsRange(Vector3 player, Vector3 enemy, float range)
    {
        float dist_x = enemy.x - player.x;
        float dist_z = enemy.z - player.z;
        float distx2 = dist_x * dist_x + dist_z * dist_z;

        // ‹——£‚ª”ÍˆÍ“à‚¾‚Á‚½‚çTrue
        return (distx2 < range * range);
    }
}
