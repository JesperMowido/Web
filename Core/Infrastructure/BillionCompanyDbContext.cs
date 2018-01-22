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

        //Brokers
        private IBillionCompanyRepository<SkandiaProduct> skandiaProductRepository;
        private IBillionCompanyRepository<WretmanProduct> wretmanProductRepository;
        private IBillionCompanyRepository<PortugalMaklarnaProduct> portugalMaklarnaProductRepository;
        private IBillionCompanyRepository<NordicFranceProduct> nordicFranceProductRepository;
        private IBillionCompanyRepository<CCHomesProduct> ccHomesProductRepository;

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

        public IBillionCompanyRepository<SkandiaProduct> SkandiaProductRepository
        {
            get
            {
                if (this.skandiaProductRepository == null)
                {
                    this.skandiaProductRepository = new BillionCompanyRepository<SkandiaProduct>(_context);
                }
                return skandiaProductRepository;
            }
        }

        public IBillionCompanyRepository<WretmanProduct> WretmanProductRepository
        {
            get
            {
                if (this.wretmanProductRepository == null)
                {
                    this.wretmanProductRepository = new BillionCompanyRepository<WretmanProduct>(_context);
                }
                return wretmanProductRepository;
            }
        }

        public IBillionCompanyRepository<PortugalMaklarnaProduct> PortugalMaklarnaProductRepository
        {
            get
            {
                if (this.portugalMaklarnaProductRepository == null)
                {
                    this.portugalMaklarnaProductRepository = new BillionCompanyRepository<PortugalMaklarnaProduct>(_context);
                }
                return portugalMaklarnaProductRepository;
            }
        }

        public IBillionCompanyRepository<NordicFranceProduct> NordicFranceProductRepository
        {
            get
            {
                if (this.nordicFranceProductRepository == null)
                {
                    this.nordicFranceProductRepository = new BillionCompanyRepository<NordicFranceProduct>(_context);
                }
                return nordicFranceProductRepository;
            }
        }

        public IBillionCompanyRepository<CCHomesProduct> CcHomesProductRepository
        {
            get
            {
                if (this.ccHomesProductRepository == null)
                {
                    this.ccHomesProductRepository = new BillionCompanyRepository<CCHomesProduct>(_context);
                }
                return ccHomesProductRepository;
            }
        }
    }
}
