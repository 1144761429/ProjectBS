using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankAim : MonoBehaviour
{
    public Transform turretTransform; // ������Transform
    public Transform target; // Ŀ���Transform
    public float rotationSpeed = 30f; // ������ת�ٶ�
    public float delayThreshold = 180f; // �����ӳٵ���ֵ��������
    public float delayDuration = 0.5f; // �ӳٳ���ʱ�䣨�룩
    private float delayTimer = 0f; // �ӳټ�ʱ��

    void Update()
    {
        // ����������ǰλ�ú�Ŀ��λ��֮�������
        Vector3 directionToTarget = target.position - turretTransform.position;

        // ��Ŀ�귽��������ת90�ȣ�ʹ�������������棨�������Ҳࣩ����
        Quaternion rightAlignedTargetRotation = Quaternion.LookRotation(directionToTarget) * Quaternion.Euler(0, 90, 0);

        // ���㵱ǰ��ת��Ŀ����ת֮��ĽǶȲ�
        float angleDifference = Quaternion.Angle(turretTransform.rotation, rightAlignedTargetRotation);

        // ����ǶȲ����ֵ����ʼ�ӳټ�ʱ��
        if (angleDifference > delayThreshold)
        {
            delayTimer += Time.deltaTime;
        }
        else // �����������Ҫ�ӳٻ��ӳ��ѽ���
        {
            delayTimer = 0f; // �����ӳټ�ʱ��
        }

        // ����ӳټ�ʱ��С���ӳٳ���ʱ�䣬����ת����
        if (delayTimer < delayDuration)
        {
            // ��ֵ��ǰ��ת��Ŀ����ת
            turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, rightAlignedTargetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
