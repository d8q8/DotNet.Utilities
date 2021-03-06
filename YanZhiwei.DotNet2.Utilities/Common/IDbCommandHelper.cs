﻿using System.Data;

namespace YanZhiwei.DotNet2.Utilities.Common
{
    /// <summary>
    /// IDbCommand帮助类
    /// </summary>
    /// 时间：2016-03-30 10:44
    /// 备注：
    public static class IDbCommandHelper
    {
        /// <summary>
        /// 从IDbCommand获取完整Sql
        /// </summary>
        /// <param name="cmd">IDbCommand</param>
        /// <returns>sql语句</returns>
        /// 时间：2016-03-30 10:45
        /// 备注：
        public static string GetGeneratedQuery(this IDbCommand cmd)
        {
            string _sql = cmd.CommandText;

            for(int i = 0; i < cmd.Parameters.Count; i++)
            {
                IDbDataParameter _parameter = cmd.Parameters[i] as IDbDataParameter;
                _sql = _sql.Replace(_parameter.ParameterName, _parameter.Value.ToStringOrDefault(string.Empty));
            }

            return _sql;
        }
    }
}