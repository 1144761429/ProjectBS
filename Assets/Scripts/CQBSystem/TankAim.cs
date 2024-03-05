using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankAim : MonoBehaviour
{
    public Transform turretTransform; // 炮塔的Transform
    public Transform target; // 目标的Transform
    public float rotationSpeed = 30f; // 炮塔旋转速度
    public float delayThreshold = 180f; // 触发延迟的阈值（度数）
    public float delayDuration = 0.5f; // 延迟持续时间（秒）
    private float delayTimer = 0f; // 延迟计时器

    void Update()
    {
        // 计算炮塔当前位置和目标位置之间的向量
        Vector3 directionToTarget = target.position - turretTransform.position;

        // 将目标方向向量旋转90度，使其与炮塔的正面（炮塔的右侧）对齐
        Quaternion rightAlignedTargetRotation = Quaternion.LookRotation(directionToTarget) * Quaternion.Euler(0, 90, 0);

        // 计算当前旋转和目标旋转之间的角度差
        float angleDifference = Quaternion.Angle(turretTransform.rotation, rightAlignedTargetRotation);

        // 如果角度差超过阈值，则开始延迟计时器
        if (angleDifference > delayThreshold)
        {
            delayTimer += Time.deltaTime;
        }
        else // 否则，如果不需要延迟或延迟已结束
        {
            delayTimer = 0f; // 重置延迟计时器
        }

        // 如果延迟计时器小于延迟持续时间，则旋转炮塔
        if (delayTimer < delayDuration)
        {
            // 插值当前旋转到目标旋转
            turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, rightAlignedTargetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
