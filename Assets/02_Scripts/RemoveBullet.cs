using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public EffectDataSO effectDataSO;

    void OnCollisionEnter(Collision coll)
    {
        // 충동한 물체를 파악
        if (coll.collider.CompareTag("BULLET"))
        {
            // 충돌 정보
            ContactPoint cp = coll.GetContact(0);
            // 충돌 좌표
            Vector3 _point = cp.point;
            // 법선 벡터
            Vector3 _normal = -cp.normal;

            // 법선 벡터가 가리키는 방향의 각도(쿼터니언)를 계산
            Quaternion rot = Quaternion.LookRotation(_normal);

            // 스파크 이펙트 생성
            GameObject obj = Instantiate(effectDataSO.sparkEffect, _point, rot);
            Destroy(obj, 0.5f);

            Destroy(coll.gameObject);
        }


        // if (coll.collider.tag == "BULLET")  // 사용 금지
        // {
        //     Destroy(coll.gameObject);
        // }
    }

    // 충돌 콜백 함수
    /*
        1. 양쪽 다 Collider 갖고 있다.
        2. 이동하는 게임오브젝트에 Rigidbody 있어야 함.

        # IsTrigger 언체크 경우
            OnCollisionEnter
            OnCollisionStay
            OnCollisionExit

        # IsTrigger 체크 경우
            OnTriggerEnter
            OnTriggerStay
            OnTriggerExit
    */
}
