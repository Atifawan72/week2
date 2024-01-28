using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace app
{
    internal class user
    {
        public string name ;
        public int password;
        public string role;
        public user(string sname , int spassword , string roles )
        {
            name = sname;
            password = spassword;
            role = roles ; 
          
        }
        public static bool UserExists(string username, int password, List<user> userList)
        {
            foreach (var user in userList)
            {
                if (user.name == username && user.password == password )
                {
                    return true;
                }
            }
            return false;
        }
       public static void removeuser(string username , int password , List<user> userlist) 
        {
            foreach (var user in userlist) 
            {
                if(user.name == username && user.password == password)
                {

                    userlist.Remove (user); 
                }
            }
        }



    }
    
    
}
