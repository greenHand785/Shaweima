using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Component.UI
{
    /// <summary>
    /// ui跟随3d物体。当目标存在时，挂载本脚本的ui对象会一直跟随着目标物体.
    /// </summary>
    public class UIFollowObject : MonoBehaviour
    {
        public Transform target; // 目标
        public Vector3 latLonAlt; // 目标经纬度
        public Vector3 screenPos;
        public Camera showCamera; // 渲染ui的摄像机
        public Vector2 offset = Vector2.zero; // 偏移
        public bool OpenDestory; // 开启自动销毁，这个状态下，当跟随目标消失，则自身也销毁。
        //private DrawLine lineUI;
        private Vector3 targetPos; // 目标位置


        //public bool isOpenLine
        //{
        //    set
        //    {
        //        if (lineUI == null)
        //        {
        //            lineUI = CreateLine();
        //        }
        //        lineUI.isOpenDraw = value;
        //    }
        //    get
        //    {
        //        if (lineUI == null)
        //        {
        //            return false;
        //        }
        //        return lineUI.isOpenDraw;
        //    }
        //}

        public void DestorySelf()
        {
            //if (lineUI != null)
            //{
            //    Destroy(lineUI.gameObject);
            //}
            Destroy(gameObject);
        }

        void LateUpdate()
        {
            Follow();
        }

        // 创建线
        //private DrawLine CreateLine()
        //{
        //    GameObject obj = Resources.Load<GameObject>("Network/Components/line");
        //    return Instantiate(obj, FindObjectOfType<DymaticUIManager>().transform).GetComponent<DrawLine>();
        //}

        private void Follow()
        {
            if (showCamera == null)
            {
                return;
            }
            if (target == null)
            {
                //if(latLonAlt == Vector3.zero)
                //{
                //    if (OpenDestory)
                //    {
                //        DestorySelf();
                //    }
                //}
                //else
                //{
                //    if(EarthObjManager.Instance.GetEarthRenderMode() == EarthRenderMode.Earth3D)
                //    {
                //        targetPos = EarthObjManager.Instance.Map.transform.TransformPoint(Conversion.GetSpherePointFromLatLon(latLonAlt, latLonAlt.z));
                //    }
                //    else
                //    {
                //        targetPos = UnityPoint2GisPoint.Instance.GisToWorldPoint(new Vector2(latLonAlt.y, latLonAlt.x), latLonAlt.z);
                //    }
                //}
            }
            else
            {
                targetPos = target.position;
            }
            Vector3 screenPoint = showCamera.WorldToScreenPoint(targetPos);
            if(screenPos != Vector3.zero)
            {
                screenPoint = screenPos;
            }
            if(screenPoint.z < 0)
            {
                return;
            }
            Vector3 uiWorldPos = showCamera.ScreenToWorldPoint(screenPoint);
            Vector2 afterTransform = NormalScreenPoint2NowScreenPoint(screenPoint, showCamera);
            //transform.position = uiWorldPos;
            Vector3 localPos = transform.localPosition;
            localPos.z = 0;
            localPos.x += offset.x;
            localPos.y += offset.y;
            //transform.localPosition = localPos;
            transform.GetComponent<RectTransform>().anchoredPosition = offset + afterTransform;
            DrawLine(afterTransform, offset + afterTransform);
        }

        private void DrawLine(Vector2 startPos, Vector2 endPos)
        {
            if (offset == Vector2.zero)
            {
                return;
            }
            SetLineStartPos(startPos);
            SetLineEndPos(endPos);
        }

        /// <summary>
        /// 设置线的起始位置
        /// </summary>
        /// <param name="startPos"></param>
        private void SetLineStartPos(Vector2 startPos)
        {
            //if (lineUI == null)
            //{
            //    Debug.LogError("子物体中不存在DrawLine组件，设置失败");
            //    return;
            //}
            //if (!lineUI.isOpenDraw)
            //{
            //    return;
            //}
            //lineUI.startPos = startPos;
        }

        /// <summary>
        /// 设置终点位置
        /// </summary>
        /// <param name="endPos"></param>
        private void SetLineEndPos(Vector2 endPos)
        {
            //if (lineUI == null)
            //{
            //    Debug.LogError("子物体中不存在DrawLine组件，设置失败");
            //    return;
            //}
            //if (!lineUI.isOpenDraw)
            //{
            //    return;
            //}
            //lineUI.endPos = endPos;
        }

        /// <summary>
        /// 普通相机坐标转到相机viewpoint发生变化的相机坐标。
        /// </summary>
        /// <param name="screenPoint"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        private Vector2 NormalScreenPoint2NowScreenPoint(Vector3 screenPoint, Camera camera)
        {
            if (camera == null)
            {
                return default(Vector3);
            }
            return screenPoint;
            //return new Vector2(screenPoint.x - camera.pixelRect.x, screenPoint.y - camera.pixelRect.y);
        }

        /// <summary>
        /// 相机viewpoint发生变化，转普通相机坐标
        /// </summary>
        /// <param name="screenPoint"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        private Vector2 NowScreenPoint2NormalScreenPoint(Vector3 screenPoint, Camera camera)
        {
            if (camera == null)
            {
                return default(Vector3);
            }
            return new Vector2(screenPoint.x + camera.pixelRect.x, screenPoint.y + camera.pixelRect.y);
        }
    }
}
