


namespace Catalog.API.Products.CreateProduct
{

    public record CreateProductCommand(string Name,List<string> Category, string Description,
                        string ImageFile, decimal Price)
        :ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);


    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            //create a product entity
            //save to database
            //return createProductresult 
           var product = new Product()
            { 
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price
            };
            
            session.Store(product);
            session.SaveChangesAsync(cancellationToken);
            //Business logic 
                     
            return new CreateProductResult(product.Id);

        }
    }
}
