using CashGen.Entities;
using System;
using System.Collections.Generic;

namespace CashGen.Services
{
    public interface ICashGenRepository
    {
        IEnumerable<Product> GetProducts();

        Product GetProduct(Guid id);

        IEnumerable<Collection> GetProductCollections(Guid id);

        void DeleteProduct(Product product);

        void AddEventLog(EventLog line);

        IEnumerable<Image> GetImages(Guid id);

        IEnumerable<Feature> GetFeatures(Guid id);

        void AddProduct(Product product);

        void UpdateProduct(Product product);

        void DeleteProductImages(Guid id);

        void DeleteProductFeatures(Guid id);

        IEnumerable<Store> GetStores();

        Store GetStore(Guid id);

        void AddStore(Store store);

        void UpdateStore(Store store);

        void DeleteStore(Store store);

        IEnumerable<Collection> GetCollections(Guid id);

        IEnumerable<ProductFilter> GetProductFilters(Guid id);

        void DeleteProductFilters(Guid id);

        void AddChat(Chat chat);

        IEnumerable<Chat> GetChat(Guid id);

        IEnumerable<Chat> GetChats(Guid id, bool admin);

        IEnumerable<Order> GetOrders();

        Order GetOrder(Guid id);

        IEnumerable<LineItem> GetLineItems(Guid id);

        void RemoveOrderLines(Guid id);

        void AddOrder(Order order);

        void AddOrderLine(LineItem line);

        void UpdateOrder(Order order);

        Collection GetCollection(Guid id);

        IEnumerable<Filter> GetFilters();

        IEnumerable<Filter> GetCollectionFilters(Guid id);

        Filter GetFilter(Guid id);

        void AddFilter(Filter filter);

        void UpdateFilter(Filter filter);

        IEnumerable<FilterCollection> GetFilterCollections(Guid id);

        IEnumerable<FilterOption> GetFilterOptions(Guid id);

        void DeleteFilterOptions(Guid id);

        void DeleteFilterCollections(Guid id);

        IEnumerable<User> GetUsers();

        User GetUser(Guid id);

        User GetUserByEmail(string Emaild);

        User GetUserByResetToken(Guid id);

        User GetUserLogin(string Email, string Password);

        void AddUser(User user);

        void UpdateUser(User user);

        void DeleteUser(User user);

        IEnumerable<Note> GetNotes(Guid id);

        void AddNote(Note note);

        IEnumerable<Store> GetUserStores(Guid id);

        void AddStoreUser(StoreUser item);

        void DeleteStoreUser(StoreUser item);

        IEnumerable<Store> GetAccounts();

        bool Save();

    }
}
