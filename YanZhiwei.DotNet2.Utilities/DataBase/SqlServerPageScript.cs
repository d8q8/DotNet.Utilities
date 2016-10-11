﻿namespace YanZhiwei.DotNet2.Utilities.DataBase
{
    using Builder;
    using Enum;
    
    /// <summary>
    /// Sql Server数据库分页脚本
    /// </summary>
    /// 创建时间:2015-05-22 11:48
    /// 备注说明:<c>null</c>
    public class SqlServerPageScript
    {
        #region Methods
        
        /// <summary>
        /// 利用[ROW_NUMBER() over]分页，生成sql语句
        /// </summary>
        /// <param name="tableName">表名称『eg:Orders』</param>
        /// <param name="columns">需要显示列『*:所有列；或者：eg:OrderID,OrderDate,ShipName,ShipCountry』</param>
        /// <param name="orderColumn">依据排序的列『eg:OrderID』</param>
        /// <param name="sqlWhere">筛选条件『eg:Order=1』</param>
        /// <param name="orderType">升序降序『1：desc;其他:asc』</param>
        /// <param name="pSize">每页页数『需大于零』</param>
        /// <param name="pIndex">页数『从壹开始算』</param>
        /// <returns>生成分页sql脚本</returns>
        public static string JoinPageSQLByRowNumber(string tableName, string columns, string orderColumn, string sqlWhere, OrderType orderType, int pSize, int pIndex)
        {
            int _pageStart = pSize * (pIndex - 1) + 1;
            int _pageEnd = pSize * pIndex + 1;
            string _sql = string.Format("select * from  (select (ROW_NUMBER() over(order by {2} {3})) as ROWNUMBER,{1}  from {0})as tp where ROWNUMBER >= {4} and ROWNUMBER< {5} ",
                                        tableName,
                                        columns,
                                        orderColumn,
                                        orderType == OrderType.Desc ? "desc" : "asc",
                                        _pageStart,
                                        _pageEnd);
            _sql = SqlScriptBuilder.JoinQueryWhereSql(_sql, sqlWhere);
            _sql = SqlScriptBuilder.JoinQueryTotalSql(_sql, tableName);
            return _sql;
        }
        
        /// <summary>
        /// 利用[Top Max]分页，生成sql语句
        /// </summary>
        /// <param name="tableName">表名称『eg:Orders』</param>
        /// <param name="columns">需要显示列『*:所有列；或者：eg:OrderID,OrderDate,ShipName,ShipCountry』</param>
        /// <param name="orderColumn">依据排序的列『eg:OrderID』</param>
        /// <param name="sqlWhere">筛选条件『eg:Order=1』</param>
        /// <param name="orderType">升序降序『1：desc;其他:asc』</param>
        /// <param name="pSize">每页页数『需大于零』</param>
        /// <param name="pIndex">页数『从壹开始算』</param>
        /// <returns>生成分页sql脚本</returns>
        public static string JoinPageSQLByTopMax(string tableName, string columns, string orderColumn, string sqlWhere, OrderType orderType, int pSize, int pIndex)
        {
            /*
             *eg:
             *1=>select top 30 orderID from Orders order by orderID asc
             *2=>(select max (orderID) from (select top 30 orderID from Orders order by orderID asc) as T) //查询前一页数据
             *3=> select top 15 OrderID,OrderDate,ShipName,ShipCountry from Orders where orderID>
                  ISNULL((select max (orderID) from (select top 30 orderID from Orders order by orderID asc) as T),0)
                  order by orderID asc
             */
            string _sql = string.Format("select top {4} {1} from {0} where {2}> ISNULL((select max ({2}) from (select top {5} {2} from {0} order by {2} {3}) as T),0) order by {2} {3}",
                                        tableName,
                                        columns,
                                        orderColumn,
                                        orderType == OrderType.Desc ? "desc" : "asc",
                                        pSize,
                                        (pIndex - 1) * pSize);
            _sql = SqlScriptBuilder.JoinQueryWhereSql(_sql, sqlWhere);
            _sql = SqlScriptBuilder.JoinQueryTotalSql(_sql, tableName);
            return _sql;
        }
        
        /// <summary>
        /// 利用[Top NotIn]分页，生成sql语句
        /// </summary>
        /// <param name="tableName">表名称『eg:Orders』</param>
        /// <param name="columns">需要显示列『*:所有列；或者：eg:OrderID,OrderDate,ShipName,ShipCountry』</param>
        /// <param name="orderColumn">依据排序的列『eg:OrderID』</param>
        /// <param name="sqlWhere">筛选条件『eg:Order=1』</param>
        /// <param name="orderType">升序降序『1：desc;其他:asc』</param>
        /// <param name="pSize">每页页数『需大于零』</param>
        /// <param name="pIndex">页数『从壹开始算』</param>
        /// <returns>生成分页sql脚本</returns>
        public static string JoinPageSQLByTopNotIn(string tableName, string columns, string orderColumn, string sqlWhere, OrderType orderType, int pSize, int pIndex)
        {
            /*
             *eg:
             *1=>SELECT orderID FROM Orders ORDER BY orderID
             *2=>SELECT TOP 20 orderID FROM Orders ORDER BY orderID //查询前一页数据
             *3=> SELECT TOP 10 * FROM Orders WHERE (orderID NOT IN (SELECT TOP 20 orderID FROM Orders ORDER BY orderID)) ORDER BY orderID //在所有数据中，截去掉上一页数据(not in)，然后select top 10 即当前页数据
             */
            string _sql = string.Format("SELECT TOP {4} {1} FROM {0} WHERE ({2} NOT IN (SELECT TOP {5} {2} FROM {0} ORDER BY {2} {3})) ORDER BY {2} {3}",
                                        tableName,
                                        columns,
                                        orderColumn,
                                        orderType == OrderType.Desc ? "desc" : "asc",
                                        pSize,
                                        (pIndex - 1) * pSize);
            _sql = SqlScriptBuilder.JoinQueryWhereSql(_sql, sqlWhere);
            _sql = SqlScriptBuilder.JoinQueryTotalSql(_sql, tableName);
            return _sql;
        }
        
        #endregion Methods
    }
}