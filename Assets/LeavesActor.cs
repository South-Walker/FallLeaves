using NVIDIA.Flex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("NVIDIA/Leaves Actor")]
public class LeavesActor : FlexActor
{

    #region Properties

    public new LeavesAsset asset
    {
        get { return m_asset; }
        set { m_asset = value; }
    }

    #endregion

    #region Messages

    void Reset()
    {
        selfCollide = false;
    }

    void OnDrawGizmos()
    {
    }

    void OnDrawGizmosSelected()
    {
    }

    #endregion

    #region Protected

    protected override FlexAsset subclassAsset { get { return m_asset; } }

    protected override void CreateInstance()
    {
        base.CreateInstance();

        if (handle)
        {
            FlexExt.Instance instance = handle.instance;
            if (instance.numParticles > 0)
            {
                if (m_asset.fixedParticles.Length == 0)
                {
                    int[] refPoints = new int[3] { 0, 1, 2 }; // @@@
                    Vector4[] particles = m_asset.particles;
                    //FlexUtils.ClothRefPoints(ref particles[0], particles.Length, ref refPoints[0]); // @@@ !!! Too slow
                    m_referencePoints = new int[] { indices[refPoints[0]], indices[refPoints[1]], indices[refPoints[2]] };
                    Vector3 p0 = particles[refPoints[0]], p1 = particles[refPoints[1]], p2 = particles[refPoints[2]];
                    m_localPosition = (p0 + p1 + p2) / 3;
                    Vector3 clothZ = Vector3.Cross(p1 - p0, p2 - p0).normalized;
                    Vector3 clothY = Vector3.Cross(clothZ, p1 - p0).normalized;
                    m_localRotation = Quaternion.LookRotation(clothZ, clothY);
                }
            }
        }
    }

    protected override void DestroyInstance()
    {
        base.DestroyInstance();
    }

    protected override void ValidateFields()
    {
        base.ValidateFields();
    }

    Vector3 topleft, topright;
    int itopleft, itopright;
    bool hasSetFixed = false;
    protected override void OnFlexUpdate(FlexContainer.ParticleData _particleData)
    {
        if (Application.isPlaying && m_referencePoints != null && m_referencePoints.Length >= 3)
        {
            Vector3 p0 = _particleData.GetParticle(m_referencePoints[0]);
            Vector3 p1 = _particleData.GetParticle(m_referencePoints[1]);
            Vector3 p2 = _particleData.GetParticle(m_referencePoints[2]);
            Vector3 clothZ = Vector3.Cross(p1 - p0, p2 - p0).normalized, clothY = Vector3.Cross(clothZ, p1 - p0).normalized;
            Quaternion globalRotation = Quaternion.LookRotation(clothZ, clothY) * Quaternion.Inverse(m_localRotation);
            Vector3 globalPosition = (p0 + p1 + p2) / 3 - globalRotation * m_localPosition;
            transform.position = globalPosition;
            transform.rotation = globalRotation;
            transform.hasChanged = false;
        }

        base.OnFlexUpdate(_particleData);
        if (hasSetFixed)
        {
            _particleData.SetParticle(indices[itopleft], new Vector4(topleft.x, topleft.y, topleft.z, 0));
            _particleData.SetParticle(indices[itopright], new Vector4(topright.x, topright.y, topright.z, 0));
        }
        else
        {
            topleft = _particleData.GetParticle(indices[0]);
            topright = _particleData.GetParticle(indices[0]);
            itopleft = 0;
            itopright = 0;
            for (int i = 1; i < indices.Length; i++)
            {
                Vector3 now = _particleData.GetParticle(indices[i]);
                Vector3 now2topleft = topleft - now;
                Vector3 now2topright = topright - now;
                if (now2topleft.y <= 0 && now2topleft.x >= 0)
                {
                    topleft = now;
                    itopleft = i;
                }
                if (now2topright.y <= 0 && now2topright.x <= 0)
                {
                    topright = now;
                    itopright = i;
                }
            }
            hasSetFixed = true;
        }
    }

    #endregion

    #region Private

    [NonSerialized]
    int[] m_referencePoints = null;
    [NonSerialized]
    Vector3 m_localPosition = Vector3.zero;
    [NonSerialized]
    Quaternion m_localRotation = Quaternion.identity;

    [SerializeField]
    LeavesAsset m_asset;

    #endregion
}
