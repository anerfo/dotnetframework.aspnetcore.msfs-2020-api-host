using JohnPenny.MSFS.SimConnectManager.REST.Interfaces;
using JohnPenny.MSFS.SimConnectManager.REST.Models;
using System.Collections.Generic;
using System.Linq;

namespace JohnPenny.MSFS.SimConnectManager.REST.Services
{
    public class SimDataRepository : IDataRepository<SimData>
    {
        public List<SimData> Items { get; set; }

        public int Count => Items.Count;

        public IEnumerable<SimData> All => Items;

        public SimDataRepository()
        {
            Items = new List<SimData>();
        }

        public void Delete(string id)
        {
            var value = Items.First(v => v.Name == id);
            Items.Remove(value);
        }

        public bool DoesItemExist(string id)
        {
            return Items.Any(v => v.Name == id);
        }

        public SimData Find(string id, bool matchCase = false)
        {
            return Items.First(v => v.Name == id);
        }

        public void Insert(SimData item)
        {
            Items.Add(item);
        }

        public void Update(SimData item)
        {
            var value = Items.FirstOrDefault(v => v.Name == item.Name);
            if (value != null)
            {
                Items.Remove(value);
            }
            Items.Add(item);
        }
    }
}
