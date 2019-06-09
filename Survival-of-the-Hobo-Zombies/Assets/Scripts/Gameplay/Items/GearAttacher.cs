using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearAttacher : MonoBehaviour {

    public Transform headBone;
    public Transform beardBone;
    public Transform chestBone;
    public Transform pelvisBone;
    public Transform armLeftBone;
    public Transform armRightBone;

    public LivingEntity livingEntity;

    public float offsetX = 0;
    public float offsetY = 0;
    public float offsetZ = 0;

    void Start()
    {
        //transform.position += new Vector3(0, offsetY, 0);
    }

    public void AttachToBones(LivingEntity _livingEntity)
    {
        livingEntity = _livingEntity;

        if (headBone != null && livingEntity.headBone != null)
        {
            headBone.parent = livingEntity.headBone;
            headBone.localPosition = new Vector3(offsetX, offsetY, offsetZ);
            //headBone.localRotation = Quaternion.identity;

        }

        if (beardBone != null && livingEntity.beardBone != null)
        {
            beardBone.parent = livingEntity.beardBone;
            beardBone.localPosition = new Vector3(offsetX, offsetY, offsetZ);
            beardBone.localRotation = Quaternion.identity;
        }

        if (chestBone != null && livingEntity.chestBone != null)
        {
            chestBone.parent = livingEntity.chestBone;
            chestBone.localPosition = new Vector3(offsetX, offsetY, offsetZ);
            chestBone.localRotation = Quaternion.identity;
        }

        if (pelvisBone != null && livingEntity.pelvisBone != null)
        {
            pelvisBone.parent = livingEntity.pelvisBone;
            pelvisBone.localPosition = new Vector3(offsetX, offsetY, offsetZ);
            pelvisBone.localRotation = Quaternion.identity;
        }

        if (armLeftBone != null && livingEntity.armLeftBone != null)
        {
            armLeftBone.parent = livingEntity.armLeftBone;
            armLeftBone.localPosition = new Vector3(offsetX, offsetY, offsetZ);
            armLeftBone.localRotation = Quaternion.identity;
        }

        if (armRightBone != null && livingEntity.armRightBone != null)
        {
            armRightBone.parent = livingEntity.armRightBone;
            armRightBone.localPosition = new Vector3(offsetX, offsetY, offsetZ);
            armRightBone.localRotation = Quaternion.identity;
        }

    }
}
