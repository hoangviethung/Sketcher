using MainProject.Core;
using MainProject.Data.Repositories;
using MainProject.ServiceAdmin.Model;
using System.Collections.Generic;
using System.Linq;

namespace MainProject.ServiceAdmin.Helper
{
    public static class ImageHelper
    {
        /// <summary>
        /// Update Image (Create image in case it not exist in db, update image in case it exist in db, delete image in case it is deleted)
        /// </summary>
        /// <param name="images">List image model from views</param>
        /// <param name="imageRepository"></param>
        /// <param name="entity"></param>
        public static void UpdateImage(List<ImageManageModel> images, ImageRepository imageRepository, Article entity)
        {
            // Save Images
            if (images != null)
            {
                // Loop for getting image is not deleted
                foreach (var image in images.Where(x => !x.IsDeleted))
                {
                    // Get image does exist in db
                    var entityImage = imageRepository.FindUnique(x => x.Id == image.Id);
                    // Create image
                    if (entityImage == null)
                    {
                        entityImage = new Core.Image() {
                            Article = entity
                        };
                        // Parse data from model to entity
                        image.ToEntity(image, ref entityImage);
                        // Insert entity into db
                        imageRepository.Insert(entityImage);
                    }
                    else // Edit image
                    {
                        // Parse data from model to entity
                        image.ToEntity(image, ref entityImage);
                        // Save change
                        imageRepository.SaveChanges();
                    }
                }

                // Delete image is removed
                var listId = images.Where(x => x.IsDeleted).Select(y => y.Id);
                imageRepository.DeleteByCriteria(x => x.Article.Id == entity.Id && listId.Contains(x.Id));
            }
            else // Delete all image is removed
            {
                imageRepository.DeleteByCriteria(x => x.Article.Id == entity.Id);
            }
        }
    }
}
