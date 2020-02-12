using System.Collections.Generic;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Entities.AggregateRoot;
using DayDayUp.BlogContext.Repositories;
using Microsoft.EntityFrameworkCore.Internal;

namespace DayDayUp.BlogContext.Services
{
    public class PostDomainService : IPostDomainService
    {
        public PostDomainService
        (
            ICategoryRepository categoryRepo,
            ITagRepository tagRepo,
            ITextConversionService textConversionService)
        {
            _categoryRepo = categoryRepo;
            _tagRepo = tagRepo;
            _textConversionService = textConversionService;
        }

        private readonly ICategoryRepository _categoryRepo;
        private readonly ITagRepository _tagRepo;
        private readonly ITextConversionService _textConversionService;

        public async Task<Category> GetOrCreateCategoryAsync(string name)
        {
            var category = _categoryRepo.Find(x => x.Name == name);
            if (category != null)
                return category;

            var categorySlug = await _textConversionService.GenerateSlugAsync(name);
            if (string.IsNullOrEmpty(categorySlug))
                categorySlug = name;

            category = new Category
            {
                Name = name,
                Slug = categorySlug
            };

            _categoryRepo.Insert(category);
            await _categoryRepo.UnitOfWork.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Tag>> GetOrCreateTagAsync(string[] tags)
        {
            var tagResult = new List<Tag>();
            if (tags == null)
            {
                return tagResult;
            }

            foreach (var item in tags)
            {
                var tag = _tagRepo.Find(x => x.Name == item);
                if (tag != null)
                {
                    tagResult.Add(tag);
                }
                else
                {
                    var tagSlug = await _textConversionService.GenerateSlugAsync(item);
                    if (string.IsNullOrEmpty(tagSlug))
                        tagSlug = item;

                    tag = new Tag
                    {
                        Name = item,
                        Slug = tagSlug
                    };

                    _tagRepo.Insert(tag);
                    await _tagRepo.UnitOfWork.SaveChangesAsync();
                }
            }

            return tagResult;
        }
    }
}