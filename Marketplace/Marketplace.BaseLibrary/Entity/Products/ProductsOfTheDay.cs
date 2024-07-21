using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.BaseLibrary.Entity.Base;

namespace Marketplace.BaseLibrary.Entity.Products;

//TODO: сделать по другому
// хотел сделать наследование от Product (что бы поля не дублировать)
// но в таком случае нужно оверрайдить OnModelCreating, прописывать билдеру стратегию генерации Tpc
// и снова НО - генерится общий sequence на обе таблицы, а если задавать кастомные, то при миграции ворнинги сыпет
// хотел что бы у каждого Id был тот счётчик, который генерится автоматом и ничего не ломается (╯°□°）╯︵ ┻━┻
public class ProductOfTheDay : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    /// <summary>
    /// Название товара
    /// </summary>
    [Required]
    [MaxLength(36)]
    public string Name { get; set; }
    
    /// <summary>
    /// Картинка товара
    /// TODO:Сделать массив картинок
    /// </summary>
    public string? Image { get; set; }
    
    /// <summary>
    /// Описание товара
    /// </summary>
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Цена товара
    /// </summary>
    /// <returns></returns>
    [Required]
    public decimal Price { get; set; }

    /// <summary>
    /// Рейтинг товара
    /// TODO: Feature
    /// </summary>
    public int Rating { get; set; } = 0;
    
    /// <summary>
    /// Сколько товаров есть в наличии
    /// </summary>
    public int StockQuantity { get; set; } = 0;

    /// <summary>
    /// Количество просмотров  
    /// </summary>
    public int ViewsQuantity { get; set; } = 0;
}