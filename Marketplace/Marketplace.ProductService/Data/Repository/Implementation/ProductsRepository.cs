using Marketplace.BaseLibrary.Entity.Products;
using Marketplace.BaseLibrary.Interfaces.Base;
using Marketplace.ProductService.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.ProductService.Data.Repository.Implementation;

public class ProductsRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : BaseRepository<Product, ApplicationDbContext>(dbContextFactory), IProductsRepository;