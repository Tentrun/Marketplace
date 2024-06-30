using Marketplace.BaseLibrary.Entity.Products;
using Marketplace.BaseLibrary.Interfaces.Base;
using Marketplace.ProductService.Data.Repository.Interface;

namespace Marketplace.ProductService.Data.Repository.Implementation;

public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository;