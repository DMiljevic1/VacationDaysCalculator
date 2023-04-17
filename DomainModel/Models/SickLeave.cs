using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
	public class SickLeave
	{
		public int Id { get; set; }
		public DateTime SickLeaveFrom { get; set; }
		public DateTime? SickLeaveTo { get; set; }
		public int UserId { get; set; }
		public User? User { get; set; }
		public bool IsClosed { get; set; }
	}
}
