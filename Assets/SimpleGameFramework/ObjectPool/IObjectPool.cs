using System;
using UnityEngine;

namespace SimpleGameFramework.ObjectPool
{
    public interface IObjectPool
    {
        #region Property

            /// <summary>
            /// 对象池名称
            /// </summary>
            string Name { get; }
     
            /// <summary>
            /// 对象池对象类型
            /// </summary>
            Type ObjectType { get; }
     
     
            /// <summary>
            /// 对象池中对象的数量。
            /// </summary>
            int Count { get; }
     
     
            /// <summary>
            /// 对象池中能被释放的对象的数量。
            /// </summary>
            int CanReleaseCount { get; }
     
            /// <summary>
            /// 对象池自动释放可释放对象的间隔秒数（隔几秒进行一次自动释放）
            /// </summary>
            float AutoReleaseInterval { get; set; }
     
            /// <summary>
            /// 对象池的容量。
            /// </summary>
            int Capacity { get; set; }
     
     
            /// <summary>
            /// 对象池对象过期秒数（被回收几秒钟视为过期，需要被释放）
            /// </summary>
            float ExpireTime { get; set; }
        #endregion

        #region 释放

            /// <summary>
            /// 释放超出对象池容量的可释放对象
            /// </summary>
            void Release();
     
            /// <summary>
            /// 释放指定数量的可释放对象
            /// </summary>
            /// <param name="toReleaseCount">尝试释放对象数量。</param>
            void Release(int toReleaseCount);
     
            /// <summary>
            /// 释放对象池中的所有未使用对象
            /// </summary>
            void ReleaseAllUnused();

        #endregion

        #region Update与ShutDown

            /// <summary>
            /// 轮询对象池
            /// </summary>
            void Update(float time);
     
            /// <summary>
            /// 清理并关闭对象池
            /// </summary>
            void Shutdown();

        #endregion
    }
}
