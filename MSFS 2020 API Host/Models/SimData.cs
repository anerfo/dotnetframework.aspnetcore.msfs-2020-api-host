using JohnPenny.MSFS.SimConnectManager.REST.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace JohnPenny.MSFS.SimConnectManager.REST.Models
{
    public class SimData : IDataRepositoryItem<SimData>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        [Required]
        public Guid GUID { get; set; }
    }
}
