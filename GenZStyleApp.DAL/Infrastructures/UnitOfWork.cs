using GenZStyleApp.DAL.DAO;
using GenZStyleApp.DAL.Models;
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
