using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectManager : MonoBehaviour
{
    [SerializeField] GameObject fabBall;//프리팹
    [SerializeField] GameObject objCursor;//마우스커서
    [SerializeField] Transform shootPoint;//발사위치
    [SerializeField] LayerMask layer;//타겟팅 레이어

    [SerializeField] Transform trsCannon;
    [SerializeField] float distanceCannon;

    [SerializeField] float time;
    [SerializeField,Range(0f,1f)] float ratio;
    
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        checkCannon();
    }

    private void checkCannon()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 15f,layer))
        {
            if(Vector3.Distance(hit.point,trsCannon.position) < distanceCannon)
            {
                objCursor.SetActive(false);
                return;
            }

            objCursor.SetActive(true);
            objCursor.transform.position = hit.point + Vector3.up * 0.001f;

            Vector3 vo = calculateVelocity(hit.point);
            trsCannon.transform.rotation = Quaternion.LookRotation(vo);

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject objBall = Instantiate(fabBall, shootPoint.position, Quaternion.identity);
                Rigidbody rigid = objBall.GetComponent<Rigidbody>();
                rigid.velocity = vo;
            }
        }
        else
        {
            objCursor.SetActive(false);
        }
    }

    private Vector3 calculateVelocity(Vector3 _target)
    {
        Vector3 distance = _target - trsCannon.position;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + ratio * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        return result;
    }
}
