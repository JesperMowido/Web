using Core.Entities;
using Core.Repositories;
using Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure
{
    public class BillionCompanyDbContext : DbContext
    {
        private BillionCompanyDbContext _context;

        public BillionCompanyDbContext()
            : base("DefaultConnection")
        {
            _context = this;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            (new BillionCompanyModelMapper()).Map(modelBuilder);
        }

        public static BillionCompanyDbContext Create()
        {
            return new BillionCompanyDbContext();
        }

        private IBillionCompanyRepository<User> userRepository;
        private IBillionCompanyRepository<Role> roleRepository;
        private IBillionCompanyRepository<TestConnection> testConnectionRepository;
        private IBillionCompanyRepository<Product> productRepository;
        private IBillionCompanyRepository<ProductType> productTypeRepository;
        private IBillionCompanyRepository<Place> placeRepository;
        private IBillionCompanyRepository<PlaceType> placeTypeRepository;

        public IBillionCompanyRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new BillionCompanyRepository<User>(_context);
                }
                return userRepository;
            }
        }

        public IBillionCompanyRepository<Role> RoleRepository
        {
            get
            {
                if (this.roleRepository == null)
                {
                    this.roleRepository = new BillionCompanyRepository<Role>(_context);
                }
                return roleRepository;
            }
        }

        public IBillionCompanyRepository<TestConnection> TestConnectionRepository
        {
            get
            {
                if (this.testConnectionRepository == null)
                {
                    this.testConnectionRepository = new BillionCompanyRepository<TestConnection>(_context);
                }
                return testConnectionRepository;
            }
        }

        public IBillionCompanyRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new BillionCompanyRepository<Product>(_context);
                }
                return productRepository;
            }
        }

        public IBillionCompanyRepository<ProductType> ProductTypeRepository
        {
            get
            {
                if (this.productTypeRepository == null)
                {
                    this.productTypeRepository = new BillionCompanyRepository<ProductType>(_context);
                }
                return productTypeRepository;
            }
        }

        public IBillionCompanyRepository<Place> PlaceRepository
        {
            get
            {
                if (this.placeRepository == null)
                {
                    this.placeRepository = new BillionCompanyRepository<Place>(_context);
                }
                return placeRepository;
            }
        }

        public IBillionCompanyRepository<PlaceType> PlaceTypeRepository
        {
            get
            {
                if (this.placeTypeRepository == null)
                {
                    this.placeTypeRepository = new BillionCompanyRepository<PlaceType>(_context);
                }
                return placeTypeRepository;
            }
        }
    }
}
