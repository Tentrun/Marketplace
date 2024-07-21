using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.BaseLibrary.Entity.Base;

namespace Marketplace.BaseLibrary.Entity.Products;

/// <summary>
/// Модель продукта для БД
/// </summary>
public class Product : BaseEntity
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
}