using CashGen.DBContexts;
using CashGen.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CashGen.Services
{
    public class CashGenRepository: ICashGenRepository, IDisposable
    {
        private readonly CashGenContext _context;

        public CashGenRepository(CashGenContext context) => this._context = context ?? throw new ArgumentNullException(nameof(context));

        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            this._context.Products.Add(product);
        }

        public void AddOrderLine(LineItem line) => this._context.LineItems.Add(line);

        public void AddEventLog(EventLog line) => this._context.EventLogs.Add(line);

        public void AddChat(Chat chat) => this._context.Chats.Add(chat);

        public IEnumerable<Chat> GetChat(Guid id) => (IEnumerable<Chat>)((IQueryable<Chat>)this._context.Chats).Where<Chat>((Expression<Func<Chat, bool>>)(c => c.Id == id || c.ParentId == id)).OrderBy<Chat, DateTime>((Expression<Func<Chat, DateTime>>)(c => c.MessageDate)).ToList<Chat>();

        public IEnumerable<Chat> GetChats(Guid id, bool admin) => (IEnumerable<Chat>)((IQueryable<Chat>)this._context.Chats).Where<Chat>((Expression<Func<Chat, bool>>)(c => (c.StoreId == id || admin == true) && c.ParentId == new Guid("00000000-0000-0000-0000-000000000000"))).OrderByDescending<Chat, DateTime>((Expression<Func<Chat, DateTime>>)(c => c.MessageDate)).Take<Chat>(100).ToList<Chat>();

        public void UpdateProduct(Product product)
        {
        }

        public IEnumerable<Product> GetProducts() => (IEnumerable<Product>)((IQueryable<Product>)this._context.Products).OrderBy<Product, string>((Expression<Func<Product, string>>)(c => c.Title)).ToList<Product>();

        public Product GetProduct(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            return ((IQueryable<Product>)this._context.Products).Where<Product>((Expression<Func<Product, bool>>)(c => c.Id == id)).FirstOrDefault<Product>();
        }

        public Collection GetCollection(Guid id)
        {
            if (id == Guid.Empty)
                return new Collection()
                {
                    Title = "All Collections",
                    Id = Guid.Empty
                };
            return ((IQueryable<Collection>)this._context.Collections).Where<Collection>((Expression<Func<Collection, bool>>)(c => c.Id == id)).FirstOrDefault<Collection>();
        }

        public IEnumerable<Collection> GetProductCollections(Guid id)
        {
            List<Collection> productCollections = new List<Collection>();
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            Product productFromRepo = ((IQueryable<Product>)this._context.Products).Where<Product>((Expression<Func<Product, bool>>)(c => c.Id == id)).FirstOrDefault<Product>();
            if (!string.IsNullOrEmpty(productFromRepo.CatLevel1))
            {
                Collection collection1 = new Collection();
                Collection collection2 = ((IQueryable<Collection>)this._context.Collections).Where<Collection>((Expression<Func<Collection, bool>>)(c => c.Id == new Guid(productFromRepo.CatLevel1))).First<Collection>();
                productCollections.Add(collection2);
            }
            if (!string.IsNullOrEmpty(productFromRepo.CatLevel2))
            {
                Collection collection3 = new Collection();
                Collection collection4 = ((IQueryable<Collection>)this._context.Collections).Where<Collection>((Expression<Func<Collection, bool>>)(c => c.Id == new Guid(productFromRepo.CatLevel2))).First<Collection>();
                productCollections.Add(collection4);
            }
            if (!string.IsNullOrEmpty(productFromRepo.CatLevel3))
            {
                Collection collection5 = new Collection();
                Collection collection6 = ((IQueryable<Collection>)this._context.Collections).Where<Collection>((Expression<Func<Collection, bool>>)(c => c.Id == new Guid(productFromRepo.CatLevel3))).First<Collection>();
                productCollections.Add(collection6);
            }
            return (IEnumerable<Collection>)productCollections;
        }

        public void DeleteProduct(Product product) => this._context.Products.Remove(product);

        public IEnumerable<Image> GetImages(Guid id) => (IEnumerable<Image>)((IQueryable<Image>)this._context.Images).Where<Image>((Expression<Func<Image, bool>>)(c => c.ProductId == id)).ToList<Image>();

        public IEnumerable<Feature> GetFeatures(Guid id) => (IEnumerable<Feature>)((IQueryable<Feature>)this._context.Features).Where<Feature>((Expression<Func<Feature, bool>>)(c => c.ProductId == id)).ToList<Feature>();

        public IEnumerable<ProductFilter> GetProductFilters(Guid id) => (IEnumerable<ProductFilter>)((IQueryable<ProductFilter>)this._context.ProductFilters).Where<ProductFilter>((Expression<Func<ProductFilter, bool>>)(c => c.ProductId == id)).ToList<ProductFilter>();

        public void DeleteProductImages(Guid id)
        {
            DbSet<Image> images = this._context.Images;
            Expression<Func<Image, bool>> predicate = (Expression<Func<Image, bool>>)(c => c.ProductId == id);
            foreach (Image image in ((IQueryable<Image>)images).Where<Image>(predicate).ToList<Image>())
                this._context.Images.Remove(image);
        }

        public void DeleteProductFeatures(Guid id)
        {
            DbSet<Feature> features = this._context.Features;
            Expression<Func<Feature, bool>> predicate = (Expression<Func<Feature, bool>>)(c => c.ProductId == id);
            foreach (Feature feature in ((IQueryable<Feature>)features).Where<Feature>(predicate).ToList<Feature>())
                this._context.Features.Remove(feature);
        }

        public void AddStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));
            this._context.Stores.Add(store);
        }

        public void UpdateStore(Store store)
        {
        }

        public IEnumerable<Store> GetStores() => (IEnumerable<Store>)((IQueryable<Store>)this._context.Stores).OrderBy<Store, string>((Expression<Func<Store, string>>)(c => c.Title)).ToList<Store>();

        public Store GetStore(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            return ((IQueryable<Store>)this._context.Stores).Where<Store>((Expression<Func<Store, bool>>)(c => c.Id == id)).FirstOrDefault<Store>();
        }

        public void DeleteStore(Store store) => this._context.Stores.Remove(store);

        public IEnumerable<Collection> GetCollections(Guid id) => (IEnumerable<Collection>)((IQueryable<Collection>)this._context.Collections).Where<Collection>((Expression<Func<Collection, bool>>)(c => c.ParentId == id)).OrderBy<Collection, string>((Expression<Func<Collection, string>>)(c => c.Title)).ToList<Collection>();

        public void DeleteFilterOptions(Guid id)
        {
            DbSet<FilterOption> filterOptions = this._context.FilterOptions;
            Expression<Func<FilterOption, bool>> predicate = (Expression<Func<FilterOption, bool>>)(c => c.FilterId == id);
            foreach (FilterOption filterOption in ((IQueryable<FilterOption>)filterOptions).Where<FilterOption>(predicate).ToList<FilterOption>())
                this._context.FilterOptions.Remove(filterOption);
        }

        public void DeleteFilterCollections(Guid id)
        {
            DbSet<FilterCollection> filterCollections = this._context.FilterCollections;
            Expression<Func<FilterCollection, bool>> predicate = (Expression<Func<FilterCollection, bool>>)(c => c.FilterId == id);
            foreach (FilterCollection filterCollection in ((IQueryable<FilterCollection>)filterCollections).Where<FilterCollection>(predicate).ToList<FilterCollection>())
                this._context.FilterCollections.Remove(filterCollection);
        }

        public void DeleteProductFilters(Guid id)
        {
            DbSet<ProductFilter> productFilters = this._context.ProductFilters;
            Expression<Func<ProductFilter, bool>> predicate = (Expression<Func<ProductFilter, bool>>)(c => c.ProductId == id);
            foreach (ProductFilter productFilter in ((IQueryable<ProductFilter>)productFilters).Where<ProductFilter>(predicate).ToList<ProductFilter>())
                this._context.ProductFilters.Remove(productFilter);
        }

        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            this._context.Orders.Add(order);
        }

        public void UpdateOrder(Order order)
        {
        }

        public IEnumerable<Order> GetOrders() => (IEnumerable<Order>)((IQueryable<Order>)this._context.Orders).OrderByDescending<Order, int>((Expression<Func<Order, int>>)(c => c.order_number)).ToList<Order>();

        public Order GetOrder(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            return ((IQueryable<Order>)this._context.Orders).Where<Order>((Expression<Func<Order, bool>>)(c => c.Id == id)).FirstOrDefault<Order>();
        }

        public IEnumerable<LineItem> GetLineItems(Guid id) => (IEnumerable<LineItem>)((IQueryable<LineItem>)this._context.LineItems).Where<LineItem>((Expression<Func<LineItem, bool>>)(c => c.OrderId == id)).ToList<LineItem>();

        public void RemoveOrderLines(Guid id)
        {
            DbSet<LineItem> lineItems = this._context.LineItems;
            Expression<Func<LineItem, bool>> predicate = (Expression<Func<LineItem, bool>>)(c => c.OrderId == id);
            foreach (LineItem lineItem in ((IQueryable<LineItem>)lineItems).Where<LineItem>(predicate).ToList<LineItem>())
                this._context.LineItems.Remove(lineItem);
        }

        public void AddFilter(Filter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            this._context.Filters.Add(filter);
        }

        public void UpdateFilter(Filter filter)
        {
        }

        public IEnumerable<Filter> GetFilters() => (IEnumerable<Filter>)((IQueryable<Filter>)this._context.Filters).OrderBy<Filter, string>((Expression<Func<Filter, string>>)(c => c.Label)).ToList<Filter>();

        public IEnumerable<Filter> GetCollectionFilters(Guid id)
        {
            Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
            List<Guid> listOfCollections = new List<Guid>()
      {
        guid,
        id
      };
            Collection collection = ((IQueryable<Collection>)this._context.Collections).Where<Collection>((Expression<Func<Collection, bool>>)(p => p.Id == id)).FirstOrDefault<Collection>();
            if (collection != null && collection.ParentId != guid)
            {
                listOfCollections.Add(collection.ParentId);
                Collection collection1 = ((IQueryable<Collection>)this._context.Collections).Where<Collection>((Expression<Func<Collection, bool>>)(p => p.Id == collection.ParentId)).FirstOrDefault<Collection>();
                if (collection1.ParentId != guid)
                    listOfCollections.Add(collection1.ParentId);
            }
            return (IEnumerable<Filter>)((IQueryable<Filter>)this._context.Filters).Where<Filter>((Expression<Func<Filter, bool>>)(p => p.Collections.Any<FilterCollection>((Func<FilterCollection, bool>)(x => listOfCollections.Contains(x.CollectionId))))).ToList<Filter>();
        }

        public Filter GetFilter(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            return ((IQueryable<Filter>)this._context.Filters).Where<Filter>((Expression<Func<Filter, bool>>)(c => c.Id == id)).FirstOrDefault<Filter>();
        }

        public IEnumerable<FilterCollection> GetFilterCollections(Guid id) => (IEnumerable<FilterCollection>)((IQueryable<FilterCollection>)this._context.FilterCollections).Where<FilterCollection>((Expression<Func<FilterCollection, bool>>)(c => c.FilterId == id)).ToList<FilterCollection>();

        public IEnumerable<FilterOption> GetFilterOptions(Guid id) => (IEnumerable<FilterOption>)((IQueryable<FilterOption>)this._context.FilterOptions).Where<FilterOption>((Expression<Func<FilterOption, bool>>)(c => c.FilterId == id)).OrderBy<FilterOption, string>((Expression<Func<FilterOption, string>>)(c => c.Value)).ToList<FilterOption>();

        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            this._context.Users.Add(user);
        }

        public void UpdateUser(User user)
        {
        }

        public IEnumerable<User> GetUsers() => (IEnumerable<User>)((IQueryable<User>)this._context.Users).OrderBy<User, string>((Expression<Func<User, string>>)(c => c.LastName)).ToList<User>();

        public User GetUser(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            return ((IQueryable<User>)this._context.Users).Where<User>((Expression<Func<User, bool>>)(c => c.Id == id)).FirstOrDefault<User>();
        }

        public User GetUserByEmail(string Email) => ((IQueryable<User>)this._context.Users).Where<User>((Expression<Func<User, bool>>)(c => c.Email == Email)).FirstOrDefault<User>();

        public User GetUserByResetToken(Guid id) => ((IQueryable<User>)this._context.Users).Where<User>((Expression<Func<User, bool>>)(c => c.ResetToken == id)).FirstOrDefault<User>();

        public User GetUserLogin(string Email, string Password) => ((IQueryable<User>)this._context.Users).Where<User>((Expression<Func<User, bool>>)(c => c.Email == Email && c.Password == Password)).FirstOrDefault<User>();

        public void DeleteUser(User user) => this._context.Users.Remove(user);

        public IEnumerable<Note> GetNotes(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            return (IEnumerable<Note>)((IQueryable<Note>)this._context.Notes).Where<Note>((Expression<Func<Note, bool>>)(c => c.LinkedId == id)).ToList<Note>();
        }

        public void AddNote(Note note)
        {
            if (note == null)
                throw new ArgumentNullException(nameof(note));
            this._context.Notes.Add(note);
        }

        public IEnumerable<Store> GetUserStores(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            List<StoreUser> list = ((IQueryable<StoreUser>)this._context.StoreUsers).Where<StoreUser>((Expression<Func<StoreUser, bool>>)(c => c.UserId == id)).ToList<StoreUser>();
            List<Store> userStores = new List<Store>();
            foreach (StoreUser storeUser in list)
            {
                StoreUser item = storeUser;
                Store store = ((IQueryable<Store>)this._context.Stores).Where<Store>((Expression<Func<Store, bool>>)(c => c.Id == item.StoreId)).FirstOrDefault<Store>();
                userStores.Add(store);
            }
            return (IEnumerable<Store>)userStores;
        }

        public IEnumerable<Store> GetAccounts()
        {
            List<Store> storeList = new List<Store>();
            return (IEnumerable<Store>)((IQueryable<Store>)this._context.Stores).OrderBy<Store, string>((Expression<Func<Store, string>>)(c => !string.IsNullOrEmpty(c.GroupName) ? c.GroupName : c.Title)).ToList<Store>();
        }

        public void AddStoreUser(StoreUser item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            this._context.StoreUsers.Add(item);
        }

        public void DeleteStoreUser(StoreUser item) => this._context.StoreUsers.Remove(item);

        public bool Save() => this._context.SaveChanges() >= 0;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            int num = disposing ? 1 : 0;
        }

    }
}
