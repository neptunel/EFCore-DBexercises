using DAL.Model;
using EFLibCore;

namespace ServicesAPI6.Controllers
{
    public class DBOperations
    {
        private UserContext _context = new UserContext();
        LoggerCls logger = new LoggerCls();
        public bool AddModel(User _user )
        {
            try
            {
                _context.User.Add(_user);
                _context.SaveChanges();
                return true;
            }
            catch(Exception exc)
            {
                logger.createLog("HATA " + exc.Message);
                return false;
            }
        }
        public bool DeleteModel(int Id)
        {
            try
            {
                _context.User.Remove(FindUser("", "", Id));
                _context.SaveChanges();
                return true;
            }
            catch (Exception exc)
            {
                logger.createLog("HATA " + exc.Message);
                return false;
            }
        }
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            users = _context.User.ToList();

           InnerJoinExample();

            return users;
        }
    
        public User FindUser(string Name = "", string UserName = "", int UserId = 0)
        {
            User? user = new User();
            if(!string.IsNullOrEmpty(Name)  && !string.IsNullOrEmpty(UserName))
            user = _context.User.FirstOrDefault(m => m.Name == Name && m.UserName == UserName);
            else if(UserId > 0)
            {
                user = _context.User.FirstOrDefault(m => m.Id == UserId);
            }
            return user;
        }
 

        public void InnerJoinExample()
        {
            var user = _context.User.Join(_context.Adress, a => a.AdressId,
                  u => u.Id,
                 (user, adres) => new UserDetail { City = adres.City, Name = user.Name }).ToList();

        }
    
    }
}
