using MainProject.Core.Commerce;
using MainProject.Core.Enums;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.ServiceAdmin.Model.Product;
using System.Linq;

namespace MainProject.ServiceAdmin.Helper
{
    public static class ProductHelper
    {
        /// <summary>
        /// Save commerce product category reference to product
        /// </summary>
        /// <param name="model"></param>
        /// <param name="product"></param>
        /// <param name="_commerceCategoryRepository"></param>
        /// <param name="_productCommerceCategoryRefRepository"></param>
        public static void SaveProductCategoryRef(ProductManageModel model, Product product, CommerceCategoryRepository _commerceCategoryRepository, ProductCommerceCategoryRefRepository _productCommerceCategoryRefRepository)
        {
            // Get old product categories
            var oldCatRefs = 
                _productCommerceCategoryRefRepository.Find(x => x.Product.Id == model.Id).Select(x => x.CommerceCategory.Id).ToList();
            // Get category User want to remove
            var deleteCatRefs = model.SelectedCommerceCategoryIds != null
                                    ? oldCatRefs.Where(x => model.SelectedCommerceCategoryIds.All(y => y != x)).ToList()
                                    : oldCatRefs;
            // Check has category to delete
            if (deleteCatRefs.Count > 0)
            {
                _productCommerceCategoryRefRepository.DeleteByCriteria(
                    x => x.Product.Id == model.Id && deleteCatRefs.Contains(x.CommerceCategory.Id));
            }
            // Insert product category reference product
            if (model.SelectedCommerceCategoryIds != null)
            {
                foreach (var catid in model.SelectedCommerceCategoryIds)
                {
                    if (!oldCatRefs.Contains(catid))
                    {
                        var cat = _commerceCategoryRepository.FindUnique(x => x.Id == catid);
                        if (cat != null)
                        {
                            _productCommerceCategoryRefRepository.Insert(new ProductCommerceCategoryRef() { CommerceCategory = cat, Product = product });
                        }
                    }
                }
            }

            // Save product url
            product.SeName = SeoHelper.ValidateSeNameAndSubmit(EntityTypeCollection.CommerceProduct, product.Id, product.SeName, product.Name, null);
        }

        /// <summary>
        /// Save properties reference to product
        /// </summary>
        /// <param name="model"></param>
        /// <param name="product"></param>
        /// <param name="_productRepository"></param>
        /// <param name="propertyRepository"></param>
        /// <param name="propertyOptionRepository"></param>
        /// <param name="productPropertyRefRepository"></param>
        public static void SaveProductPropertyRefs(ProductManageModel model, Product product,
            PropertyRepository propertyRepository,
            ProductPropertyRefRepository productPropertyRefRepository)
        {
            if (model.Properties != null)
            {
                foreach (var property in model.Properties.Where(x => !x.IsDeleted))
                {
                    var propertyEntity = property.Id != 0 ? productPropertyRefRepository.FindUnique(x => x.Id == property.Id) : null;
                    // Insert property ref
                    if (propertyEntity == null)
                    {
                        propertyEntity = new ProductPropertyRef()
                        {
                            Product = product,
                            Property = propertyRepository.FindUnique(x => x.Id == property.PropertySelectedId),
                            Value = property.Value,
                            Price = property.Price
                        };
                        productPropertyRefRepository.Insert(propertyEntity);
                    }
                    else // Update property ref
                    {
                        property.Value = property.Value;
                        property.Price = property.Price;
                        productPropertyRefRepository.SaveChanges();
                    }
                }

                model.Properties.Where(x => x.IsDeleted).Select(x => x.Id).ToList().ForEach(x => { productPropertyRefRepository.Delete(x); });
            }
            else // Incase user remove all property
            {
                productPropertyRefRepository.Find(
                        x => x.Product.Id == product.Id).Select(x => x.Id)
                                .ToList().ForEach(x => { productPropertyRefRepository.Delete(x); });
            }
        }
    }
}
