using SqlSugar;
using System;

namespace Th.Templete.Domain.Demo
{
    [SugarTable("Role")]
    public class DemoEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Remark { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDefault { get; set; }
        public bool IsSystem { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime DeletedTime { get; set; }
    }
}
