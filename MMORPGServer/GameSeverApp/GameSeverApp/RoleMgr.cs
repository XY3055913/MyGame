using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerApp
{
    /// <summary>
    /// 角色管理类
    /// </summary>
    public class RoleMgr
    {
        #region  单例
        //锁
        private static object lock_object = new object();

        private static RoleMgr instance;
        public static RoleMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lock_object)
                    {
                        instance = new RoleMgr();
                    }
                }
                return instance;
            }
        }
        #endregion

        //角色账本
        private List<Role> m_AllRole;
        public List<Role> AllRole
        {
            get { return m_AllRole; }

        }

        private RoleMgr()
        {
            m_AllRole = new List<Role>();

        }








    }
}
