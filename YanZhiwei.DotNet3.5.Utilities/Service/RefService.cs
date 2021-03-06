﻿using YanZhiwei.DotNet3._5.Utilities.Common;

namespace YanZhiwei.DotNet3._5.Utilities.Service
{
    /// <summary>
    /// 直接引用提供服务
    /// </summary>
    public abstract class RefService
    {
        /// <summary>
        /// 创建服务
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>
        /// 类型
        /// </returns>
        /// 时间：2016/9/6 16:51
        /// 备注：
        public virtual T CreateService<T>()
            where T : class
        {
            //override 可以做一些类似缓存的优化处理
            return AssemblyHelper.FindTypeByInterface<T>();
            //return CacheHelper.Get<T>(string.Format("Service_{0}", _interfaceName), () =>
            //{
            //    return AssemblyHelper.FindTypeByInterface<T>();
            //});
        }
    }
}