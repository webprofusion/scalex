using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Scalex.Models;

using Xamarin.Forms;

[assembly: Dependency(typeof(Scalex.Services.MockDataStore))]

namespace Scalex.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        private bool isInitialized;
        private List<Item> items;

        public async Task<bool> AddItemAsync(Item item)
        {
            await InitializeAsync();

            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            await InitializeAsync();

            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Item item)
        {
            await InitializeAsync();

            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            await InitializeAsync();

            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(items);
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;

            items = new List<Item>();
            var _items = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "Learn scales in any key", Description="Awesome"},
                 new Item { Id = Guid.NewGuid().ToString(), Text = "Learn chords in any key", Description="Awesome"},
                  new Item { Id = Guid.NewGuid().ToString(), Text = "Tablature", Description="Awesome"},
            };

            var w = new Webprofusion.Scalex.Guitar.GuitarModel();
            foreach (var s in w.AllScales)
            {
                _items.Add(new Item { Id = Guid.NewGuid().ToString(), Text = s.Name, Description = s.Name });
            }

            foreach (Item item in _items)
            {
                items.Add(item);
            }

            isInitialized = true;
        }
    }
}