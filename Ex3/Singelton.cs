using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ex3.Models;

namespace Ex3
{
    public class Singelton
    {
        private static Server m_Instance = null;
        public static Server Instance
        {
            get
            {   //if not exist create it
                if (m_Instance == null)
                {
                    m_Instance = new Server();
                }
                return m_Instance;
            }
        }
    }
}