using BMOS.DAL.DAOs;
using GenZStyleApp.DAL.DAO;
using GenZStyleApp.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectParticipantManagement.DAL.Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private GenZStyleDbContext _dbContext;
        private AccountDAO _accountDAO;
        private RoleDAO _roleDAO;
        private UserDAO _userDAO;
        private WalletDAO _walletDAO;
        private TokenDAO _tokenDAO;
        private HashTagDAO _hashTagDAO;
        private LikeDAO _likeDAO;
        private PostDAO _postDAO;
        private CommentDAO _commentDAO;
        private NotificationDAO _notificationDAO;
        private UserRelationDAO  _userRelationDAO;
        

        public UnitOfWork()
        {
            if (this._dbContext == null)
            {
                this._dbContext = DbFactory.Instance.InitDbContext();
            }
        }
        public RoleDAO RoleDAO
        {
            get
            {
                if (_roleDAO == null)
                {
                    _roleDAO = new RoleDAO(_dbContext);
                }
                return _roleDAO;
            }
        }
        public UserRelationDAO userRelationDAO
        {
            get
            {
                if (_userRelationDAO == null)
                {
                    _userRelationDAO = new UserRelationDAO(_dbContext);
                }
                return _userRelationDAO;
            }
        }
        public NotificationDAO NotificationDAO
        {
            get
            {
                if (_notificationDAO == null)
                {
                    _notificationDAO = new NotificationDAO(_dbContext);
                }
                return _notificationDAO;
            }
        }
        public LikeDAO LikeDAO
        {
            get
            {
                if (_likeDAO == null)
                {
                    _likeDAO = new LikeDAO(_dbContext);
                }
                return _likeDAO;
            }
        }
        public PostDAO PostDAO
        {
            get
            {
                if (_postDAO == null)
                {
                    _postDAO = new PostDAO(_dbContext);
                }
                return _postDAO;
            }
        }
        public CommentDAO CommentDAO
        {
            get
            {
                if (_commentDAO == null)
                {
                    _commentDAO = new CommentDAO(_dbContext);
                }
                return _commentDAO;
            }
        }
        public TokenDAO TokenDAO
        {
            get
            {
                if (_tokenDAO == null)
                {
                    _tokenDAO = new TokenDAO(_dbContext);
                }
                return _tokenDAO;
            }
        }

        public HashTagDAO HashTagDAO
        {
            get
            {
                if (_hashTagDAO == null)
                {
                    _hashTagDAO = new HashTagDAO(_dbContext);
                }
                return _hashTagDAO;
            }
        }
        public WalletDAO WalletDAO
        {
            get
            {
                if (_walletDAO == null)
                {
                    _walletDAO = new WalletDAO(_dbContext);
                }
                return _walletDAO;
            }
        }
        public AccountDAO AccountDAO
        {
            get
            {
                if (this._accountDAO == null)
                {
                    this._accountDAO = new AccountDAO(this._dbContext);
                }
                return this._accountDAO;
            }
        }
        public UserDAO UserDAO
        {
            get
            {
                if (this._userDAO == null)
                {
                    this._userDAO = new UserDAO(this._dbContext);
                }
                return this._userDAO;
            }
        }
        



        /*public EmployeeDAO EmployeeDAO
        {
            get
            {
                if(this._employeeDAO == null)
                {
                    this._employeeDAO = new EmployeeDAO(this._dbContext);
                }
                return this._employeeDAO;
            }
        }

        public DepartmentDAO DepartmentDAO
        {
            get
            {
                if(this._departmentDAO == null)
                {
                    this._departmentDAO = new DepartmentDAO(this._dbContext);
                }
                return this._departmentDAO;
            }
        }

        public CompanyProjectDAO CompanyProjectDAO
        {
            get
            {
                if(this._companyProjectDAO == null)
                {
                    this._companyProjectDAO = new CompanyProjectDAO(this._dbContext);
                }
                return this._companyProjectDAO;
            }
        }*/

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
