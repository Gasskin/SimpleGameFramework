using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework.Core
{
    /// <summary>
    /// 模块管理基类
    /// </summary>
    public abstract class ManagerBase
    {
        /// 模块优先级，优先级高的模块会被先被更新，后被关闭 
        public virtual int Priority
        {
            get { return 0; }
        }

        /// 初始化模块
        public abstract void Init();

        /// 模块更新
        public abstract void Update(float time);

        /// 关闭模块
        public abstract void ShutDown();
    }
}
