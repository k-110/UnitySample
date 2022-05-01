using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandAnimation : MonoBehaviour
{
    [SerializeField] GameObject PanelHand;

    /// <summary>
    /// モデルの構造
    /// Ra
    ///  -Pr1-Mi1-Di1
    ///  -Me2-Pr2-Mi2-Di2
    ///  -Me3-Pr3-Mi3-Di3
    ///  -Me4-Pr4-Mi4-Di4
    ///  -Me5-Pr5-Mi5-Di5
    /// </summary>
    private readonly List<CBoneItem> MyHand = new List<CBoneItem>();


    // Start is called before the first frame update
    void Start()
    {
        MyHand.Add(new CBoneItem(transform.Find("Armature").Find("Ra").Find("Pr1")));
        MyHand.Add(new CBoneItem(transform.Find("Armature").Find("Ra").Find("Me2").Find("Pr2")));
        MyHand.Add(new CBoneItem(transform.Find("Armature").Find("Ra").Find("Me3").Find("Pr3")));
        MyHand.Add(new CBoneItem(transform.Find("Armature").Find("Ra").Find("Me4").Find("Pr4")));
        MyHand.Add(new CBoneItem(transform.Find("Armature").Find("Ra").Find("Me5").Find("Pr5")));
    }

    // Update is called once per frame
    void Update()
    {
        if (PanelHand.activeInHierarchy)
        {
            Scrollbar sba = PanelHand.transform.Find("ScrollbarHandA").GetComponent<Scrollbar>();
            Scrollbar sbb = PanelHand.transform.Find("ScrollbarHandB").GetComponent<Scrollbar>();
            Scrollbar sbc = PanelHand.transform.Find("ScrollbarHandC").GetComponent<Scrollbar>();
            MyHand[1].Ins = new Vector3(sba.value * 90, 0, 0);
            MyHand[1].Child[0].Ins = new Vector3(sbb.value * 90, 0, 0);
            MyHand[1].Child[0].Child[0].Ins = new Vector3(sbc.value * 90, 0, 0);
        }
        foreach (CBoneItem item in MyHand)
        {
            item.Move();
        }
    }

    /// <summary>
    /// ボーンの制御情報
    /// </summary>
    public class CBoneItem
    {
        public Transform TfParent;
        public Transform Tf;
        public Vector3 Rot;
        public Vector3 Ins;
        public List<CBoneItem> Child;

        public CBoneItem(Transform Bone, Transform ParentBone = null)
        {
            TfParent = ParentBone;
            Tf = Bone;
            Rot = Vector3.zero;
            Ins = Vector3.zero;
            Child = new List<CBoneItem>();
            for (int i = 0; i < Tf.childCount; i++)
            {
                Child.Add(new CBoneItem(Tf.GetChild(i), Tf));
            }
        }

        public void Move()
        {
            if (!Ins.Equals(Rot))
            {
                Tf.rotation = Quaternion.Euler(Tf.eulerAngles + Ins - Rot);
                Rot = Ins;
            }
            foreach (CBoneItem item in Child)
            {
                item.Move();
            }
        }
    }
}
