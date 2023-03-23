//
//SpingManager.cs for unity-chan!
//
//Original Script is here:
//ricopin / SpingManager.cs
//Rocket Jump : http://rocketjump.skr.jp/unity3d/109/
//https://twitter.com/ricopin416
//
//Revised by N.Kobayashi 2014/06/24
//           Y.Ebata
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityChan
{
	public class SpringManager : MonoBehaviour
	{
		//Kobayashi
		// DynamicRatio is paramater for activated level of dynamic animation 
		public float dynamicRatio = 1.0f;

		//Ebata
		public float			stiffnessForce;
		public AnimationCurve	stiffnessCurve;
		public float			dragForce;
		public AnimationCurve	dragCurve;
		public List<SpringBone> springBones;
		public SpringCollider[] springColliders;
		public float coliderradius;
		public float gamespeed;

		void Start ()
		{
			UpdateParameters ();
		}
	
		void Update ()
		{
#if UNITY_EDITOR
		//Kobayashi
		if(dynamicRatio >= 1.0f)
			dynamicRatio = 1.0f;
		else if(dynamicRatio <= 0.0f)
			dynamicRatio = 0.0f;
		//Ebata
		UpdateParameters();
#endif
		}
	
		private void LateUpdate ()
		{
			Time.timeScale = gamespeed;
			//Kobayashi
			if (dynamicRatio != 0.0f) {
				for (int i = 0; i < springBones.Count; i++) {
					if (dynamicRatio > springBones [i].threshold) {
						springBones [i].UpdateSpring ();
					}
				}
			}
		}
		[ContextMenu("cloth-bone-add")]
		public void GetEveryclothBonest()
		{
			Transform[] t= GetComponentsInChildren<Transform>();
			foreach(Transform o in t)
			{
				if (o.name.StartsWith("裙子"))
				{
					if (o.childCount > 0)
					{
						print("addbone");
						o.gameObject.AddComponent<SpringBone>();
						o.gameObject.GetComponent<SpringBone>().child=o.GetChild(0);
						o.gameObject.GetComponent<SpringBone>().boneAxis=new Vector3 (0.0f,1.0f,0.0f);
						o.gameObject.GetComponent<SpringBone>().radius = coliderradius;
						springBones.Add(o.gameObject.GetComponent<SpringBone>());

                    }
				}
			}
			
		}
        [ContextMenu("hair-bone-add")]
        public void GetEveryhairBonest()
        {
            Transform[] t = GetComponentsInChildren<Transform>();
            foreach (Transform o in t)
            {
                if ( o.name.StartsWith("发"))
                {
                    if (o.childCount > 0)
                    {
                        print("addbone");
                        o.gameObject.AddComponent<SpringBone>();
                        o.gameObject.GetComponent<SpringBone>().child = o.GetChild(0);
                        o.gameObject.GetComponent<SpringBone>().boneAxis = new Vector3(0.0f, 1.0f, 0.0f);
                        o.gameObject.GetComponent<SpringBone>().radius = coliderradius;
                        springBones.Add(o.gameObject.GetComponent<SpringBone>());

                    }
                }
            }

        }
        [ContextMenu("shiping-bone-add")]
        public void GetEveryshipingBonest()
        {
            Transform[] t = GetComponentsInChildren<Transform>();
            foreach (Transform o in t)
            {
                if (o.name.StartsWith("+"))
                {
                    if (o.childCount > 0)
                    {
                        print("addbone");
                        o.gameObject.AddComponent<SpringBone>();
                        o.gameObject.GetComponent<SpringBone>().child = o.GetChild(0);
                        o.gameObject.GetComponent<SpringBone>().boneAxis = new Vector3(0.0f, 1.0f, 0.0f);
                        o.gameObject.GetComponent<SpringBone>().radius = coliderradius;
                        springBones.Add(o.gameObject.GetComponent<SpringBone>());
                    }
                }
            }
             
        }
        [ContextMenu("xiong-add")]
        public void GetEveryxiong()
        {
            Transform[] t = GetComponentsInChildren<Transform>();
            foreach (Transform o in t)
            {
                if (o.name.StartsWith("胸"))
                {
                    if (o.childCount > 0)
                    {
                        print("addxiong");
                        o.gameObject.AddComponent<SpringBone>();
                        o.gameObject.GetComponent<SpringBone>().child = o.GetChild(0);
                        o.gameObject.GetComponent<SpringBone>().boneAxis = new Vector3(0.0f, 1.0f, 0.0f);
                        springBones.Add(o.gameObject.GetComponent<SpringBone>());
                    }
                }
            }
            
        }
        [ContextMenu("bone-remove")]
        public void GetEveryremove()
        {
            Transform[] t = GetComponentsInChildren<Transform>();
            foreach (Transform o in t)
            {
                if (o.name.StartsWith("+") || o.name.StartsWith("裙子") || o.name.StartsWith("发")|| o.name.StartsWith("胸"))
                {
                    if (o.childCount > 0)
                    {
                        print("removebone");
						
						
						DestroyImmediate(o.gameObject.GetComponent<SpringBone>());
					}
				}
            }
			springBones.Clear();
        }
        [ContextMenu("bone-collid")]
        public void Everycol()
        {
            Transform[] t = GetComponentsInChildren<Transform>();
            foreach (Transform o in t)
            {
                if (o.name.StartsWith("+") || o.name.StartsWith("裙子")||o.name.StartsWith("发"))
                {
                    if (o.childCount > 0)
                    {
						o.gameObject.GetComponent<SpringBone>().colliders = springColliders;

                    }
                }
            }
            
        }
		[ContextMenu("radius-update")]
		public void updateradius()
		{
            
            foreach(SpringBone x in springBones)
            {
                x.radius=coliderradius;
            }
        }
        private void UpdateParameters ()
		{
			UpdateParameter ("stiffnessForce", stiffnessForce, stiffnessCurve);
			UpdateParameter ("dragForce", dragForce, dragCurve);
		}
	
		private void UpdateParameter (string fieldName, float baseValue, AnimationCurve curve)
		{
			var start = curve.keys [0].time;
			var end = curve.keys [curve.length - 1].time;
			//var step	= (end - start) / (springBones.Length - 1);
		
			var prop = springBones [0].GetType ().GetField (fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
		
			for (int i = 0; i < springBones.Count; i++) {
				//Kobayashi
				if (!springBones [i].isUseEachBoneForceSettings) {
					var scale = curve.Evaluate (start + (end - start) * i / (springBones.Count - 1));
					prop.SetValue (springBones [i], baseValue * scale);
				}
			}
		}
	}
}