using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Manipulatable : MonoBehaviour
{
    public Collider Collider;
    public Transform LaserColliderRoot;
    public Transform ModelRoot;

    [SerializeField] List<LaserReciever_Dice> diceSides;

    [Header("Rotation")]
    public bool EnableRotation = true;
    public float RotationIncrement = 90f;
    public float RotationLength = 0.3f;
    public Ease RotationEase = Ease.OutBounce;

    [Header("Move")]
    public bool EnableMove = true;
    public float MoveLength = 0.3f;
    public Ease MoveEase = Ease.OutBounce;

    [Header("Grab")]
    public float GroundY = 0.7f;
    public float SkyY = 2.2f;
    public float GrabLength = 0.3f;
    public Ease GrabEase = Ease.OutBack;
    public Ease ReleaseEase = Ease.OutBounce;

    float currentRotation = 0f;

    void Start()
    {
        currentRotation = ModelRoot.localEulerAngles.y;

        transform.position = new Vector3(transform.position.x, GroundY, transform.position.z);
    }

    public void Rotate()
    {
        if (!EnableRotation)
            return;

        currentRotation += RotationIncrement;
        currentRotation = Mathf.Repeat(currentRotation, 360f);

        diceSides.ForEach(x => x.IsReflective = false);
        ModelRoot.DOLocalRotate(new Vector3(0, currentRotation, 0), RotationLength)
                 .SetEase(RotationEase)
                 .onComplete += () => diceSides.ForEach(x => x.IsReflective = true);

        LaserColliderRoot.localRotation = Quaternion.AngleAxis(currentRotation, Vector3.up);
    }

    public void SnapTo(Vector3 destination)
    {
        if (!EnableMove)
            return;

        destination.y = transform.position.y;

        transform.DOComplete();
        transform.DOMove(destination, MoveLength).SetEase(MoveEase);
    }

    public void MoveTo(Vector3 destination)
    {
        if (!EnableMove)
            return;

        destination.y = transform.position.y;
        transform.position = destination;
    }

    public void Grab()
    {
        Collider.enabled = false;
        transform.DOMoveY(SkyY, GrabLength).SetEase(GrabEase);
    }

    public void Release()
    {
        Collider.enabled = true;
        transform.DOMoveY(GroundY, GrabLength).SetEase(ReleaseEase);
    }
}
