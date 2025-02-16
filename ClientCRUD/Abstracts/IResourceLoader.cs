using ClientCRUD.Models;

namespace ClientCRUD.Abstracts
{
    public interface IResourceLoader
    {
        /// <summary>
        /// Retrieves all resources.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation that returns a collection of response models.</returns>
        Task<IEnumerable<UnifiedResponseModel>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a resource by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the target resource.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation that returns a single response model.</returns>
        Task<UnifiedResponseModel> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new resource based on the provided request model.
        /// </summary>
        /// <param name="request">The request model containing the resource data.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(UnifiedRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing resource based on the provided request model.
        /// </summary>
        /// <param name="request">The request model containing the updated resource data.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(UnifiedRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a resource by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the resource to delete.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
