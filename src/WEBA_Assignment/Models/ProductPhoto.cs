using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    public class ProductPhoto
    {
        /// <summary>
        /// Class used to represent brand image uploaded to Cloudinary.
        /// </summary>

        /// <summary>
        /// Primary key of the database table
        /// </summary>
        public int ProductPhotoId { get; set; }

        /// <summary>
        /// The public id used by Cloudinary
        /// </summary>
        public string PublicCloudinaryId { get; set; }

        /// <summary>
        /// File upload version
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// The width of the uploaded image
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of the uploaded image
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Image format: JPEG, JPG, PNG
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// The time this image is created on, as reported by Cloudinary server
        /// </summary>
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedById { get; set; }
        public string DeletedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser DeletedBy { get; set; }
        /// <summary>
        /// The size of the uploaded image in bytes
        /// </summary>
        public int ImageSize { get; set; }
        /// <summary>
        /// URL to access the image [HTTP]
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// URL to access the image [HTTPs]
        /// </summary>
        public string SecureUrl { get; set; }
        /// <summary>
        /// Navigation Property
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// Foreign Key
        /// </summary>
        public int ProdId { get; set; }
        // Variable to define whether this is the main photo
        // of a product.
        public int isPrimaryPhoto { get; set; }
    }
}
